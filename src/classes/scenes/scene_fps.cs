
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Template;

namespace UV_Practical3
{
    public class SceneFPS : Scene
    {
        /* Hacky way of syncing the camera with the spotlight
         * because the light model calculation doesn't work when it's a child
         * Too lazy to fix that now so..
         */
        private SpotLight playerSpotlight = new SpotLight("Player_spotlight");

        public SceneFPS(float aspectRatio, KeyboardState keyboardState) : base(aspectRatio, keyboardState)
        {
        }

        protected override void Init()
        {
            AmbientLight = new Color(0.1f, 0.1f, 0.1f);

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

            Entity floorEntity = new Entity("Floor");
            floorEntity.SetScale(new Vector3(4.0f, 4.0f, 4.0f));
            floorEntity.Mesh = floor;
            floorEntity.Texture = wood;
            floorEntity.Material = new Material();
            floorEntity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            floorEntity.Material.SpecularReflectance = new Color(1.0f, 0.0f, 1.0f);
            floorEntity.Material.PhongExponent = Constants.PHONG_EXPONENT_NEARLY_MIRROR_LIKE;

            Entity wallEntity = new Entity("Wall_0");
            floorEntity.SetScale(new Vector3(4.0f, 4.0f, 4.0f));
            floorEntity.Mesh = floor;
            floorEntity.Texture = brick;
            floorEntity.Material = new Material();
            floorEntity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            floorEntity.Material.SpecularReflectance = new Color(1.0f, 0.0f, 1.0f);
            floorEntity.Material.PhongExponent = Constants.PHONG_EXPONENT_NEARLY_MIRROR_LIKE;

            Entity wallEntity1 = new Entity("Wall_1");
            floorEntity.SetScale(new Vector3(4.0f, 4.0f, 4.0f));
            floorEntity.Translate(new Vector3(-10, 0, 0));
            floorEntity.SetRotation(new Vector3(90, 90, 0));
            floorEntity.Mesh = floor;
            floorEntity.Texture = brick;
            floorEntity.Material = new Material();
            floorEntity.Material.DiffuseReflectance = new Color(1.0f, 1.0f, 1.0f);
            floorEntity.Material.SpecularReflectance = new Color(1.0f, 0.0f, 1.0f);
            floorEntity.Material.PhongExponent = Constants.PHONG_EXPONENT_NEARLY_MIRROR_LIKE;

            /* Setup Lights */
            playerSpotlight.Intensity = new Color(1.0f, 1.0f, 1.0f);
            playerSpotlight.Direction = new Vector3(0, 0, -1); /* set the direction the same as camera direction */
            playerSpotlight.Cutoff = 20f;
            playerSpotlight.SetTranslation(Camera.GetTranslation());

            /* Add entities to scene graph */
            World.AddChild(Camera);
            World.AddChild(floorEntity);
            World.AddChild(wallEntity);
            World.AddChild(wallEntity1);
            World.AddChild(playerSpotlight);

            /* Add Lights to lights list */
            Lights.Add(playerSpotlight);
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
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.MoveForward(deltaTime);
                }
            }
            if (keyboardState[Keys.S])
            {
                SelectedEntity.MoveBackwards(deltaTime);
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.MoveBackwards(deltaTime);
                }
            }
            if (keyboardState[Keys.A])
            {
                SelectedEntity.MoveLeft(deltaTime);
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.MoveLeft(deltaTime);
                }
            }
            if (keyboardState[Keys.D])
            {
                SelectedEntity.MoveRight(deltaTime);
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.MoveRight(deltaTime);
                }
            }
            if (keyboardState[Keys.E])
            {
                SelectedEntity.MoveUp(deltaTime);
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.MoveUp(deltaTime);
                }
            }
            if (keyboardState[Keys.C])
            {
                SelectedEntity.MoveDown(deltaTime);
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.MoveDown(deltaTime);
                }
            }
            if (keyboardState[Keys.Left])
            {
                SelectedEntity.Yaw(1);
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.Yaw(-1);
                }
            }
            if (keyboardState[Keys.Right])
            {
                SelectedEntity.Yaw(-1);
                if (SelectedEntity == Camera)
                {
                    playerSpotlight.Yaw(1);
                }
            }
        }
    }
}