#pragma once
#include <GL\glew.h>
#include <GLFW\glfw3.h>
#include <glm\glm.hpp>
#include <glm\gtc\type_ptr.hpp>
#include "Primitive.h"

class Game
{
public:
	Game();
	~Game();

	virtual GLuint defaultShaderProgram();
	virtual void initialize(GLFWwindow *window, int LvlNumber);
	virtual void willBeginRender(GLFWwindow *window);
	virtual GLuint render(GLFWwindow *window);
	virtual void didEndRender(GLFWwindow *window);
	virtual void handleWindowEvents(GLFWwindow *window);

};

