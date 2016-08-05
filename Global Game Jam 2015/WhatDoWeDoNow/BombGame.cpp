#include "BombGame.h"

//Load all images as textures:

// Title Images:
//Cut Green Title Image
GLFWimage CutGreen = GlfwCreateImage("Images\\CutGreen.png", 300, 150);
//Cut Green Title Image
GLFWimage CutRed = GlfwCreateImage("Images\\CutRed.png", 300, 150);
//Cut Green Title Image
GLFWimage CutBlue = GlfwCreateImage("Images\\CutBlue.png", 300, 150);

//Mouse:
//Unclicked Mouse cursor
GLFWimage UnclkMouse = GlfwCreateImage("Images\\PliersOpen.png", 64, 64);
//Clicked Mouse cursor
GLFWimage ClkMouse = GlfwCreateImage("Images\\PliersClosed.png", 64, 64);

//Bomb:
//BombClock:
GLFWimage BombClock = GlfwCreateImage("Images\\BombClock.png", 150, 200);

BombGame::BombGame()
{
}


BombGame::~BombGame()
{
}

GLuint BombGame::defaultShaderProgram()
{
	/*
	const char* vertex_shader =
		"#version 400\n"
		"uniform mat4 transform;"
		"in vec3 vp;"
		"out vec2 texturePosition;"
		"void main () {"
		"  gl_Position = vec4 (vp, 1.0);"
		"  texturePosition = vp.zw;"
		"}";

	const char* fragment_shader =
		"#version 400\n"
		"uniform sampler2D texture;"
		"uniform vec4 color;"
		"in vec2 texturePosition;"
		"out vec4 frag_colour;"
		"void main () {"
		"  frag_colour = color;"
		"}";*/
		
	
	//Original shadder:
	const char* vertex_shader =
		"#version 400\n"
		"uniform mat4 transform;"
		"in vec3 vp;"
		"void main () {"
		"  gl_Position = vec4 (vp, 1.0);"
		"}";

	const char* fragment_shader =
		"#version 400\n"
		"uniform vec4 color;"
		"out vec4 frag_colour;"
		"void main () {"
		"  frag_colour = color;"
		"}";


	GLuint vShaderID = glCreateShader(GL_VERTEX_SHADER);
	glShaderSource(vShaderID, 1, &vertex_shader, NULL);
	glCompileShader(vShaderID);

	GLuint fShaderID = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fShaderID, 1, &fragment_shader, NULL);
	glCompileShader(fShaderID);

	GLuint programID = glCreateProgram();
	glAttachShader(programID, vShaderID);
	glAttachShader(programID, fShaderID);
	glLinkProgram(programID);

	return programID;
}

void BombGame::initialize(GLFWwindow* window, int lvl)
{
	setGame = -1;
	// Load Shader
	programID = defaultShaderProgram();
	glUseProgram(programID);
	colorAttributeID = glGetUniformLocation(programID, "color");
	//textureAttributeID = glGetUniformLocation(programID, "texture");

	// Init Data
	rectangle = new Primitive();
	rectangle->vertices.push_back(glm::vec3(0.5, 0.25, -1));
	rectangle->vertices.push_back(glm::vec3(0.5, -0.25, -1));
	rectangle->vertices.push_back(glm::vec3(-0.5, -0.25, -1));
	rectangle->vertices.push_back(glm::vec3(-0.5, 0.25, -1));

	rectangle->findOrCreateShaderBinding(programID);
	//rectangle->CreateTexturedPrimitive(textureAttributeID, "Images\\BombClock.png", 300, 150);

	Primitive *redWire = new Primitive();
	redWire->vertices.push_back(glm::vec3(-1, 0, 0));
	redWire->vertices.push_back(glm::vec3(-0.5, 0, 0));
	redWire->color = glm::vec4(1, 0, 0, 1);
	redWire->findOrCreateShaderBinding(programID);
	wires.push_back(redWire);

	Primitive *blueWire = new Primitive();
	blueWire->vertices.push_back(glm::vec3(0, -0.25, 0));
	blueWire->vertices.push_back(glm::vec3(0, -1, 0));
	blueWire->color = glm::vec4(0, 0, 1, 1);
	blueWire->findOrCreateShaderBinding(programID);
	wires.push_back(blueWire);

	Primitive *greenWire = new Primitive();
	greenWire->vertices.push_back(glm::vec3(0.5, 0, 0));
	greenWire->vertices.push_back(glm::vec3(1, 0, 0));
	greenWire->color = glm::vec4(0, 1, 0, 1);
	greenWire->findOrCreateShaderBinding(programID);
	wires.push_back(greenWire);

	newCursor = false;
	cursor = glfwCreateCursor(&UnclkMouse, 26, 14);
	glfwSetCursor(window, cursor);
}

void BombGame::willBeginRender(GLFWwindow* window)
{
}

GLuint BombGame::render(GLFWwindow* window)
{
	while (!glfwWindowShouldClose(window) && setGame == -1) {
		//get mouse position
		glfwGetCursorPos(window, &xpos, &ypos);
		xpos = (xpos / 400) - 1;
		ypos = -(ypos / 300) + 1;
		//printf("%f, %f \n", xpos, ypos);

		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		glClearColor(1, 1, 1, 1);
		glBindVertexArray(rectangle->vertexArrayID);
		glUniform4fv(colorAttributeID, 1, glm::value_ptr(rectangle->color));
		glUniform2d(textureAttributeID, rectangle->vertices[3].x, rectangle->vertices[3].y);
		glDrawArrays(GL_LINE_LOOP, 0, rectangle->vertices.size());

		for (Primitive* primtive : wires) {
			glBindVertexArray(primtive->vertexArrayID);
			glUniform4fv(colorAttributeID, 1, glm::value_ptr(primtive->color));
			glDrawArrays(GL_LINE_LOOP, 0, 2);
			if (!primtive->isCut)  {
				collision(primtive);
			}
			else
			{
				if (primtive->vertexCutArrayID < 1)
					primtive->findOrCreateCutBinding(programID);
				glBindVertexArray(primtive->vertexCutArrayID);
				glUniform4fv(colorAttributeID, 1, glm::value_ptr(glm::vec4(1.0f)));
				glDrawArrays(GL_POINTS, 0, 1);
			}
		}
		handleWindowEvents(window);
		glfwSwapBuffers(window);
	}
	return setGame;
}

void BombGame::didEndRender(GLFWwindow* window)
{
	while (!wires.empty())
	{
		wires.pop_back();
	}
	delete(rectangle);
	glfwDestroyCursor(cursor);
}


void BombGame::handleWindowEvents(GLFWwindow* window)
{
	glfwPollEvents();
	if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_ESCAPE)) {
		glfwSetWindowShouldClose(window, 1);
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_1)) {
		setGame = 0;
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_2)) {
		setGame = 1;
	}
	if (GLFW_PRESS == glfwGetMouseButton(window, GLFW_MOUSE_BUTTON_LEFT))
	{
		if (!newCursor)
		{
			glfwDestroyCursor(cursor);
			cursor = glfwCreateCursor(&ClkMouse, 26, 14);
			newCursor = true;
			printf("CLOSE");
			glfwSetCursor(window, cursor);
		}
	}
	if (GLFW_RELEASE == glfwGetMouseButton(window, GLFW_MOUSE_BUTTON_LEFT))
	{
		if (newCursor)
		{
			glfwDestroyCursor(cursor);
			cursor = glfwCreateCursor(&UnclkMouse, 26, 14);
			newCursor = false;
			printf("OPEN");
			glfwSetCursor(window, cursor);
		}
	}

	// Add more events
}

void BombGame::collision(Primitive* w)
{
	//set min max variables
	GLfloat minX = 0;
	GLfloat maxX = 0;
	GLfloat minY = 0;
	GLfloat maxY = 0;
	//get line width
	GLint value;
	glGetIntegerv(GL_LINE_WIDTH, &value);

	//set epsilon to line width
	GLfloat epsilonX = value / 400.0;
	GLfloat epsilonY = value / 300.0;

	//populate
	for (int i = 0; i < w->vertices.capacity(); i++)
	{
		//first point
		if (i == 0)
		{
			minX = w->vertices[i].x;
			maxX = w->vertices[i].x;
			minY = w->vertices[i].y;
			maxY = w->vertices[i].y;
		}
		//other points
		else
		{
			if (minX > w->vertices[i].x)
			{
				minX = w->vertices[i].x;
			}
			if (maxX < w->vertices[i].x)
			{
				maxX = w->vertices[i].x;
			}
			if (minY > w->vertices[i].y)
			{
				minY = w->vertices[i].y;
			}
			if (maxY < w->vertices[i].y)
			{
				maxY = w->vertices[i].y;
			}
		}
	}
	//add in epsilon
	minX -= epsilonX;
	maxX += epsilonX;
	minY -= epsilonY;
	maxY += epsilonY;
	//check vs mouse position
	if (xpos >= minX && xpos <= maxX)
	{
		if (ypos >= minY && ypos <= maxY)
		{
			if (newCursor)
			{
				printf("hit! \n");
				glClearColor(w->color.r, w->color.g, w->color.b, w->color.a);
				w->isCut = true;
				if (w->vertices[0].y < 0)
				{
					w->vertices.push_back(glm::vec3(0, ypos, -1));
				}
				else
				{
					w->vertices.push_back(glm::vec3(xpos, 0, -1));
				}
			}
		}
	}
}