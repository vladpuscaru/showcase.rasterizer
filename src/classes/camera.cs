
using OpenTK.Mathematics;

namespace UV_Practical3
{
    public class Camera : Entity
    {
        public Matrix4 ProjectionMatrix { get; }

        public Matrix4 GetInverseTransform()
        {
            return Matrix4.Invert(Transform);
        }

        public Camera(string tag, float FOV, float aspectRatio, float near, float far) : base(tag)
        {
            SetTranslation(new Vector3(0, 0, 10));
            ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, aspectRatio, near, far);
        }
    }
}