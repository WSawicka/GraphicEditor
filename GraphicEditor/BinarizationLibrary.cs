using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GraphicEditor
{
    class BinarizationLibrary
    {
        public Bitmap ConvertToGrayscale(Bitmap sourceImage)
        {
            Bitmap renderedImage = sourceImage;

            uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
            decimal Const = 255 / (decimal)pixels;

            int x, y, Rgrey, Ggrey, Bgrey;

            for (y = 0; y < renderedImage.Height; y++)
            {
                for (x = 0; x < renderedImage.Width; x++)
                {
                    Color pixelColor = renderedImage.GetPixel(x, y);
                    Rgrey = (int)((pixelColor.R * 0.299) + (pixelColor.G * 0.587) + (pixelColor.B * 0.114));
                    Ggrey = (int)((pixelColor.R * 0.299) + (pixelColor.G * 0.587) + (pixelColor.B * 0.114));
                    Bgrey = (int)((pixelColor.R * 0.299) + (pixelColor.G * 0.587) + (pixelColor.B * 0.114));

                    Color newColor = Color.FromArgb(Rgrey, Ggrey, Bgrey);
                    renderedImage.SetPixel(x, y, newColor);
                }
            }
            return renderedImage;
        }

        public Bitmap TransformBinary(Bitmap sourceImage, int boundary)
        {
            Bitmap renderedImage = sourceImage;

            uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
            decimal Const = 255 / (decimal)pixels;

            int x, y;
            for (y = 0; y < renderedImage.Height; y++)
            {
                for (x = 0; x < renderedImage.Width; x++)
                {
                    Color pixelColor = renderedImage.GetPixel(x, y);
                    int brightness = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                    if (brightness > boundary) renderedImage.SetPixel(x, y, Color.White);
                    else renderedImage.SetPixel(x, y, Color.Black);
                }
            }
            return renderedImage;
        }

        public Bitmap TransformOtsu(Bitmap sourceImage)
        {
            Bitmap renderedImage = ConvertToGrayscale(sourceImage);
            uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
            decimal Const = 255 / (decimal)pixels;

            int x, y;
            byte[] grey = new byte[256]; // stworz histogram
            for (int i = 0; i < 255; i++) grey[i] = 0;

            for (y = 0; y < renderedImage.Height; y++)
            {
                for (x = 0; x < renderedImage.Width; x++)
                {
                    Color pixelColor = renderedImage.GetPixel(x, y);
                    int brightness = (int)(pixelColor.R * 0.299 + pixelColor.G * 0.587 + pixelColor.B * 0.114);
                    grey[brightness]++;
                }
            }
            double[] variancies = new double[256];
            for (int group = 0; group < 256; group++)
            {
                double objectDepth = 0, backgrDepth = 0;
                for (int i = 0; i < group; i++)
                {
                    objectDepth += grey[i];
                }
                for (int i = group; i < 256; i++)
                {
                    backgrDepth += grey[i];
                }

                double objectMiddleDepth = 0, backgrMiddleDepth = 0;
                for (int i = 0; i < group; i++)
                {
                    objectMiddleDepth += (grey[i] * i) / objectDepth;
                }
                for (int i = group; i < 256; i++)
                {
                    backgrMiddleDepth += (grey[i] * i) / backgrDepth;
                }

                variancies[group] = Math.Sqrt(objectDepth * backgrDepth * Math.Pow(objectMiddleDepth - backgrMiddleDepth, 2));
            }
            int boundary = Array.IndexOf(variancies, variancies.Max());
            return TransformBinary(renderedImage, boundary);
        }

        public Bitmap TransformBinaryNiblack(Bitmap sourceImage, int boxSize, double k)
        {
            Bitmap renderedImage = ConvertToGrayscale(sourceImage);
            int size = boxSize / 2;

            uint pixels = (uint)sourceImage.Height * (uint)sourceImage.Width;
            decimal Const = 255 / (decimal)pixels;

            double srednia; //średnia arytmetyczna = (suma xi od i=0 do n )/n 
            double odchStand;//odchylenie standardowe = pierwiastek z ( ((suma xi od i=0 do n ) - srednia) / n-1 )

            // lista z wczytanymii obliczonymi wartościami jasności obrazu
            List<List<int>> boxBrightness = new List<List<int>>();

            for (int y = 0; y < sourceImage.Height; y++)
            {
                setFirstLineBoxBrightnessList(y, size, boxBrightness, sourceImage);
                Console.WriteLine(y);
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    Color pixel = sourceImage.GetPixel(x, y);
                    int brightness = pixel.R + pixel.G + pixel.B;

                    srednia = 0;
                    foreach (var sublist in boxBrightness)
                    {
                        srednia += sublist.Average();
                    }
                    srednia /= boxBrightness.Count();

                    odchStand = 0;
                    double tempOdchStand = 0;
                    foreach (var sublist in boxBrightness)
                    {
                        foreach (int b in sublist)
                        {
                            tempOdchStand += Math.Pow((b - srednia), 2);
                        }
                    }
                    tempOdchStand /= (Math.Pow(size, 2));
                    odchStand = Math.Sqrt(tempOdchStand);

                    double boundary = srednia - odchStand * k;
                    if (brightness > boundary)
                    {
                        renderedImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        renderedImage.SetPixel(x, y, Color.Black);
                    }

                    //dodaj nowe elementy i usuń stare
                    if (x + 1 < sourceImage.Width) updateBoxBrightness(x, y, size, boxBrightness, sourceImage);
                }
            }
            return renderedImage;
        }

        public void setFirstLineBoxBrightnessList(int y, int size, List<List<int>> boxBrightness, Bitmap image)
        {
            int Ystart = y - size, Yend = y + size;
            //uwzględnienie wartości brzegowych
            if (Ystart < 0) Ystart = 0;
            else if (Yend > image.Height) Yend = image.Height;

            boxBrightness.Clear();

            for (int h = Ystart; h < Yend; h++)
            {
                List<int> sublist = new List<int>();
                for (int w = 0; w < size; w++)
                {
                    Color p = image.GetPixel(w, h);
                    sublist.Add((int)(p.R * 0.299 + p.G * 0.587 + p.B * 0.114));
                }
                boxBrightness.Add(sublist);
            }
        }

        public void updateBoxBrightness(int x, int y, int size, List<List<int>> boxBrightness, Bitmap image)
        {
            //usuń linię z kolumny (y- size) a dodaj linię z kolumny (y + size + 1)
            int X = x;
            int Ystart = (y - size < 0) ? 0 : y - size;
            int Yend = (y + size > image.Height) ? image.Height : y + size;

            boxBrightness.RemoveAt(0);

            if (x >= image.Width) return;

            List<int> sublist = new List<int>();
            for (int h = Ystart; h < Yend; h++)
            {
                Color p = image.GetPixel(X, h);
                sublist.Add((int)(p.R * 0.299 + p.G * 0.587 + p.B * 0.114));
            }
            boxBrightness.Add(sublist);

        }
    }
}
