using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class SwirlCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private float _degree = 0;
        private bool _smoothing = false;

        public SwirlCommand() { }

        public SwirlCommand(Bitmap srcBitmap, float degree, bool smoothing)
        {
            _srcBitmap = srcBitmap;
            _degree = degree;
            _smoothing = smoothing;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {
            int nWidth = _srcBitmap.Width;
            int nHeight = _srcBitmap.Height;

            Shelland.ImagingLibrary.Commands.SharedEffects.FloatPoint[,] fp = new Shelland.ImagingLibrary.Commands.SharedEffects.FloatPoint[nWidth, nHeight];
            Point[,] pt = new Point[nWidth, nHeight];

            Point mid = new Point();
            mid.X = nWidth / 2;
            mid.Y = nHeight / 2;

            double theta, radius;
            double newX, newY;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    int trueX = x - mid.X;
                    int trueY = y - mid.Y;
                    theta = Math.Atan2((trueY), (trueX));

                    radius = Math.Sqrt(trueX * trueX + trueY * trueY);

                    newX = mid.X + (radius * Math.Cos(theta + _degree * radius));
                    if (newX > 0 && newX < nWidth)
                    {
                        fp[x, y].X = newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                        fp[x, y].X = pt[x, y].X = x;

                    newY = mid.Y + (radius * Math.Sin(theta + _degree * radius));
                    if (newY > 0 && newY < nHeight)
                    {
                        fp[x, y].Y = newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                        fp[x, y].Y = pt[x, y].Y = y;
                }

            if (_smoothing)
            {
                OffsetAntialiasCommand cmd = new OffsetAntialiasCommand(_srcBitmap, fp);
                return cmd.ProcessImage(_srcBitmap);
            }
            else
            {
                OffsetAbsCommand cmd = new OffsetAbsCommand(_srcBitmap, pt);
                return cmd.ProcessImage(_srcBitmap);
            }
        }

    }
}
