#version 330 core

out vec4 FragColor;

in vec3 TexCoord;

struct WindowLevel {
	float ww;
	float wl;
};

uniform isampler3D u_texture;
uniform float minPeak;
uniform float maxPeak;
uniform WindowLevel winLevel;

float normalizeHistogram(float currentPixel, float minPeak, float maxPeak, float newMin, float newMax) {
	return newMin + (currentPixel - minPeak) / (maxPeak - minPeak) * (newMax-newMin);
}

float applyWindowLevel(float currentPixel, WindowLevel winLevel, float newMin, float newMax) {
	float minVal = winLevel.wl - 0.5 * winLevel.ww;
	float maxVal = winLevel.wl + 0.5 * winLevel.ww;

	if (currentPixel <= minVal) { 
		return newMin;
	}
	else if (currentPixel > maxVal) { 
		return newMax; 
	} 
	else {
		return newMin + (currentPixel - winLevel.wl) * (newMax - newMin) / winLevel.ww; 
	}

}



void main() {
	int texValue = texture(u_texture, TexCoord).r;
	float newValue = normalizeHistogram(float(texValue), minPeak, maxPeak, 0.0, 1.0);
	FragColor = vec4(newValue,newValue,newValue, 1);
}

