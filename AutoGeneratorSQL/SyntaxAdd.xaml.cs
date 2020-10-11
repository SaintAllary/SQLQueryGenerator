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

namespace AutoGeneratorSQL
{
    /// <summary>
    /// Interaction logic for SyntaxAdd.xaml
    /// </summary>
    ///
    public partial class SyntaxAdd : Window
    {
        public bool Executed { get; set; }
        public SyntaxAdd()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Executed = true;
        }
    }
}
