using System;
using System.Drawing;
using System.Drawing.Imaging;

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
    }
}
