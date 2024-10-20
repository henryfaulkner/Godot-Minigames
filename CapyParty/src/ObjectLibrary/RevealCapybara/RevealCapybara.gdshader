shader_type spatial;

uniform vec4 emission_color = vec4(1.0, 1.0, 1.0, 1.0); // Glow color
uniform float emission_strength = 1.0; // Strength of the glow

void fragment() {
    // Set the emission for the glowing effect around the mesh
    EMISSION = emission_color.rgb * emission_strength;
}
