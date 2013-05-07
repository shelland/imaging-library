using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Shelland.ImagingLibrary.Commands.Processing.AnimatedGIF
{
    public class GifAnimationCommand
    {


        /// <summary>
        /// Creates a new GIF animation
        /// </summary>
        /// <param name="OutputFile">Output GIF animation file name</param>
        /// <param name="Width">Animation width</param>
        /// <param name="Height">Animation height</param>
        /// <param name="NumberOfLoops">Number of times to play animation. 0 - infinite</param>
        /// <param name="TransparentColor">Transparent color</param>
        /// <param name="FrameDelay">A number of seconds to display each frame (milliseconds)</param>
        /// <param name="Quality">Output quality factor. 1 - highest quality, 255 - lower quality</param>
        /// <param name="SourceFiles">An array of source files to create animation from</param>
        public void CreateAnimation(FileStream OutputFile, int Width, int Height, int NumberOfLoops, Color TransparentColor, int FrameDelay, int Quality, string[] SourceFiles)
        {
            if (OutputFile == null || Width <= 0 || Height <= 0 || NumberOfLoops < 0 || FrameDelay <= 0 || Quality <= 0 || SourceFiles.Length == 0)
            {
                throw new ArgumentException("Please provide a valid arguments");
            }
            AnimatedGifEncoder Encoder = new AnimatedGifEncoder();
            Encoder.Start(OutputFile);
            Encoder.SetDelay(FrameDelay);
            Encoder.SetQuality(Quality);
            Encoder.SetRepeat(NumberOfLoops);
            Encoder.SetSize(Width, Height);
            Encoder.SetTransparent(TransparentColor);
            foreach (string fileName in SourceFiles)
            {
                if (File.Exists(fileName))
                {
                    Encoder.AddFrame(Image.FromFile(fileName));
                }
                else
                {
                    // throw new FileNotFoundException("Не удалось найти исходные изображения. Проверьте указанные параметры");
                    throw new ILException("Не удалось найти файлы изображений. Пожалуйста, проверьте исходные параметры", "FILE_NOT_FOUND");
                }
            }
            Encoder.Finish();
        }

        /// <summary>
        /// Extracts animation into specified folder
        /// </summary>
        /// <param name="FileName">Source file name</param>
        /// <param name="OutputDirectory">Output directory</param>
        /// <param name="OutputFilePrefix"></param>
        /// <param name="OutputFormat"></param>
        public void ExtractAnimation(string FileName, string OutputDirectory, string OutputFilePrefix, ImageFormat OutputFormat)
        {
            if (string.IsNullOrEmpty(FileName) || string.IsNullOrEmpty(OutputDirectory) || string.IsNullOrEmpty(OutputFilePrefix))
            {
                throw new ArgumentException("Not all necessary arguments were provided");
            }
            GifDecoder Decoder = new GifDecoder();
            Decoder.Read(FileName);
            for (int i = 0; i < Decoder.GetFrameCount(); i++)
            {
                Image OutputImage = Decoder.GetFrame(i);
                OutputImage.Save(OutputDirectory + OutputFilePrefix + i.ToString() + ".png", OutputFormat);
            }
        }

    }
}
