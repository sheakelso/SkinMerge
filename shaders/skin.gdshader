shader_type spatial;
render_mode depth_draw_opaque;

uniform sampler2D albedo_texture: filter_nearest;
uniform vec4 color : source_color;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 albedo = texture(albedo_texture, UV);
	ALBEDO = albedo.rgb * color.rgb;
	ALPHA = 1.0f;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
