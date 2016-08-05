#include "Input.h"

using namespace Input;

Keyboard TheKeyboard;
KeyboardState TheKeyboardState;
KeyboardStateTracker TheKeyboardStateTracker;

void Input::UpdateKeyboard()
{
	TheKeyboardState = TheKeyboard.GetState();
	TheKeyboardStateTracker.Update(TheKeyboardState);
}

bool Input::KeyPressedThisFrame(Key key)
{
	return TheKeyboardStateTracker.IsKeyPressed(key);
}

bool Input::KeyIsDown(Key key)
{
	return TheKeyboardState.IsKeyDown(key);
}

bool Input::KeyReleasedThisFrame(Key key)
{
	return TheKeyboardStateTracker.IsKeyReleased(key);
}

bool Input::KeyIsUp(Key key)
{
	return TheKeyboardState.IsKeyUp(key);
}
