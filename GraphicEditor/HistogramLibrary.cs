using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor
{
    public class HistogramLibrary
    {

        public void CreateHistogram(Bitmap bmp)
        {
            createBHistogram(bmp);
            createGHistogram(bmp);
            createRHistogram(bmp);
        }

        private int[] createRHistogram(Bitmap bmp)
        {

            int[] histogram_r = new int[256];
            float max = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int redValue = bmp.GetPixel(i, j).R;
                    histogram_r[redValue]++;
                    if (max < histogram_r[redValue])
                        max = histogram_r[redValue];
                }
            }

            int histHeight = 128;

            Bitmap img = new Bitmap(256, histHeight + 10);
            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < histogram_r.Length; i++)
                {
                    float pct = histogram_r[i] / max;   // What percentage of the max is this value?
                    g.DrawLine(Pens.Black,
                        new Point(i, img.Height - 5),
                        new Point(i, img.Height - 5 - (int)(pct * histHeight))  // Use that percentage of the height
                        );
                }
            }
            img.Save(@"R.jpg");
            return histogram_r;

        }

        private int[] createGHistogram(Bitmap bmp)
        {

            int[] histogram_g = new int[256];
            float max = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int GValue = bmp.GetPixel(i, j).G;
                    histogram_g[GValue]++;
                    if (max < histogram_g[GValue])
                        max = histogram_g[GValue];
                }
            }

            int histHeight = 128;

            Bitmap img = new Bitmap(256, histHeight + 10);
            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < histogram_g.Length; i++)
                {
                    float pct = histogram_g[i] / max;   // What percentage of the max is this value?
                    g.DrawLine(Pens.Black,
                        new Point(i, img.Height - 5),
                        new Point(i, img.Height - 5 - (int)(pct * histHeight))  // Use that percentage of the height
                        );
                }
            }
            img.Save(@"G.jpg");
            return histogram_g;

        }

        private int[] createBHistogram(Bitmap bmp)
        {

            int[] histogram_b = new int[256];
            float max = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int BValue = bmp.GetPixel(i, j).R;
                    histogram_b[BValue]++;
                    if (max < histogram_b[BValue])
                        max = histogram_b[BValue];
                }
            }

            int histHeight = 128;

            Bitmap img = new Bitmap(256, histHeight + 10);
            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < histogram_b.Length; i++)
                {
                    float pct = histogram_b[i] / max;   // What percentage of the max is this value?
                    g.DrawLine(Pens.Black,
                        new Point(i, img.Height - 5),
                        new Point(i, img.Height - 5 - (int)(pct * histHeight))  // Use that percentage of the height
                        );
                }
            }
            img.Save(@"B.jpg");
            return histogram_b;
        }

        public Bitmap Rozjasnij(Bitmap bmp, int val)
        {
            byte[] LUT = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                if ((val + i) > 255)
                {
                    LUT[i] = 255;
                }
                else if ((val + i) < 0)
                {
                    LUT[i] = 0;
                }
                else
                {
                    LUT[i] = (byte)(val + i);
                }
            }

            //Pobierz wartosc wszystkich punktow obrazu
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] pixelValues = new byte[Math.Abs(bmpData.Stride) * bmp.Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, pixelValues, 0, pixelValues.Length);

            //Zmien jasnosc kazdego punktu zgodnie z tablica LUT
            for (int i = 0; i < pixelValues.Length; i++)
            {
                pixelValues[i] = LUT[pixelValues[i]];
            }

            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, bmpData.Scan0, pixelValues.Length);
            bmp.UnlockBits(bmpData);
            return bmp;

        }

        public Bitmap histogramEqualization(Bitmap sourceImage)
        {
            Bitmap renderedImage = sourceImage;

            uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
            decimal Const = 255 / (decimal)pixels;

            int x, y, R, G, B;


            //Create histogram arrays for R,G,B channels
            int[] cdfR = createRHistogram(sourceImage);
            int[] cdfG = createGHistogram(sourceImage);
            int[] cdfB = createBHistogram(sourceImage);

            //Convert arrays to cumulative distribution frequency data
            for (int r = 1; r <= 255; r++)
            {
                cdfR[r] = cdfR[r] + cdfR[r - 1];
                cdfG[r] = cdfG[r] + cdfG[r - 1];
                cdfB[r] = cdfB[r] + cdfB[r - 1];
            }

            for (y = 0; y < renderedImage.Height; y++)
            {
                for (x = 0; x < renderedImage.Width; x++)
                {
                    Color pixelColor = renderedImage.GetPixel(x, y);

                    R = (int)((decimal)cdfR[pixelColor.R] * Const);
                    G = (int)((decimal)cdfG[pixelColor.G] * Const);
                    B = (int)((decimal)cdfB[pixelColor.B] * Const);

                    Color newColor = Color.FromArgb(R, G, B);
                    renderedImage.SetPixel(x, y, newColor);
                }
            }
            return renderedImage;
        }

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

            double srednia = 0; //średnia arytmetyczna = (suma xi od i=0 do n )/n 
            double odchStand = 0;//odchylenie standardowe = pierwiastek z ( ((suma xi od i=0 do n ) - srednia) / n-1 )
            
            // lista z wczytanymii obliczonymi wartościami jasności obrazu
            List<int> boxBrightness = new List<int>();
            

            for (int y = 0; y < sourceImage.Height; y++)
            {
                for (int x = 0; x < sourceImage.Width; x++)
                {
                    Color pixel = sourceImage.GetPixel(x, y);
                    int brightness = pixel.R + pixel.G + pixel.B;

                    if (x - size >= 0 && y - size >= 0 && x + size < sourceImage.Width && y + size < sourceImage.Height)
                    {
                        int i = 0;
                        //pobranie jasności obszaru
                        
                        srednia += boxBrightness.Average();

                        foreach (int b in boxBrightness)
                        {
                            odchStand += Math.Pow((b - srednia), 2);
                        }
                        odchStand = Math.Sqrt(odchStand / ((Math.Pow(size, 2)) - 1));
                        double boundary = srednia + odchStand * k;

                        if (brightness > boundary) renderedImage.SetPixel(x, y, Color.White);
                        else renderedImage.SetPixel(x, y, Color.Black);
                    }
                }
            }
            return renderedImage;
        }

        public List<int> setBoxBrightnessList(int size, Bitmap image)
        {
            List<int> boxBrightness = new List<int>();
            for (int h = 0; h < size; h++)
            {
                for (int w = 0; w < size; w++)
                {
                    Color p = image.GetPixel(w, h);
                    boxBrightness.Add((int)(p.R * 0.299 + p.G * 0.587 + p.B * 0.114));
                }
            }
            return boxBrightness;
        }
    }
}
