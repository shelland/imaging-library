using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class MeanRemovalCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private int _weight = 0;

        public MeanRemovalCommand() { }

        public MeanRemovalCommand(Bitmap srcBitmap, int weight)
        {
            _srcBitmap = srcBitmap;
            _weight = weight;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {
            ConvolutionMatrix m = new ConvolutionMatrix();
            m.SetAll(-1);
            m.Pixel = _weight;
            m.Factor = _weight - 8;
            ConvolutionMatrix.Conv3x3(_srcBitmap, m);
            return _srcBitmap;
        }

    }
}
