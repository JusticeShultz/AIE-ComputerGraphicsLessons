//#version 410
//out vec4 vertColor;
//void main()
//{
//	vertColor = vec4(1.0, 0.0, 0.0, 1.0); //red
//}
// fragment shader
// this is run for each vertex on a mesh

//#version 330 core
//out vec4 FragColor;
//uniform vec4 vertColor;
//void main()
//{
//   FragColor = vertColor;
//}

#version 430

layout(location = 2) uniform mat4 model;

layout (location = 3) uniform sampler2D albedo;
layout (location = 4) uniform vec3 lightDir;
layout (location = 5) uniform sampler2D emissive;
layout (location = 6) uniform sampler2D normal;
layout (location = 7) uniform sampler2D specular;

in vec2 vUV;
in vec3 vNormal;

out vec4 vertColor;

void main() 
{
	vec4 normals = texture(normal, vUV);
	vec4 normalMap = normalize(normals * 2.0 - 1.0);
	float d = max(0, dot(normalMap.xyz, -lightDir));
	vec4 diffuse = d * vec4(1, 1, 1, 1);
	vec4 base = texture(albedo, vUV);
	vec4 emission = texture(emissive, vUV);
	vec4 specularMap = texture(specular, vUV);

	//vertColor = vec4(normalMap.xyz,1);
	vertColor = vec4((diffuse * (specularMap + base)).xyz, 1) + (emission * model);
};