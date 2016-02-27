using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace GraphicEditor
{
    public partial class MainWindow : Window
    {
        private Bitmap imageDisplayed = null;
        private Color? color = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HandleButton_OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
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

        private void HandleImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (imageDisplayed != null)
            {
                Console.WriteLine("Inside!");
                //imageDisplayed.GetPixel();
            }

        }
    }
}
