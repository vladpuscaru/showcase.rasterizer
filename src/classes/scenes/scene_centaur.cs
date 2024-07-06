
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Template;

namespace UV_Practical3
{
    public class SceneCentaur : Scene
    {
        public SceneCentaur(float aspectRatio, KeyboardState keyboardState) : base(aspectRatio, keyboardState)
        {
        }

        protected override void Init()
        {
            AmbientLight = new Color(0.5f, 0.5f, 0.5f);
            /* Load meshes */
            Mesh centaur = new Mesh(Utils.GetPathFromSrc("assets/wado.obj"));
            Mesh teapot = new Mesh(Utils.GetPathFromSrc("assets/teapot.obj"));

            /* Load textures */
            Texture sword = new Texture(Utils.GetPathFromSrc("assets/sword_base.png"));
            Texture swordNormal = new Texture(Utils.GetPathFromSrc("assets/sword_normal.png"));

            /* Load shaders */
            ShaderProgram = new Shader(Utils.GetPathFromSrc("shaders/processing/vs.glsl"), Utils.GetPathFromSrc("shaders/processing/fs.glsl"));

            /* Setup renderable Entities */
            Entity swordEntity = new Entity("Centaur");
            swordEntity.SetScale(new Vector3(0.1f, 0.1f, 0.1f));
            swordEntity.Mesh = centaur;
            swordEntity.Texture = sword;
            swordEntity.NormalMap = swordNormal;
            swordEntity.Material = new Material();
            swordEntity.Material.DiffuseReflectance = new Color(1.0f, 0.0f, 1.0f);
            // swordEntity.Material.SpecularReflectance = new Color(1.0f, 1.0f, 1.0f);
            // swordEntity.Material.PhongExponent = 32;

            /* Setup Lights */
            Light pointLight = new PointLight("Point_Light_0");
            pointLight.SetTranslation(new Vector3(-5.0f, 15.0f, -15.0f));
            pointLight.Intensity = new Color(200.0f, 200.0f, 200.0f);
            pointLight.Mesh = teapot;

            /* Add entities to scene graph */
            World.AddChild(Camera);
            World.AddChild(swordEntity);

            World.AddChild(pointLight);

            Lights.Add(pointLight);
        }

        public override void Render()
        {
            RenderEntities(); /* See Scene class */

            /* Other specific renders for this scene */
            Console.Clear();
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