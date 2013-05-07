using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Processing
{

    public class BatchProcessing
    {

        private List<IImageProcessing> _effectsQueue;

        public List<IImageProcessing> QueueList
        {
            get
            {
                return _effectsQueue;
            }
        }

        /// <summary>
        /// Returns a current effects queue
        /// </summary>
        /// <param name="effect"></param>
        public void AddEffectToQueue(IImageProcessing effect)
        {
            _effectsQueue.Add(effect);
        }

        public void ProcessBitmap(Bitmap bmp)
        {
            Bitmap tmpBmp = bmp;
            foreach (IImageProcessing proc in this.QueueList)
            {
                tmpBmp = proc.ProcessImage(tmpBmp);
            }
        }

        /// <summary>
        /// Performs a batch input processing of directory
        /// </summary>
        /// <param name="InputPath"></param>
        /// <param name="OutputPath"></param>
        /// <param name="DeleteInputFiles">If True the all source file will be deleted after processing</param>
        public void ProcessDirectory(string InputPath, string OutputPath, bool DeleteInputFiles)
        {

            string[] files = Directory.GetFiles(InputPath, "*.jpg");

            string outputImage = null;
            Bitmap tempBmp = null;

            foreach (string file in files)
            {
                outputImage = OutputPath + "\\" + Path.GetFileNameWithoutExtension(file) + ".jpg";
                foreach (IImageProcessing effect in this.QueueList)
                {
                    tempBmp = effect.ProcessImage(tempBmp);
                }
                tempBmp.Save(outputImage, ImageFormat.Jpeg);
                if (DeleteInputFiles)
                {
                    File.Delete(file);
                }
            }

        }


    }


}