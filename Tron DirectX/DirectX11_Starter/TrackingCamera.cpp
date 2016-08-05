#include "TrackingCamera.h"



TrackingCamera::TrackingCamera(XMFLOAT3 pos, XMFLOAT3 fw, GameObject* targ, XMFLOAT3 u, float ts, float ar) : Camera(ar)
{
	position = pos;
	forward = fw;
	target = targ;
	trackStrength = ts;
	up = u;

	this->CreateProjMatrix(ar);
}


TrackingCamera::~TrackingCamera()
{
}

void TrackingCamera::SetPosition(XMFLOAT3 p)
{
	position = p;
}

void TrackingCamera::SetForward(XMFLOAT3 f)
{
	forward = f;
}

void TrackingCamera::BindNewTarget(GameObject * t)
{
	target = t;
}

void TrackingCamera::SetTrackingStrength(float ts)
{
	trackStrength = ts;
}

void TrackingCamera::Update(float deltaTime, float totalTime)
{
	XMVECTOR vPosition = XMLoadFloat3(&position);
	XMVECTOR vForward = XMLoadFloat3(&forward);
	XMVECTOR vTarget = XMLoadFloat3(&(target->GetTranslation()));
	XMVECTOR vUp = XMLoadFloat3(&up);

	//get vector to object from camera
	XMVECTOR vToObject = vTarget - vPosition;

	//get vector projection and rejection of toObject onto forward
	XMVECTOR vProjection = (XMVector3Dot(vToObject, vForward) / XMVector3Dot(vForward, vForward)) * vForward;
	XMVECTOR vRejection = vToObject - vProjection;

	//multiply rejection by trackStrength
	vRejection = vRejection * trackStrength;

	//add projection and multiplied rejection
	XMVECTOR vDirection = vProjection + vRejection;

	//use new vector as look to
	XMMATRIX mViewMat = XMMatrixLookToLH(vPosition, vDirection, vUp);

	XMStoreFloat4x4(&viewMat, XMMatrixTranspose(mViewMat));
}