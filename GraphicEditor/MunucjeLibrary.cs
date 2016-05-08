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
                    if(i==7)
                    {
                        var sfd = "";
                    }

                    int przeciecia = 0;

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

                            if(color.R == 0 )
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

                            if (color.R == 0)
                            {
                                przeciecia2++;
                            }

                        }
                    }


                    
                    if (przeciecia == 1 && przeciecia2==1)
                    {
                        map[i, j] = 1; /// zakonczenie
                    }
                    else if(przeciecia == 3 && przeciecia2 == 3)
                    {
                        map[i, j] = 2; // rozwidlenie
                    }

                    
                }
            }

            Zaznaczpunkty(renderedImage, map);

            return renderedImage;

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
                using (Brush brsh = new SolidBrush(color))
                {
                    grf.FillEllipse(brsh, x, y, 2, 2);
                }
            }
        }
    }
}
