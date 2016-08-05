#include "PlatformerGame.h"
#include <iostream>
#include <glm\gtc\matrix_transform.hpp>
#include <glm\gtc\type_ptr.hpp>
#include <cmath>
#include <chrono>

using namespace std;

const float frameInterval = 0.0167; // 60FPS 
const double animationDuration = 0.1;

PlatformerGame::PlatformerGame()
{
	//projection = glm::perspective(75.0f, 4.0f / 3.0f, 0.1f, 1000.f);
	projection = glm::ortho(-10.0, 10.0, -10.0, 10.0, 0.1, 100.0);
	cameraPosition = glm::vec3(0, 0, 5.0);
	cameraDirection = glm::vec3(0, 0, -1);
	sceneOffset = glm::vec3(0.0);
	activePlatformIndex = 0;
	animating = false;
	StopRendering = false;
}


PlatformerGame::~PlatformerGame()
{

}

GLuint PlatformerGame::defaultShaderProgram()
{
	const char* vertex_shader =
		"#version 400\n"
		"uniform mat4 transform;"
		"in vec3 vp;"
		"void main () {"
		"  gl_Position = transform * vec4 (vp, 1.0);"
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

void PlatformerGame::initialize(GLFWwindow* window, int LvlNumber)
{
	setGame = -1;
	// Load Shader
	programID = defaultShaderProgram();
	glUseProgram(programID);

	transformID = glGetUniformLocation(programID, "transform");
	colorID = glGetUniformLocation(programID, "color");

	
	switch (LvlNumber)
	{
	case(0):

		createPlatformWithAttributes({ 0.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 4.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 8.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		break;

	case(1) :
		createPlatformWithAttributes({ 0.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 1.0, -2.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ -1.0, -4.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 1.0, -6.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ -1.0, -8.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });

		break;
	case(2) :
		createPlatformWithAttributes({ 0.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 2.0, -2.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 4.0, -4.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 6.0, -2.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 8.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		break;

	default:
		createPlatformWithAttributes({ 0.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 4.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
		createPlatformWithAttributes({ 8.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });

		break;
	}

//	createPlatformWithAttributes({ 0.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
//	createPlatformWithAttributes({ 4.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });
//	createPlatformWithAttributes({ 8.0, 0.0, 0.0 }, { 2.0, 0.15, 1.0 }, { 1.0, 1.0, 1.0, 1.0 });

	ball = new Primitive();
	ball->asSphere(5, 10);
	ball->color = { 0.92, 0.96, 1.0, 1.0 };
	ball->position.y = 2.0;
	ball->scale = glm::vec3(0.5);
	ball->renderMode = GL_TRIANGLES;
	ball->acceleration = glm::vec3(0, -9.8, 0);
	ball->findOrCreateShaderBinding(programID);
}

// Loads a level acording to the level number.
void loadLevel(GLFWwindow* window, int LvlNumber)
{

}

Primitive* PlatformerGame::createPlatformWithAttributes(glm::vec3 position, glm::vec3 scale, glm::vec4 color)
{
	Primitive* platform = new Primitive();
	platform->vertices.push_back({ -0.5, 0.5, 0.0 });
	platform->vertices.push_back({ -0.5, -0.5, 0.0 });
	platform->vertices.push_back({ 0.5, 0.5, 0.0 });
	platform->vertices.push_back({ 0.5, 0.5, 0.0 });
	platform->vertices.push_back({ -0.5, -0.5, 0.0 });
	platform->vertices.push_back({ 0.5, -0.5, 0.0 });
	platform->position = position;
	platform->color = color;
	platform->scale = scale;
	platform->renderMode = GL_TRIANGLES;
	platform->findOrCreateShaderBinding(programID);
	platforms.push_back(platform);
	platformRotations.push_back(0.0);
	sceneInterpolationVectors.push_back(vector<glm::vec3>());

	return platform;
}

void PlatformerGame::willBeginRender(GLFWwindow* window)
{
	previousTimeStamp = glfwGetTime();
}

GLuint PlatformerGame::render(GLFWwindow* window)
{
	double previousTimeStamp = glfwGetTime();
	while (!glfwWindowShouldClose(window) && setGame == -1) {
		double currentTimeStamp = glfwGetTime();
		deltaTime = currentTimeStamp - previousTimeStamp;
		previousTimeStamp = currentTimeStamp;
		if (deltaTime > frameInterval) {
			//continue;
		}
		

		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
		glClearColor(0.32f, 0.55f, 0.69f, 1.0f);
		glCullFace(GL_BACK);
		
		glm::mat4 view = glm::lookAt(cameraPosition, cameraDirection, glm::vec3(0, 1, 0));


		detectCollisions();

		if (ballInterpolationVectors.size() > 0) {
			ball->position = ballInterpolationVectors.front();
			ballInterpolationVectors.erase(ballInterpolationVectors.begin());
		}

		ball->update(deltaTime);

		
		glm::mat4 MVP = projection * view * ball->getWorldTransform();
		glPolygonMode(GL_FRONT_AND_BACK, ball->polygonMode);
		glBindVertexArray(ball->vertexArrayID);
		glUniformMatrix4fv(transformID, 1, GL_FALSE, &MVP[0][0]);
		glUniform4fv(colorID, 1, glm::value_ptr(ball->color));
		glDrawArrays(ball->renderMode, 0, ball->vertices.size());


		for (int platformIndex = 0; platformIndex < platforms.size(); platformIndex++) {
			Primitive* platform = platforms[platformIndex];
			platform->rotation = glm::rotate(glm::mat4(1.0), platformRotations[platformIndex], glm::vec3(0.0, 0.0, 1.0));

			if (sceneInterpolationVectors[platformIndex].size() > 0) {
				platform->position = sceneInterpolationVectors[platformIndex].front();
				sceneInterpolationVectors[platformIndex].erase(sceneInterpolationVectors[platformIndex].begin());
			}

			glm::mat4 MVP = projection * view * platform->getWorldTransform();
			glPolygonMode(GL_FRONT_AND_BACK, platform->polygonMode);
			glBindVertexArray(platform->vertexArrayID);
			glUniformMatrix4fv(transformID, 1, GL_FALSE, &MVP[0][0]);
			glUniform4fv(colorID, 1, glm::value_ptr(platform->color));
			glDrawArrays(platform->renderMode, 0, platform->vertices.size());
			platforms[platformIndex]->color = { 1.0, 1.0, 1.0, 1.0 };
		}

		handleWindowEvents(window);
		glfwSwapBuffers(window);
	}
	return setGame;
}

void PlatformerGame::didEndRender(GLFWwindow* window)
{
	platforms.clear();
	platformRotations.clear();
	sceneInterpolationVectors.clear();
	ballInterpolationVectors.clear();
	delete(ball);
}
void PlatformerGame::handleWindowEvents(GLFWwindow* window)
{
	glfwWaitEvents();
	if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_ESCAPE)) {
		glfwSetWindowShouldClose(window, 1);
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_P)) {
		setGame = 2;
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_1)) {
		setGame = 0;
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_2)) {
		setGame = 1;
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_UP)) {
		platformRotations[activePlatformIndex]++;
		glfwPostEmptyEvent();
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_UP)) {
		platformRotations[activePlatformIndex]++;
		glfwPostEmptyEvent();
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_DOWN)) {
		platformRotations[activePlatformIndex]--;
		glfwPostEmptyEvent();
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_LEFT)) {
		if (activePlatformIndex > 0) {
			previousPlatformIndex = activePlatformIndex;
			activePlatformIndex--;

			sceneOffset = platforms[previousPlatformIndex]->position - platforms[activePlatformIndex]->position;
			ballInterpolationVectors = createInterpolationVector(ball->position, ball->position + sceneOffset, animationDuration);
			for (int platformIndex = 0; platformIndex < platforms.size(); platformIndex++) {
				sceneInterpolationVectors[platformIndex] = createInterpolationVector(platforms[platformIndex]->position, platforms[platformIndex]->position + sceneOffset, animationDuration);
			}
		}
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_RIGHT)) {
		if (activePlatformIndex < platforms.size()-1) {
			previousPlatformIndex = activePlatformIndex;
			activePlatformIndex++;

			sceneOffset = platforms[previousPlatformIndex]->position - platforms[activePlatformIndex]->position;
			ballInterpolationVectors = createInterpolationVector(ball->position, ball->position + sceneOffset, animationDuration);
			for (int platformIndex = 0; platformIndex < platforms.size(); platformIndex++) {
				sceneInterpolationVectors[platformIndex] = createInterpolationVector(platforms[platformIndex]->position, platforms[platformIndex]->position + sceneOffset, animationDuration);
			}
		}
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_A)) {
		for (Primitive* platform : platforms) {
			platform->position.x++;
		}
	}
	else if (GLFW_PRESS == glfwGetKey(window, GLFW_KEY_D)) {
		for (Primitive* platform : platforms) {
			platform->position.x--;
		}
	}
	else {
		glfwPostEmptyEvent();
	}
	platforms[activePlatformIndex]->color = { 0.06, 0.19, 0.25, 1.0 };

}

float PlatformerGame::LERP(float v0, float v1, float t)
{
	return (1.0f - t) * v0 + t * v1;
}


void PlatformerGame::detectCollisions()
{
	glm::vec4 ballPosition = glm::translate(glm::mat4(1.0f), ball->positionAfterSeconds(deltaTime) + sceneOffset) * ball->rotation * glm::scale(glm::mat4(1.0f), ball->scale) * glm::vec4(0, 0, 0, 1.0);
	for (Primitive* platform : platforms) {
		glm::vec4 platformMin = glm::translate(glm::mat4(1.0f), platform->position + sceneOffset) * platform->rotation * glm::scale(glm::mat4(1.0f), platform->scale) * glm::vec4(-0.25f,0.25f, 0, 1.0);
		glm::vec4 platformMax = glm::translate(glm::mat4(1.0f), platform->position + sceneOffset) * platform->rotation * glm::scale(glm::mat4(1.0f), platform->scale) * glm::vec4(0.25f, 0.25f, 0, 1.0);

		float platformLength = glm::length(glm::vec3(platformMax.x - platformMin.x, platformMax.y - platformMin.y, platformMax.z - platformMin.z));
		glm::vec3 d = glm::normalize(glm::vec3(platformMax.x - platformMin.x, platformMax.y - platformMin.y, platformMax.z - platformMin.z));
		glm::vec3 e = { ballPosition.x - platformMin.x, ballPosition.y - platformMin.y, ballPosition.z - platformMin.z };
		float a = glm::dot(e, d);

		float ballRadius = (0.5f * ball->scale.x);
		float e2 = glm::dot(e, e);
		float a2 = glm::dot(a, a);
		float r2 = ballRadius * ballRadius;
		float f = sqrtf(r2 - e2 + a2);
		float t = a - f;
		if (t < platformLength + ballRadius && t > -2 * ballRadius) {
			cout << "Ball Position:" << ballPosition.x << endl;
			cout << "Platform Length: " << platformLength << endl;
			cout << "T: " << t << endl;
			glm::vec3 platformNormal = glm::vec3(-d.y, d.x, d.z);
			float platformAngle = glm::degrees(atan2(platformNormal.y, platformNormal.x));
			float incomingAngle = glm::degrees(atan2(ball->velocity.y, ball->velocity.x));
			float outgoingAngle = 2 * platformAngle - 180 - incomingAngle;
			float magnitude = hypot(ball->velocity.x, ball->velocity.y);

			ball->velocity.x = cosf(glm::radians(outgoingAngle)) * magnitude;
			ball->velocity.y = sinf(glm::radians(outgoingAngle)) * magnitude;
			
			return;
		}

	}
}

void PlatformerGame::updateCollision(Primitive *platform)
{
}

vector<glm::vec3> PlatformerGame::createInterpolationVector(glm::vec3 start, glm::vec3 end, double duration)
{
	vector<glm::vec3> v;

	v.push_back(start);
	for (double startTime = 0; startTime < duration; startTime += deltaTime) {
		v.push_back(glm::vec3(LERP(start.x, end.x, startTime/duration), LERP(start.y, end.y, startTime/duration), 0));
	}
	v.push_back(end);

	return v;
}