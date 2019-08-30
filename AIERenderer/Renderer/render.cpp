#pragma region Useful links
//https://docs.gl
//https://github.com/g-truc/glm/
//https://github.com/glfw/glfw/
//https://github.com/nigels-com/glew/
#pragma endregion

#define STB_IMAGE_IMPLEMENTATION
#include "render.h"
#include "glm/gtc/type_ptr.hpp"
#include "stb/stb_image.h"
#include <iostream>
#define TINYOBJLOADER_IMPLEMENTATION
#include "tinyObj/tiny_obj_loader.h"

geometry makeGeometry(vertex * verts, size_t vertCount, unsigned * indices, size_t indexCount)
{
	//create an instance of geometry
	geometry newGeo = {};
	newGeo.size = indexCount;

	//generate buffers
	glGenVertexArrays(1, &newGeo.vao); //vertex array object
	glGenBuffers(1, &newGeo.vbo); //vertex buffer object
	glGenBuffers(1, &newGeo.ibo); //index buffer object

	//bind buffers
	glBindVertexArray(newGeo.vao);
	glBindBuffer(GL_ARRAY_BUFFER, newGeo.vbo);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, newGeo.ibo);

	//populate buffers
	glBufferData(GL_ARRAY_BUFFER, vertCount * sizeof(vertex), verts, GL_STATIC_DRAW);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, indexCount * sizeof(unsigned), indices, GL_STATIC_DRAW);

	//describe vertex data
	glEnableVertexAttribArray(0); //position
	glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)0);

	glEnableVertexAttribArray(1); //normals
	glVertexAttribPointer(1, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)16);

	glEnableVertexAttribArray(2); //UVs
	glVertexAttribPointer(2, 2, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)32);

	//unbind buffers (in a SPECIFIC order)
	glBindVertexArray(0);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

	//return the geometry
	return newGeo;
}

void freeGeometry(geometry &geo)
{
	glDeleteBuffers(1, &geo.vbo);
	glDeleteBuffers(1, &geo.ibo);
	glDeleteVertexArrays(1, &geo.vao);

	geo = {};
}

shader makeShader(const char * vertSource, const char * fragSource)
{
	//make the shader object
	shader newShad = {};
	newShad.program = glCreateProgram();

	//create the shaders
	GLuint vert = glCreateShader(GL_VERTEX_SHADER);
	GLuint frag = glCreateShader(GL_FRAGMENT_SHADER);

	//compile the shaders
	glShaderSource(vert, 1, &vertSource, 0);
	glShaderSource(frag, 1, &fragSource, 0);
	glCompileShader(vert);
	//[error check here]
	glCompileShader(frag);
	//[error check here]

	//attach the shaders
	glAttachShader(newShad.program, vert);
	glAttachShader(newShad.program, frag);

	//link the shaders
	glLinkProgram(newShad.program);
	GLint errorCode = 0;
	glGetProgramiv(newShad.program, GL_LINK_STATUS, &errorCode);
	if (errorCode != GL_TRUE)
	{
		GLsizei logLen = 0;
		glGetProgramiv(newShad.program, GL_INFO_LOG_LENGTH, &logLen);
		char * log = new char[logLen + 1];
		glGetProgramInfoLog(newShad.program, logLen, &logLen, log);

		std::cerr << log << std::endl;

		delete[] log;
		assert(false && "Bad program link");
	}

	//delete the shaders
	glDeleteShader(vert);
	glDeleteShader(frag);

	//return the shader object
	return newShad;
}

void freeShader(shader &shad)
{
	glDeleteProgram(shad.program);
	shad = {};
}

void draw(const shader &shad, const geometry &geo)
{
	//bind the shader program
	glUseProgram(shad.program);
	//bind the VAO (geo and indices)
	glBindVertexArray(geo.vao);
	//draw
	glDrawElements(GL_TRIANGLES, geo.size, GL_UNSIGNED_INT, 0);
}

void setUniform(const shader &shad, GLuint location, const glm::vec3 &value)
{
	glProgramUniform3fv(shad.program, location, 1, glm::value_ptr(value));
}
void setUniform(const shader &shad, GLuint location, const glm::mat4 &value)
{
	glProgramUniformMatrix4fv(shad.program, location, 1, GL_FALSE, glm::value_ptr(value));
}
void setUniform(const shader &shad, GLuint location, const texture &value, int textureSlot)
{
	glActiveTexture(GL_TEXTURE0 + textureSlot);
	glBindTexture(GL_TEXTURE_2D, value.handle);
	glProgramUniform1i(shad.program, location, textureSlot);
}

texture makeTexture(unsigned width, unsigned height, unsigned channels, const unsigned char *pixels)
{
	GLenum oglFormat = GL_RGBA;

	switch (channels)
	{
	case 1:
		oglFormat = GL_RED;
		break;
	case 2:
		oglFormat = GL_RG;
		break;
	case 3:
		oglFormat = GL_RGB;
		break;
	case 4:
		oglFormat = GL_RGBA;
		break;
	default:
		//todo: fatal error, might wanna die
		break;
	}

	texture tex = { 0, width, height, channels };
	glGenTextures(1, &tex.handle);
	glBindTexture(GL_TEXTURE_2D, tex.handle);

	glTexImage2D(GL_TEXTURE_2D, 0, oglFormat, width, height, 0, oglFormat, GL_UNSIGNED_BYTE, pixels);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glBindTexture(GL_TEXTURE_2D, 0);

	return tex;
}

void freeTexture(texture &tex)
{
	glDeleteTextures(1, &tex.handle);
	tex = {};
}

texture loadTexture(const char * imagePath)
{
	int imageWidth, imageHeight, imageFormat;
	unsigned char *rawPixelData = nullptr;

	stbi_set_flip_vertically_on_load(true);
	rawPixelData = stbi_load(imagePath, &imageWidth, &imageHeight, &imageFormat, STBI_default);

	//todo check rawPixelData null

	texture tex = makeTexture(imageWidth, imageHeight, imageFormat, rawPixelData);

	stbi_image_free(rawPixelData);

	return tex;
}

geometry loadModel(std::string inputfile)
{
	std::string warn;
	std::string err;

	tinyobj::attrib_t attrib;
	std::vector<tinyobj::shape_t> shapes;
	std::vector<tinyobj::material_t> materials;

	bool ret = tinyobj::LoadObj(&attrib, &shapes, &materials, &warn, &err, inputfile.c_str());

	if (!warn.empty()) 
	{
		std::cout << warn << std::endl;
	}

	if (!err.empty()) 
	{
		std::cerr << err << std::endl;
	}

	std::vector<vertex> verts;
	std::vector<unsigned> indices;

	
	for (size_t s = 0; s < shapes.size(); s++)
	{
		// Loop over faces(polygon)
		size_t index_offset = 0;
		unsigned counter = 0;
		for (size_t f = 0; f < shapes[s].mesh.num_face_vertices.size(); f++)
		{
			int fv = shapes[s].mesh.num_face_vertices[f];

			// Loop over vertices in the face.
			for (size_t v = 0; v < fv; v++) {
				// access to vertex
				tinyobj::index_t idx = shapes[s].mesh.indices[index_offset + v];
				
				//vertices
				tinyobj::real_t vx = attrib.vertices[3 * idx.vertex_index + 0];
				tinyobj::real_t vy = attrib.vertices[3 * idx.vertex_index + 1];
				tinyobj::real_t vz = attrib.vertices[3 * idx.vertex_index + 2];
				
				//normals
				tinyobj::real_t nx = attrib.normals[3 * idx.normal_index + 0];
				tinyobj::real_t ny = attrib.normals[3 * idx.normal_index + 1];
				tinyobj::real_t nz = attrib.normals[3 * idx.normal_index + 2];

				//tangents?
				//tinyobj::real_t tanx = attrib.[3 * idx.normal_index + 0];

				//texture cords
				tinyobj::real_t tx = attrib.texcoords[2 * idx.texcoord_index + 0];
				tinyobj::real_t ty = attrib.texcoords[2 * idx.texcoord_index + 1];

				verts.emplace_back(vertex{ {vx, vy, vz, 1.0f}, {nx, ny, nz, 1.0f}, {tx, ty} });
				indices.emplace_back(counter++);
			}
			index_offset += fv;

			// per-face material
			shapes[s].mesh.material_ids[f];
		}
	}

	return makeGeometry(verts.data(), verts.size(), indices.data(), indices.size());
}