using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor
{
    public class MunucjeLibrary
    {


        public Bitmap ZnajdzZakonczenia(Bitmap renderedImage)
        {

            int[,] map = new int[renderedImage.Width, renderedImage.Height];


            for (int i = 4; i < renderedImage.Width-4; i++)
            {
                for (int j = 4; j < renderedImage.Height-4; j++)
                {
                   
                    int przeciecia = 0;

                    Color colorPunktu = renderedImage.GetPixel(i , j );


                    for (int u = -2; u <= 2; u++)
                    {
                        for (int k = -2; k <= 2; k++)
                        {


                            if (u == 0 && k == 0)
                                continue;

                            if((u!=-2 && k!=-2) && (u != 2 && k != 2))
                            {
                                continue;
                            }

                            Color color = renderedImage.GetPixel(i + u, j + k);

                            ///Color color = renderedImage.GetPixel(i + u, j + k);

                            //Color color2 = renderedImage.GetPixel((i + u +1 ) % 8 , ( j + k +1 )  % 8 );

                            if(color.R == 0 && colorPunktu.R==0)
                            {
                                przeciecia++;
                            }

                        }
                    }




                    int przeciecia2 = 0;

                    for (int u = -4; u <= 4; u++)
                    {
                        for (int k = -4; k <= 4; k++)
                        {


                            if (u == 0 && k == 0)
                                continue;

                            if ((u != -4 && k != -4 ) && (u != 4 && k != 4))
                            {
                                continue;
                            }

                           

                            Color color = renderedImage.GetPixel(i + u, j + k);

                            ///Color color = renderedImage.GetPixel(i + u, j + k);

                            //Color color2 = renderedImage.GetPixel((i + u + 1) % 8, (j + k + 1) % 8);

                            if (color.R == 0 && colorPunktu.R == 0)
                            {
                                przeciecia2++;
                            }

                        }
                    }




                    if (przeciecia == 1 && przeciecia2 == 1 && DodatkowaMaska(renderedImage, i, j))
                    {
                        map[i, j] = 1; /// zakonczenie
                    }
                    //if (przeciecia == 1 && przeciecia2 == 1)
                    //{
                    //    map[i, j] = 1; /// zakonczenie
                    //}
                    else if (przeciecia == 3 && przeciecia2 == 3)
                    {
                        map[i, j] = 2; // rozwidlenie
                    }

               

                    
                }
            }

            Zaznaczpunkty(renderedImage, map);

            return renderedImage;

        }

        //public int[,] UsunDuplikaty(int[,] map)
        //{
        //    for (int i = 0; i < map.GetLength(0); i++)
        //    {
        //        for (int k = 0; k < map.GetLength(1); k++)
        //        {
        //            if (map[i, k] == 2)
        //            {
        //                bool endregion = true;

        //                var indeksRemove = new List<Tuple<int, int>>();



        //                while (endregion)
        //                {
        //                    bool end = true;

        //                    int count = 0;
        //                    int ii = i;
        //                    int kk = k;

        //                    while (end)
        //                    {
        //                        if (map[ii, kk] == 0)
        //                        {
        //                            end = false;
        //                            kk++;
        //                            ii = i;
        //                        }
        //                        else
        //                        {
        //                            ii++;
        //                            count++;
        //                        }

        //                        if (map[ii, kk] == 0)
        //                        {
        //                            endregion = false;
        //                            end = false;
        //                        }

        //                    }
        //                }

        //            }
        //        }
        //    }

        //}

        //public Tuple<int,int> PoliczDlugosc(int[,] map, int x, int y)
        //{

        //    int wys = 0;
        //    int szer = 0;

        //    int i = 1;
        //    bool end = false;

        //    while (end)
        //    {
        //        if (map[x + i, y + i] == 2 &&
        //           map[x - i, y - i] == 2 &&
        //           map[x + i, y - i] == 2 &&
        //           map[x - i, y + i] == 2 &&

        //           map[x, y - i] == 2 &&
        //           map[x, y + i] == 2 &&
        //           map[x + i, y] == 2 &&
        //           map[x - i, y] == 2
        //          )
        //        {

        //            i++;
        //            wys++; szer++;
        //        }
        //        else
        //        {
        //            end = false;
        //            return Tuple.Create(wys, szer);
        //        }

        //    }
        //}

        

        public bool DodatkowaMaska(Bitmap renderedImage, int x, int y)
        {
      
            int przeciecia = 0;

            Color colorPunktu = renderedImage.GetPixel(x, y);

            for (int u = -1; u <= 1; u++)
            {
                for (int k = -1; k <= 1; k++)
                {

                    
                    if (u == 0 && k == 0)
                        continue;

                    if ((u != -1&& k != -1) && (u != 1 && k != 1))
                    {
                        continue;
                    }

                    Color color = renderedImage.GetPixel(x + u, y + k);

                    ///Color color = renderedImage.GetPixel(i + u, j + k);

                    //Color color2 = renderedImage.GetPixel((i + u +1 ) % 8 , ( j + k +1 )  % 8 );

                    if (color.R == 0 && colorPunktu.R == 0)
                    {
                        przeciecia++;
                    }

                }
            }

            if (przeciecia < 3)
                return true;
            else
                return false;


        }

        public void Zaznaczpunkty(Bitmap bitmap, int[, ] map)
        {
            for (int i = 1; i < bitmap.Width - 1; i++)
            {
                for (int j = 1; j < bitmap.Height - 1; j++)
                {


                   if(map[i,j]==1)
                   {

                        RysujKolo(bitmap, i, j, Color.Red);

                   }

                   else if(map[i, j] == 2)
                   {
                        RysujKolo(bitmap, i, j, Color.Blue);
                   }


                }
            }
        }

        private void RysujKolo(Bitmap image, int x, int y, Color color)
        {

            using (Graphics grf = Graphics.FromImage(image))
            {
                
                    Pen pen = new Pen(color);
                    grf.DrawEllipse(pen, x-2, y-2, 3, 3);
                
            }
        }
    }
}
