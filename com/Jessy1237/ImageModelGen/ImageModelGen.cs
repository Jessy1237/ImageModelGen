using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace com.Jessy1237.ImageModelGen
{
    public class ImageModelGen
    {
        public static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                Bitmap bmp = new Bitmap(args[0]);
                if (bmp != null)
                {
                    LinkedList<string> pixels = GetPixelEdges(bmp);
                    WriteFile(args[0], pixels);
                }
                else
                {
                    Console.WriteLine("Image '" + args[0] + "' does not exist");
                }
            }
            else
            {
                Console.WriteLine("USAGE: ImageModelGen.exe 'image'");
            }

        }

        public static void WriteFile(string file, LinkedList<string> pixels)
        {
            StreamWriter modelFile = new StreamWriter(file + ".model");

            foreach( string pixel in pixels)
            {
                modelFile.WriteLine(pixel);
            }
            modelFile.Flush();
            modelFile.Close();
        }

        /// <summary>
        /// Finds the outer edge pixels of the image and adds their coordinates to a LinkedList to form a model of the image.
        /// </summary>
        /// <param name="bmp">The image file to generate the model from</param>
        /// <returns> A LinkedList that contains the coordinates of the outer edge pixels of the image</returns>
        public static LinkedList<string> GetPixelEdges(Bitmap bmp)
        {
            LinkedList<string> pixels = new LinkedList<string>();

            for (int h = 0; h < bmp.Height; h++)
            {
                Boolean addedFromRowAlready = false;
                Boolean lookingForEdge = false;
                for (int w = 0; w < bmp.Width; w++)
                {
                    //Pixel is not transparent
                    if (bmp.GetPixel(w, h).A != 0)
                    {
                        if (!lookingForEdge)
                        {
                            //Only add the outer edge pixels of the image model
                            if (!addedFromRowAlready)
                            {
                                pixels.AddLast(w + "," + h);
                                lookingForEdge = true;
                                addedFromRowAlready = true;
                            }
                            else
                            {
                                pixels.RemoveLast();
                                lookingForEdge = true;
                            }
                        }
                    }
                    else
                    {
                        if (lookingForEdge)
                        {
                            pixels.AddLast((w - 1) + "," + h);
                            lookingForEdge = false;
                        }
                    }
                }
            }

            return pixels;
        }
    }
}
