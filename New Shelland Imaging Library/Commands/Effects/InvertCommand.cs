using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    /// <summary>
    /// Represents an image inversion effect
    /// </summary>
    public class InvertCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;

        public InvertCommand(Bitmap srcBitmap)
        {
            _srcBitmap = srcBitmap;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_srcBitmap == null)
            {
                throw new ArgumentException("Input bitmap can not be null");
            }

            BitmapData bmData = _srcBitmap.LockBits(new Rectangle(0, 0, _srcBitmap.Width, _srcBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - _srcBitmap.Width * 3;
                int nWidth = _srcBitmap.Width * 3;

                for (int y = 0; y < _srcBitmap.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        p[0] = (byte)(255 - p[0]);
                        ++p;
                    }
                    p += nOffset;
                }
            }

            _srcBitmap.UnlockBits(bmData);

            return _srcBitmap;
        }

    }
}
