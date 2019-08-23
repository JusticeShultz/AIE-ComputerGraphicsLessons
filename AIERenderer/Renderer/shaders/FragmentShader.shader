//#version 410
//out vec4 vertColor;
//void main()
//{
//	vertColor = vec4(1.0, 0.0, 0.0, 1.0); //red
//}
// fragment shader
// this is run for each vertex on a mesh

#version 330 core
out vec4 FragColor;
uniform vec4 vertColor;
void main()
{
   FragColor = vertColor;
}