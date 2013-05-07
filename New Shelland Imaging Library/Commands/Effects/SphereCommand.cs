using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class SphereCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private bool _smooth = false;

        /// <summary>
        /// Represents a sphere effect
        /// </summary>
        public SphereCommand() { }

        /// <summary>
        /// Represents a sphere effect
        /// </summary>
        /// <param name="srcBitmap"></param>
        /// <param name="smoothing"></param>
        public SphereCommand(Bitmap srcBitmap, bool smoothing)
        {
            _srcBitmap = srcBitmap;
            _smooth = smoothing;
        }

        /// <summary>
        /// Returns processed image
        /// </summary>
        /// <returns></returns>
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

                    double newRadius = radius * radius / (Math.Max(mid.X, mid.Y));

                    newX = mid.X + (newRadius * Math.Cos(theta));

                    if (newX > 0 && newX < nWidth)
                    {
                        fp[x, y].X = newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        fp[x, y].X = fp[x, y].Y = 0.0;
                        pt[x, y].X = pt[x, y].Y = 0;
                    }

                    newY = mid.Y + (newRadius * Math.Sin(theta));

                    if (newY > 0 && newY < nHeight && newX > 0 && newX < nWidth)
                    {
                        fp[x, y].Y = newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        fp[x, y].X = fp[x, y].Y = 0.0;
                        pt[x, y].X = pt[x, y].Y = 0;
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
