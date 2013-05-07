using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class LaplacianEmbossCommand: IImageProcessing
    {

        private Bitmap _srcBitmap = null;

        public LaplacianEmbossCommand() { }

        public LaplacianEmbossCommand(Bitmap srcBitmap)
        {
            _srcBitmap = srcBitmap;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_srcBitmap == null)
            {
                throw new ILException("Input bitmap can not be null");
            }

            ConvolutionMatrix m = new ConvolutionMatrix();
            m.SetAll(-1);
            m.TopMid = m.MidLeft = m.MidRight = m.BottomMid = 0;
            m.Pixel = 4;
            m.Offset = 127;

            ConvolutionMatrix.Conv3x3(_srcBitmap, m);
            return _srcBitmap;
        }

    }
}
