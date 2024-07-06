
using OpenTK.Mathematics;

namespace UV_Practical3
{
    /**
     * Spot Light
     * A Light that's located at a point in the Scene and emits light in a cone shape
     */
    public class SpotLight : Light
    {
        public Vector3 Direction { get; set; }
        public float Cutoff { get; set; }

        public SpotLight(string tag) : base(tag)
        {
            Direction = Vector3.Zero;
            Cutoff = 0.0f;
        }

        public override void Rotate(Vector3 rotation)
        {
            base.Rotate(rotation);
            /* Rotate the Direction as well */
            Direction = Matrix3.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                        Matrix3.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                        Matrix3.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z)) *
                        Direction;
        }
    }
}