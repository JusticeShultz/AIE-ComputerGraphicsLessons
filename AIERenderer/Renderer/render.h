#pragma once

#include "glew/GL/glew.h"
#include "glm/glm.hpp"

struct vertex
{
	glm::vec4 pos;
};

struct geometry
{
	GLuint vao, vbo, ibo;	//buffers
	GLuint size;			//index count
};

struct shader
{
	GLuint program;
};

geometry makeGeometry(vertex * verts, size_t vertCount, unsigned * indices, size_t indexCount);

void freeGeometry(geometry &geo);

shader makeShader(const char * vertSource, const char * fragSource);

void freeShader(shader &shad);

void draw(const shader &shad, const geometry &geo);