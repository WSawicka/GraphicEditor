using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor
{
    public class FiltrLibrary
    {

        public Bitmap FiltrKonwolucyjny(Bitmap image, int[,] mask)
        {
            ///image.GetPixel()
            
            int height = image.Height-1;
            int widht = image.Width - 1;

            this.height = image.Height;
            this.width = image.Width;


            int[,] tabR = new int[image.Width, image.Height];
            int[,] tabG = new int[image.Width, image.Height];
            int[,] tabB = new int[image.Width, image.Height];

            for (int x = 1; x < widht; x++)
            {
                for (int y = 1; y < height; y++)
                {
                    int sumR = 0;
                    int sumG = 0;
                    int sumB = 0;

                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            /// suma += image.GetPixel(x + i, y + j) * mask[i + 1, j + 1];
                             sumR +=  image.GetPixel(x + i, y + j).R * mask[i +1 , j+1];
                             sumG += image.GetPixel(x + i, y + j).G * mask[i+1 , j+1];
                             sumB += image.GetPixel(x + i, y + j).B * mask[i +1, j +1];

                        }
                    }

                    tabR[x, y] = sumR;
                    tabG[x, y] = sumG;
                    tabB[x, y] = sumB;

                }
            }

            tabR = Normalizuj(tabR);
            tabG = Normalizuj(tabR);
            tabB = Normalizuj(tabR);

            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    
                      image.SetPixel(x, y, Color.FromArgb(tabR[x,y], tabG[x,y], tabB[x,y]));
                }
            }


            
            return image;


        }

        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }





        int width = 0;
        int height = 0;

        public int[,] Normalizuj(int[,] tab)
        {

           int Nmin = 0; // nowe przedzialy
           int Nmax = 1;

           int min = tab.Cast<int>().Min();

           int max = tab.Cast<int>().Max(); ;


            for (int i = 0; i < width; i++)
            {

                for (int k = 0; k < height; k++)
                {
                    double val = tab[i, k];
                    double valAfterNorm = 255 * ((val - min) / (max - min));

                    Debug.WriteLine(val + "/" + valAfterNorm);
                    tab[i, k] = (int)valAfterNorm;




                }
            }

            return tab;

    }



        public int[ , ] getArea(Bitmap image, int[,] mask, int startX ,int startY, int endX, int endY, int obszar,int x,int y)
        {

          
                    int[ , ] Wartosci = new int[3,3];

                    #region Area1

                    int RjasnoscAr1 = 0;
                    int GjasnoscAr1 = 0;
                    int BjasnoscAr1 = 0;

                    int RwariancjaAr1 = 0;
                    int GwariancjaAr1 = 0;
                    int BwariancjaAr1 = 0;

                    for (int i = startX; i < endX; i++)
                    {

                        for (int k = startY; k < endY; k++)
                        {
                            RjasnoscAr1 += image.GetPixel(x+i, y+ k).R;
                            GjasnoscAr1 += image.GetPixel(x + i, y + k).G;
                            BjasnoscAr1 += image.GetPixel(x + i, y + k).B;


                        }
                    }

                    RjasnoscAr1 = RjasnoscAr1 / 9;
                    GjasnoscAr1 = GjasnoscAr1 / 9;
                    BjasnoscAr1 = BjasnoscAr1 / 9;

                    Wartosci[0, 0] = RjasnoscAr1;
                    Wartosci[1, 0] = GjasnoscAr1;
                    Wartosci[2, 0] = BjasnoscAr1;



                    for (int i = startX; i < endX; i++)
                    {

                        for (int k = startY; k < endY; k++)
                        {

                            int val = (int)image.GetPixel(x + i, y + k).R - RjasnoscAr1;
                            RwariancjaAr1 = val * val;

                            int val1 = (int)image.GetPixel(x + i, y + k).G - GjasnoscAr1;
                            GwariancjaAr1 = val1 * val1;

                            int val2 = (int)image.GetPixel(x + i, y + k).B - BjasnoscAr1;
                            BwariancjaAr1 = val2 * val2;


                        }
                    }


                    RwariancjaAr1 = RwariancjaAr1 / 9;
                    GwariancjaAr1 = GwariancjaAr1 / 9;
                    BwariancjaAr1 = BwariancjaAr1 / 9;


            Wartosci[0, 1] = RwariancjaAr1;
            Wartosci[1, 1] = GwariancjaAr1;
            Wartosci[2, 1] = BwariancjaAr1;

            Wartosci[0, 2] = obszar;

            return Wartosci;
            #endregion






        }


        public void  ustawWartosc(Bitmap image, int[] wartosci, int x, int y)
        {
            image.SetPixel(x, y, Color.FromArgb(wartosci[0], wartosci[1], wartosci[2]));
        }

      
        public Bitmap FiltrKuwahara(Bitmap image, int[,] mask)
        {
            ///image.GetPixel()

            int height = image.Height - 1;
            int widht = image.Width - 1;

            this.height = image.Height;
            this.width = image.Width;



    for (int x = 1; x < widht-4; x++)
    {
        for (int y = 1; y < height-4; y++)
        {
                    var area1 = getArea(image, mask, 0, 0, 3, 3, 1, x,y);
                    var area2 = getArea(image, mask, 0, 3, 3, 6, 2, x, y);
                    var area3 = getArea(image, mask, 3, 3, 6, 6, 3, x, y);
                    var area4 = getArea(image, mask, 3, 0, 6, 3, 4, x, y);

 
                    ////dla R
                    int[] wartosciR = new int[5] {10000, area1[0,1], area2[0, 1], area3[0, 1], area4[0, 1] };

                    int min = wartosciR.Min();

                    int obszar = Array.IndexOf(wartosciR, min);



                    int[] wartosci= new int[3];
                    switch (obszar)
                    {

                        case 1:
                            wartosci[0] = area1[0, 0];
                            break;
                        case 2:
                            wartosci[0] = area2[0, 0];
                            break;
                        case 3:
                            wartosci[0] = area3[0, 0];
                            break;
                        case 4:
                            wartosci[0] = area4[0, 0];
                            break;

                        default:
                            break;
                    }


                    ///dla G
                    ///
                         
                    int[] wartosciG = new int[5] { 10000, area1[1, 1], area2[1, 1], area3[1, 1], area4[1, 1] };

                      min = wartosciG.Min();

                      obszar = Array.IndexOf(wartosciG, min);

                    switch (obszar)
                    {

                        case 1:
                            wartosci[1] = area1[1, 0];
                            break;
                        case 2:
                            wartosci[1] = area2[1, 0];
                            break;
                        case 3:
                            wartosci[1] = area3[1, 0];
                            break;
                        case 4:
                            wartosci[1] = area4[1, 0];
                            break;
                          

                        default:
                            break;
                    }


                    ///dla G
                    ///

                    int[] wartosciB = new int[5] { 10000, area1[2, 1], area2[2, 1], area3[2, 1], area4[2, 1] };

                    min = wartosciB.Min();

                    obszar = Array.IndexOf(wartosciB, min);

                    switch (obszar)
                    {

                        case 1:
                            wartosci[2] = area1[2, 0];
                            break;
                        case 2:
                            wartosci[2] = area2[2, 0];
                            break;
                        case 3:
                            wartosci[2] = area3[2, 0];
                            break;
                        case 4:
                            wartosci[2] = area4[2, 0];
                            break;


                        default:
                            break;
                    }



                    ustawWartosc(image, wartosci, x, y);

                }
    }


            return image;


        }

        public Bitmap FiltrMediana(Bitmap image, int[ ,  ] mask, int maskType)
        {
            ///image.GetPixel()

            int height = image.Height - 1;
            int widht = image.Width - 1;

            this.height = image.Height;
            this.width = image.Width;



            for (int x = 1; x < widht - 4; x++)
            {
                for (int y = 1; y < height - 4; y++)
                {
                    int[,] area1 = null;
                    int[,] area2 = null;
                    int[,] area3 = null;
                    int[,] area4 = null;

                    if(maskType == 1)
                    {
                        area1 = getAreaMediana(image, mask, 0, 0, 3, 3, 1, x, y);
                        area2 = getAreaMediana(image, mask, 0, 3, 3, 6, 2, x, y);
                        area3 = getAreaMediana(image, mask, 3, 3, 6, 6, 3, x, y);
                        area4 = getAreaMediana(image, mask, 3, 0, 6, 3, 4, x, y);
                    }

                    else if (maskType == 2)
                    {
                        area1 = getAreaMediana(image, mask, 0, 0, 2, 2, 1, x, y,1);
                        area2 = getAreaMediana(image, mask, 0, 1, 2, 3, 2, x, y,1);
                        area3 = getAreaMediana(image, mask, 1, 0, 3, 2, 3, x, y,1);
                        area4 = getAreaMediana(image, mask, 1, 1, 3, 3, 4, x, y,1);
                    }





                    ////dla R
                    int[] wartosciR = new int[5] { 10000, area1[0, 1], area2[0, 1], area3[0, 1], area4[0, 1] };

                    int min = wartosciR.Min();

                    int obszar = Array.IndexOf(wartosciR, min);



                    int[] wartosci = new int[3];
                    switch (obszar)
                    {

                        case 1:
                            wartosci[0] = area1[0, 0];
                            break;
                        case 2:
                            wartosci[0] = area2[0, 0];
                            break;
                        case 3:
                            wartosci[0] = area3[0, 0];
                            break;
                        case 4:
                            wartosci[0] = area4[0, 0];
                            break;

                        default:
                            break;
                    }


                    ///dla G
                    ///

                    int[] wartosciG = new int[5] { 10000, area1[1, 1], area2[1, 1], area3[1, 1], area4[1, 1] };

                    min = wartosciG.Min();

                    obszar = Array.IndexOf(wartosciG, min);

                    switch (obszar)
                    {

                        case 1:
                            wartosci[1] = area1[1, 0];
                            break;
                        case 2:
                            wartosci[1] = area2[1, 0];
                            break;
                        case 3:
                            wartosci[1] = area3[1, 0];
                            break;
                        case 4:
                            wartosci[1] = area4[1, 0];
                            break;


                        default:
                            break;
                    }


                    ///dla G
                    ///

                    int[] wartosciB = new int[5] { 10000, area1[2, 1], area2[2, 1], area3[2, 1], area4[2, 1] };

                    min = wartosciB.Min();

                    obszar = Array.IndexOf(wartosciB, min);

                    switch (obszar)
                    {

                        case 1:
                            wartosci[2] = area1[2, 0];
                            break;
                        case 2:
                            wartosci[2] = area2[2, 0];
                            break;
                        case 3:
                            wartosci[2] = area3[2, 0];
                            break;
                        case 4:
                            wartosci[2] = area4[2, 0];
                            break;


                        default:
                            break;
                    }



                    ustawWartosc(image, wartosci, x, y);

                }
            }


            return image;


        }

        private int[,] getAreaMediana(Bitmap image, int[,] mask, int startX, int startY, int endX, int endY, int obszar, int x, int y, int mask3x3=0)
        {


            int[,] Wartosci = new int[3, 3];

            #region Area1

            int RjasnoscAr1 = 0;
            int GjasnoscAr1 = 0;
            int BjasnoscAr1 = 0;

            int RwariancjaAr1 = 0;
            int GwariancjaAr1 = 0;
            int BwariancjaAr1 = 0;

            int[] Rwartosci = null;
            int[] Gwartosci = null;
            int[] Bwartosci = null;

            if (mask3x3 != 0)
            {
                 Rwartosci = new int[4];
                 Gwartosci = new int[4];
                 Bwartosci = new int[4];
            }
            else
            {
                Rwartosci = new int[9];
                Gwartosci = new int[9];
                 Bwartosci = new int[9];
            }

            int indeks = 0;

            for (int i = startX; i < endX; i++)
            {

                for (int k = startY; k < endY; k++)
                {
                    RjasnoscAr1 += image.GetPixel(x + i, y + k).R;
                    GjasnoscAr1 += image.GetPixel(x + i, y + k).G;
                    BjasnoscAr1 += image.GetPixel(x + i, y + k).B;

                    Rwartosci[indeks] = image.GetPixel(x + i, y + k).R;
                    Gwartosci[indeks] = image.GetPixel(x + i, y + k).G;
                    Bwartosci[indeks] = image.GetPixel(x + i, y + k).B;
                    indeks++;

                }
            }

            if (mask3x3 != 0)
            {
                RjasnoscAr1 = RjasnoscAr1 / 4;
                GjasnoscAr1 = GjasnoscAr1 / 4;
                BjasnoscAr1 = BjasnoscAr1 / 4;
            }
            else
            {
                RjasnoscAr1 = RjasnoscAr1 / 9;
                GjasnoscAr1 = GjasnoscAr1 / 9;
                BjasnoscAr1 = BjasnoscAr1 / 9;
            }

            Array.Sort(Rwartosci);
            Array.Sort(Gwartosci);
            Array.Sort(Bwartosci);

            if(Rwartosci.Length%2==0)
            {
                Wartosci[0, 0] = (Rwartosci[Rwartosci.Length/2]+ Rwartosci[(Rwartosci.Length / 2)-1])/2;
                Wartosci[1, 0] = (Gwartosci[Gwartosci.Length / 2] + Gwartosci[(Gwartosci.Length / 2) - 1]) / 2;
                Wartosci[2, 0] = (Bwartosci[Bwartosci.Length / 2] + Bwartosci[(Bwartosci.Length / 2) - 1]) / 2;
            }

            else
            {
                Wartosci[0, 0] = Rwartosci[Rwartosci.Length/2];
                Wartosci[1, 0] = Gwartosci[Gwartosci.Length/2];
                Wartosci[2, 0] = Bwartosci[Bwartosci.Length/2];
            }

         



            for (int i = startX; i < endX; i++)
            {

                for (int k = startY; k < endY; k++)
                {

                    int val = (int)image.GetPixel(x + i, y + k).R - RjasnoscAr1;
                    RwariancjaAr1 = val * val;

                    int val1 = (int)image.GetPixel(x + i, y + k).G - GjasnoscAr1;
                    GwariancjaAr1 = val1 * val1;

                    int val2 = (int)image.GetPixel(x + i, y + k).B - BjasnoscAr1;
                    BwariancjaAr1 = val2 * val2;


                }
            }

            if (mask3x3 != 0)
            {
                RwariancjaAr1 = RwariancjaAr1 / 4;
                GwariancjaAr1 = GwariancjaAr1 / 4;
                BwariancjaAr1 = BwariancjaAr1 / 4;
            }
            else {
                RwariancjaAr1 = RwariancjaAr1 / 9;
                GwariancjaAr1 = GwariancjaAr1 / 9;
                BwariancjaAr1 = BwariancjaAr1 / 9;
            }


            Wartosci[0, 1] = RwariancjaAr1;
            Wartosci[1, 1] = GwariancjaAr1;
            Wartosci[2, 1] = BwariancjaAr1;

            Wartosci[0, 2] = obszar;

            return Wartosci;
            #endregion






        }
    }


}
