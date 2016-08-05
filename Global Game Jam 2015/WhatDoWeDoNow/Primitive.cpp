#define _USE_MATH_DEFINES 
#include <math.h>
#include "Primitive.h"


Primitive::Primitive()
{
	worldTransform = glm::mat4(1.0f);
	localTransform = glm::mat4(1.0f);
	position = glm::vec3(0.0f);
	rotation = glm::mat4(1.0f);
	velocity = glm::vec3(0.0f);
	acceleration = glm::vec3(0.0f);
	scale = glm::vec3(1.0f);
	color = glm::vec4(0.0f);
	vertexArrayID = 0;
	vertexBufferID = 0;
	vertexCutArrayID = 0;
	vertexCutBufferID = 0;
	isCut = false;

	/*
	//Texture Variables
	FrameBufferID = 0;
	RenderedTextureID = 0;
	ImageAsTexture = nullptr;
	*/
	polygonMode = GL_FILL;
	renderMode = GL_POINTS;
}


Primitive::~Primitive()
{

}

glm::vec3 Primitive::positionAfterSeconds(double seconds)
{
	return position + (velocity * (float)seconds);
}

glm::vec3 Primitive::velocityAfterSeconds(double seconds)
{
	return velocity + acceleration * (float)seconds;
}

glm::mat4 Primitive::getWorldTransform()
{
	glm::mat4 translation = glm::translate(localTransform, position);
	glm::mat4 scalar = glm::scale(localTransform, scale);

	return translation * rotation * scalar;
}

void Primitive::findOrCreateShaderBinding(GLuint shaderID)
{
	glGenBuffers(1, &vertexBufferID);
	glBindBuffer(GL_ARRAY_BUFFER, vertexBufferID); // OpenGL.GL_Array_Buffer = buffer with ID(vertexBufferID)
	glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec3) * vertices.size(), &vertices[0], GL_STATIC_DRAW);

	glGenVertexArrays(1, &vertexArrayID);
	glBindVertexArray(vertexArrayID);

	GLuint attributeID = glGetAttribLocation(shaderID, "vp");
	glVertexAttribPointer(attributeID, 3, GL_FLOAT, GL_FALSE, 0, NULL);
	glEnableVertexAttribArray(attributeID);
}

void Primitive::findOrCreateCutBinding(GLuint shaderID)
{
	glGenBuffers(1, &vertexCutBufferID);
	glBindBuffer(GL_ARRAY_BUFFER, vertexCutBufferID); // OpenGL.GL_Array_Buffer = buffer with ID(vertexBufferID)
	glBufferData(GL_ARRAY_BUFFER, sizeof(glm::vec3), &vertices[2], GL_STATIC_DRAW);

	glGenVertexArrays(1, &vertexCutArrayID);
	glBindVertexArray(vertexCutArrayID);

	GLuint attributeID = glGetAttribLocation(shaderID, "vp");
	glVertexAttribPointer(attributeID, 3, GL_FLOAT, GL_FALSE, 0, NULL);
	glEnableVertexAttribArray(attributeID);
}



void Primitive::CreateTexturedPrimitive(GLuint shaderID, char* ImagePath, int ImageWidth, int ImageHeight)
{
	// Generates the Frame Buffer:
	glGenFramebuffers(1, &FrameBufferID);
	glBindFramebuffer(GL_FRAMEBUFFER, FrameBufferID);

	// Load the Texture:
	glGenTextures(1, &RenderedTextureID);

	//Bind Current Texture Name:
	glBindTexture(GL_TEXTURE_2D, RenderedTextureID);

	// Load Image as Texture on the Current Texture Name.
	ImageAsTexture = SoilLoadImageAsTexture(ImagePath, &ImageWidth, &ImageHeight);

	// Set "renderedTexture" as our colour attachement #0
	glFramebufferTexture(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, RenderedTextureID, 0);

	// Set the list of draw buffers.
	GLenum DrawBuffers[1] = { GL_COLOR_ATTACHMENT0 };
	glDrawBuffers(1, DrawBuffers); // "1" is the size of DrawBuffers
}

void Primitive::update(double seconds)
{
	position = positionAfterSeconds(seconds);
	velocity = velocityAfterSeconds(seconds);
}

float Primitive::LERP(float v0, float v1, float t)
{
	return (1.0 - t) * v0 + t * v1;
}

void Primitive::asSphere(int stackCount, int sliceCount)
{
	vector<float> phiCoordinates = createInterpolationVector(-M_PI_2, M_PI, stackCount);
	vector<float> thetaCoordinates = createInterpolationVector(0, M_PI * 2, sliceCount * 2);
	float radius = 0.5f;

	for (int i = 0; i < phiCoordinates.size() - 1; i++)
	{
		for (int j = 0; j < thetaCoordinates.size() - 1; j++)
		{
			glm::vec3 vertex1 = glm::vec3(radius * cosf(phiCoordinates[i]) * sinf(thetaCoordinates[j]), radius * sinf(phiCoordinates[i]) * sinf(thetaCoordinates[j]), radius * cosf(thetaCoordinates[j]));
			glm::vec3 vertex2 = glm::vec3(radius * cosf(phiCoordinates[i]) * sinf(thetaCoordinates[j + 1]), radius * sinf(phiCoordinates[i]) * sinf(thetaCoordinates[j + 1]), radius * cosf(thetaCoordinates[j + 1]));
			glm::vec3 vertex3 = glm::vec3(radius * cosf(phiCoordinates[i + 1]) * sinf(thetaCoordinates[j + 1]), radius * sinf(phiCoordinates[i + 1]) * sinf(thetaCoordinates[j + 1]), radius * cosf(thetaCoordinates[j + 1]));
			glm::vec3 vertex4 = glm::vec3(radius * cosf(phiCoordinates[i + 1]) * sinf(thetaCoordinates[j]), radius * sinf(phiCoordinates[i + 1]) * sinf(thetaCoordinates[j]), radius * cosf(thetaCoordinates[j]));

			if (thetaCoordinates[j] <= M_PI)
			{
				vertices.push_back(vertex1);
				vertices.push_back(vertex2);
				vertices.push_back(vertex3);

				vertices.push_back(vertex1);
				vertices.push_back(vertex3);
				vertices.push_back(vertex4);
			}

			if (thetaCoordinates[j] >= M_PI)
			{
				vertices.push_back(vertex1);
				vertices.push_back(vertex3);
				vertices.push_back(vertex2);

				vertices.push_back(vertex1);
				vertices.push_back(vertex4);
				vertices.push_back(vertex3);
			}
		}
	}
}

vector<float> Primitive::createInterpolationVector(float start, float length, int divisions)
{
	vector<float> v;
	float step = length / divisions;

	for (int i = 0; i < divisions + 1; i++)
	{
		float value = start + (i * step);
		v.push_back(value);
	}

	return v;
}
