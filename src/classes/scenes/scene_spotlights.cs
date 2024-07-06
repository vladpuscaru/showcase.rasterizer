
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Template;

namespace UV_Practical3
{
    public class SceneSpotlights : Scene
    {
        public SceneSpotlights(float aspectRatio, KeyboardState keyboardState) : base(aspectRatio, keyboardState)
        {
        }

        protected override void Init()
        {
            AmbientLight = new Color(0.25f, 0.25f, 0.25f);

            Camera.Translate(new Vector3(0, 50, 40));
            Camera.Rotate(new Vector3(-45, 0, 0));

            /* Load meshes */
            Mesh teapot = new Mesh(Utils.GetPathFromSrc("assets/teapot.obj"));
            Mesh floor = new Mesh(Utils.GetPathFromSrc("assets/floor.obj"));

            /* Load textures */
            Texture wood = new Texture(Utils.GetPathFromSrc("assets/wood.jpg"));
            Texture woodNormal = new Texture(Utils.GetPathFromSrc("assets/generated/normalmap_0.jpg"));

            Texture brick = new Texture(Utils.GetPathFromSrc("assets/brick_wall.jpeg"));
            Texture brickNormal = new Texture(Utils.GetPathFromSrc("assets/generated/normalmap_1.jpg"));

            /* Load shaders */
            ShaderProgram = new Shader(Utils.GetPathFromSrc("shaders/processing/vs.glsl"), Utils.GetPathFromSrc("shaders/processing/fs.glsl"));

            /* Setup renderable Entities */
            Entity teapotEntity = new Entity("Teapot");
            teapotEntity.Mesh = teapot;
            teapotEntity.Texture = wood;
            teapotEntity.Material = new Material();
            teapotEntity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            teapotEntity.Material.SpecularReflectance = new Color(1.0f, 1.0f, 1.0f);
            teapotEntity.Material.PhongExponent = Constants.PHONG_EXPONENT_EGGSHELL;


            Entity floorEntity = new Entity("Floor");
            floorEntity.SetScale(new Vector3(4.0f, 4.0f, 4.0f));
            floorEntity.Mesh = floor;
            floorEntity.Texture = brick;
            floorEntity.Material = new Material();
            floorEntity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            floorEntity.Material.SpecularReflectance = new Color(1.0f, 0.0f, 1.0f);
            floorEntity.Material.PhongExponent = Constants.PHONG_EXPONENT_NEARLY_MIRROR_LIKE;

            /* Setup Lights */
            SpotLight spotLight = new SpotLight("Spotlight");
            spotLight.SetTranslation(new Vector3(-5.0f, 15.0f, -15.0f));
            spotLight.Intensity = new Color(1.0f, 1.0f, 1.0f);
            spotLight.Direction = new Vector3(0, -1, 0);
            spotLight.Cutoff = 20.0f;
            spotLight.Mesh = teapot;

            /* Add entities to scene graph */
            World.AddChild(Camera);
            World.AddChild(teapotEntity);
            World.AddChild(floorEntity);
            World.AddChild(spotLight);

            /* Add Lights to lights list */
            Lights.Add(spotLight);
        }

        public override void Render()
        {
            RenderEntities(); /* See Scene class */

            /* Other specific renders for this scene */
        }

        public override void Update()
        {
            /* Handle Update */
        }

        public override void Input(float deltaTime)
        {
            /* Handle Input */

            /* Selected Entity (Can also be the camera) */
            if (keyboardState[Keys.W])
            {
                SelectedEntity.MoveForward(deltaTime);
            }
            if (keyboardState[Keys.S])
            {
                SelectedEntity.MoveBackwards(deltaTime);
            }
            if (keyboardState[Keys.A])
            {
                SelectedEntity.MoveLeft(deltaTime);
            }
            if (keyboardState[Keys.D])
            {
                SelectedEntity.MoveRight(deltaTime);
            }
            if (keyboardState[Keys.E])
            {
                SelectedEntity.MoveUp(deltaTime);
            }
            if (keyboardState[Keys.C])
            {
                SelectedEntity.MoveDown(deltaTime);
            }
            if (keyboardState[Keys.Left])
            {
                SelectedEntity.Yaw(1);
            }
            if (keyboardState[Keys.Right])
            {
                SelectedEntity.Yaw(-1);
            }
            if (keyboardState[Keys.Up])
            {
                SelectedEntity.Pitch(1);
            }
            if (keyboardState[Keys.Down])
            {
                SelectedEntity.Pitch(-1);
            }
        }
    }
}