using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class FlipCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private bool _vert = false;
        private bool _hor = false;

        public FlipCommand() { }

        public FlipCommand(Bitmap srcBitmap, bool vertFlip, bool horFlip)
        {
            _srcBitmap = srcBitmap;
            _vert = vertFlip;
            _hor = horFlip;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_srcBitmap == null)
            {
                throw new ILException("Input bitmap can not be null");
            }

            Point[,] ptFlip = new Point[_srcBitmap.Width, _srcBitmap.Height];

            int nWidth = _srcBitmap.Width;
            int nHeight = _srcBitmap.Height;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    ptFlip[x, y].X = (_hor) ? nWidth - (x + 1) : x;
                    ptFlip[x, y].Y = (_vert) ? nHeight - (y + 1) : y;
                }

            OffsetAbsCommand cmd = new OffsetAbsCommand(_srcBitmap, ptFlip);
            return cmd.ProcessImage(srcBitmap);
        }

    }
}
