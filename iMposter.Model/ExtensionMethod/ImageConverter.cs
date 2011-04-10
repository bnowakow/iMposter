using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Emgu.CV;
using System.Runtime.InteropServices;
using System.Windows;
using Emgu.CV.Structure;
using System.Drawing;

namespace iMposter.Model.ExtensionMethod
{
    public static class ImageConverter
    {
        public static BitmapSource ToBitmapSource(this Image<Bgr, byte> image)
        {
            return IImageToBitmapSource(image);
        }

        public static Int32Rect ToInt32Rect(this Rectangle rectangle)
        {
            return new Int32Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr obj);

        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>
        public static BitmapSource IImageToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr imagePointer = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    imagePointer,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(imagePointer); //release the HBitmap
                return bitmapSource;
            }
        }
    }
}