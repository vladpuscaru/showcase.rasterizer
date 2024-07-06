
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using Template;

namespace UV_Practical3
{
    public class ScenePointLights : Scene
    {
        public ScenePointLights(float aspectRatio, KeyboardState keyboardState) : base(aspectRatio, keyboardState)
        {
        }

        private Entity createEntity(Mesh mesh, Vector3 scale, string tag, Color diffuse, Color specular, float phong, Texture diffuseTexture, Texture normalMap)
        {
            Entity entity = new Entity(tag);
            entity.SetScale(scale);
            entity.Mesh = mesh;
            entity.Texture = diffuseTexture;
            entity.NormalMap = normalMap;
            entity.Material = new Material();
            entity.Material.DiffuseReflectance = diffuse;
            entity.Material.SpecularReflectance = specular;
            entity.Material.PhongExponent = phong;

            return entity;
        }

        protected override void Init()
        {
            AmbientLight = new Color(0.05f, 0.05f, 0.05f);
            Camera.moveSpeed = 55.0f;

            Camera.SetTranslation(new Vector3(48.0f, 70, 140.0f));
            Camera.Rotate(new Vector3(0, 35.0f, 0.0f));
            Camera.Rotate(new Vector3(-15.0f, 0.0f, 0.0f));

            /* Load meshes */
            Mesh teapot = new Mesh(Utils.GetPathFromSrc("assets/teapot.obj"));
            Mesh floor = new Mesh(Utils.GetPathFromSrc("assets/floor.obj"));

            /* Load textures */
            Texture text0 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex0/tex0_diffuse.jpg"));
            Texture text0Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex0/tex0_normal.jpg"));

            Texture text1 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex1/tex1_diffuse.png"));
            Texture text1Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex1/tex1_normal.png"));

            Texture text2 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex2/tex2_diffuse.png"));
            Texture text2Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex2/tex2_normal.png"));

            Texture text3 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex3/tex3_diffuse.png"));
            Texture text3Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex3/tex3_normal.png"));

            Texture text4 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex4/tex4_diffuse.png"));
            Texture text4Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex4/tex4_normal.png"));

            Texture text5 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex5/tex5_diffuse.jpg"));
            Texture text5Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex5/tex5_normal.jpg"));

            Texture text6 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex6/tex6_diffuse.png"));
            Texture text6Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex6/tex6_normal.png"));

            Texture text7 = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex7/tex7_diffuse.png"));
            Texture text7Normal = new Texture(Utils.GetPathFromSrc("assets/poliigon/tex7/tex7_normal.png"));

            Texture wood = new Texture(Utils.GetPathFromSrc("assets/wood.jpg"));
            Texture woodNormal = new Texture(Utils.GetPathFromSrc("assets/generated/normalmap_0.jpg"));

            Texture brick = new Texture(Utils.GetPathFromSrc("assets/brick_wall.jpeg"));
            Texture brickNormal = new Texture(Utils.GetPathFromSrc("assets/generated/normalmap_1.jpg"));


            /* Load shaders */
            ShaderProgram = new Shader(Utils.GetPathFromSrc("shaders/processing/vs.glsl"), Utils.GetPathFromSrc("shaders/processing/fs.glsl"));

            /* Setup renderable Entities */
            Entity t0 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_0", new Color(1.0f, 0.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, null, null);
            Entity t1 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_1", new Color(0.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text1, text1Normal);
            Entity t2 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_2", new Color(0.0f, 0.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text2, text2Normal);
            Entity t3 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_3", new Color(1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text3, text3Normal);
            Entity t4 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_4", new Color(0.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text4, text4Normal);
            Entity t5 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_5", new Color(1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, wood, text5Normal);
            Entity t6 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_6", new Color(1.0f, 0.5f, 0.5f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text6, text6Normal);
            Entity t7 = createEntity(teapot, new Vector3(2.0f, 2.0f, 2.0f), "Teapot_7", new Color(0.2f, 1.0f, 0.5f), new Color(1.0f, 1.0f, 1.0f), 8, brick, text7Normal);

            Entity f = createEntity(floor, new Vector3(24.0f, 24.0f, 24.0f), "Floor", new Color(1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text0, text0Normal);
            Entity w0 = createEntity(floor, new Vector3(14.0f, 14.0f, 14.0f), "Wall_0", new Color(1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text0, text0Normal);
            Entity w1 = createEntity(floor, new Vector3(14.0f, 14.0f, 14.0f), "Wall_1", new Color(1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text0, text0Normal);
            Entity w2 = createEntity(floor, new Vector3(24.0f, 24.0f, 24.0f), "Wall_2", new Color(1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text0, text0Normal);
            Entity roof = createEntity(floor, new Vector3(24.0f, 24.0f, 24.0f), "Roof", new Color(1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f), Constants.PHONG_EXPONENT_EGGSHELL, text0, text0Normal);

            w0.Translate(new Vector3(10, 0, 0));
            w0.Rotate(new Vector3(90, 90, 0));

            w1.Translate(new Vector3(-10, 5, 0));
            w1.Rotate(new Vector3(-90, -90, 0));

            w2.Translate(new Vector3(0, 0, -2.5f));
            w2.Rotate(new Vector3(90, 0, 0));

            roof.Translate(new Vector3(0, 7, 0));


            t0.Translate(new Vector3(-15.0f, 0.0f, 0.0f));
            t1.Translate(new Vector3(-15.0f, 0.0f, 0.0f));
            t2.Translate(new Vector3(-15.0f, 0.0f, 0.0f));
            t3.Translate(new Vector3(-15.0f, 0.0f, 0.0f));
            t4.Translate(new Vector3(-15.0f, 0.0f, 0.0f));
            t5.Translate(new Vector3(-15.0f, 0.0f, 0.0f));
            t6.Translate(new Vector3(-15.0f, 0.0f, 0.0f));
            t7.Translate(new Vector3(-15.0f, 0.0f, 0.0f));

            float rotationAngle = 360.0f / 8;
            RotateEntityAboutPoint(t0, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 1, 0.0f));
            RotateEntityAboutPoint(t1, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 2, 0.0f));
            RotateEntityAboutPoint(t2, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 3, 0.0f));
            RotateEntityAboutPoint(t3, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 4, 0.0f));
            RotateEntityAboutPoint(t4, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 5, 0.0f));
            RotateEntityAboutPoint(t5, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 6, 0.0f));
            RotateEntityAboutPoint(t6, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 7, 0.0f));
            RotateEntityAboutPoint(t7, new Vector3(15, 0, 0), new Vector3(0.0f, rotationAngle * 8, 0.0f));

            /* Setup Lights */
            // Light pointLight0 = new PointLight("Point_Light_0");
            // pointLight0.SetTranslation(new Vector3(0.0f, 80.0f, -25.0f));
            // pointLight0.Intensity = new Color(1000.0f, 1000.0f, 1000.0f);
            // pointLight0.Mesh = teapot;

            Light pointLight1 = new PointLight("Point_Light_1");
            pointLight1.SetTranslation(new Vector3(-100.0f, 80.0f, -25.0f));
            pointLight1.Intensity = new Color(5000.0f, 0.0f, 0.0f);
            pointLight1.Mesh = teapot;

            Light pointLight2 = new PointLight("Point_Light_2");
            pointLight2.SetTranslation(new Vector3(-100.0f, 80.0f, 60.0f));
            pointLight2.Intensity = new Color(0.0f, 5000.0f, 0.0f);
            pointLight2.Mesh = teapot;

            Light pointLight3 = new PointLight("Point_Light_3");
            pointLight3.SetTranslation(new Vector3(0.0f, 80.0f, 60.0f));
            pointLight3.Intensity = new Color(0.0f, 0.0f, 5000.0f);
            pointLight3.Mesh = teapot;

            Light pointLight4 = new PointLight("Point_Light_4");
            pointLight4.SetTranslation(new Vector3(50.0f, 0.0f, 60.0f));
            pointLight4.Intensity = new Color(5000.0f, 5000.0f, 5000.0f);
            pointLight4.Mesh = teapot;

            SpotLight spotLight1 = new("Spotlight_1");
            spotLight1.SetTranslation(new Vector3(0f, 50f, 20f));
            spotLight1.Intensity = new Color(1f, 0f, 1f);
            spotLight1.Mesh = teapot;
            spotLight1.Cutoff = 30f;
            spotLight1.Direction = new Vector3(0f, -1f, 0f);

            SpotLight spotLight2 = new("Spotlight_2");
            spotLight2.SetTranslation(new Vector3(0f, 50f, -100f));
            spotLight2.Intensity = new Color(1f, 1f, 0f);
            spotLight2.Mesh = teapot;
            spotLight2.Cutoff = 45f;
            spotLight2.Direction = new Vector3(0f, -1f, 0f);

            // Light pointLight5 = new PointLight("Point_Light_5");
            // pointLight5.SetTranslation(new Vector3(100.0f, 0.0f, -65.0f));
            // pointLight5.Intensity = new Color(3000.0f, 3000.0f, 3000.0f);
            // pointLight5.Mesh = teapot;


            /* Add entities to scene graph */
            World.AddChild(Camera);
            World.AddChild(t0);
            World.AddChild(t1);
            World.AddChild(t2);
            World.AddChild(t3);
            World.AddChild(t4);
            World.AddChild(t5);
            World.AddChild(t6);
            World.AddChild(t7);
            World.AddChild(f);
            World.AddChild(w0);
            World.AddChild(w1);
            World.AddChild(w2);
            World.AddChild(roof);

            // World.AddChild(pointLight0);
            World.AddChild(pointLight1);
            World.AddChild(pointLight2);
            World.AddChild(pointLight3);
            World.AddChild(pointLight4);
            World.AddChild(spotLight1);
            World.AddChild(spotLight2);

            // Lights.Add(pointLight0);
            Lights.Add(pointLight1);
            Lights.Add(pointLight2);
            Lights.Add(pointLight3);
            Lights.Add(pointLight4);
            Lights.Add(spotLight1);
            Lights.Add(spotLight2);
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