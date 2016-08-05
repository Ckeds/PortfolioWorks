#include <GL\glew.h>
#include <GLFW\glfw3.h>
#include <glm\glm.hpp>
#include <glm\gtc\type_ptr.hpp>
#include <SOIL\SOIL.h>

using namespace std;


/* Loads an image as texture using soil and then configures it to openGL.
Inputs: char* ImagePath
int* ImageWidth, ImageHeight. Must be passed as a reference (using &)
*/
unsigned char* SoilLoadImageAsTexture(char* ImagePath, int* ImageWidth, int* ImageHeight);

/* Creates a GLFW Image based on a Image path.
Inputs: char* ImagePath
int* ImageWidth, ImageHeight. Must be passed as a reference (using &)
*/
GLFWimage GlfwCreateImage(char* ImagePath, int ImageWidth, int ImageHeight);