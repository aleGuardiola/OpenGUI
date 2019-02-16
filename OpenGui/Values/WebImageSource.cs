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
        SKBitmap _cachedImage = null;
        string _url;

        public WebImageSource(string url)
        {
            _url = url;
            _client = new HttpClient();            
        }

        public override async Task<SKBitmap> GetImage(int width, int height)
        {
            if(_cachedImage == null)
            {
                var stream = await _client.GetStreamAsync(_url);
                _cachedImage = SKBitmap.Decode(stream);
            }

            return _cachedImage;
        }

        ~WebImageSource()
        {
            _client.Dispose();
        }
    }
}
