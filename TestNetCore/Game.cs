using OpenGui.OpenGLHelpers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestNetCore
{
    public class Game
    {
        int VAO, VBO;
        ShaderProgram shader;
        Triangle triangle;

        const string vShader = @"
           #version 330

           layout (location = 0) in vec3 pos;

           void main()
           {
               gl_Position = vec4(0.4 * pos.x, 0.4 * pos.y, 0.4 * pos.z, 1.0);
           }
        ";

        const string fShader = @"
           #version 330

           out vec4 colour;

           void main()
           {
               colour = vec4(1.0, 0.0, 0.0, 1.0);
           }
        ";

        GameWindow window;
        public Game(GameWindow window)
        {
            this.window = window;

            window.Load += Window_Load;
            window.UpdateFrame += Window_UpdateFrame;
            window.RenderFrame += Window_RenderFrame;

        }

        private void AddShader(int program, string code, ShaderType type)
        {
            var theShader = GL.CreateShader(type);

            GL.ShaderSource(theShader, code);
            GL.CompileShader(theShader);

            GL.AttachShader(program, theShader);

        }
        
        private void CreateTriangle()
        {
            float[] vertices =
            {
                -1.0f, -1.0f, 0.0f,
                1.0f, -1.0f, 0.0f,
                0.0f, 1.0f, 0.0f
            };

            GL.GenVertexArrays(1, out VAO);
            GL.BindVertexArray(VAO);

            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);

            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.BindVertexArray(0);

        }

        float cameraMoveCount = 0;

        private void Window_RenderFrame(object sender, FrameEventArgs e)
        {
            GL.ClearColor(Color.Gray);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            triangle.GLDraw(camera.Projection, camera.View);

            //shader.StartUsing();
            //GL.BindVertexArray(VAO);
            //GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            //GL.BindVertexArray(0);
            //shader.StopUsing();

            window.SwapBuffers();
        }

        private void Window_UpdateFrame(object sender, FrameEventArgs e)
        {
            
        }

        Camera camera;
        
        private void Window_Load(object sender, EventArgs e)
        {
            //CreateTriangle();

            //var vertexShader = new VertexShader(vShader);
            //var fragmentShader = new FragmentShader(fShader);

            //shader = new ShaderProgram(vertexShader, fragmentShader);

            GL.Enable(EnableCap.DepthTest);

            camera = new Camera(
                new Vector3(0.0f, 0.0f, 3.0f),
                Vector3.UnitY,
                Vector3.Zero,
                Matrix4.CreatePerspectiveFieldOfView((float)MathHelper.DegreesToRadians(45.0), window.Width / window.Height, 0.1f, 100.0f)
                );
            
            triangle = new Triangle();
            triangle.TransformationMatrix = Matrix4.CreateTranslation(0.0f, 0.0f, 0.0f);
            
        }
    }
}
