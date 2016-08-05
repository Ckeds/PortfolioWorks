#include "DebugCamera.h"

#include "Input.h"

using Input::KeyIsDown;
using Input::Keys;


/// <summary>
/// Standard constructor.
/// </summary>
/// <param name="p">the starting position of the camera</param>
/// <param name="d">the initial facing direction of the camera</param>
/// <param name="ar">the aspect ratio of the window</param>
DebugCamera::DebugCamera(XMFLOAT3 p, XMFLOAT3 d, float ar) : Camera(ar)
{
	position = p;
	direction = d;

	rotX = 0;
	rotY = 0; 

	viewMatIsDirty = true;
}

/// <summary>
/// Gets the current location of the camera.
/// </summary>
/// <returns>the location of the camera in world space</returns>
XMFLOAT3 DebugCamera::GetPosition()
{
	return position;
}

/// <summary>
/// Gets the initial direction that the camera was facing.
/// 
/// This currently doesn't make much sense, and you probably 
/// don't need this value anyway, but if you want the direction
/// that the camera is currently facing, use this result in conjunction
/// with the results from DebugCamera::GetXRotation and DebugCamera::GetYRotation.
/// </summary>
/// <returns>the reference direction from which rotations are caluclated</returns>
XMFLOAT3 DebugCamera::GetDirection()
{
	return direction;
	//TODO:  Make direction work more intelligently
}

/// <summary>
/// Gets the pitch of the camera in radians.
/// </summary>
/// <returns>the pitch of the camera</returns>
float DebugCamera::GetXRotation()
{
	return rotX;
}

/// <summary>
/// Gets the yaw of the camera in radians.
/// </summary>
/// <returns>the yaw of the camera</returns>
float DebugCamera::GetYRotation()
{
	return rotY;
}


/// <summary>
/// Set the current position of the camera.
/// </summary>
/// <param name="p">the new position of the camera</param>
void DebugCamera::SetPosition(XMFLOAT3 p)
{
	position = p;
	viewMatIsDirty = true;
}

/// <summary>
/// Sets a new "basis direction" for the camera to face and sets X and Y
/// rotation to 0.  You PROBABLY don't want to be using this function if
/// you're not sure what is meant by "basis direction"; a more rational
/// implementation of camera direction is forthcoming.
/// </summary>
/// <param name="d">the new "basis direction" of the camera</param>
void DebugCamera::SetDirection(XMFLOAT3 d)
{
	direction = d;
	viewMatIsDirty = true;
}

/// <summary>
/// Sets the rotation of the camera around the X axis.  Also known as 'pitch'.
/// </summary>
/// <param name="x">the pitch of the camera in radians</param>
void DebugCamera::SetRotationX(float x)
{
	rotX = x;
	viewMatIsDirty = true;
}

/// <summary>
/// Sets the rotation of the camera around the Y axis.  Also known as 'yaw'. 
/// </summary>
/// <param name="y">the yaw of the camera in radians</param>
void DebugCamera::SetRotationY(float y)
{
	rotY = y;
	viewMatIsDirty = true;
}

/// <summary>
/// Rotate the camera from its current position.
/// </summary>
/// <param name="x">amount to rotate around the X axis, in radians (pitch)</param>
/// <param name="y">amount to rotate around the Y axis, in radians (yaw)</param>
void DebugCamera::Rotate(float x, float y)
{
	rotX += x;
	rotY += y;

	viewMatIsDirty = true;
}


/// <summary>
/// Updates the view matrix, if it needs to be updated.
/// </summary>
/// <param name="deltaTime">time the last frame took, in seconds</param>
void DebugCamera::Update(float deltaTime, float totalTime)
{
	XMVECTOR vPosition = XMLoadFloat3(&position);
	XMVECTOR vDirection = XMLoadFloat3(&direction);
	XMMATRIX mRotation = XMMatrixRotationRollPitchYaw(rotY, rotX, 0);
	XMVECTOR vFinalDirection = XMVector3Transform(vDirection, mRotation);
	XMVECTOR vUp = XMLoadFloat3(&XMFLOAT3(0, 1, 0));

	if (KeyIsDown(Keys::W))
	{
		XMVECTOR vNeedToAdd = XMVector3Normalize(vFinalDirection) * deltaTime;
		vPosition += vNeedToAdd;
		
		viewMatIsDirty = true;
	}
	if (KeyIsDown(Keys::S))
	{
		XMVECTOR vNeedToAdd = XMVector3Normalize(vFinalDirection) * deltaTime;
		vPosition -= vNeedToAdd;

		viewMatIsDirty = true;
	}
	if (KeyIsDown(Keys::A))
	{
		XMVECTOR vNeedToAdd = XMVector3Cross(vFinalDirection, vUp);
		vNeedToAdd = XMVector3Normalize(vNeedToAdd) * deltaTime;
		vPosition += vNeedToAdd;

		viewMatIsDirty = true;
	}
	if (KeyIsDown(Keys::D))
	{
		XMVECTOR vNeedToAdd = XMVector3Cross(vFinalDirection, vUp);
		vNeedToAdd = XMVector3Normalize(vNeedToAdd) * deltaTime;
		vPosition -= vNeedToAdd;

		viewMatIsDirty = true;
	}
	if (KeyIsDown(Keys::Space))
	{
		XMVECTOR vNeedToAdd = XMVector3Normalize(vUp) * deltaTime;
		vPosition += vNeedToAdd;

		viewMatIsDirty = true;
	}
	if (KeyIsDown(Keys::X))
	{
		XMVECTOR vNeedToAdd = XMVector3Normalize(vUp) * deltaTime;
		vPosition -= vNeedToAdd;

		viewMatIsDirty = true;
	}

	XMStoreFloat3(&position, vPosition);

	if (Input::GetMouseMode() == Input::MouseMode::MODE_RELATIVE) {
		auto x = Input::GetMouseX();
		auto y = Input::GetMouseY();
		Rotate(float(x) / 1200.0f, float(y) / 1200.0f);
	}

	if (viewMatIsDirty)
	{

		XMMATRIX vViewMat = XMMatrixLookToLH(
			vPosition,
			vFinalDirection, 
			vUp);

		XMStoreFloat4x4(&viewMat, XMMatrixTranspose(vViewMat));

		viewMatIsDirty = false;
	}
}

/// <summary>
/// Empty destructor.
/// </summary>
DebugCamera::~DebugCamera()
{
}