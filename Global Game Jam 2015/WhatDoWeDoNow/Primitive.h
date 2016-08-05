#pragma once
#include <vector>
#include <glm\glm.hpp>
#include "GL\glew.h"
#include "ImageLoader.h"
#include <glm\gtc\matrix_transform.hpp>

using namespace std;

class Primitive
{
public:
	// Geometry
	vector<glm::vec3>		vertices;
	vector<glm::vec3>		interpolationVector;
	glm::vec4				color;
	bool                    isCut;

	glm::mat4				worldTransform;
	glm::mat4				localTransform;
	glm::mat4				rotation;
	glm::vec3				position;
	glm::vec3				scale;

	// Physics
	glm::vec3				acceleration;
	glm::vec3				velocity;

	// OpenGL Identifiers
	GLuint					vertexArrayID;
	GLuint					vertexBufferID;
	GLuint                  vertexCutArrayID;
	GLuint                  vertexCutBufferID;

	
	//Texture Variables
	GLuint					FrameBufferID;
	GLuint					RenderedTextureID;
	unsigned char*			ImageAsTexture;
	
	GLenum					polygonMode;
	GLenum					renderMode;

	Primitive();
	~Primitive();

	void asSphere(int stackCount, int sliceCount);
	vector<float> createInterpolationVector(float start, float length, int divisions);

	glm::vec3 velocityAfterSeconds(double seconds);
	glm::vec3 positionAfterSeconds(double seconds);
	glm::mat4 getWorldTransform();
	float LERP(float v0, float v1, float t);

	void Primitive::findOrCreateShaderBinding(GLuint shaderID);
	void Primitive::findOrCreateCutBinding(GLuint shaderID);
	void Primitive::CreateTexturedPrimitive(GLuint shaderID, char* ImagePath, int ImageWidth, int ImageHeight);
	void update(double seconds);
};

