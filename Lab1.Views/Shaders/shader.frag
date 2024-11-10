#version 330 core

out vec4 FragColor;

in vec3 TexCoord;
uniform sampler3D u_texture;

void main() {
	FragColor = 5 * texture(u_texture, TexCoord);	
//	FragColor = vec4(1, 0.5,0.5, 1.0);
}