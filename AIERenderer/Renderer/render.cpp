//https://docs.gl
//https://github.com/g-truc/glm/
//https://github.com/glfw/glfw/
//https://github.com/nigels-com/glew/

#include "render.h"

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