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
	game.init(1600, 900, "Computer Graphics Assessment");

	/*vertex triVerts[] =
	{
		{ { -.5f, -.5f, 0, 1 }, {0,0,1,0}, {0.f,0.f}  },
		{ {  .5f, -.5f, 0, 1 }, {0,0,1,0}, {1.f, 0.f} },
		{ {   0,   .5f, 0, 1 }, {0,0,1,0}, {.5f, 1.f} }
	};*/

	unsigned int triIndices[] = { 0, 1, 2 };

	#pragma region Load shaders from file
		std::ifstream ifs("resources/shaders/FragmentShader.shader");
		std::string content1((std::istreambuf_iterator<char>(ifs)),
			(std::istreambuf_iterator<char>()));
		const char * basicFrag = content1.c_str();

		std::ifstream iffs("resources/shaders/VertexShader.shader");
		std::string content2((std::istreambuf_iterator<char>(iffs)),
			(std::istreambuf_iterator<char>()));
		const char * basicVert = content2.c_str();
	#pragma endregion

	//geometry triangle = makeGeometry(triVerts, 3, triIndices, 3);

	geometry spear = loadModel("resources/Robot.obj");

	shader basicShad = makeShader(basicVert, basicFrag);
	//int vertexColorLocation = glGetUniformLocation(basicShad.program, "modColor");

	glm::mat4 triModel = glm::identity<glm::mat4>();
	glm::mat4 camProj = glm::perspective(glm::radians(50.f), 1600.f / 900.f, 0.1f, 100.0f);

	texture triTex = loadTexture("resources/Robot.PNG");
	texture triEmissive = loadTexture("resources/RobotEmissive.PNG");
	texture triNormalMap = loadTexture("resources/RobotNormals.PNG");
	texture triSpecular = loadTexture("resources/RobotSpecular.PNG");

	light sun;
	sun.direction = glm::vec4{ -1, 0, 0, 1 };

	while (!game.shouldClose())
	{
		game.tick();
		game.clear();

		float timeValue = game.getTime();
		//glm::mat4 camView = glm::lookAt(glm::vec3(0, 0, -10), glm::vec3(sin(timeValue * 3), cos(timeValue * 3), 0), glm::vec3(0, 1, 0));
		glm::mat4 camView = glm::lookAt(glm::vec3(0, 0, -10), glm::vec3(0, 0, 0), glm::vec3(0, 1, 0));

		triModel = glm::rotate(triModel, glm::radians(1.f), glm::vec3(0, 1, 0));
		setUniform(basicShad, 0, camProj);
		setUniform(basicShad, 1, camView);
		setUniform(basicShad, 2, triModel);
		setUniform(basicShad, 3, triTex, 0);
		setUniform(basicShad, 4, sun.direction);
		setUniform(basicShad, 5, triEmissive, 1);
		setUniform(basicShad, 6, triNormalMap, 2);
		setUniform(basicShad, 7, triSpecular, 3);
		
		/*float timeValue = game.getTime();
		float redValue = sin(timeValue * 3) / 2.0f + 0.5f;
		float greenValue = sin(timeValue) / 1 + 0.5f;
		float blueValue = sin(timeValue * 4) / 4.0f + 0.5f;
		glUniform4f(vertexColorLocation, redValue, greenValue, blueValue, 1.0f);*/

		draw(basicShad, spear);

		//assert(glGetError() == GL_NO_ERROR);
	}

	game.term();
	return 0;
}