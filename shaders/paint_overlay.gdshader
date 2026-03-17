shader_type spatial;
render_mode depth_prepass_alpha;

uniform sampler2D albedo_texture: filter_nearest;
uniform vec4 color : source_color;

uniform vec4 grid_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform float grid_line_width = 0.1;
uniform vec2 res = vec2(64.0);

uniform vec4 hovered_pixel_color : source_color;
uniform ivec2 hovered_pixel;

varying vec2 pos;

void vertex() {
	pos = UV * res + grid_line_width * 0.5;
}

void fragment() {
	vec2 m = mod(pos, 1.0);
	
	float alpha = 0.0;
	float m_min = min(m.x, m.y);
	
	ivec2 pixel_pos = ivec2(int(pos.x), int(pos.y));
	
	ALBEDO = texture(albedo_texture, UV).rgb * color.rgb;
	ALPHA = texture(albedo_texture, UV).a * color.a;
	
	if(pixel_pos == hovered_pixel){
		float one_minus_alpha_pixel = 1.0f - hovered_pixel_color.a;
		float one_minus_alpha = 1.0f - ALPHA;
		ALBEDO = one_minus_alpha_pixel * ALBEDO + hovered_pixel_color.rgb * hovered_pixel_color.a;
		ALPHA += one_minus_alpha * hovered_pixel_color.a;
	}
	if(m_min < grid_line_width) {
		ALPHA = 1.0f * grid_color.a;
		ALBEDO = grid_color.xyz;
	}
}