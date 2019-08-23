using AForge.Imaging;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DuplicateQuestion
{
    public static class CheckDuplicatedImage
    {

        public static bool CheckDuplicateImage(string image1, string image2)
        {
            // The threshold is the minimal acceptable similarity between template candidate. 
            // Min (loose) is 0.0 Max (strict) is 1.0
            const float similarityThreshold = 0.50f;


            //const string testImageThree = @"E:\NhiUlti\demo_ASIFT_src\demo_ASIFT_src\adam2.png";

            // Comparison level is initially set to 0.95
            // Increment loop in steps of .01
            var compareLevel = 0.98;
            //for (var compareLevel = 0.98; compareLevel <= 1.00; compareLevel += 0.01)
            //{
                var isDuplicated = CompareImages(image1, image2, compareLevel,similarityThreshold);              
                return isDuplicated;
               
            //}
            //return false;
           
        }

        private static Bitmap ScaleImage(Bitmap image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(maxWidth, maxHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, maxWidth, maxHeight);

            return newImage;
        }
        private static Bitmap ConvertToFormat(Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }
        private static Bitmap Base64StringToBitmap(this string
                                            base64String)
        {
            Bitmap bmpReturn = null;


            byte[] byteBuffer = Convert.FromBase64String(base64String);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);


            memoryStream.Position = 0;


            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
            //bmpReturn = ConvertToFormat(bmpReturn, PixelFormat.Format24bppRgb);

            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;


            return bmpReturn;
        }
        public static Bitmap ResizeBitMap(Bitmap currentBitmap)
        {
            int newWidth = 40;
            int newHeight = 40;
            Bitmap animage = new Bitmap(newWidth, newHeight);
            using (Graphics gr = Graphics.FromImage(animage))
            {
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.DrawImage(currentBitmap, new Rectangle(0, 0, newWidth, newHeight));
            }
            return animage;
        }
        private static bool CompareImages(string base1, string base2, double compareLevel, float similarityThreshold)
        {
           
            var imageOne = Base64StringToBitmap(base1);
            var imageTwo = Base64StringToBitmap(base2);
            imageOne = ScaleImage(imageOne, 40, 40);
            imageTwo = ScaleImage(imageTwo, 40, 40);

            var newBitmap1 = ChangePixelFormat(imageOne, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var newBitmap2 = ChangePixelFormat(imageTwo, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            //newBitmap1 = ResizeBitMap(newBitmap1);
            //newBitmap2 = ResizeBitMap(newBitmap2);
            // Setup the AForge library
            var tm = new ExhaustiveTemplateMatching(similarityThreshold);

            // Process the images
            var results = tm.ProcessImage(newBitmap2, newBitmap1);

            // Compare the results, 0 indicates no match so return false
            if (results.Length <= 0)
            {
                return false;
            }

            // Return true if similarity score is equal or greater than the comparison level
            var match = results[0].Similarity >= compareLevel;

            return match;
        }

        private static Bitmap ChangePixelFormat(Bitmap inputImage, System.Drawing.Imaging.PixelFormat newFormat)
        {
            return (inputImage.Clone(new Rectangle(0, 0, inputImage.Width, inputImage.Height), newFormat));
        }
    }
}
