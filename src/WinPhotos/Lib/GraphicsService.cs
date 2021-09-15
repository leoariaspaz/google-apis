using System;
using System.Drawing;

namespace WinPhotos.Lib
{
    public class GraphicsService
    {
        public Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
                if (newImage.Width < maxWidth || newImage.Height < maxHeight)
                {
                    Bitmap newImage2 = new Bitmap(maxWidth, maxHeight, image.PixelFormat);
                    using (Graphics g = Graphics.FromImage(newImage2))
                    {
                        // fill target image with color
                        g.FillRectangle(Brushes.Black, 0, 0, maxWidth, maxHeight);

                        // place source image inside the target image
                        var dstX = (maxWidth - newImage.Width) / 2;
                        var dstY = (maxHeight - newImage.Height) / 2;
                        g.DrawImage(newImage, dstX, dstY);
                    }
                    return newImage2;
                }
            }
            return newImage;
        }
    }
}
