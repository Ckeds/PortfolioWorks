#include "ImageLoader.h"

using namespace std;


/* Loads an image as texture using soil and then configures it to openGL.
	Inputs: char* ImagePath
			int* ImageWidth, ImageHeight. Must be passed as a reference (using &)*/ 
unsigned char* SoilLoadImageAsTexture(char* ImagePath, int* ImageWidth, int* ImageHeight)
{
	unsigned char* ReturnImage;
	// Loads image as texture using SOIL function. Stardart used is RGBA
	ReturnImage = SOIL_load_image(ImagePath, ImageWidth, ImageHeight, 0, SOIL_LOAD_RGBA);

	return ReturnImage;
}

/* Creates a GLFW Image based on a Image path.
	Inputs: char* ImagePath
			int* ImageWidth, ImageHeight. Must be passed as a reference (using &)*/
GLFWimage GlfwCreateImage(char* ImagePath, int ImageWidth, int ImageHeight)
{
	GLFWimage ReturnImage;

	ReturnImage.width = ImageWidth;
	ReturnImage.height = ImageHeight;
	ReturnImage.pixels = SoilLoadImageAsTexture(ImagePath, &ReturnImage.width, &ReturnImage.height);

	return ReturnImage;
}

GLuint GlfwCreateTexture(char* ImagePath, int ImageWidth, int ImageHeight){

	// Create one OpenGL texture
	GLuint textureID;
	glGenTextures(1, &textureID);

	// "Bind" the newly created texture : all future texture functions will modify this texture
	glBindTexture(GL_TEXTURE_2D, textureID);

	// Read the file and  call glTexImage2D with the right parameters
	unsigned char* LoadedImage = SoilLoadImageAsTexture(ImagePath, &ImageWidth, &ImageHeight);

	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, ImageWidth, ImageHeight, 0, GL_RGBA,
		GL_UNSIGNED_BYTE, LoadedImage);

	// Nice trilinear filtering.
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR_MIPMAP_LINEAR);
	glGenerateMipmap(GL_TEXTURE_2D);

	// Return the ID of the texture we just created
	return textureID;
}

