#version 330
 
// shader inputs
in vec4 positionWorld;              // fragment position in World Space
in vec4 normalWorld;                // fragment normal in World Space
in vec2 uv;                         // fragment uv texture coordinates

// shader output
out vec4 outputColor;

// fragment shader
void main()
{
    outputColor = 0.5 * normalWorld;
}