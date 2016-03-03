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
    /// Interaction logic for getOneValueWindow.xaml
    /// </summary>
    public partial class getOneValueWindow : Window
    {

        public int _value { set; get; }
        public getOneValueWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _value = Convert.ToInt32(value.Text);
            this.DialogResult = true;
            this.Close();
        }
    }
}
