using OpenGui.Core;
using OpenGui.Helpers;
using OpenGui.Values;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenGui.Controls
{
    public class Image : View
    {
        SubscriptionPool subscriptionPool = new SubscriptionPool();
        SKBitmap imageToRender;
        int lastWidth = 0, lastHeight = 0;
        int imageWidth = 0, imageHeight = 0;

        public ImageSource Source
        {
            get => GetValue<ImageSource>();
            set => SetValue<ImageSource>(value);
        }
         
        public bool IsLoading
        {
            get => GetValue<bool>();
            set => SetValue<bool>(value);
        }

        public ImageMode ImageMode
        {
            get => GetValue<ImageMode>();
            set => SetValue<ImageMode>(value);
        }

        public Image()
        {
            //subscribe to changes in the image source
            subscriptionPool.Add( GetObservable<ImageSource>(nameof(Source)).Subscribe(imageSourceChanged) );
            subscriptionPool.Add(GetObservable<ImageMode>(nameof(ImageMode)).Subscribe((mode) => ForzeDraw()));
            
            SetValue<bool>(nameof(IsLoading), ReactiveObject.SYSTEM_VALUE, false);
            SetValue<ImageMode>(nameof(ImageMode), ReactiveObject.SYSTEM_VALUE, ImageMode.Fit);
        }

        private void imageSourceChanged(ImageSource source)
        {
            if (source == null)
                return;

            if (source.IsCached(imageWidth, imageHeight))
            {
                imageToRender = source.GetImage(imageWidth, imageHeight).GetAwaiter().GetResult();
                return;
            }

            if (!IsAttachedToWindows)
                return;

            imageToRender = null;

            ForzeDraw();

            IsLoading = true;

            Task.Run(async () =>
            {
                var image = await source.GetImage(imageWidth, imageHeight);

                Window.RunInUIThread(() =>
                {
                    imageToRender = image;

                    IsLoading = false;

                    ForzeDraw();
                });
            });            
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            var imageWidth = imageToRender?.Width ?? 0;
            var imageHeight = imageToRender?.Height ?? 0;

            var minWidth = MinWidth;
            var minHeight = MinHeight;

            float width = 0;
            float height = 0;

            switch (mode)
            {
                case MeasureSpecMode.Exactly:
                    width = Math.Max(widthSpec, minWidth);
                    height = Math.Max(heightSpec, minWidth);
                    break;
                case MeasureSpecMode.AtMost:
                    //calculate width                    
                    width = Math.Max(PaddingLeft + PaddingRight + imageWidth, minWidth);
                    if (width > widthSpec)
                        width = Math.Max(widthSpec, minWidth);

                    //calculate height
                    height = Math.Max(PaddingTop + PaddingBottom + imageHeight, minHeight);
                    if (height > heightSpec)
                        height = Math.Max(heightSpec, minHeight);
                    break;
                case MeasureSpecMode.Unspecified:

                    //calculate width
                    width = Math.Max(PaddingLeft + PaddingRight + imageWidth, minWidth);

                    //calculate height
                    height = Math.Max(PaddingTop + PaddingBottom + imageHeight, minHeight);
                    break;
            }

            return (width, height);
        }

        protected override void DrawTexture(SKCanvas canvas, int width, int height)
        {
            base.DrawTexture(canvas, width, height);

            var destRect = new SKRect(PaddingLeft, PaddingTop, width - PaddingRight, height - PaddingBottom);
            imageWidth = (int)destRect.Width;
            imageHeight = (int)destRect.Height;
            
            if (lastWidth != imageWidth || lastHeight != imageHeight)
                imageSourceChanged(Source);
            
            lastWidth = imageWidth;
            lastHeight = imageHeight;
            
            if (imageToRender == null)
                return;

            using (var imagePaint = new SKPaint())
            {
                SKRect srcRect = new SKRect();
                switch (ImageMode)
                {
                    case ImageMode.Crop:

                        var newHeight = (imageToRender.Width * height) / width;

                        if(newHeight <= imageToRender.Height)
                          srcRect = new SKRect(0, 0, imageToRender.Width, newHeight);
                        else
                        {
                            var newWidth = (width * imageToRender.Height) / height;
                            srcRect = new SKRect(0, 0, newWidth, imageToRender.Height);
                        }

                        break;
                    case ImageMode.Fit:
                        srcRect = new SKRect(0, 0, imageToRender.Width, imageToRender.Height);
                        break;
                }

                canvas.DrawBitmap(imageToRender, srcRect, destRect, imagePaint);


            }

        }

        ~Image()
        {
            //unsubscribe
            subscriptionPool.UnsubscribeAll();
        }

    }
}
