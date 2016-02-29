using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
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
            openFileDialog.Filter = "Image File|*.jpg; *.jpeg; *.tiff; *.gif; *.png;";
            bool? userClickedOK = openFileDialog.ShowDialog();
            if (userClickedOK == true)
            {
                image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                imageDisplayed = new Bitmap(openFileDialog.FileName);
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
                  
                catch (Exception r) { MessageBox.Show(r.Message);  }
               
            }
        }

        private void HandleButton_Save(object sender, RoutedEventArgs e)
        {
            color = Color.FromArgb(Int32.Parse(RBox.Text), Int32.Parse(GBox.Text), Int32.Parse(BBox.Text));
            imageDisplayed.SetPixel((int)point.X, (int)point.Y, Color.FromArgb(color.A, color.R, color.G, color.B));
          /// image.Source = null;
           /// SaveImage();
           /// 
            MessageBox.Show("Zapisano nowe wartości.");

            // TODO: repaint image to show changes
            // image.Source = (BitmapImage) imageDisplayed;
        }

        private void saveFile_Click(object sender, RoutedEventArgs e)
        {
            SaveImage();
        }


        private void SaveImage()
        {

            //SaveFileDialog dialog = new SaveFileDialog();

            //if (dialog.ShowDialog().Value)
            //{
            //    imageDisplayed.Save("test.jpg", ImageFormat.Png);
            //}

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                   (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        imageDisplayed.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        imageDisplayed.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        imageDisplayed.Save(fs,
                           System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
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
                catch (Exception r) { MessageBox.Show(r.Message);  }
               
            }
        }

    }
}