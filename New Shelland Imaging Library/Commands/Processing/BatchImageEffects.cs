using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Shelland.ImagingLibrary.Commands.Processing
{
    /// <summary>
    /// Allows to process a single bitmap with multiple filters at once
    /// </summary>
    public class BatchImageEffects: IImageProcessing
    {

        List<IImageProcessing> _tempList = new List<IImageProcessing>();
        Bitmap _tempBmp=null;
        Bitmap _outputImage = null;

        public void AddFilter(IImageProcessing imageFilter)
        {
            _tempList.Add(imageFilter);
        }

        public List<IImageProcessing> CurrentFilters
        {
            get
            {
                return _tempList;
            }
        }

     

        /// <summary>
        /// Gets a processed image
        /// </summary>
        public Bitmap ProcessedImage
        {
            get
            {
                return _outputImage;
            }
        }

        /// <summary>
        /// Gets or sets a source image that will be processed
        /// </summary>
        public Bitmap SourceImage
        {
            get
            {
                return _tempBmp;
            }
            set
            {
                _tempBmp = value;
            }
        }

        public Bitmap ProcessImage(Bitmap srcBitmap)
        {

            return null;

        }

    }
}
