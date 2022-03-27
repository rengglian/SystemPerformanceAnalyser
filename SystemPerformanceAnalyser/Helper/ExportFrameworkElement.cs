using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SystemPerformanceAnalyser.Helper
{
    public class ExportFrameworkElement
    {
        public static void AsPng(object? obj, string filename)
        {
            if (obj is not FrameworkElement frameworkElement)
            {
                throw new Exception("Invalid parameter");
            }

            var heightWithMargin = (int)Math.Ceiling(frameworkElement.RenderSize.Height);
            var widthWithMargin = (int)Math.Ceiling(frameworkElement.RenderSize.Width);
            RenderTargetBitmap renderTargetBitmap = new(widthWithMargin, heightWithMargin, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(frameworkElement);
            PngBitmapEncoder pngImage = new();
            pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            using Stream fileStream = File.Create(filename);
            pngImage.Save(fileStream);
        }
    }
}
