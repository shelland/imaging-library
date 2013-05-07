using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class WaterEffectCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private short _factor = 0;
        private bool _smooth = false;

        public WaterEffectCommand() { }

        public WaterEffectCommand(Bitmap srcBitmap, short waveFactor, bool smoothing)
        {
            _srcBitmap = srcBitmap;
            _factor = waveFactor;
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

            double newX, newY;
            double xo, yo;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    xo = ((double)_factor * Math.Sin(2.0 * 3.1415 * (float)y / 128.0));
                    yo = ((double)_factor * Math.Cos(2.0 * 3.1415 * (float)x / 128.0));

                    newX = (x + xo);
                    newY = (y + yo);

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
                OffsetAbsCommand cmd = new OffsetAbsCommand(_srcBitmap, pt);
                _srcBitmap = cmd.ProcessImage(_srcBitmap);
            }
            else
            {
                OffsetAntialiasCommand cmd = new OffsetAntialiasCommand(_srcBitmap, fp);
                _srcBitmap = cmd.ProcessImage(_srcBitmap);
            }

            return _srcBitmap;
        }

    }
}
