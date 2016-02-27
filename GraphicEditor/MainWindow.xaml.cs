﻿using Microsoft.Win32;
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
        private Color color;
        System.Windows.Point point;

        public MainWindow()
        {
            InitializeComponent();
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
                point = Mouse.GetPosition(image);
                positionX.Content = (int)point.X;
                positionY.Content = (int)point.Y;
                color = imageDisplayed.GetPixel((int)point.X, (int)point.Y);
                RBox.Text = color.R.ToString();
                GBox.Text = color.G.ToString();
                BBox.Text = color.B.ToString();
            }
        }

        private void HandleButton_Save(object sender, RoutedEventArgs e)
        {
            color = Color.FromArgb(Int32.Parse(RBox.Text), Int32.Parse(GBox.Text), Int32.Parse(BBox.Text));
            imageDisplayed.SetPixel((int)point.X, (int)point.Y, Color.FromArgb(color.A, color.R, color.G, color.B));
            image.Source = null;
            // TODO: repaint image to show changes
            // image.Source = (BitmapImage) imageDisplayed;
        }
    }
}
