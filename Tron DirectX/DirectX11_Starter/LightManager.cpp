#include "LightManager.h"


/// <summary>
/// Default constructor. 
/// </summary>
/// <param name="totalTime">current totalTime for update timing</param>
LightManager::LightManager(float totalTime)
{
	directionals = new DirectionalLight[NUM_DIRECTIONAL_LIGHTS];
	memset(directionals, 0, sizeof(DirectionalLight)*NUM_DIRECTIONAL_LIGHTS);
	points = new PointLight[NUM_POINT_LIGHTS];
	memset(points, 0, sizeof(PointLight)*NUM_POINT_LIGHTS);
	ambient = XMFLOAT4(0, 0, 0, 0);
	lastUpdated = totalTime;
}

/// <summary>
/// Set the color of the world's ambient light.
/// </summary>
/// <param name="ambientLight">the color of the world's ambient light</param>
void LightManager::SetAmbientLight(XMFLOAT4* ambientLight)
{
	ambient = *ambientLight;
}

/// <summary>
/// Set the properties of a directional light in the world.
/// </summary>
/// <param name="index">an index for this light below NUM_DIRECTIONAL_LIGHTS</param>
/// <param name="light">the properties of the directional light</param>
void LightManager::SetDirectionalLight(int index, DirectionalLight * light)
{
	directionals[index] = *light;
}

/// <summary>
/// Remove a directional light from the world.  This essentially sets all of the light's
/// properties to zero.
/// </summary>
/// <param name="index">the index of the light</param>
void LightManager::UnsetDirectionalLight(int index)
{
	directionals[index] = { {0, 0, 0, 0}, {0, 0, 0} };
}

/// <summary>
/// Set the properties of a point light in the world.
/// </summary>
/// <param name="index">an index for this light below NUM_POINT_LIGHTS</param>
/// <param name="light">the properties of the point light</param>
void LightManager::SetPointLight(int index, PointLight * light)
{
	points[index] = *light;
}

/// <summary>
/// Remove a point light from the world.  This essentially sets all the lights' properties
/// to zero.
/// </summary>
/// <param name="index">the index of the light</param>
void LightManager::UnsetPointLight(int index)
{
	points[index] = { {0, 0, 0, 0}, {0, 0, 0} };
}

/// <summary>
/// Signal the LightManager to update the lights in the shaders.
/// </summary>
/// <param name="totalTime">the timestamp for this update</param>
void LightManager::UpdateLights(float totalTime)
{
	lastUpdated = totalTime;
}

/// <summary>
/// Get the timestamp from the last time this LightManager's lights were
/// changed.
/// </summary>
/// <returns>the timestamp</returns>
float LightManager::GetLastUpdated()
{
	return lastUpdated;
}

/// <summary>
/// Sends all of the light information to the given shader.
/// 
/// For efficiency, only call this method if the shader hasn't been updated since
/// the return value of LightManager::GetLastUpdated.
/// </summary>
/// <param name="shader">the shader to set the values in</param>
void LightManager::SetLightBufferData(SimplePixelShader * shader)
{
	std::stringstream ss;
	shader->SetFloat4("ambientColor", ambient);

	for (int i = 0; i < NUM_DIRECTIONAL_LIGHTS; i++)
	{
		ss << "dLight" << i;
		shader->SetData(ss.str(), &(directionals[i]), sizeof(DirectionalLight));
		ss.str("");
	}

	for (int i = 0; i < NUM_POINT_LIGHTS; i++)
	{
		ss << "pLight" << i;
		shader->SetData(ss.str(), &(points[i]), sizeof(PointLight));
		ss.str("");
	}
	shader->CopyBufferData("lightBuffer");
}

/// <summary>
/// Standard destructor.
/// </summary>
LightManager::~LightManager()
{
	delete[] directionals;
	delete[] points;
}
