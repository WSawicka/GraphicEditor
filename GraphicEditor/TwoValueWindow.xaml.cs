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
    /// Interaction logic for TwoValueWindow.xaml
    /// </summary>
    public partial class TwoValueWindow : Window
    {
        public double _k { set; get; }
        public int _size { set; get; }

        public TwoValueWindow()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _size = Convert.ToInt32(size.Text);
            _k = Convert.ToDouble(k.Text);
            this.DialogResult = true;
            this.Close();
        }
    }
}
