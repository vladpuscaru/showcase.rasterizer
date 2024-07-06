using System.Globalization;
using System.Runtime.InteropServices;
using Dear_ImGui_Sample;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using UV_Practical3;

// The template provides you with a window which displays a 'linear frame buffer', i.e.
// a 1D array of pixels that represents the graphical contents of the window.

// Under the hood, this array is encapsulated in a 'Surface' object, and copied once per
// frame to an OpenGL texture, which is then used to texture 2 triangles that exactly
// cover the window. This is all handled automatically by the template code.

// Before drawing the two triangles, the template calls the Tick method in MyApplication,
// in which you are expected to modify the contents of the linear frame buffer.

// After (or instead of) rendering the triangles you can add your own OpenGL code.

// We will use both the pure pixel rendering as well as straight OpenGL code in the
// tutorial. After the tutorial you can throw away this template code, or modify it at
// will, or maybe it simply suits your needs.

namespace Template
{
    public class OpenTKApp : GameWindow
    {
        ImGuiController _controller;
        public const bool allowPrehistoricOpenGL = false; // not supported on MacOS
        public static readonly bool isMac = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        static int screenID;            // unique integer identifier of the OpenGL texture
        static MyApplication? app;       // instance of the application
        static bool terminated = false; // application terminates gracefully when this is true

        ScreenQuad? quad;
        Shader? screenShader;

        public OpenTKApp()
            : base(GameWindowSettings.Default, new NativeWindowSettings()
            {
                ClientSize = new Vector2i(1980, 720),
                Profile = (allowPrehistoricOpenGL && !isMac) ? ContextProfile.Compatability : ContextProfile.Core,
                Flags = (isMac ? ContextFlags.Default : ContextFlags.Debug)
                    | ((allowPrehistoricOpenGL && !isMac) ? ContextFlags.Default : ContextFlags.ForwardCompatible),
            })
        {
        }

        // Debug output (not supported on MacOS)
        public void DebugCallback(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            Dictionary<DebugSource, string> sourceStrings = new Dictionary<DebugSource, string>
            {
                { DebugSource.DebugSourceApi, "API - A call to the OpenGL API" },
                { DebugSource.DebugSourceWindowSystem, "Window System - A call to a window system API" },
                { DebugSource.DebugSourceShaderCompiler, "Shader Compiler" },
                { DebugSource.DebugSourceThirdParty, "Third Party - A third party application associated with OpenGL" },
                { DebugSource.DebugSourceApplication, "Application - A call to GL.DebugMessageInsert() in this application" },
                { DebugSource.DebugSourceOther, "Other" },
                { DebugSource.DontCare, "Ignored" },
            };
            string? sourceString;
            if (!sourceStrings.TryGetValue(source, out sourceString)) sourceString = "Unknown";
            string? typeString = Enum.GetName(type);
            if (typeString != null) typeString = typeString.Substring(9);
            string? severityString = Enum.GetName(severity);
            if (severityString != null) severityString = severityString.Substring(13);
            Console.Error.WriteLine("OpenGL Error:\n  Source: " + sourceString + "\n  Type: " + typeString + "\n  Severity: " + severityString
                + "\n  Message ID: " + id + "\n  Message: " + Marshal.PtrToStringAnsi(message, length) + "\n");
        } // put a breakpoint here and inspect the stack to pinpoint where the error came from

        protected override void OnLoad()
        {
            base.OnLoad();
            // called during application initialization
            Console.WriteLine("OpenGL Version: " + GL.GetString(StringName.Version) + " (" + (Profile == ContextProfile.Compatability ? "Compatibility" : Profile) + " profile)");
            Console.WriteLine("OpenGL Renderer: " + GL.GetString(StringName.Renderer) + (GL.GetString(StringName.Vendor) == "Intel" ? " (read DiscreteGPU.txt if you have another GPU that you would like to use)" : ""));
            // configure debug output (not supported on MacOS)
            if (!isMac)
            {
                GL.Enable(EnableCap.DebugOutput);
                // disable all debug messages
                GL.DebugMessageControl(DebugSourceControl.DontCare, DebugTypeControl.DontCare, DebugSeverityControl.DontCare, 0, new int[0], false);
                // enable selected debug messages based on source, type, and severity
                foreach (DebugSourceControl source in new DebugSourceControl[] { DebugSourceControl.DebugSourceApi, DebugSourceControl.DebugSourceShaderCompiler })
                {
                    foreach (DebugTypeControl type in new DebugTypeControl[] { DebugTypeControl.DebugTypeError, DebugTypeControl.DebugTypeDeprecatedBehavior, DebugTypeControl.DebugTypeUndefinedBehavior, DebugTypeControl.DebugTypePortability })
                    {
                        foreach (DebugSeverityControl severity in new DebugSeverityControl[] { DebugSeverityControl.DebugSeverityHigh })
                        {
                            GL.DebugMessageControl(source, type, severity, 0, new int[0], true);
                        }
                    }
                }
                GL.DebugMessageCallback(DebugCallback, (IntPtr)0);
            }
            // prepare for rendering
            GL.ClearColor(0, 0, 0, 0);
            GL.Disable(EnableCap.DepthTest);
            Surface screen = new(ClientSize.X, ClientSize.Y);
            _controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            app = new MyApplication(screen, KeyboardState);
            screenID = app.screen.GenTexture();
            if (allowPrehistoricOpenGL)
            {
                GL.Enable(EnableCap.Texture2D);
                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            }
            else
            {
                quad = new ScreenQuad();
                screenShader = new Shader(Utils.GetPathFromSrc("shaders/screen_vs.glsl"), Utils.GetPathFromSrc("shaders/screen_fs.glsl"));
            }
            app.Init();
        }
        protected override void OnUnload()
        {
            base.OnUnload();
            // called upon app close
            GL.DeleteTextures(1, ref screenID);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            // called upon window resize. Note: does not change the size of the pixel buffer.
            int retinaScale = isMac ? 2 : 1; // this code assumes all Macs have retina displays
            GL.Viewport(0, 0, retinaScale * e.Width, retinaScale * e.Height);
            _controller.WindowResized(retinaScale * e.Width, retinaScale * e.Height);
            if (allowPrehistoricOpenGL)
            {
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
            }
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            // called once per frame; app logic
            var keyboard = KeyboardState;
            if (keyboard[Keys.Escape]) terminated = true;
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _controller.Update(this, (float)e.Time);

            // called once per frame; render
            if (app != null) app.Tick();

            if (terminated)
            {
                Close();
                return;
            }
            // convert MyApplication.screen to OpenGL texture
            if (app != null)
            {
                GL.ClearColor(Color4.Black);
                GL.Disable(EnableCap.DepthTest);
                GL.BindTexture(TextureTarget.Texture2D, screenID);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                               app.screen.width, app.screen.height, 0,
                               PixelFormat.Bgra,
                               PixelType.UnsignedByte, app.screen.pixels
                             );
                if (allowPrehistoricOpenGL)
                {
                    GL.Enable(EnableCap.Texture2D);
                    GL.Color3(1.0f, 1.0f, 1.0f);
                    // draw screen filling quad
                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.Begin(PrimitiveType.Quads);
                    GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(1.0f, -1.0f);
                    GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(1.0f, 1.0f);
                    GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-1.0f, 1.0f);
                    GL.End();
                    GL.Disable(EnableCap.Texture2D);
                }
                else
                {
                    if (quad != null && screenShader != null)
                        quad.Render(screenShader, screenID);
                }
                // prepare for generic OpenGL rendering
                GL.Enable(EnableCap.DepthTest);
                GL.Clear(ClearBufferMask.DepthBufferBit);
                // do OpenGL rendering
                app.RenderGL((float)e.Time);
            }

            // ImGui.ShowDemoWindow();
            // ImGuiController.SubmitUI();

            ImGuiController.CheckGLError("End of frame");

            _controller.Render();

            // tell OpenTK we're done rendering
            SwapBuffers();
        }
        private static void StartApp()
        {
            // entry point
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            using OpenTKApp app = new();
            app.UpdateFrequency = 30.0;
            app.Run();
        }

        private static void GenerateNormalMaps()
        {
            /**
             * Takes every .jpg texture in the assets folder
             * and computes:
             * - a greyscale version which represents the heightmap
             * - based on the heightmap, a normal map
             * These are then saved as textures in the generated folder, under assets
             */
            List<Surface> surfaces = IOUtils.ReadAllTextures();
            for (int i = 0; i < surfaces.Count; i++)
            {
                Surface surface = surfaces[i];

                SKBitmap bitmap = Utils.CreateBitmapFromIntArray(surface.pixels, surface.width, surface.height);
                SKBitmap heightMap = Generator.GenerateHeightMap(bitmap);
                IOUtils.SaveSurface("heightmap_" + i + ".jpg", heightMap);

                SKBitmap normalMap = Generator.GenerateNormalMap(heightMap);
                IOUtils.SaveSurface("normalmap_" + i + ".jpg", normalMap);
            }
        }

        public static void Main()
        {
            int mode = 0;

            switch (mode)
            {
                case 0:
                    StartApp();
                    break;
                case 1:
                    GenerateNormalMaps();
                    break;
            }
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);


            _controller.PressChar((char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _controller.MouseScroll(e.Offset);
        }
    }
}