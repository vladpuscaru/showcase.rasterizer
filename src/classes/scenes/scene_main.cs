
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Template;

namespace UV_Practical3
{
    public class SceneMain : Scene
    {
        public SceneMain(float aspectRatio, KeyboardState keyboardState) : base(aspectRatio, keyboardState)
        {
        }

        protected override void Init()
        {
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
            Entity teapotEntity = new Entity("Teapot_Small");
            teapotEntity.SetScale(new Vector3(0.5f, 0.5f, 0.5f));
            teapotEntity.Mesh = teapot;
            teapotEntity.Texture = wood;
            teapotEntity.NormalMap = woodNormal;
            teapotEntity.Material = new Material();
            teapotEntity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            teapotEntity.Material.SpecularReflectance = new Color(1.0f, 1.0f, 1.0f);
            teapotEntity.Material.PhongExponent = 32;


            Entity floorEntity = new Entity("Floor");
            floorEntity.SetScale(new Vector3(4.0f, 4.0f, 4.0f));
            floorEntity.Mesh = floor;
            floorEntity.Texture = wood;
            floorEntity.Material = new Material();
            floorEntity.Material.DiffuseReflectance = new Color(0.0f, 1.0f, 1.0f);
            floorEntity.Material.SpecularReflectance = new Color(1.0f, 0.0f, 1.0f);
            floorEntity.Material.PhongExponent = Constants.PHONG_EXPONENT_NEARLY_MIRROR_LIKE;

            Entity wallEntity = new Entity("Wall_0");
            wallEntity.SetScale(new Vector3(4.0f, 4.0f, 4.0f));
            wallEntity.Mesh = floor;
            wallEntity.Texture = brick;
            wallEntity.Material = new Material();
            wallEntity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            wallEntity.Material.SpecularReflectance = new Color(1.0f, 1.0f, 1.0f);
            wallEntity.Material.PhongExponent = Constants.PHONG_EXPONENT_NEARLY_MIRROR_LIKE;
            wallEntity.SetTranslation(new Vector3(0, 0, -5.0f));
            wallEntity.Rotate(new Vector3(90, 0, 0));

            Entity wallEntity1 = new Entity("Wall_1");
            wallEntity1.SetScale(new Vector3(4.0f, 4.0f, 4.0f));
            wallEntity1.Mesh = floor;
            wallEntity1.Texture = brick;
            wallEntity1.NormalMap = brickNormal;
            wallEntity1.Material = new Material();
            wallEntity1.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            wallEntity1.Material.SpecularReflectance = new Color(1.0f, 1.0f, 1.0f);
            wallEntity1.Material.PhongExponent = Constants.PHONG_EXPONENT_NEARLY_MIRROR_LIKE;
            wallEntity1.SetTranslation(new Vector3(6.0f, 0, 0));
            wallEntity1.Rotate(new Vector3(-90, 90, 0));

            Entity teapot2Entity = new Entity("Teapot_Child");
            teapot2Entity.SetScale(new Vector3(2.5f, 2.5f, 2.5f));
            teapot2Entity.SetTranslation(new Vector3(-5.0f, 0.0f, 0.0f));
            teapot2Entity.Mesh = teapot;
            teapot2Entity.Texture = wood;
            teapot2Entity.Material = new Material();
            teapot2Entity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            teapot2Entity.Material.SpecularReflectance = new Color(1.0f, 0.0f, 1.0f);
            teapot2Entity.Material.PhongExponent = Constants.PHONG_EXPONENT_EGGSHELL;

            /* Setup Lights */
            Light pointLight = new PointLight("Point_Light_0");
            pointLight.SetTranslation(new Vector3(20.0f, 30.0f, -15.0f));
            pointLight.Intensity = new Color(2000.0f, 2000.0f, 2000.0f);
            pointLight.Mesh = teapot;

            Light pointLight2 = new PointLight("Point_Light_1");
            pointLight2.SetTranslation(new Vector3(5.0f, 2.0f, 15.0f));
            pointLight2.Intensity = new Color(200.0f, 200.0f, 200.0f);
            pointLight2.Mesh = teapot;

            /* Add entities to scene graph */
            World.AddChild(Camera);
            World.AddChild(teapotEntity);
            teapotEntity.AddChild(teapot2Entity);
            World.AddChild(floorEntity);

            World.AddChild(pointLight);
            World.AddChild(pointLight2);
            World.AddChild(wallEntity);
            World.AddChild(wallEntity1);

            Lights.Add(pointLight);
            Lights.Add(pointLight2);
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