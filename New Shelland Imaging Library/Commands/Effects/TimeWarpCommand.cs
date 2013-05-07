using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class TimeWarpCommand: IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private byte _factor = 0;
        private bool _smooth = false;

        public TimeWarpCommand() { }

        public TimeWarpCommand(Bitmap srcBitmap, byte factor, bool smoothing)
        {
            _srcBitmap = srcBitmap;
            _factor = factor;
            _smooth = smoothing;
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

                    double newRadius = Math.Sqrt(radius) * _factor;

                    newX = mid.X + (newRadius * Math.Cos(theta));
                    if (newX > 0 && newX < nWidth)
                    {
                        fp[x, y].X = newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        fp[x, y].X = 0.0;
                        pt[x, y].X = 0;
                    }

                    newY = mid.Y + (newRadius * Math.Sin(theta));
                    if (newY > 0 && newY < nHeight)
                    {
                        fp[x, y].Y = newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        fp[x, y].Y = 0.0;
                        pt[x, y].Y = 0;
                    }
                }

            if (_smooth)
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
