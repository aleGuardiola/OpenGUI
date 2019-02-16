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
            if (_cachedImage == null)
            {
                var stream = _client.GetStreamAsync(_url).GetAwaiter().GetResult();
                _cachedImage = SKBitmap.Decode(stream);
            }

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

        public override bool IsCached(int width, int height)
        {
            return _cachedImage != null;
        }

        ~WebImageSource()
        {
            _client.Dispose();
        }
    }
}
