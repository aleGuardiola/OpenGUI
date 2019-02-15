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
        

        public ImageSource ImageSource
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
            subscriptionPool.Add( GetObservable<ImageSource>(nameof(ImageSource)).Subscribe(imageSourceChanged) );
            subscriptionPool.Add(GetObservable<ImageMode>(nameof(ImageMode)).Subscribe((mode) => ForzeDraw()));

            SetValue<bool>(nameof(IsLoading), ReactiveObject.LAYOUT_VALUE, false);
            SetValue<ImageMode>(nameof(ImageMode), ReactiveObject.LAYOUT_VALUE, ImageMode.Fit);


        }

        private void imageSourceChanged(ImageSource source)
        {
            if(!source.TryGetImagePlaceholder((int)Width, (int)Height, out imageToRender))            
                imageToRender = new SKBitmap(0,0);

            ForzeDraw();

            Task.Run(async () =>
            {
                IsLoading = true;
                imageToRender = await source.GetImage((int)Width, (int)Height);
                IsLoading = false;
                ForzeDraw();
            });            
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
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
                    width = Math.Max(PaddingLeft + PaddingRight + imageToRender.Width, minWidth);
                    if (width > widthSpec)
                        width = Math.Max(widthSpec, minWidth);

                    //calculate height
                    height = Math.Max(PaddingTop + PaddingBottom + imageToRender.Height, minHeight);
                    if (height > heightSpec)
                        height = Math.Max(heightSpec, minHeight);
                    break;
                case MeasureSpecMode.Unspecified:

                    //calculate width
                    width = Math.Max(PaddingLeft + PaddingRight + imageToRender.Width, minWidth);

                    //calculate height
                    height = Math.Max(PaddingTop + PaddingBottom + imageToRender.Height, minHeight);
                    break;
            }

            return (width, height);
        }

        protected override void DrawTexture(SKCanvas canvas, int width, int height)
        {
            base.DrawTexture(canvas, width, height);

            if (imageToRender == null)
                return;

            var destRect = new SKRect(PaddingLeft, PaddingTop, width - PaddingRight, height - PaddingBottom);

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
