using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GraphicEditor
{
    /// <summary>
    /// Interaction logic for GetMask.xaml
    /// </summary>
    public partial class GetMask5x5 : Window
    {
        public GetMask5x5()
        {
            InitializeComponent();
        }

        public int[,] _mask { set; get; }
 
        private void button_Click(object sender, RoutedEventArgs e)
        {
            int[,] mask = new int[3,3];

            mask[0, 0] = Convert.ToInt32(textBlock.Text);
            mask[0, 1] = Convert.ToInt32(textBlock1.Text);
            mask[0, 2] = Convert.ToInt32(textBlock2.Text);

            mask[1, 0] = Convert.ToInt32(textBlock3.Text);
            mask[1, 1] = Convert.ToInt32(textBlock4.Text);
            mask[1, 2] = Convert.ToInt32(textBlock5.Text);

            mask[2, 0] = Convert.ToInt32(textBlock6.Text);
            mask[2, 1] = Convert.ToInt32(textBlock7.Text);
            mask[2, 1] = Convert.ToInt32(textBlock8.Text);

            _mask = mask;
            this.Close();
        }

        private void buttonFiltrD_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonPrewitta_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "-1";
            textBlock1.Text = "0";
            textBlock2.Text = "1";
            textBlock3.Text = "-1";
            textBlock4.Text = "0";
            textBlock5.Text = "1";
            textBlock6.Text = "-1";
            textBlock7.Text = "0";
            textBlock8.Text = "1";
        }

        private void buttonSobela_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "1";
            textBlock1.Text = "2";
            textBlock2.Text = "1";
            textBlock3.Text = "0";
            textBlock4.Text = "0";
            textBlock5.Text = "0";
            textBlock6.Text = "-1";
            textBlock7.Text = "-2";
            textBlock8.Text = "-1";
        }

        private void buttonLaplace_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "0";
            textBlock1.Text = "-1";
            textBlock2.Text = "0";
            textBlock3.Text = "-1";
            textBlock4.Text = "4";
            textBlock5.Text = "-1";
            textBlock6.Text = "0";
            textBlock7.Text = "-1";
            textBlock8.Text = "0";
        }

        private void buttonNar_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "-1";
            textBlock1.Text = "-1";
            textBlock2.Text = "-1";
            textBlock3.Text = "2";
            textBlock4.Text = "2";
            textBlock5.Text = "2";
            textBlock6.Text = "-1";
            textBlock7.Text = "-1";
            textBlock8.Text = "-1";
        }

        private void buttonFltrD_Click(object sender, RoutedEventArgs e)
        {
            textBlock.Text = "1";
            textBlock1.Text = "1";
            textBlock2.Text = "1";
            textBlock3.Text = "1";
            textBlock4.Text = "1";
            textBlock5.Text = "1";
            textBlock6.Text = "1";
            textBlock7.Text = "1";
            textBlock8.Text = "1";
        }
    }
}
