using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace OpenGui.Values
{
    public class WebImageSource : ImageSource
    {
        HttpClient _client;
        SKBitmap _placeholder;
        string _url;

        public WebImageSource(string url, SKBitmap placeholder) : this(url)
        {
            _placeholder = placeholder;            
        }

        public WebImageSource(string url)
        {
            _url = url;
            _client = new HttpClient();            
        }

        public override async Task<SKBitmap> GetImage(int width, int height)
        {
            var stream = await _client.GetStreamAsync(_url);
            var bitmap = SKBitmap.Decode(stream);
            return bitmap;
        }

        public override bool TryGetImagePlaceholder(int width, int height, out SKBitmap placeholder)
        {
            placeholder = _placeholder;
            return placeholder != null;
        }

        ~WebImageSource()
        {
            _client.Dispose();
        }
    }
}
