using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace JFPGeneric
{
    public partial class Functions
    {
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Image ResizeImage_FIXEDASPECT(Image image, Int32 maxDimension)
        {
            int w = 0;
            int h = 0;
            int val = image.Width.CompareTo(image.Height);
            if (val == 0) { w = maxDimension; h = maxDimension; }
            else if (val > 0) { w = maxDimension; h = Convert.ToInt32(((float)image.Height / (float)image.Width) * maxDimension); }
            else { h = maxDimension; w = Convert.ToInt32(((float)image.Width / (float)image.Height) * maxDimension); }

            return (Image)ResizeImage(image, w, h);
        }

        public static Byte[] ResizeImage_FIXEDASPECT_Byte(Image image, Int32 maxDimension)
        {
            byte[] retVal = new byte[0];
            Image imgNEW = ResizeImage_FIXEDASPECT(image, maxDimension);
            using (MemoryStream ms = new MemoryStream())
            {
                imgNEW.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                retVal = ms.ToArray();
            }
            return retVal;
        }

        public static Byte[] ResizeImage_FIXEDASPECT(Byte[] imageBytes, Int32 maxDimension)
        {
            byte[] retVal = new byte[0];
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image imgNEW = ResizeImage_FIXEDASPECT(Image.FromStream(ms), maxDimension);
                using (MemoryStream ms2 = new MemoryStream())
                {
                    imgNEW.Save(ms2, System.Drawing.Imaging.ImageFormat.Png);
                    retVal = ms2.ToArray();
                }
            }
            return retVal;
        }

        public static void ResizeImage_FIXEDASPECT(Stream imageStream, Int32 maxDimension)
        {
            Image imgNEW = ResizeImage_FIXEDASPECT(Image.FromStream(imageStream), maxDimension);
            imgNEW.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
        }

        public static Stream ResizeImage_FIXEDASPECT_Stream(Stream imageStream, Int32 maxDimension)
        {
            Image imgNEW = ResizeImage_FIXEDASPECT(Image.FromStream(imageStream), maxDimension);
            imgNEW.Save(imageStream, System.Drawing.Imaging.ImageFormat.Png);
            return imageStream;
        }
    }
}
