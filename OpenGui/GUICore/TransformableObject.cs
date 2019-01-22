using OpenGui.OpenGLHelpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OpenGui.GUICore
{
    /// <summary>
    /// Object that support transformations.
    /// </summary>
    public abstract class TransformableObject : IDrawableGLObject
    {        
        //shader program used for transformable objects
        static ShaderProgram ShaderProgram;
        const string transmormationMatrixUnif = "transfromMatrix";
        const string perspectivenMatrixUnif = "perspectivenMatrix";
        const string viewMatrixUnif = "view";
        const string RectWidth = "rectWidth";
        const string RectHeight = "rectHeight";
        const string RectPosX = "rectPosX";
        const string RectPosY = "rectPosY";

        //Create the shader program used in Transformable objects
        static TransformableObject()
        {
            string vShaderCode;
            string fShaderCode;

            //Get the assembly
            var assembly = Assembly.GetExecutingAssembly();

            //Get the shader codes
            using (var stream = assembly.GetManifestResourceStream("Shaders.Transformable.Vert"))
            using (var reader = new StreamReader(stream))
                vShaderCode = reader.ReadToEnd();

            using (var stream = assembly.GetManifestResourceStream("Shaders.Transformable.Frag"))
            using (var reader = new StreamReader(stream))
                fShaderCode = reader.ReadToEnd();

            //Create the shaders
            var fShader = new FragmentShader(fShaderCode);
            var vShader = new VertexShader(vShaderCode);

            //Create the program
            ShaderProgram = new ShaderProgram(fShader, vShader);
        }

        RectangleF _clipRectangle;
        Matrix4 _transformationMatrix = Matrix4.CreateScale(1);

        /// <summary>
        /// The transformation matrix used to transform the object
        /// </summary>
        public Matrix4 TransformationMatrix
        {
            get => _transformationMatrix;
            set => _transformationMatrix = value;
        }

        /// <summary>
        /// Get or set the rectangle where the object will be drawn, 
        /// any part outside the rectangle will be discarted by the fragment shader.
        /// </summary>
        public RectangleF ClipRectangle
        {
            get => _clipRectangle;
            set => _clipRectangle = value;
        }

        
        /// <summary>
        /// Draw the object in the GL context.
        /// </summary>
        public void GLDraw(Matrix4 projection, Matrix4 view)
        {
            //Use the program
            ShaderProgram.StartUsing();

            //Set transformation values
            ShaderProgram.SetUniformMatrix4(transmormationMatrixUnif, _transformationMatrix);
            //Set perspective projection
            ShaderProgram.SetUniformMatrix4(perspectivenMatrixUnif, projection);
            //Set view
            ShaderProgram.SetUniformMatrix4(viewMatrixUnif, view);

            //Set the clip rectangle to the shader
            ShaderProgram.SetUniform1Value(RectWidth, _clipRectangle.Width);
            ShaderProgram.SetUniform1Value(RectHeight, _clipRectangle.Height);
            ShaderProgram.SetUniform1Value(RectPosX, _clipRectangle.X);
            ShaderProgram.SetUniform1Value(RectPosY, _clipRectangle.Y);


            //Draw the vertex
            DrawVertex();
            
            //Stop using the shader program
            ShaderProgram.StopUsing();
        }

        /// <summary>
        /// Draw vertex.
        /// </summary>
        protected abstract void DrawVertex();

        public void Draw(Matrix4 projectionMatrix, Matrix4 view)
        {
            GLDraw(projectionMatrix, view);
        }
    }
}
