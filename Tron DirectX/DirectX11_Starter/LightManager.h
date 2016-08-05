#pragma once
#include <sstream>
#include <DirectXMath.h>
#include "DirectionalLight.h"
#include "PointLight.h"
#include "SimpleShader.h"

#define NUM_DIRECTIONAL_LIGHTS 8
#define NUM_POINT_LIGHTS 8

class LightManager
{
private:
	DirectionalLight* directionals;
	PointLight* points;
	XMFLOAT4 ambient;

	float lastUpdated;

public:
	LightManager(float totalTime);

	void SetAmbientLight(XMFLOAT4* ambientLight);

	void SetDirectionalLight(int index, DirectionalLight* light);
	void UnsetDirectionalLight(int index);

	void SetPointLight(int index, PointLight* light);
	void UnsetPointLight(int index);

	void UpdateLights(float totalTime);
	float GetLastUpdated();

	void SetLightBufferData(SimplePixelShader* shader);

	~LightManager();
};

