#version 330

flat in int vDiscard;

out vec4 colour;
in vec2 texCoord;

uniform sampler2D myTexture;

void main()
{
   colour = texture(myTexture, texCoord);   
}