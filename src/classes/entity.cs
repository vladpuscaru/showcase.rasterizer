
using ImGuiNET;
using OpenTK.Graphics.ES11;
using OpenTK.Mathematics;
using Template;

namespace UV_Practical3
{
    public class Entity
    {
        /* Entity Data */
        public Matrix4 Transform { get; set; } /* Transform relative to the parent */
        public string Tag { get; set; }

        /* Useful for Scene Graph */
        public Entity? ParentEntity { get; set; }
        public List<Entity> ChildrenEntities { get; set; }

        /* Entity appearance and properties */
        public Mesh? Mesh { get; set; }
        public Texture? Texture { get; set; }
        public Texture? NormalMap { get; set; }
        public Material? Material { get; set; }


        public bool IsSelected { get; set; }

        public float moveSpeed = 10.5f;
        public float rotateSpeed = 5.5f;

        public Entity(string tag)
        {
            Tag = tag;

            Transform = Matrix4.Identity;
            ParentEntity = null;
            ChildrenEntities = new List<Entity>();

            Mesh = null;
            Texture = null;
            NormalMap = null;
            Material = null;

            IsSelected = false;
        }

        public void SetScale(Vector3 scale)
        {
            /* Descale current scale and then set new scale */
            Vector3 currentScale = GetScale();
            Transform = Matrix4.CreateScale(scale) * Matrix4.Invert(Matrix4.CreateScale(currentScale)) * Transform;
        }

        public void SetTranslation(Vector3 translation)
        {
            /* Translate back to origin and set new translation */
            Vector3 currentTranslation = GetTranslation();
            Transform = Matrix4.CreateTranslation(translation) * Matrix4.Invert(Matrix4.CreateTranslation(currentTranslation)) * Transform;
        }

        public void SetRotation(Vector3 rotation)
        {
            /* Descale current scale, derotate the current rotation and then reapply the scale and the new rotation */
            Vector3 currentScale = GetScale();
            Vector3 currentRotation = GetRotation();

            Matrix4 currentScaleMatrix = Matrix4.CreateScale(currentScale);
            Matrix4 currentRotationMatrix = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(currentRotation.X)) *
                                            Matrix4.CreateRotationY(MathHelper.DegreesToRadians(currentRotation.Y)) *
                                            Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(currentRotation.Z));

            Matrix4 newRotationMatrix = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));

            Transform = newRotationMatrix * currentScaleMatrix * Matrix4.Invert(currentRotationMatrix) * Matrix4.Invert(currentScaleMatrix) * Transform;
        }

        public void Translate(Vector3 translation)
        {
            Transform = Matrix4.CreateTranslation(translation) * Transform;
        }

        public virtual void Rotate(Vector3 rotation)
        {
            Transform = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X)) *
                        Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y)) *
                        Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z)) *
                        Transform;
        }

        public void AddChild(Entity entity)
        {
            ChildrenEntities.Add(entity);
            entity.ParentEntity = this;
        }

        public void MoveForward(float deltaTime)
        {
            Translate(new Vector3(0, 0, -moveSpeed * deltaTime));
        }

        public void MoveBackwards(float deltaTime)
        {
            Translate(new Vector3(0, 0, moveSpeed * deltaTime));
        }

        public void MoveRight(float deltaTime)
        {
            Translate(new Vector3(moveSpeed * deltaTime, 0, 0));
        }

        public void MoveLeft(float deltaTime)
        {
            Translate(new Vector3(-moveSpeed * deltaTime, 0, 0));
        }

        public void MoveUp(float deltaTime)
        {
            Translate(new Vector3(0, moveSpeed * deltaTime, 0));
        }

        public void MoveDown(float deltaTime)
        {
            Translate(new Vector3(0, -moveSpeed * deltaTime, 0));
        }

        public void Pitch(float direction)
        {
            Rotate(new Vector3(direction * rotateSpeed, 0, 0));
        }

        public void Yaw(float direction)
        {
            Rotate(new Vector3(0, direction * rotateSpeed, 0));
        }

        /**
         * Extracting SRT from transform matrix https://math.stackexchange.com/questions/237369/given-this-transformation-matrix-how-do-i-decompose-it-into-translation-rotati
         */
        public Vector3 GetTranslation()
        {
            /* Extracts x y z coords from Transform Matrix */
            float x = Transform.Column0[3];
            float y = Transform.Column1[3];
            float z = Transform.Column2[3];
            return new Vector3(x, y, z);
        }

        public Vector3 GetScale()
        {
            float x = Transform.Row0.Length;
            float y = Transform.Row1.Length;
            float z = Transform.Row2.Length;
            return new Vector3(x, y, z);
        }

        public Vector3 GetRotation()
        {
            Vector3 currentScale = GetScale();
            Matrix4 rotationMatrix = Matrix4.Invert(Matrix4.CreateScale(currentScale)) * Transform;
            float rotationX = (float)MathHelper.RadiansToDegrees(-MathHelper.Asin(rotationMatrix.Column2.Y));
            float rotationY = (float)MathHelper.RadiansToDegrees(MathHelper.Acos(rotationMatrix.Column2.X));
            float rotationZ = (float)MathHelper.RadiansToDegrees(MathHelper.Acos(rotationMatrix.Column0.X));

            return new Vector3(rotationX, rotationY, rotationZ);
        }

        /* UI Helpers */
        public virtual void SubmitUI()
        {
            ImGui.Text("Transform");

            /* Translation */
            Vector3 translation = GetTranslation();
            System.Numerics.Vector3 translationUI = new System.Numerics.Vector3(translation.X, translation.Y, translation.Z);

            if (ImGui.InputFloat3("Translation", ref translationUI))
            {
                SetTranslation(new Vector3(translationUI.X, translationUI.Y, translationUI.Z));
            }

            /* Rotation */
            Vector3 rotation = GetRotation();
            System.Numerics.Vector3 rotationUI = new System.Numerics.Vector3(rotation.X, rotation.Y, rotation.Z);

            if (ImGui.InputFloat3("Rotation", ref rotationUI))
            {
                // SetRotation(new Vector3(rotationUI.X, rotationUI.Y, rotationUI.Z));
            }
            /* Scale */
            Vector3 scale = GetScale();
            System.Numerics.Vector3 scaleUI = new System.Numerics.Vector3(scale.X, scale.Y, scale.Z);

            if (ImGui.InputFloat3("Scale", ref scaleUI))
            {
                SetScale(new Vector3(scaleUI.X, scaleUI.Y, scaleUI.Z));
            }


            /* Material Diffuse Color */
            // ImGui.Text("")
            // ImGui::ColorPicker4("##picker", (float*)&color, misc_flags | ImGuiColorEditFlags_NoSidePreview | ImGuiColorEditFlags_NoSmallPreview);

            /* Material Specular Color */
            /* Mesh Properties */
            if (Mesh != null)
            {
                ImGui.Text("Mesh Properties");
                ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new System.Numerics.Vector2(10.0f, 10.0f));
                if (ImGui.BeginTable("Mesh Properties", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.NoHostExtendX | ImGuiTableFlags.SizingFixedFit))
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.TextUnformatted("Model");
                    ImGui.TableSetColumnIndex(1);
                    ImGui.TextUnformatted("Vertices");
                    ImGui.TableSetColumnIndex(2);
                    ImGui.TextUnformatted("Triangles");
                    ImGui.TableSetColumnIndex(3);
                    ImGui.TextUnformatted("Quads");

                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.TextUnformatted(Mesh.filename);
                    ImGui.TableSetColumnIndex(1);
                    ImGui.TextUnformatted(Mesh.vertices != null ? Mesh.vertices.Length.ToString() : "0");
                    ImGui.TableSetColumnIndex(2);
                    ImGui.TextUnformatted(Mesh.triangles != null ? Mesh.triangles.Length.ToString() : "0");
                    ImGui.TableSetColumnIndex(3);
                    ImGui.TextUnformatted(Mesh.quads != null ? Mesh.quads.Length.ToString() : "0");

                    ImGui.EndTable();
                }
                ImGui.PopStyleVar();
            }

            ImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new System.Numerics.Vector2(10.0f, 10.0f));
            if (ImGui.BeginTable("Instructions", 1, ImGuiTableFlags.Borders | ImGuiTableFlags.NoHostExtendX | ImGuiTableFlags.SizingFixedFit))
            {
                ImGui.Text("Instructions");

                ImGui.TableNextRow();
                uint bg_color = ImGui.GetColorU32(new System.Numerics.Vector4(0.5f, 0.5f, 0.2f, 1.0f));
                ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, bg_color);
                ImGui.TableSetColumnIndex(0);
                ImGui.TextUnformatted("* To move entity around the world - WASD + EC");

                ImGui.TableNextRow();
                bg_color = ImGui.GetColorU32(new System.Numerics.Vector4(0.0f, 0.0f, 0.2f, 1.0f));
                ImGui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, bg_color);
                ImGui.TableSetColumnIndex(0);
                ImGui.TextUnformatted("* To rotate entity around its center - Arrow Keys");

                ImGui.EndTable();
            }
            ImGui.PopStyleVar();
        }
    }
}