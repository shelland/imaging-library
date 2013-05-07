using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands
{
    public interface IImageProcessing
    {
        /// <summary>
        /// Returns a processed image
        /// </summary>
        /// <returns></returns>
        Bitmap ProcessImage(Bitmap srcBitmap);

    }
}
