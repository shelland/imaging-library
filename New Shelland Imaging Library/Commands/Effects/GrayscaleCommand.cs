using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class GrayscaleCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;

        public GrayscaleCommand(Bitmap srcBitmap)
        {
            _srcBitmap = srcBitmap;
        }

        public GrayscaleCommand() { }

        public Bitmap SourceImage
        {
            get
            {
                return _srcBitmap;
            }
            set
            {
                _srcBitmap = value;
            }
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

                byte red, green, blue;

                for (int y = 0; y < _srcBitmap.Height; ++y)
                {
                    for (int x = 0; x < _srcBitmap.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        p[0] = p[1] = p[2] = (byte)(.299 * red + .587 * green + .114 * blue);

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            _srcBitmap.UnlockBits(bmData);

            return _srcBitmap;
        }


    }
}
