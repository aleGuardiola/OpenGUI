#version 330

//position
layout (location = 0) in vec3 pos;

//texture
layout (location = 1) in vec2 tex;

//send if the fragment should be discarted to the fragment shader
flat out int vDiscard;
out vec2 texCoord;

//define the rectangle where the object is
uniform float rectWidth;
uniform float rectHeight;
uniform float rectPosX;
uniform float rectPosY;

uniform mat4 transfromMatrix;
uniform mat4 perspectivenMatrix;
uniform mat4 view;

bool isInsideClipArea(vec4 pos);

void main()
{
    vec4 positon = perspectivenMatrix * view * transfromMatrix * vec4(pos, 1.0);
	gl_Position = positon;

	//if the vertex is outside the rectangle discard vertex
	if(isInsideClipArea(positon))
	   vDiscard = 0;
    else
	   vDiscard = 1;

	texCoord = tex;
}

//return true if the object is inside the rectangle
bool isInsideClipArea(vec4 pos)
{
   float x = pos.x;
   float y = pos.y;
   
   if(x >= rectPosX && x <= rectPosX + rectWidth && y >= rectPosY && y <= rectPosY + rectHeight)
      return true;
   else 
      return false;
}