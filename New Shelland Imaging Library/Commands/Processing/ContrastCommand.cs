using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Effects
{

    /// <summary>
    /// Represents a contrast command
    /// </summary>
    public class ContrastCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private sbyte _level = 0;

        public ContrastCommand(Bitmap srcBitmap, sbyte contrastLevel)
        {
            _srcBitmap = srcBitmap;
            _level = contrastLevel;
        }

        /// <summary>
        /// Gets/sets a contrast level
        /// </summary>
        public sbyte ContrastLevel
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_srcBitmap == null)
            {
                throw new ArgumentException("Input bitmap can not be null");
            }
            if (_level < -100 || _level > 100)
            {
                throw new ArgumentException("Contrast level should be between -100 and 100");
            }

            double pixel = 0, contrast = (100.0 + _level) / 100.0;
            contrast *= contrast;
            int red, green, blue;
            BitmapData bmData = _srcBitmap.LockBits(new Rectangle(0, 0, _srcBitmap.Width, _srcBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - _srcBitmap.Width * 3;

                for (int y = 0; y < _srcBitmap.Height; ++y)
                {
                    for (int x = 0; x < _srcBitmap.Width; ++x)
                    {
                        blue = p[0];
                        green = p[1];
                        red = p[2];

                        pixel = red / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[2] = (byte)pixel;

                        pixel = green / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[1] = (byte)pixel;

                        pixel = blue / 255.0;
                        pixel -= 0.5;
                        pixel *= contrast;
                        pixel += 0.5;
                        pixel *= 255;
                        if (pixel < 0) pixel = 0;
                        if (pixel > 255) pixel = 255;
                        p[0] = (byte)pixel;

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
