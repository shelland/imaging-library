using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Shelland.ImagingLibrary.Internal;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class HueCommand: IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private float _hueFactor = 0;

        public HueCommand() { }

        public HueCommand(Bitmap srcBitmap, float hueFactor)
        {
            _srcBitmap = srcBitmap;
            _hueFactor = hueFactor;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {


            if (_srcBitmap == null)
            {
                throw new ILException("Input bitmap can not be null");
            }

            int width = _srcBitmap.Width;
            int height = _srcBitmap.Height;

            Rectangle rc = new Rectangle(0, 0, width, height);

            if (_srcBitmap.PixelFormat != PixelFormat.Format24bppRgb) _srcBitmap = _srcBitmap.Clone(rc, PixelFormat.Format24bppRgb);

            Bitmap dest = new Bitmap(width, height, _srcBitmap.PixelFormat);

            BitmapData dataSrc = _srcBitmap.LockBits(rc, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dataDest = dest.LockBits(rc, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int offset = dataSrc.Stride - (width * 3);

            unsafe
            {
                byte* bytesSrc = (byte*)(void*)dataSrc.Scan0;
                byte* bytesDest = (byte*)(void*)dataDest.Scan0;

                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        HSL hsl = HSL.FromRGB(bytesSrc[2], bytesSrc[1], bytesSrc[0]); // Still BGR
                        hsl.Hue *= _hueFactor;

                        Color c = hsl.RGB;

                        bytesDest[0] = c.B;
                        bytesDest[1] = c.G;
                        bytesDest[2] = c.R;

                        bytesSrc += 3;
                        bytesDest += 3;
                    }

                    bytesDest += offset;
                    bytesSrc += offset;
                }

                _srcBitmap.UnlockBits(dataSrc);
                dest.UnlockBits(dataDest);
            }

            return dest;

        }

    }
}
