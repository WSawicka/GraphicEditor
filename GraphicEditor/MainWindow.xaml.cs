using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private Bitmap imageDisplayed = null;
        private Color color;
        private System.Windows.Point point;
        int dept;

        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void HandleButton_OpenFile(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.FilterIndex = 1;
            //openFileDialog.InitialDirectory = @"C:\Users\mloda\Desktop";
            openFileDialog.Filter = "Image File|*.jpg; *.jpeg; *.tiff; *.gif; *.png; *.bmp";
            bool? userClickedOK = openFileDialog.ShowDialog();
            if (userClickedOK == true)
            {
                imageToCheck.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));

                imageToCheck.Stretch = System.Windows.Media.Stretch.Uniform;
                image.Stretch = System.Windows.Media.Stretch.Uniform;

                imageDisplayed = new Bitmap(openFileDialog.FileName);
                dept = Image.GetPixelFormatSize(imageDisplayed.PixelFormat);
            }
        }

        private void HandleImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            RBox.Text = "";
            GBox.Text = "";
            BBox.Text = "";

            if (imageDisplayed != null)
            {
                try
                {
                    point = Mouse.GetPosition(image);
                    positionX.Content = (int)point.X;
                    positionY.Content = (int)point.Y;
                    color = imageDisplayed.GetPixel((int)point.X, (int)point.Y);
                    RBox.Text = color.R.ToString();
                    GBox.Text = color.G.ToString();
                    BBox.Text = color.B.ToString();
                }

                catch (Exception r) { MessageBox.Show(r.Message); }

            }
        }

        private void HandleButton_Save(object sender, RoutedEventArgs e)
        {
            color = Color.FromArgb(Int32.Parse(RBox.Text), Int32.Parse(GBox.Text), Int32.Parse(BBox.Text));
            imageDisplayed.SetPixel((int)point.X, (int)point.Y, Color.FromArgb(color.A, color.R, color.G, color.B));
            MessageBox.Show("Zapisano nowe wartości.");
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveImage();
        }


        private void SaveImage()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|TIFF|*.tiff";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                PixelFormat test = imageDisplayed.PixelFormat;

                Encoder myEncoder;
                ImageCodecInfo myImageCodecInfo;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;
                myEncoder = Encoder.ColorDepth;


                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, dept);

                myEncoderParameters.Param[0] = myEncoderParameter;

                var encode = new EncoderParameter(Encoder.ColorDepth, 24);

                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");
                        imageDisplayed.Save(fs, myImageCodecInfo, myEncoderParameters);
                        break;
                    case 2:
                        myImageCodecInfo = GetEncoderInfo("image/bmp");
                        imageDisplayed.Save(fs, myImageCodecInfo, myEncoderParameters);
                        break;
                    case 3:
                        myImageCodecInfo = GetEncoderInfo("image/gif");
                        imageDisplayed.Save(fs, myImageCodecInfo, myEncoderParameters);
                        break;
                    case 4:
                        myImageCodecInfo = GetEncoderInfo("image/tiff");
                        imageDisplayed.Save(fs, myImageCodecInfo, myEncoderParameters);
                        break;
                }
                fs.Close();
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }


        private void getPixel_Click(object sender, RoutedEventArgs e)
        {
            RBox.Text = "";
            GBox.Text = "";
            BBox.Text = "";

            if (imageDisplayed != null)
            {
                try
                {
                    color = imageDisplayed.GetPixel(Convert.ToInt16(xPixel.Text), Convert.ToInt16(yPixel.Text));
                    RBox.Text = color.R.ToString();
                    GBox.Text = color.G.ToString();
                    BBox.Text = color.B.ToString();
                }
                catch (Exception r) { MessageBox.Show(r.Message); }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            HistogramLibrary srv = new HistogramLibrary();
            srv.CreateHistogram(imageDisplayed);
            MessageBox.Show("Zakończono kreowanie histogramów!");
        }

        private void Przyciemnij_Click(object sender, RoutedEventArgs e)
        {
            getOneValueWindow wi = new getOneValueWindow();

            if (wi.ShowDialog().Value)
            {
                HistogramLibrary srv = new HistogramLibrary();
                image.Source = BitmapToImageSource(srv.Rozjasnij(imageDisplayed, -1 * wi._value));

            }
        }

        private void Rozjaśnij_Click(object sender, RoutedEventArgs e)
        {
            getOneValueWindow wi = new getOneValueWindow();

            if (wi.ShowDialog().Value)
            {
                HistogramLibrary srv = new HistogramLibrary();
                image.Source = BitmapToImageSource(srv.Rozjasnij(imageDisplayed, wi._value));

            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void Rozciągnij_histogram_Click(object sender, RoutedEventArgs e)
        {
            HistogramLibrary srv = new HistogramLibrary();
            image.Source = BitmapToImageSource(srv.histogramEqualization(imageDisplayed));
        }

        private void Odcienie_Szarosci_Click(object sender, RoutedEventArgs e)
        {
            BinarizationLibrary bl = new BinarizationLibrary();
            image.Source = BitmapToImageSource(bl.ConvertToGrayscale(imageDisplayed));
        }

        private void Bin_Reczna_Click(object sender, RoutedEventArgs e)
        {
            getOneValueWindow wi = new getOneValueWindow();

            if (wi.ShowDialog().Value)
            {
                BinarizationLibrary bl = new BinarizationLibrary();
                image.Source = BitmapToImageSource(bl.TransformBinary(imageDisplayed, wi._value));
            }
        }

        private void Bin_Automat_Click(object sender, RoutedEventArgs e)
        {
            BinarizationLibrary bl = new BinarizationLibrary();
            image.Source = BitmapToImageSource(bl.TransformOtsu(imageDisplayed));
        }

        private void Bin_Lok_Click(object sender, RoutedEventArgs e)
        {
            TwoValueWindow window = new TwoValueWindow();
            if (window.ShowDialog().Value)
            {
                BinarizationLibrary bl = new BinarizationLibrary();
                image.Source = BitmapToImageSource(bl.TransformBinaryNiblack(imageDisplayed, window._size, window._k));
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Filtr_konwolucyjny_Click(object sender, RoutedEventArgs e)
        {
            var getMask = new GetMask();
            getMask.ShowDialog();

            var mask = getMask._mask;

            FiltrLibrary lib = new FiltrLibrary();
            Bitmap result = lib.FiltrKonwolucyjny(imageDisplayed,mask);

            MessageBox.Show("Koniec");
            image.Source = BitmapToImageSource(result);


        }

        private void Filtr_Kuwahara_Click(object sender, RoutedEventArgs e)
        {
           
            FiltrLibrary lib = new FiltrLibrary();
            Bitmap result = lib.FiltrKuwahara(imageDisplayed, null);

            MessageBox.Show("Koniec");
            image.Source = BitmapToImageSource(result);
        }

        private void FiltrM3x3_Click(object sender, RoutedEventArgs e)
        {
            FiltrLibrary lib = new FiltrLibrary();
            Bitmap result = lib.FiltrMediana(imageDisplayed, null,1);

            MessageBox.Show("Koniec");
            image.Source = BitmapToImageSource(result);
        }

        private void FiltrM5x5_Click(object sender, RoutedEventArgs e)
        {
            FiltrLibrary lib = new FiltrLibrary();
            Bitmap result = lib.FiltrMediana(imageDisplayed, null, 2);

            MessageBox.Show("Koniec");
            image.Source = BitmapToImageSource(result);
        }

        private void Thinning_Click(object sender, RoutedEventArgs e)
        {
            //krok 1: binaryzacja
            BinarizationLibrary bl = new BinarizationLibrary();
            Bitmap bi = bl.TransformBinary(imageDisplayed, 125);

            //krok2: algorytm ścieniania
            ThinningLibrary thinning = new ThinningLibrary();
            image.Source = BitmapToImageSource(thinning.thin(bi));
        }

        private void PunktyCharakterystyczne_Click(object sender, RoutedEventArgs e)
        {
            MunucjeLibrary lib = new MunucjeLibrary();
            image.Source = BitmapToImageSource( lib.ZnajdzZakonczenia(imageDisplayed));
            MessageBox.Show("Koniec");
        }
    }
}