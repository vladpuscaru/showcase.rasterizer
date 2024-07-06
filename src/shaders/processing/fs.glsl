#version 330

/* Defines */
#define MAX_POINT_LIGHTS 10
#define MAX_SPOT_LIGHTS 10

/* A pointlight is going to occupy 2 uniform locations */
struct PointLight {
    vec3 position;
    vec3 intensity;
};

/* A pointlight is going to occupy 4 uniform locations */
struct SpotLight {
    vec3 position;
    vec3 intensity;
    vec3 direction;
    float cutoff;
};

/* Shader inputs */
in vec4 positionWorld;              // fragment position in World Space
in vec4 normalWorld;                // fragment normal in World Space
in vec2 uv;                         // fragment uv texture coordinates
uniform sampler2D diffuseTexture;	// texture sampler

uniform sampler2D normalMapTexture; // normal map

uniform PointLight pointLights[MAX_POINT_LIGHTS];
uniform int pointLightsCount;

uniform SpotLight spotLights[MAX_SPOT_LIGHTS];
uniform int spotLightsCount;

uniform vec3 materialDiffuseReflectance; // Used for Diffuse Shading
uniform vec3 materialSpecularReflectance; // Used for Blinn-Phong Shading

uniform vec3 cameraPositionWorld;   // camera/eye position in World Space. Used for Phong Shading
uniform float phongExponent;        // Used for Phong Shading

uniform vec3 ambientLight;

/* Shader input flags */
uniform int hasMaterial;  // 1 - True | 0 - False
uniform int hasTexture;   // 1 - True | 0 - False
uniform int hasNormalMap; // 1 - True | 0 - False
uniform int isSelected;   // 1 - True | 0 - False

/* Shader outputs */
out vec4 outputColor;

/* Fragment Shader */
void main()
{
    vec3 color = vec3(0);

    if (isSelected == 1) {
        color += vec3(0.5f, 0.5f, 0.5f);
    } else if (hasMaterial == 1) {
        vec3 normal = normalWorld.xyz;

        /* If there's a normal map, use that normal instead */
        if (hasNormalMap == 1) {
            normal = texture(normalMapTexture, uv).rgb;
        }

        vec3 position = positionWorld.xyz;
        vec3 eyeDir = cameraPositionWorld - position;

        for (int i = 0; i < pointLightsCount; i++) {
            vec3 lightDir = pointLights[i].position - position;
            vec3 halfwayDir = normalize(lightDir + eyeDir);

            float lightSpreadRadius = length(lightDir);
            lightDir = normalize(lightDir);
            vec3 lightIntensityAtPoint = pointLights[i].intensity * 1 / (lightSpreadRadius * lightSpreadRadius);

            /* Diffuse Component */
            vec3 diffuse = lightIntensityAtPoint * materialDiffuseReflectance * max(0, dot(normal, lightDir));

            /* Specular Blinn-Phong Highlight Component */
            vec3 specular = lightIntensityAtPoint * materialSpecularReflectance * pow(max(0.0f, dot(normal, halfwayDir)), phongExponent);

            color += diffuse + specular;
        }

        for (int i = 0; i < spotLightsCount; i++) {
            vec3 lightLength = position - spotLights[i].position;
            vec3 directionToVertex = normalize(lightLength);
            vec3 halfwayDir = normalize(-directionToVertex + eyeDir);

            float cosTheta = cos(spotLights[i].cutoff);
            float cosAlpha = dot(spotLights[i].direction, directionToVertex);

            if (cosAlpha > cosTheta) {
                /* vertex is inside the illumination cone of the spotlight */
                float ratio = 1.0 / (1.0 - cosTheta);
                float intensityFactor = 1.0 - (1.0 - cosAlpha) * ratio;

                // (1.0 - (1.0 - SpotFactor) * 1.0/(1.0 - l.Cutoff))

                vec3 lightIntensityAtPoint = spotLights[i].intensity * 1 / (lightLength * lightLength); /* point light attenuation */
                // lightIntensityAtPoint = lightIntensityAtPoint * intensityFactor; /* spot light attenuation */
                lightIntensityAtPoint = spotLights[i].intensity * intensityFactor;

                /* Diffuse Component */
                vec3 diffuse = lightIntensityAtPoint * materialDiffuseReflectance * max(0, dot(normal, -directionToVertex));

                /* Specular Blinn-Phong Highlight Component */
                vec3 specular = lightIntensityAtPoint * materialSpecularReflectance * pow(max(0.0f, dot(normal, halfwayDir)), phongExponent);

                color += diffuse + specular;
            }
        }
    }

    color += ambientLight;

    if (hasTexture == 1) {
        /* Applying texture */
        color *= texture(diffuseTexture, uv).rgb;
    }

    outputColor = vec4(color, 1.0f);
}