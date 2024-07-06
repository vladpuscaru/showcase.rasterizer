
namespace UV_Practical3
{
    public class Material
    {
        /* Diffuse Shading (Lambertian) Properties */
        public Color DiffuseReflectance { get; set; }

        /* Blinn-Phong Shading - Highlights */
        public Color SpecularReflectance { get; set; }
        public float PhongExponent { get; set; }

        public Material()
        {
            DiffuseReflectance = new Color(0.0f, 0.0f, 0.0f);

            SpecularReflectance = new Color(0.0f, 0.0f, 0.0f);
            PhongExponent = 0.0f;
        }
    }
}