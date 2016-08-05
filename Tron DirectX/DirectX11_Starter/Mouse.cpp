#include "Input.h"

using namespace Input;

Mouse TheMouse;
MouseState TheMouseState;
MouseButtonStateTracker TheMouseButtonStateTracker;

void Input::UpdateMouse()
{
	TheMouseState = TheMouse.GetState();
	TheMouseButtonStateTracker.Update(TheMouseState);
}

void Input::SetMouseWindow(HWND hWnd)
{
	TheMouse.SetWindow(hWnd);
}

void Input::SetMouseMode(MouseMode mode)
{
	TheMouse.SetMode(mode);
}

MouseMode Input::GetMouseMode()
{
	return TheMouseState.positionMode;
}

int Input::GetMouseX()
{
	return TheMouseState.x;
}

int Input::GetMouseY()
{
	return TheMouseState.y;
}

void Input::ResetScrollWheel()
{
	return TheMouse.ResetScrollWheelValue();
}

int Input::GetScrollWheel()
{
	return TheMouseState.scrollWheelValue;
}

#pragma region Left Funcs

bool Input::LeftMouseIsDown()
{
	return TheMouseState.leftButton;
}

bool Input::LeftMouseDownThisFrame()
{
	return TheMouseButtonStateTracker.leftButton == MouseButtonState::PRESSED;
}

bool Input::LeftMouseIsHeld()
{
	return TheMouseButtonStateTracker.leftButton == MouseButtonState::HELD;
}

bool Input::LeftMouseIsUp()
{
	return !TheMouseState.leftButton;
}

bool Input::LeftMouseReleasedThisFrame()
{
	return TheMouseButtonStateTracker.leftButton == MouseButtonState::RELEASED;
}

#pragma endregion

#pragma region Right Funcs

bool Input::RightMouseIsDown()
{
	return TheMouseState.rightButton;
}

bool Input::RightMouseDownThisFrame()
{
	return TheMouseButtonStateTracker.rightButton == MouseButtonState::PRESSED;
}

bool Input::RightMouseIsHeld()
{
	return TheMouseButtonStateTracker.rightButton == MouseButtonState::HELD;
}

bool Input::RightMouseIsUp()
{
	return !TheMouseState.rightButton;
}

bool Input::RightMouseReleasedThisFrame()
{
	return TheMouseButtonStateTracker.rightButton == MouseButtonState::RELEASED;
}

#pragma endregion

#pragma region Middle Funcs

bool Input::MiddleMouseIsDown()
{
	return TheMouseState.middleButton;
}

bool Input::MiddleMouseDownThisFrame()
{
	return TheMouseButtonStateTracker.middleButton == MouseButtonState::PRESSED;
}

bool Input::MiddleMouseIsHeld()
{
	return TheMouseButtonStateTracker.middleButton == MouseButtonState::HELD;
}

bool Input::MiddleMouseIsUp()
{
	return !TheMouseState.middleButton;
}

bool Input::MiddleMouseReleasedThisFrame()
{
	return TheMouseButtonStateTracker.middleButton == MouseButtonState::RELEASED;
}

#pragma endregion

#pragma region X1 Funcs

bool Input::X1MouseIsDown()
{
	return TheMouseState.xButton1;
}

bool Input::X1MouseDownThisFrame()
{
	return TheMouseButtonStateTracker.xButton1 == MouseButtonState::PRESSED;
}

bool Input::X1MouseIsHeld()
{
	return TheMouseButtonStateTracker.xButton1 == MouseButtonState::HELD;
}

bool Input::X1MouseIsUp()
{
	return !TheMouseState.xButton1;
}

bool Input::X1MouseReleasedThisFrame()
{
	return TheMouseButtonStateTracker.xButton1 == MouseButtonState::RELEASED;
}

#pragma endregion

#pragma region X2 Funcs

bool Input::X2MouseIsDown()
{
	return TheMouseState.xButton2;
}

bool Input::X2MouseDownThisFrame()
{
	return TheMouseButtonStateTracker.xButton2 == MouseButtonState::PRESSED;
}

bool Input::X2MouseIsHeld()
{
	return TheMouseButtonStateTracker.xButton2 == MouseButtonState::HELD;
}

bool Input::X2MouseIsUp()
{
	return !TheMouseState.xButton2;
}

bool Input::X2MouseReleasedThisFrame()
{
	return TheMouseButtonStateTracker.xButton2 == MouseButtonState::RELEASED;
}

#pragma endregion
