using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Template
{
    public class Shader
    {
        // data members
        public int programID, vsID, fsID;
        public int in_vertexPositionObject;
        public int in_vertexNormalObject;
        public int in_vertexUV;
        public int uniform_objectToScreen;
        public int uniform_objectToWorld;

        public int uniform_pointLights;
        public int uniform_pointLightsCount;
        public int uniform_spotLights;
        public int uniform_spotLightsCount;
        public int uniform_materialDiffuseReflectance;
        public int uniform_materialSpecularReflectance;
        public int uniform_cameraPositionWorld;
        public int uniform_phongExponent;
        public int uniform_ambientLight;

        public int uniform_hasMaterial;
        public int uniform_hasTexture;
        public int uniform_hasNormalMap;

        public int uniform_isSelected;

        // constructor
        public Shader(string vertexShader, string fragmentShader)
        {
            // compile shaders
            programID = GL.CreateProgram();
            if (!OpenTKApp.isMac) GL.ObjectLabel(ObjectLabelIdentifier.Program, programID, -1, vertexShader + " + " + fragmentShader);
            Load(vertexShader, ShaderType.VertexShader, programID, out vsID);
            Load(fragmentShader, ShaderType.FragmentShader, programID, out fsID);
            GL.LinkProgram(programID);
            string infoLog = GL.GetProgramInfoLog(programID);
            if (infoLog.Length != 0) Console.WriteLine(infoLog);

            // GL.GetProgram(programID, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            // for (int i = 0; i < uniformCount; i++)
            // {
            //     GL.GetActiveUniform(programID, i, 256, out int length, out int size, out ActiveUniformType type, out string name);
            //     int location = GL.GetUniformLocation(programID, name);
            //     Console.WriteLine(name + " - " + location);
            // }

            // get locations of shader parameters
            in_vertexPositionObject = GL.GetAttribLocation(programID, "vertexPositionObject");
            in_vertexNormalObject = GL.GetAttribLocation(programID, "vertexNormalObject");
            in_vertexUV = GL.GetAttribLocation(programID, "vertexUV");
            uniform_objectToScreen = GL.GetUniformLocation(programID, "objectToScreen");
            uniform_objectToWorld = GL.GetUniformLocation(programID, "objectToWorld");
            uniform_materialDiffuseReflectance = GL.GetUniformLocation(programID, "materialDiffuseReflectance");
            uniform_materialSpecularReflectance = GL.GetUniformLocation(programID, "materialSpecularReflectance");
            uniform_pointLights = GL.GetUniformLocation(programID, "pointLights[0].position"); /* Start of the pointLights array */
            uniform_pointLightsCount = GL.GetUniformLocation(programID, "pointLightsCount");
            uniform_spotLights = GL.GetUniformLocation(programID, "spotLights[0].position"); /* Start of the spotLights array */
            uniform_spotLightsCount = GL.GetUniformLocation(programID, "spotLightsCount");
            uniform_cameraPositionWorld = GL.GetUniformLocation(programID, "cameraPositionWorld");
            uniform_phongExponent = GL.GetUniformLocation(programID, "phongExponent");
            uniform_ambientLight = GL.GetUniformLocation(programID, "ambientLight");

            uniform_hasMaterial = GL.GetUniformLocation(programID, "hasMaterial");
            uniform_hasTexture = GL.GetUniformLocation(programID, "hasTexture");
            uniform_hasNormalMap = GL.GetUniformLocation(programID, "hasNormalMap");

            uniform_isSelected = GL.GetUniformLocation(programID, "isSelected");
        }

        // loading shaders
        void Load(String filename, ShaderType type, int program, out int ID)
        {
            // source: http://neokabuto.blogspot.nl/2013/03/opentk-tutorial-2-drawing-triangle.html
            ID = GL.CreateShader(type);
            if (!OpenTKApp.isMac) GL.ObjectLabel(ObjectLabelIdentifier.Shader, ID, -1, filename);
            using (StreamReader sr = new StreamReader(filename)) GL.ShaderSource(ID, sr.ReadToEnd());
            GL.CompileShader(ID);
            GL.AttachShader(program, ID);
            string infoLog = GL.GetShaderInfoLog(ID);
            if (infoLog.Length != 0) Console.WriteLine(infoLog);
        }

        /* Helper Functions */
        public void SetUniform(int id, int value)
        {
            GL.UseProgram(programID);
            GL.Uniform1(id, value);
            GL.UseProgram(0);
        }

        public void SetUniform(int id, float value)
        {
            GL.UseProgram(programID);
            GL.Uniform1(id, value);
            GL.UseProgram(0);
        }

        public void SetUniform(int id, Vector3 value)
        {
            GL.UseProgram(programID);
            GL.Uniform3(id, value);
            GL.UseProgram(0);
        }

        public void SetUniform(int id, Color value)
        {
            GL.UseProgram(programID);
            GL.Uniform3(id, value.r, value.g, value.b);
            GL.UseProgram(0);
        }

        public void SetUniform(int id, Matrix4 value)
        {
            GL.UseProgram(programID);
            GL.UniformMatrix4(id, false, ref value);
            GL.UseProgram(0);
        }

        public int GetUniformLocation(string name)
        {
            GL.UseProgram(programID);
            int location = GL.GetUniformLocation(programID, name);
            GL.UseProgram(0);
            return location;
        }
    }
}
