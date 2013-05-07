using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class PixelateCommand : IImageProcessing
    {

        public PixelateCommand() { }

        private Bitmap _srcBitmap = null;
        private short _cell = 0;
        private bool _grid = false;

        public PixelateCommand(Bitmap srcBitmap, short pixelCell, bool drawGrid)
        {
            _srcBitmap = srcBitmap;
            _cell = pixelCell;
            _grid = drawGrid;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {
            int nWidth = _srcBitmap.Width;
            int nHeight = _srcBitmap.Height;

            Point[,] pt = new Point[nWidth, nHeight];

            int newX, newY;

            for (int x = 0; x < nWidth; ++x)
                for (int y = 0; y < nHeight; ++y)
                {
                    newX = _cell - x % _cell;

                    if (_grid && newX == _cell)
                        pt[x, y].X = -x;
                    else if (x + newX > 0 && x + newX < nWidth)
                        pt[x, y].X = newX;
                    else
                        pt[x, y].X = 0;

                    newY = _cell - y % _cell;

                    if (_grid && newY == _cell)
                        pt[x, y].Y = -y;
                    else if (y + newY > 0 && y + newY < nHeight)
                        pt[x, y].Y = newY;
                    else
                        pt[x, y].Y = 0;
                }

            OffsetCommand cmd = new OffsetCommand(_srcBitmap, pt);

            return cmd.ProcessImage(srcBitmap);

        }

    }
}
