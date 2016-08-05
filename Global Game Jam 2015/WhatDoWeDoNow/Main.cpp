#include <GL\glew.h>
#include <GLFW\glfw3.h>
#include <glm\glm.hpp>
#include <glm\gtc\type_ptr.hpp>
#include "Primitive.h"
#include "Game.h"
#include "BombGame.h"
#include "PlatformerGame.h"
#include "ImageLoader.h"

using namespace std;

int main() {

	bool glfwSuccess = glfwInit();
	if (!glfwSuccess) {
		return 1;
	}

	// Create Window
	GLFWmonitor* monitor = glfwGetPrimaryMonitor();
	const GLFWvidmode* videoMode = glfwGetVideoMode(monitor);

	//GLFWwindow* window = glfwCreateWindow(videoMode->width, videoMode->height, "Sphere", NULL, NULL);
	GLFWwindow* window = glfwCreateWindow(800, 600, "Collision Detection", NULL, NULL);

	if (!window) {
		glfwTerminate();
	}

	glfwMakeContextCurrent(window);
	glewInit();

	glPointSize(10.0);
	glEnable(GL_DEPTH_TEST);
	glDepthFunc(GL_LESS);
	glLineWidth(4);

	vector<Game*> games;

	BombGame* bombGame = new BombGame();
	games.push_back(bombGame);

	int totalBallLevels = 3;

	for (int i = 0; i < totalBallLevels; i++)
	{
		PlatformerGame *platformerGame = new PlatformerGame();
		games.push_back(platformerGame);
	}

	int LvlSelect = 0;

	Game* currentGame;
	GLuint newGame = 0;
	do{
		currentGame = games[newGame];
		currentGame->initialize(window, LvlSelect);
		currentGame->willBeginRender(window);
		newGame = currentGame->render(window);
		currentGame->didEndRender(window);
		if (newGame == 2)
		{
			LvlSelect++;
			LvlSelect = LvlSelect % totalBallLevels;
		}
		if (newGame == 0)
			LvlSelect = 0;
	} while (newGame != -1);

	return 1;
}

