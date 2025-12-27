#version 330

float saturate(float self) {
	return clamp(self, 0.0, 1.0);
}

vec3 saturate(vec3 self) {
	return vec3(saturate(self.x), saturate(self.y), saturate(self.z));
}
