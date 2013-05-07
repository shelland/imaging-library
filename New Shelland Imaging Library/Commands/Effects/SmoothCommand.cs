using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class SmoothCommand: IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private int _weight = 0;

        public SmoothCommand(Bitmap srcBitmap, int weight)
        {
            _srcBitmap = srcBitmap;
            _weight = weight;
        }

        public SmoothCommand() { }

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

        public int SmoothWeight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_srcBitmap == null)
            {
                throw new ILException("Input bitmap can not be null");
            }

            ConvolutionMatrix m = new ConvolutionMatrix();
            m.SetAll(1);
            m.Pixel = _weight;
            m.Factor = _weight + 8;

            ConvolutionMatrix.Conv3x3(_srcBitmap, m);

            return _srcBitmap;
        }


    }
}
