using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    /// <summary>
    /// Represents a color level customization command
    /// </summary>
    public class ColorCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private int _red = 0;
        private int _green = 0;
        private int _blue = 0;

        public ColorCommand(Bitmap srcBitmap, int redValue, int greenValue, int blueValue)
        {
            _srcBitmap = srcBitmap;
            _red = redValue;
            _green = greenValue;
            _blue = blueValue;
        }

        /// <summary>
        /// Gets/sets red color value
        /// </summary>
        public int RedValue
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
            }
        }

        /// <summary>
        /// Gets/sets green color value
        /// </summary>
        public int GreenValue
        {
            get
            {
                return _green;
            }
            set
            {
                _green = value;
            }
        }

        /// <summary>
        /// Gets/sets blue color value
        /// </summary>
        public int BlueValue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
            }
        }

        /// <summary>
        /// Processes an image with defined settings
        /// </summary>
        /// <param name="srcImage">Source bitmap to process</param>
        /// <returns></returns>
        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_srcBitmap == null)
            {
                throw new ILException("Input bitmap can not be null");
            }

            if (_red < -255 || _red > 255 || _green < -255 || _green > 255 || _blue < -255 || _blue > 255)
            {
                throw new ILException("Input values should be in range of -255 and 255");
            }

            BitmapData bmData = _srcBitmap.LockBits(new Rectangle(0, 0, _srcBitmap.Width, _srcBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - _srcBitmap.Width * 3;
                int nPixel;

                for (int y = 0; y < _srcBitmap.Height; ++y)
                {
                    for (int x = 0; x < _srcBitmap.Width; ++x)
                    {
                        nPixel = p[2] + _red;
                        nPixel = Math.Max(nPixel, 0);
                        p[2] = (byte)Math.Min(255, nPixel);

                        nPixel = p[1] + _green;
                        nPixel = Math.Max(nPixel, 0);
                        p[1] = (byte)Math.Min(255, nPixel);

                        nPixel = p[0] + _blue;
                        nPixel = Math.Max(nPixel, 0);
                        p[0] = (byte)Math.Min(255, nPixel);

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
