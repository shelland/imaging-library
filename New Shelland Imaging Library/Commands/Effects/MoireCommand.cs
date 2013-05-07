using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class MoireCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private double _degree = 0;

        public MoireCommand() { }

        public MoireCommand(Bitmap srcBitmap, double degree)
        {
            _srcBitmap = srcBitmap;
            _degree = degree;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {
            int nWidth = _srcBitmap.Width;
            int nHeight = _srcBitmap.Height;

            Point[,] pt = new Point[nWidth, nHeight];

            Point mid = new Point();
            mid.X = nWidth / 2;
            mid.Y = nHeight / 2;

            double theta, radius;
            int newX, newY;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    int trueX = x - mid.X;
                    int trueY = y - mid.Y;
                    theta = Math.Atan2((trueX), (trueY));

                    radius = Math.Sqrt(trueX * trueX + trueY * trueY);

                    newX = (int)(radius * Math.Sin(theta + _degree * radius));
                    if (newX > 0 && newX < nWidth)
                    {
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        pt[x, y].X = 0;
                    }

                    newY = (int)(radius * Math.Sin(theta + _degree * radius));
                    if (newY > 0 && newY < nHeight)
                    {
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        pt[x, y].Y = 0;
                    }
                }

            OffsetAbsCommand cmd = new OffsetAbsCommand(_srcBitmap, pt);
            return cmd.ProcessImage(srcBitmap);

        }

    }
}
