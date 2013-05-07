using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Effects
{
    public class OffsetAbsCommand : IImageProcessing
    {

        private Bitmap _srcBitmap = null;
        private Point[,] _offset = null;

        public OffsetAbsCommand() { }

        public OffsetAbsCommand(Bitmap srcBitmap, Point[,] offsetPoints)
        {
            _srcBitmap = srcBitmap;
            _offset = offsetPoints;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {
            Bitmap bSrc = (Bitmap)_srcBitmap.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = _srcBitmap.LockBits(new Rectangle(0, 0, _srcBitmap.Width, _srcBitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int scanline = bmData.Stride;

            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr SrcScan0 = bmSrc.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* pSrc = (byte*)(void*)SrcScan0;

                int nOffset = bmData.Stride - _srcBitmap.Width * 3;
                int nWidth = _srcBitmap.Width;
                int nHeight = _srcBitmap.Height;

                int xOffset, yOffset;

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        xOffset = _offset[x, y].X;
                        yOffset = _offset[x, y].Y;

                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            p[0] = pSrc[(yOffset * scanline) + (xOffset * 3)];
                            p[1] = pSrc[(yOffset * scanline) + (xOffset * 3) + 1];
                            p[2] = pSrc[(yOffset * scanline) + (xOffset * 3) + 2];
                        }

                        p += 3;
                    }
                    p += nOffset;
                }
            }

            _srcBitmap.UnlockBits(bmData);
            bSrc.UnlockBits(bmSrc);
            return _srcBitmap;
        }

    }
}
