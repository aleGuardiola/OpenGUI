using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES30;

namespace OpenGui.GUICore
{
    public class ViewModelProvider : IModelProvider
    {
        private int VAO;
        private int VBO;
        
        public ViewModelProvider()
        {
            CreateTriangle();
        }

        public void Draw()
        {
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);
        }

        private void CreateTriangle()
        {
            float[] vertices =
            {
                //triangle one
                //x      y      z       u      v
                 -1.0f,  1.0f,  0.0f,   0.0f,  0.0f,
                 -1.0f, -1.0f,  0.0f,   0.0f,  1.0f,
                  1.0f,  1.0f,  0.0f,   1.0f,  0.0f,

                //triangle two
                //x      y      z       u      v
                  1.0f,  1.0f,  0.0f,   1.0f,  0.0f,
                 -1.0f, -1.0f,  0.0f,   0.0f,  1.0f,
                  1.0f, -1.0f,  0.0f,   1.0f,  1.0f
            };


            GL.GenVertexArrays(1, out VAO);
            GL.BindVertexArray(VAO);

            GL.GenBuffers(1, out VBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * vertices.Length, vertices, BufferUsageHint.StaticDraw);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, sizeof(float) * 3);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
    }
}
