#pragma once

#include <DirectXMath.h>

struct Vertex
{
	DirectX::XMFLOAT3 Position;	    // The position of the vertex
	DirectX::XMFLOAT3 Normal;		// Normal of the vertex
	DirectX::XMFLOAT2 Uv;			// UV coords of the vertex
};