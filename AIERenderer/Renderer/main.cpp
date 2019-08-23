#include <iostream>
#include <fstream>
#include <string>
#include <cassert>

#include "context.h"
#include "render.h"

int main()
{
	context game;
	game.init(640, 480, "Thonk");

	vertex triVerts[] =
	{
		{ { -.5f, -.5f, 0, 1 } },
		{ {  .5f, -.5f, 0, 1 } },
		{ {   0,   .5f, 0, 1 } }
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
	int vertexColorLocation = glGetUniformLocation(basicShad.program, "vertColor");

	while (!game.shouldClose())
	{
		game.tick();
		game.clear();

		float timeValue = game.getTime();
		float redValue = sin(timeValue * 3) / 2.0f + 0.5f;
		float greenValue = sin(timeValue) / 1 + 0.5f;
		float blueValue = sin(timeValue * 4) / 4.0f + 0.5f;
		glUniform4f(vertexColorLocation, redValue, greenValue, blueValue, 1.0f);

		draw(basicShad, triangle);

		//assert(glGetError() == GL_NO_ERROR);
	}

	game.term();
	return 0;
}