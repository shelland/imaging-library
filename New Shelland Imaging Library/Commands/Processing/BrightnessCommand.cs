using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class BrightnessCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private int _level = 0;

        public BrightnessCommand() { }

        public BrightnessCommand(Bitmap srcBitmap, int brightnessLevel)
        {
            this._srcBitmap = srcBitmap;
            this._level = brightnessLevel;
        }

        /// <summary>
        /// Represents a source image
        /// </summary>
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

        /// <summary>
        /// Gets/sets brightness level
        /// </summary>
        public int BrightnessLevel
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
                throw new ILException("Input bitmap can not be null");
            }

            if (_level < -255 || _level > 255)
            {
                throw new ILException("Brightness level should be between -255 and 255");
            }

            BitmapData bmData = _srcBitmap.LockBits(new Rectangle(0, 0, _srcBitmap.Width, _srcBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            IntPtr Scan0 = bmData.Scan0;

            int nVal = 0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                int nOffset = stride - _srcBitmap.Width * 3;
                int nWidth = _srcBitmap.Width * 3;

                for (int y = 0; y < _srcBitmap.Height; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        nVal = (int)(p[0] + _level);

                        if (nVal < 0) nVal = 0;
                        if (nVal > 255) nVal = 255;

                        p[0] = (byte)nVal;

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
