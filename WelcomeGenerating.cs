using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net;
using System.Threading;

namespace Welcome
{
    class WelcomeGenerating
    {
        private String folder = @"C:\Users\pysiak\Downloads\Discord_Img\";
        private string avatarFolder = @"C:\Users\pysiak\Downloads\Discord_Img\User_Avatar.jpeg";
        private string defaultFolder = @"C:\Users\pysiak\Downloads\Discord_Img\default.jpeg";

        public byte[] createImage(string _user, String sourceImagePath, String beiAnStampImagePath)
        {
            Bitmap bitMapImage = new System.Drawing.Bitmap(sourceImagePath);
            return this.createImage(_user, bitMapImage, beiAnStampImagePath);
        }

        public byte[] createImage(string user, Bitmap bitMapImage, String beiAnStampImagePath)
        {
            Graphics graphicImage = Graphics.FromImage(bitMapImage);

            //Smooth graphics is nice.
            graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

            //Write your text.
            //graphicImage.DrawString("That's my boy!",
            //   new Font("Arial", 12, FontStyle.Bold),
            //   SystemBrushes.WindowText, new Point(0, 0));

            Font font = new Font("Ultra", 100);
            int x_axis = 100;
            int y_axis = -20;
            graphicImage.DrawString("Hello There", font, Brushes.White, new PointF(x_axis, y_axis));
            graphicImage.DrawString(user, font, Brushes.White, new PointF(0, 550));
            String tempFile = folder + "test.jpeg";
            bitMapImage.Save(tempFile, ImageFormat.Jpeg);
            MemoryStream ms = new MemoryStream();
            //bitMapImage.Save(ms, ImageFormat.Jpeg);
            //graphicImage.Save();

            //I am drawing a oval around my text.
            //graphicImage.DrawArc(new Pen(Color.Red, 3), 90, 235, 150, 50, 0, 360);

            //Set the content type
            //Response.ContentType = "image/jpeg";
            //Save the new image to the response output stream.
            //bitMapImage.Save(Response.OutputStream, ImageFormat.Jpeg);

            //Clean house.
            //graphicImage.Dispose();
            //bitMapImage.Dispose();
            byte[] bytes = MergeJpeg(tempFile, beiAnStampImagePath, folder + "output.jpeg");

            //Delete temp file
            if (System.IO.File.Exists(tempFile))
            {
                // Use a try block to catch IOExceptions, to 
                // handle the case of the file already being 
                // opened by another process. 
                try
                {
                    System.IO.File.Delete(tempFile);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return bytes;
                }
            }
            return bytes;
        }
        /// <summary>
        /// Merges two Jpeg images vertically
        /// </summary>
        /// <param name="inputJpeg1">filename with complete path of the first jpeg file.</param>
        /// <param name="inputJpeg2">filname with complete path of the second jpeg file.</param>
        /// <param name="outputJpeg">filename with complete path where you want to save the output jpeg file.</param>
        private byte[] MergeJpeg(string inputJpeg1, string inputJpeg2, string outputJpeg)
        {
            Image image1 = Image.FromFile(inputJpeg1);
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFileAsync(new Uri(inputJpeg2), avatarFolder);
                    var i = 0;
                    while (i <= 4)
                    {
                        try
                        {
                            Image image3 = Image.FromFile(avatarFolder);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Thread.Sleep(2000);
                            i++;
                            Console.WriteLine(ex.Message);
                            if (i <= 3)
                            {
                                Console.WriteLine(ex.Message);
                                File.Replace(defaultFolder, avatarFolder, defaultFolder);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    Image image3 = Image.FromFile(defaultFolder);
                }

                Image image2 = Image.FromFile(avatarFolder);

                int width = image1.Width;
                int height = image1.Height;

                Bitmap outputImage = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(outputImage);

                graphics.Clear(Color.Black);
                graphics.DrawImage(image1, new Point(0, 0));
                Point stampPoint = new Point(600, 200);

                /*Color newColor = Color.FromArgb(0, Color.Red);
                using (Brush br = new SolidBrush(newColor))
                {
                    graphics.FillRectangle(br, 750, 200, image2.Width, image2.Height);
                }*/
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(850, 120, image2.Width + 280, image2.Height + 280);
                graphics.SetClip(path);
                graphics.DrawImage(image2, 850, 120, 410, 410);

                //graphics.DrawImage(image2, stampPoint);
                //graphics.DrawImage(image2, new Point(0, image1.Height - image2.Height));

                graphics.Dispose();
                image1.Dispose();

                MemoryStream stream = new MemoryStream();
                //outputImage.Save(outputJpeg, System.Drawing.Imaging.ImageFormat.Jpeg);
                outputImage.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                outputImage.Dispose();
                return stream.ToArray();
            }
        }

        public Bitmap ByteArraytoBitmap(Byte[] byteArray)
        {
            MemoryStream stream = new MemoryStream(byteArray);

            return new System.Drawing.Bitmap(stream);
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }


    }
}
