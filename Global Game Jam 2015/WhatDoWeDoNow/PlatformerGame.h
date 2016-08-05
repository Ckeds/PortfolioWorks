#pragma once
#include "Game.h"
class PlatformerGame :
	public Game
{
public:

	vector<Primitive*>		platforms;
	vector<vector<glm::vec3>>	sceneInterpolationVectors;
	vector<glm::vec3>			ballInterpolationVectors;
	vector<float>			platformRotations;
	Primitive*				ball;
	glm::mat4				projection;
	glm::vec3				cameraPosition;
	glm::vec3				cameraDirection;
	glm::vec3				sceneOffset;

	double					deltaTime;
	double					previousTimeStamp;
	int						activePlatformIndex;
	int						previousPlatformIndex;

	GLuint					programID;
	GLuint					transformID;
	GLuint					colorID;
	GLuint                  setGame;

	bool					animating;
	bool					StopRendering;

	PlatformerGame();
	~PlatformerGame();

	Primitive* createPlatformWithAttributes(glm::vec3 position, glm::vec3 scale, glm::vec4 color);
	float LERP(float v0, float v1, float t);
	vector<glm::vec3> createInterpolationVector(glm::vec3 start, glm::vec3 end, double duration);


	GLuint defaultShaderProgram();
	void initialize(GLFWwindow* window, int LvlNumber);
	void willBeginRender(GLFWwindow *window);
	GLuint render(GLFWwindow *window);
    void didEndRender(GLFWwindow *window);
	void handleWindowEvents(GLFWwindow *window);

	void detectCollisions();
	void updateCollision(Primitive *platform);
};

