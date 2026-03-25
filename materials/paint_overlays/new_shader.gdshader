shader_type spatial;
render_mode unshaded, depth_prepass_alpha;

uniform sampler2D albedo_texture: filter_nearest, source_color;
uniform vec4 background_color : source_color;

uniform vec4 grid_color : source_color = vec4(0.0, 0.0, 0.0, 1.0);
uniform float grid_line_width = 0.1;
uniform vec2 res = vec2(64.0);

uniform vec4 hovered_pixel_color : source_color;
uniform ivec2 hovered_pixel;

varying vec2 pos;

void vertex() {
	pos = (UV * res);
}

vec4 get_grid_color(){
	vec2 dvx = vec2(dFdx(pos.x), dFdy(pos.x));
	vec2 dvy = vec2(dFdx(pos.y), dFdy(pos.y));
	float lx = length(dvx);
	float ly = length(dvy);
	vec2 dudv = vec2(lx, ly) * grid_line_width;
	
	vec2 mod_div_dudv = mod(pos + fwidth(pos.x) * 0.5f, 1.0f) / dudv;
	vec2 vec = vec2(1.0f) - abs(clamp(vec2(0.0f), vec2(1.0f), mod_div_dudv) * 2.0f - vec2(1.0f));
	float alpha = max(vec.x, vec.y) * grid_color.a;
	
	return vec4(grid_color.rgb, alpha);
}

vec4 get_hover_color(){
	ivec2 pixel_pos = ivec2(int(pos.x), int(pos.y));
	if(pixel_pos == hovered_pixel){
		return hovered_pixel_color;
	}
	return vec4(0);
}

vec4 layer_colours(vec4 under, vec4 over){
	vec3 color = mix(under.rgb, over.rgb, over.a);
	return vec4(color, (1.0f - under.a) * over.a + under.a);
}

void fragment() {
	vec4 frag_albedo_color = texture(albedo_texture, UV);
	vec4 frag_grid_color = get_grid_color();
	vec4 frag_hover_color = get_hover_color();
	
	vec4 albedo_color = layer_colours(background_color, frag_albedo_color);
	albedo_color = layer_colours(albedo_color, frag_grid_color);
	albedo_color = layer_colours(albedo_color, frag_hover_color);

	ALPHA = albedo_color.a;
	ALBEDO = albedo_color.rgb;
}