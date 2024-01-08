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

namespace ACMD
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        public OutputWindow(string text)
        {
            InitializeComponent();

            // 출력 값을 TextBlock에 표시합니다.
            outputTextBox.Text = text;
        }
    }
}
