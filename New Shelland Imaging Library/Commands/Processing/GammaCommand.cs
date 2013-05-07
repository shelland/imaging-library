using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class GammaCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private double _red = 0;
        private double _green = 0;
        private double _blue = 0;

        public GammaCommand(Bitmap srcBitmap, double redValue, double greenValue, double blueValue)
        {
            _srcBitmap = srcBitmap;
            _red = redValue;
            _green = greenValue;
            _blue = blueValue;
        }

        /// <summary>
        /// Gets/sets a red gamma component
        /// </summary>
        public double RedGammaValue
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
        /// Gets/sets a green gamma component
        /// </summary>
        public double GreenGammaValue
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
        /// Gets/sets a blue gamma component
        /// </summary>
        public double BlueGammaValue
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

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_red < .2 || _red > 5 || _green < .2 || _green > 5 || _blue < .2 || _blue > 5)
            {
                throw new ArgumentException("Input gamma values are out of range. Possible values between 0.2 and 5");
            }
            if (_srcBitmap == null)
            {
                throw new ArgumentException("Input bitmap can not be null");
            }

            byte[] redGamma = new byte[256];
            byte[] greenGamma = new byte[256];
            byte[] blueGamma = new byte[256];

            for (int i = 0; i < 256; ++i)
            {
                redGamma[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / _red)) + 0.5));
                greenGamma[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / _green)) + 0.5));
                blueGamma[i] = (byte)Math.Min(255, (int)((255.0 * Math.Pow(i / 255.0, 1.0 / _blue)) + 0.5));
            }

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = _srcBitmap.LockBits(new Rectangle(0, 0, _srcBitmap.Width, _srcBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - _srcBitmap.Width * 3;

                for (int y = 0; y < _srcBitmap.Height; ++y)
                {
                    for (int x = 0; x < _srcBitmap.Width; ++x)
                    {
                        p[2] = redGamma[p[2]];
                        p[1] = greenGamma[p[1]];
                        p[0] = blueGamma[p[0]];

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
