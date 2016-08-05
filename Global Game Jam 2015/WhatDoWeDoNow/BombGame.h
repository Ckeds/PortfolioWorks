#pragma once
#include "Game.h"
class BombGame :
	public Game
{
public:

	vector<Primitive*>		wires;
	Primitive*				rectangle;
	GLuint					programID;
	GLuint					colorAttributeID;
	GLuint                  textureAttributeID;
	GLuint                  setGame;
	double					xpos, ypos;
	GLFWcursor*				cursor;
	bool					newCursor;

	BombGame();
	~BombGame();

	GLuint defaultShaderProgram();
	void initialize(GLFWwindow* window, int lvl);
	void willBeginRender(GLFWwindow *window);
	GLuint render(GLFWwindow *window);
	void didEndRender(GLFWwindow *window);
	void handleWindowEvents(GLFWwindow *window);
	void collision(Primitive* w);
};

