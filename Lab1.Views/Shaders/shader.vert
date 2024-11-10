#version 330 core
layout(location = 0) in vec2 aPos;
layout(location = 1) in vec2 aTexCoord;
uniform mat4 u_transform_matrix;
out vec3 TexCoord;

void main() {
	gl_Position = vec4(aPos, 0, 1.0);
	TexCoord = (vec4(aTexCoord, 0, 1) * u_transform_matrix).xyz;
}