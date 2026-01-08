#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;

uniform vec3 lightDir;
uniform vec3 viewPos;

void main()
{
    // Simple diffuse + ambient
    vec3 color = vec3(0.2, 0.6, 1.0); // blueish
    float diff = max(dot(normalize(Normal), -lightDir), 0.0);
    vec3 diffuse = diff * color;
    vec3 ambient = 0.1 * color;
    FragColor = vec4(diffuse + ambient, 1.0);
}
