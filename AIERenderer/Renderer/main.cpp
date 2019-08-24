#include <iostream>
#include <fstream>
#include <string>
#include <cassert>

#include "context.h"
#include "render.h"
#include "glm/glm.hpp"
#include "glm/ext.hpp"

int main()
{
	context game;
	game.init(640, 480, "Thonk");

	vertex triVerts[] =
	{
		{ { -.5f, -.5f, 0, 1 }, {0.f,0.f}  },
		{ {  .5f, -.5f, 0, 1 }, {1.f, 0.f} },
		{ {   0,   .5f, 0, 1 }, {.5f, 1.f} }
	};

	unsigned int triIndices[] = { 0, 1, 2 };

	#pragma region Load shaders from file
		std::ifstream ifs("shaders/FragmentShader.shader");
		std::string content1((std::istreambuf_iterator<char>(ifs)),
			(std::istreambuf_iterator<char>()));
		const char * basicFrag = content1.c_str();

		std::ifstream iffs("shaders/VertexShader.shader");
		std::string content2((std::istreambuf_iterator<char>(iffs)),
			(std::istreambuf_iterator<char>()));
		const char * basicVert = content2.c_str();
	#pragma endregion

	geometry triangle = makeGeometry(triVerts, 3, triIndices, 3);
	shader basicShad = makeShader(basicVert, basicFrag);
	//int vertexColorLocation = glGetUniformLocation(basicShad.program, "modColor");

	glm::mat4 triModel = glm::identity<glm::mat4>();
	glm::mat4 camProj = glm::perspective(glm::radians(50.f), 640.f / 480.f, 0.1f, 100.0f);

	texture triTex = loadTexture("tex.tga");

	while (!game.shouldClose())
	{
		game.tick();
		game.clear();

		float timeValue = game.getTime();
		glm::mat4 camView = glm::lookAt(glm::vec3(0, 0, -10), glm::vec3(sin(timeValue * 3), cos(timeValue * 3), 0), glm::vec3(0, 1, 0));

		triModel = glm::rotate(triModel, glm::radians(15.f), glm::vec3(0, 1, 0));
		setUniform(basicShad, 0, camProj);
		setUniform(basicShad, 1, camView);
		setUniform(basicShad, 2, triModel);
		setUniform(basicShad, 3, triTex, 0);
		
		/*float timeValue = game.getTime();
		float redValue = sin(timeValue * 3) / 2.0f + 0.5f;
		float greenValue = sin(timeValue) / 1 + 0.5f;
		float blueValue = sin(timeValue * 4) / 4.0f + 0.5f;
		glUniform4f(vertexColorLocation, redValue, greenValue, blueValue, 1.0f);*/

		draw(basicShad, triangle);

		//assert(glGetError() == GL_NO_ERROR);
	}

	game.term();
	return 0;
}