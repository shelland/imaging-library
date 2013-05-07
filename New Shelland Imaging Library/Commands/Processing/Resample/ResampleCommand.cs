using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using Shelland.ImagingLibrary.Processing;

namespace Shelland.ImagingLibrary.Commands.Processing
{
    public class ResampleCommand : IImageProcessing
    {

        private ResamplingFilters res_filter = ResamplingFilters.Box;
        private Bitmap _srcBitmap = null;
        private InternalResamplingService Resampler = new InternalResamplingService();
        private Size _newSize = Size.Empty;

        public ResampleCommand() { }

        public ResampleCommand(Bitmap srcBitmap, Size newSize, ResamplingFilters filterType)
        {
            _srcBitmap = srcBitmap;
            _newSize = newSize;
            res_filter = filterType;
        }

        public ResamplingFilters ResamplingFilter
        {
            get { return res_filter; }
            set { res_filter = value; }
        }

        private ushort[][,] ConvertBitmapToArray(Bitmap bmp)
        {
            ushort[][,] array = new ushort[4][,];
            for (int i = 0; i < 4; i++)
            {
                array[i] = new ushort[bmp.Width, bmp.Height];
            }
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int nOffset = (bd.Stride - bd.Width * 4);
            unsafe
            {
                byte* p = (byte*)bd.Scan0;
                for (int y = 0; y < bd.Height; y++)
                {
                    for (int x = 0; x < bd.Width; x++)
                    {
                        array[3][x, y] = (ushort)p[3];
                        array[2][x, y] = (ushort)p[2];
                        array[1][x, y] = (ushort)p[1];
                        array[0][x, y] = (ushort)p[0];
                        p += 4;
                    }
                    p += nOffset;
                }
            }
            bmp.UnlockBits(bd);
            return array;
        }

        private Bitmap ConvertArrayToBitmap(ushort[][,] array)
        {
            int width = array[0].GetLength(0);
            int height = array[0].GetLength(1);
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            int nOffset = (bd.Stride - bd.Width * 4);
            unsafe
            {
                byte* p = (byte*)bd.Scan0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        p[3] = (byte)Math.Min(Math.Max(array[3][x, y], Byte.MinValue), Byte.MaxValue);
                        p[2] = (byte)Math.Min(Math.Max(array[2][x, y], Byte.MinValue), Byte.MaxValue);
                        p[1] = (byte)Math.Min(Math.Max(array[1][x, y], Byte.MinValue), Byte.MaxValue);
                        p[0] = (byte)Math.Min(Math.Max(array[0][x, y], Byte.MinValue), Byte.MaxValue);
                        p += 4;
                    }
                    p += nOffset;
                }
            }
            bmp.UnlockBits(bd);
            return bmp;
        }

        public System.Drawing.Bitmap ProcessImage(Bitmap srcBitmap)
        {

            if (_newSize.IsEmpty || _srcBitmap == null)
            {
                throw new ILException("Invalid arguments passed to resampling command");
            }

            ushort[][,] SourceImage = ConvertBitmapToArray(new Bitmap(_srcBitmap));
            ushort[][,] FinalImage = Resampler.Resample(SourceImage, _newSize.Width, _newSize.Height);
            Resampler.Filter = res_filter;
            Bitmap OutputImage = ConvertArrayToBitmap(FinalImage);
            return OutputImage;
        }

    }
}
