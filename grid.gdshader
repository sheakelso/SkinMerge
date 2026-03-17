shader_type canvas_item;

uniform int grid_scale;
uniform vec4 grid_color_a : source_color;
uniform vec4 grid_color_b : source_color;

varying vec2 coord;

void vertex() {
	coord = ((MODEL_MATRIX * CANVAS_MATRIX) * vec4(VERTEX, 1.0f, 1.0f)).xy / float(grid_scale);
}

void fragment() {
	int rx = int(coord.x) % 2;
	int ry = int(coord.y) % 2;
	
	if(rx != ry){
		COLOR = grid_color_a;
	}
	else COLOR = grid_color_b;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
