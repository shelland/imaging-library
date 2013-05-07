using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class RandomJitterCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private int _degree = 0;

        public RandomJitterCommand() { }

        public RandomJitterCommand(Bitmap srcBitmap, int degree)
        {
            _srcBitmap = srcBitmap;
            _degree = degree;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {
            Point[,] ptRandJitter = new Point[_srcBitmap.Width, _srcBitmap.Height];

            int nWidth = _srcBitmap.Width;
            int nHeight = _srcBitmap.Height;

            int newX, newY;

            short nHalf = (short)Math.Floor((double)_degree / 2);
            Random rnd = new Random();

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    newX = rnd.Next(_degree) - nHalf;

                    if (x + newX > 0 && x + newX < nWidth)
                        ptRandJitter[x, y].X = newX;
                    else
                        ptRandJitter[x, y].X = 0;

                    newY = rnd.Next(_degree) - nHalf;

                    if (y + newY > 0 && y + newY < nWidth)
                        ptRandJitter[x, y].Y = newY;
                    else
                        ptRandJitter[x, y].Y = 0;
                }

            OffsetCommand cmd = new OffsetCommand(_srcBitmap, ptRandJitter);
            return cmd.ProcessImage(_srcBitmap);

        }

    }
}
