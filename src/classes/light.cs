
using ImGuiNET;
using OpenTK.Mathematics;

namespace UV_Practical3
{
    public abstract class Light : Entity
    {
        public Color Intensity { get; set; } /* RGB [0, 1] */

        public Light(string tag) : base(tag)
        {
            Intensity = new Color(0.0f, 0.0f, 0.0f);
        }

        public override void Rotate(Vector3 rotation)
        {
            base.Rotate(rotation);
            /* Other child classes need this override (check spotlight) */
        }

        public override void SubmitUI()
        {
            base.SubmitUI();

            ImGui.Spacing();
            ImGui.Spacing();
            ImGui.Spacing();

            /* Options to set the light's intensity */
            ImGui.Text("Intensity");
            float r = Intensity.r;
            float g = Intensity.g;
            float b = Intensity.b;
            if (ImGui.DragFloat("R: ", ref r))
            {
                Intensity = new Color(r, Intensity.g, Intensity.b);
            }
            if (ImGui.DragFloat("G: ", ref g))
            {
                Intensity = new Color(Intensity.r, g, Intensity.b);
            }
            if (ImGui.DragFloat("B: ", ref b))
            {
                Intensity = new Color(Intensity.r, Intensity.g, b);
            }
        }
    }
}