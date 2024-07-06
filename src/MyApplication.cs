using System.Diagnostics;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using UV_Practical3;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        Mesh? teapot, floor;                    // meshes to draw using OpenGL
        float a = 0;                            // teapot rotation angle
        readonly Stopwatch timer = new();       // timer for measuring frame duration
        Shader? shader;                         // shader to use for rendering
        Shader? postproc;                       // shader to use for post processing
        Texture? wood;                          // texture to use for rendering
        RenderTarget? target;                   // intermediate render target
        ScreenQuad? quad;                       // screen filling quad for post processing
        readonly bool useRenderTarget = true;   // required for post processing

        KeyboardState keyboardState;

        private List<Scene> scenes;
        private int activeSceneIdx = 0;

        // constructor
        public MyApplication(Surface screen, KeyboardState keyboardState)
        {
            this.screen = screen;
            this.keyboardState = keyboardState;
        }
        // initialize
        public void Init()
        {
            // initialize stopwatch
            timer.Reset();
            timer.Start();

            // create the render target
            if (useRenderTarget) target = new RenderTarget(screen.width, screen.height);
            quad = new ScreenQuad();

            scenes = new List<Scene>();
            Scene mainScene = new SceneMain((float)screen.width / screen.height, this.keyboardState);
            Scene spotLightScene = new SceneSpotlights((float)screen.width / screen.height, this.keyboardState);
            Scene pointLightScene = new ScenePointLights((float)screen.width / screen.height, this.keyboardState);
            // Scene fpsScene = new SceneFPS((float)screen.width / screen.height, this.keyboardState);

            scenes.Add(mainScene);
            scenes.Add(spotLightScene);
            scenes.Add(pointLightScene);
            // scenes.Add(fpsScene);
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0);
            // screen.Print("hello world", 2, 2, 0xffff00);
        }

        // tick for OpenGL rendering code
        public void RenderGL(float deltaTime)
        {
            // measure frame duration
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            if (scenes.Count > 0)
            {
                scenes[activeSceneIdx].Input(deltaTime);
                scenes[activeSceneIdx].Update();
                scenes[activeSceneIdx].Render();
                scenes[activeSceneIdx].SubmitUI(ref activeSceneIdx, ref scenes);
            }
        }
    }
}