using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Helpers
{
    public class BitmapPool
    {
        private ConcurrentBag<SKBitmap> _objects;

        public BitmapPool()
        {
            _objects = new ConcurrentBag<SKBitmap>();
        }

        SKBitmap GetBitmap(int width, int height)
        {
            throw new Exception();
        }

    }
}
