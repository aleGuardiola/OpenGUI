﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Exceptions;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGui.OpenGLHelpers
{
    /// <summary>
    /// Represent a Program that contains multiple shaders.
    /// </summary>
    public class ShaderProgram
    {
        //store the id of the current program in use
        static int ProgramUsing = 0;

        //The id of the program
        int programId;

        //Uniform values of the program
        Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();

        /// <summary>
        /// Get the id generated by OpenGl
        /// </summary>
        public int Id { get => programId; }

        /// <summary>
        /// Create and Link program.
        /// </summary>
        /// <param name="shaders">the shaders to use in this program.</param>
        public ShaderProgram(params Shader[] shaders)
        {
            //Create the program
            programId = GL.CreateProgram();

            if (programId == 0)
                throw new CantCreateProgramException();

            //Attach shaders to the program
            for(int i = 0; i < shaders.Length; i++)            
              GL.AttachShader(programId, shaders[i].Id);

            GL.LinkProgram(programId);

            int linkStatusResult;
            GL.GetProgram(programId, GetProgramParameterName.LinkStatus, out linkStatusResult);

            if(linkStatusResult == 0)
            {
                string infoLog;
                GL.GetProgramInfoLog(programId, out infoLog);
                throw new LinkProgramFaildException(programId, infoLog);
            }

            //Get uniform values

        }
        
        /// <summary>
        /// Set uniform matrix 4.
        /// </summary>
        /// <param name="name">name of the uniform value.</param>
        /// <param name="matrix4">the matrix 4.</param>
        public void SetUniformMatrix4(string name, Matrix4 matrix4)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (ProgramUsing != programId)
                throw new InvalidOperationException("The current program is not running");

            var uniformLocation = GetUniformLocation(name);

            GL.UniformMatrix4(uniformLocation, false, ref matrix4);
        }

        /// <summary>
        /// Set uniform value to be used in the shader.
        /// </summary>
        /// <param name="name">Name of the uniform value</param>
        /// <param name="value"></param>
        public void SetUniform1Value(string name, float value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (ProgramUsing != programId)
                throw new InvalidOperationException("The current program is not running");

            var uniformLocation = GetUniformLocation(name);

            GL.Uniform1(uniformLocation, value);
        }

        /// <summary>
        /// Set uniform value to be used in the shader.
        /// </summary>
        /// <param name="name">Name of the uniform value</param>
        /// <param name="value"></param>
        public void SetUniform1Value(string name, double value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (ProgramUsing != programId)
                throw new InvalidOperationException("The current program is not running");

            var uniformLocation = GetUniformLocation(name);

            GL.Uniform1(uniformLocation, value);
        }

        /// <summary>
        /// Set uniform value to be used in the shader.
        /// </summary>
        /// <param name="name">Name of the uniform value</param>
        /// <param name="value"></param>
        public void SetUniform1Value(string name, int value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (ProgramUsing != programId)
                throw new InvalidOperationException("The current program is not running");

            var uniformLocation = GetUniformLocation(name);

            GL.Uniform1(uniformLocation, value);
        }
                
        private int GetUniformLocation(string name)
        {            
            int uniformLocation;
            var exist = _uniformLocations.TryGetValue(name, out uniformLocation);

            //Get the value if not exist
            if (!exist)
            {
                uniformLocation = GL.GetUniformLocation(programId, name);

                //if the name is not in the shader
                if (uniformLocation == -1)
                    throw new UniformLocationNotFound(name, programId);

                _uniformLocations.Add(name, uniformLocation);
            }

            return uniformLocation;
        }

        /// <summary>
        /// Use the program in GL context.
        /// </summary>
        public void StartUsing()
        {
            GL.UseProgram( ProgramUsing = programId );
        }

        /// <summary>
        /// Stop using the program in GL context.
        /// </summary>
        public void StopUsing()
        {
            GL.UseProgram( ProgramUsing = 0);
        }

    }
}
