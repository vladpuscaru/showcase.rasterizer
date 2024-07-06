using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Template;

namespace UV_Practical3
{
    public abstract class Scene
    {

        public Entity World { get; } /* Root Node of the Scene Tree Graph */
        public List<Entity> EntityList { get; }
        public Entity SelectedEntity { get; set; }

        public List<Light> Lights { get; set; }
        public Color AmbientLight { get; set; }

        public Camera Camera { get; }

        protected Shader? ShaderProgram { get; set; }

        protected KeyboardState keyboardState;

        public Scene(float aspectRatio, KeyboardState _keyboardState)
        {
            World = new Entity("World");
            Lights = new List<Light>();
            AmbientLight = new Color(0.005f, 0.005f, 0.005f);

            Camera = new Camera("Main_Camera", MathHelper.DegreesToRadians(60.0f), aspectRatio, .1f, 1000);
            ShaderProgram = null;
            keyboardState = _keyboardState;

            Init();

            EntityList = GetEntitiesList();

            SelectedEntity = Camera;
            Camera.IsSelected = true;
        }

        protected abstract void Init();

        public abstract void Render();
        public abstract void Input(float deltaTime);
        public abstract void Update();

        protected Matrix4 ComputeEntityGlobalTransform(Entity entity)
        {
            if (entity.ParentEntity != null)
            {
                return ComputeEntityGlobalTransform(entity.ParentEntity) * entity.Transform;
            }
            return entity.Transform;
        }

        protected void RotateEntityAboutPoint(Entity entity, Vector3 point, Vector3 rotation)
        {
            /* Move point to origin, rotate about origin, translate back */
            Matrix4 transform = ComputeEntityGlobalTransform(entity);
            transform = Matrix4.CreateTranslation(-point) * transform;
            transform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z)) *
                        transform;
            transform = Matrix4.CreateTranslation(point) * transform;
            entity.Transform = transform;
        }

        protected void SetupEntityMaterial(Entity entity)
        {
            if (ShaderProgram == null)
            {
                Console.WriteLine("Shaders missing. Check Scene.cs");
                Environment.Exit(-1);
            }

            if (entity.Material != null)
            {
                ShaderProgram.SetUniform(ShaderProgram.uniform_hasMaterial, 1);
                ShaderProgram.SetUniform(ShaderProgram.uniform_materialDiffuseReflectance, entity.Material.DiffuseReflectance);
                ShaderProgram.SetUniform(ShaderProgram.uniform_materialSpecularReflectance, entity.Material.SpecularReflectance);
                ShaderProgram.SetUniform(ShaderProgram.uniform_phongExponent, entity.Material.PhongExponent);
            }
            else
            {
                ShaderProgram.SetUniform(ShaderProgram.uniform_hasMaterial, 0);
            }


            ShaderProgram.SetUniform(ShaderProgram.uniform_hasTexture, entity.Texture != null ? 1 : 0);

            ShaderProgram.SetUniform(ShaderProgram.uniform_hasNormalMap, entity.NormalMap != null ? 1 : 0);

            ShaderProgram.SetUniform(ShaderProgram.uniform_isSelected, entity.IsSelected ? 1 : 0);

        }

        protected void SetupLights()
        {
            if (ShaderProgram == null)
            {
                Console.WriteLine("Shaders missing. Check Scene.cs");
                Environment.Exit(-1);
            }

            /* Ambient Light */
            ShaderProgram.SetUniform(ShaderProgram.uniform_ambientLight, AmbientLight);

            /**
             * Send each light's data to the shaders
             */
            int plCount = 0;
            int slCount = 0;

            for (int i = 0; i < Lights.Count; i++)
            {
                Light light = Lights[i];

                // TODO: I realised lights don't work if they are child of other entities
                // because I don't compute their model like the other entities, they just send their translation
                // which technically is in object space but for them is also world space coincidentally
                // This is not true though when they have parents

                if (light is PointLight)
                {
                    PointLight pl = (PointLight)light;

                    if (ShaderProgram.uniform_pointLights != -1)
                    {
                        ShaderProgram.SetUniform(ShaderProgram.GetUniformLocation("pointLights[" + plCount + "].position"), pl.GetTranslation()); ;
                        ShaderProgram.SetUniform(ShaderProgram.GetUniformLocation("pointLights[" + plCount + "].intensity"), pl.Intensity); ;
                    }

                    plCount++;
                }
                else if (light is SpotLight)
                {
                    SpotLight sl = (SpotLight)light;

                    if (ShaderProgram.uniform_spotLights != -1)
                    {
                        ShaderProgram.SetUniform(ShaderProgram.GetUniformLocation("spotLights[" + slCount + "].position"), sl.GetTranslation()); ;
                        ShaderProgram.SetUniform(ShaderProgram.GetUniformLocation("spotLights[" + slCount + "].intensity"), sl.Intensity);
                        ShaderProgram.SetUniform(ShaderProgram.GetUniformLocation("spotLights[" + slCount + "].direction"), sl.Direction);
                        ShaderProgram.SetUniform(ShaderProgram.GetUniformLocation("spotLights[" + slCount + "].cutoff"), MathHelper.DegreesToRadians(sl.Cutoff));
                    }

                    slCount++;
                }
            }

            ShaderProgram.SetUniform(ShaderProgram.uniform_pointLightsCount, plCount);
            ShaderProgram.SetUniform(ShaderProgram.uniform_spotLightsCount, slCount);
        }

        protected void RenderEntities()
        {
            if (ShaderProgram == null)
            {
                Console.WriteLine("Shaders missing. Check Scene.cs");
                Environment.Exit(-1);
            }

            // TODO: Calling setuplights at every render is redundant.
            //       Only call when initing or a light's properties changed
            SetupLights();

            // TODO: Setup the camera position only when it actually changes, not every render
            ShaderProgram.SetUniform(ShaderProgram.uniform_cameraPositionWorld, Camera.GetTranslation());


            /* Traverse the scene graph and render all objects */

            /**
             * Depth-First-Search Tree Traversal
             */
            Dictionary<Entity, bool> visited = new Dictionary<Entity, bool>();

            Stack<Entity> open = new Stack<Entity>();
            open.Push(World);

            while (open.Count > 0)
            {
                Entity currentEntity = open.Pop();

                if (!visited.ContainsKey(currentEntity) && currentEntity.Mesh != null)
                {
                    Matrix4 model = ComputeEntityGlobalTransform(currentEntity);
                    Matrix4 view = model * Camera.GetInverseTransform();
                    Matrix4 screen = view * Camera.ProjectionMatrix;

                    SetupEntityMaterial(currentEntity);

                    currentEntity.Mesh.Render(ShaderProgram, screen, model, currentEntity.Texture, currentEntity.NormalMap);

                    visited.Add(currentEntity, true);
                }

                foreach (Entity child in currentEntity.ChildrenEntities)
                {
                    if (visited.ContainsKey(child))
                    {
                        continue;
                    }
                    else
                    {
                        open.Push(child);
                    }
                }
            }
        }

        public List<Entity> GetEntitiesList()
        {
            /* Traverse the scene graph and render all objects */

            /**
             * Depth-First-Search Tree Traversal
             */
            List<Entity> entities = new List<Entity>();
            Dictionary<Entity, bool> visited = new Dictionary<Entity, bool>();

            Stack<Entity> open = new Stack<Entity>();
            open.Push(World);

            while (open.Count > 0)
            {
                Entity currentEntity = open.Pop();

                if (!visited.ContainsKey(currentEntity))
                {
                    entities.Add(currentEntity);
                    visited.Add(currentEntity, true);
                }

                foreach (Entity child in currentEntity.ChildrenEntities)
                {
                    if (visited.ContainsKey(child))
                    {
                        continue;
                    }
                    else
                    {
                        open.Push(child);
                    }
                }
            }

            return entities;
        }

        public void SubmitWorldUI(int depth, Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            int TAB_SIZE = 20;
            for (int i = 0; i < depth; i++)
            {
                ImGui.SetCursorPosX(TAB_SIZE * i + ImGui.GetCursorPosX());
            }

            if (ImGui.Selectable(entity.Tag, SelectedEntity == entity))
            {
                SelectedEntity.IsSelected = false;

                SelectedEntity = entity;
                entity.IsSelected = true;
            }


            foreach (Entity child in entity.ChildrenEntities)
            {
                SubmitWorldUI(depth + 1, child);
            }
        }

        /* ImGui Helper */
        public void SubmitUI(ref int activeSceneIdx, ref List<Scene> scenes)
        {
            /* Left */
            if (ImGui.Begin("Scene - " + this.GetType().Name))
            {
                ImGui.BeginGroup();
                for (int i = 0; i < scenes.Count; i++)
                {
                    if (ImGui.Button(scenes[i].GetType().Name))
                    {
                        activeSceneIdx = i;
                    }
                    ImGui.SameLine();
                }
                ImGui.EndGroup();

                ImGui.Separator();

                ImGui.BeginChild("Entities", new System.Numerics.Vector2(450, 0), ImGuiChildFlags.Border);
                SubmitWorldUI(1, World);
                ImGui.EndChild();
            }

            ImGui.SameLine();

            /* Right */
            ImGui.BeginGroup();
            ImGui.BeginChild("Selected Entity", new System.Numerics.Vector2(0, -ImGui.GetFrameHeightWithSpacing()));

            ImGui.Text("Selected Entity - " + SelectedEntity.Tag);
            ImGui.Separator();

            SelectedEntity.SubmitUI();

            ImGui.EndChild();
            ImGui.EndGroup();

            ImGui.End();
        }
    }
}