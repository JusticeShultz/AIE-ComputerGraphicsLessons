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
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(0, 4, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)0);
	glEnableVertexAttribArray(1);
	glVertexAttribPointer(1, 2, GL_FLOAT, GL_FALSE, sizeof(vertex), (void*)sizeof(vertex::pos));

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