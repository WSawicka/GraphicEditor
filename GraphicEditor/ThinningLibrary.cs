using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor
{
    class ThinningLibrary
    {
        private Bitmap renderedImage;
        private int[,] picture;
        private int[,] tempPicture;
        private int[,] table = new int[3, 3] { { 128, 1, 2 }, { 64, 0, 4 }, { 32, 16, 8 } };
        private List<int> twoList = new List<int>(new int[]
          { 3,   6,   7,   12,  14,  15,  24,  28,  30,  31,  48,
            56,  60,  62,  63,  96,  112, 120, 124, 126, 127, 129,
            131, 135, 143, 159, 191, 192, 193, 195, 199, 207, 223,
            224, 225, 227, 231, 239, 240, 241, 243, 247, 248, 249,
            251, 252, 253, 254 });
        private List<int> fourList = new List<int>(new int[]
          { 3,   6,   12,  24,  48,  96,  192, 129,
            7,   14,  28,  56,  112, 224, 193, 131,
            15,  30,  60,  120, 240, 225, 195, 135});
        private List<int> neighb34567 = new List<int>(new int[]
          { 7,   14,  15,  28,  30,  31,  56,  60,  62,  63,  112,
            120, 124, 126, 131, 135, 143, 159, 191, 193, 195, 199,
            207, 224, 225, 227, 231, 239, 240, 241, 243, 248, 249,
            251, 252, 254 });
        private List<int> deletionList = new List<int>(new int[]
          { 5,   13,  15,  20,  21,  22,  23,  29,  30,  31,  52,  53,  54,  55,  60,  61,
            62,  63,  65,  67,  69,  71,  77,  79,  80,  81,  83,  84,  85,  86,  87,  88,
            89,  91,  92,  93,  94,  95,  97,  99,  101, 103, 109, 111, 113, 115, 116, 117,
            118, 119, 120, 121, 123, 124, 125, 126, 127, 133, 135, 141, 143, 149, 151, 157,
            159, 181, 183, 189, 191, 195, 197, 199, 205, 207, 208, 209, 211, 212, 213, 214,
            215, 216, 217, 219, 220, 221, 222, 223, 225, 227, 229, 231, 237, 239, 240, 241,
            243, 244, 245, 246, 247, 248, 249, 251, 252, 253, 254, 255,
            3,   12,  48,  192, 14,  56,  131, 224, 7,   28,  112, 193 });

        public Bitmap thin(Bitmap sourceImage)
        {
            renderedImage = sourceImage;
            picture = new int[sourceImage.Width, sourceImage.Height];

            bool continueAlgorithm = true;
            while (continueAlgorithm == true)
            {
                int changed = doAlgorithm(renderedImage);
                if (changed == 0) continueAlgorithm = false;
                Console.WriteLine(changed);
                saveChangesToBitmap();
            }

            //showInColors(sourceImage);
            return renderedImage;
        }

        private int doAlgorithm(Bitmap image)
        {
            int changed = 0;

            // krok 1: ustaw 0 i 1
            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    Color pixelColor = image.GetPixel(i, j);
                    if (pixelColor.R == 0 && pixelColor.G == 0 && pixelColor.B == 0) picture[i, j] = 1;
                    else picture[i, j] = 0;
                }
            }

            // krok 2: ustaw 2 i 3
            // TODO: POPRAW WCZYTYWANIE GRANIC
            for (int j = 1; j < image.Height - 1; j++)
            {
                for (int i = 1; i < image.Width - 1; i++)
                {
                    if (picture[i, j] == 1)
                    {
                        if (twoList.Contains(getWeightOf(i, j)))
                        {
                            if (isFour(i, j) == true) picture[i, j] = 4;
                            else picture[i, j] = 2;
                        }
                        if (isThree(i, j) == true) picture[i, j] = 3;
                    }
                }
            }

            // krok 3: usuń niepotrzebne
            for (int j = 1; j < image.Height - 1; j++)
            {
                for (int i = 1; i < image.Width - 1; i++)
                {
                    if (picture[i, j] == 4)
                    {
                        picture[i, j] = 0;
                        ++changed;
                    }
                }
            }

            for (int j = 1; j < image.Height - 1; j++)
            {
                for (int i = 1; i < image.Width - 1; i++)
                {
                    if (picture[i, j] == 2 && deletionList.Contains(getWeightOf(i, j)))
                    {
                        picture[i, j] = 0;
                        ++changed;
                    }
                }
            }

            for (int j = 1; j < image.Height - 1; j++)
            {
                for (int i = 1; i < image.Width - 1; i++)
                {
                    if (picture[i, j] == 3 && deletionList.Contains(getWeightOf(i, j)))
                    {
                        picture[i, j] = 0;
                        ++changed;
                    }
                }
            }

            //krok 4: sprowadź do 0 i 1
            for (int j = 0; j < image.Height; j++)
            {
                for (int i = 0; i < image.Width; i++)
                {
                    if (picture[i, j] != 0) picture[i, j] = 1;
                }
            }
            return changed;
        }

        private bool isThree(int x, int y)
        {
            if (picture[x - 1, y] != 0 && picture[x, y - 1] != 0 && picture[x + 1, y] != 0 && picture[x, y + 1] != 0)
            {
                if (picture[x + 1, y - 1] == 0 || picture[x - 1, y + 1] == 0 || picture[x + 1, y + 1] == 0 || picture[x - 1, y - 1] == 0) return true;
            }
            return false;
        }

        private bool isFour(int x, int y)
        {
            int weight = getWeightOf(x, y);
            if (fourList.Contains(weight)) return true;
            return false;
        }

        private int getWeightOf(int x, int y)
        {
            int weight = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (picture[x + i - 1, y + j - 1] != 0) weight += table[i, j];
                }
            }
            return weight;
        }

        private void showInColors(Bitmap sourceImage)
        {
            for (int j = 0; j < sourceImage.Height; j++)
            {
                for (int i = 0; i < sourceImage.Width; i++)
                {
                    if (picture[i, j] == 0) renderedImage.SetPixel(i, j, Color.White);
                    else if (picture[i, j] == 1) renderedImage.SetPixel(i, j, Color.Black);
                    else if (picture[i, j] == 2) renderedImage.SetPixel(i, j, Color.Yellow);
                    else if (picture[i, j] == 3) renderedImage.SetPixel(i, j, Color.Red);
                    else if (picture[i, j] == 4) renderedImage.SetPixel(i, j, Color.Orange);
                }
            }
        }

        private void saveChangesToBitmap()
        {
            for (int j = 0; j < renderedImage.Height; j++)
            {
                for (int i = 0; i < renderedImage.Width; i++)
                {
                    if (picture[i, j] == 1) renderedImage.SetPixel(i, j, Color.Black);
                    else renderedImage.SetPixel(i, j, Color.White);
                }
            }
        }
    }
}
