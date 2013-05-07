using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class QuickEdgeDetectCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;

        public QuickEdgeDetectCommand() { }

        public QuickEdgeDetectCommand(Bitmap srcBitmap)
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
            m.TopLeft = m.TopMid = m.TopRight = -1;
            m.MidLeft = m.Pixel = m.MidRight = 0;
            m.BottomLeft = m.BottomMid = m.BottomRight = 1;

            m.Offset = 127;

            ConvolutionMatrix.Conv3x3(_srcBitmap, m);
            return _srcBitmap;

        }

    }
}
