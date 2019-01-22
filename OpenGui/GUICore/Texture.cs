﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES20;
using SkiaSharp;

namespace OpenGui.GUICore
{
    public class Texture : IDisposable
    {
        int _textureId;
        bool _isLoaded;

        /// <summary>
        /// Get the id of the texture generated by opengl
        /// </summary>
        public int Id
        {
            get => _textureId;
        }

        /// <summary>
        /// Return true if the texture is loaded.
        /// </summary>
        public bool IsLoaded
        {
            get => _isLoaded;
        }

        public Texture()
        {
            
        }

        /// <summary>
        /// Load and create the texture
        /// </summary>
        /// <param name="bitmap"></param>
        public void LoadTexture(SKBitmap bitmap)
        {
            GL.GenTextures(1, out _textureId);
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)All.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)All.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)All.Linear);

            GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bitmap.GetPixels());
            GL.GenerateMipmap(TextureTarget.Texture2D);
            _isLoaded = true;
        }
        
        /// <summary>
        /// Change the texture data.
        /// </summary>
        /// <param name="bitmap">The new data.</param>
        public void ChangeBitmap(SKBitmap bitmap)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.TexImage2D(TextureTarget2d.Texture2D, 0, TextureComponentCount.Rgba, bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, bitmap.Bytes);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        /// <summary>
        /// Change the data with a same size bitmap, faster than Change Bitmap function
        /// </summary>
        /// <param name="bitmap">The new data.</param>
        public void ChangeBitmapSameSize(SKBitmap bitmap)
        {
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
            GL.TexSubImage2D(TextureTarget2d.Texture2D, 0, 0, 0, bitmap.Width, bitmap.Height, PixelFormat.Rgba, PixelType.Byte, bitmap.Bytes);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void StartUsingTexture()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
        }

        public void StopUsingTexture()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void Dispose()
        {
            GL.DeleteTexture(_textureId);
        }       
    }
}
