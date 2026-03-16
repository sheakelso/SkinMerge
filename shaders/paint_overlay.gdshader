shader_type spatial;
render_mode unshaded;

uniform vec4 albedo : source_color = vec4(1.0);
uniform vec4 grid_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform float grid_line_width = 0.1;
uniform vec3 offset = vec3(0.0);
uniform vec2 res = vec2(64.0);
uniform vec3 mouse_position;

varying vec2 pos;
varying vec3 vert;
varying vec3 mouse;

void vertex() {
	pos = UV * res + grid_line_width * 0.5;
	vert = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	mouse = mouse_position;
}

void fragment() {
	vec2 m = mod(pos, 1.0);
	
	float alpha = 0.0;
	float m_min = min(m.x, m.y);
	
	float dist = distance(mouse_position, vert);
	
	if(m_min < grid_line_width) alpha = 1.0;
	
	ALBEDO = vert; //grid_color.xyz;
	ALPHA = dist;//alpha * grid_color.a;
}