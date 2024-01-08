using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ACMD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (inputTextBox.Text == "Enter text here..." && inputTextBox.Foreground == Brushes.Gray)
            {
                return;
            }

            if (inputTextBox.Text.Length > 80)
            {
                var messageBox = new CustomMessageBox("You have exceeded the character limit.");
                messageBox.Owner = this;
                messageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                messageBox.ShowDialog();
                inputTextBox.Text = string.Empty;
                return;
            }

            string[] words = inputTextBox.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            wordListBox.Items.Clear();

            foreach (var word in words)
            {
                var checkBox = new CheckBox { Content = word };
                wordListBox.Items.Add(checkBox);
            }
        }

        private OutputWindow outputWindow = default;

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (newWordTextBox.Text == "Enter text here..." && newWordTextBox.Foreground == Brushes.Gray)
            {
                return;
            }

            var selectedWords = wordListBox.Items.OfType<CheckBox>().Where(cb => cb.IsChecked == true).Select(cb => cb.Content.ToString()).ToList();

            if (!selectedWords.Any())
            {
                CustomMessageBox customMessageBox = new CustomMessageBox("No selected value");
                customMessageBox.Owner = this;
                customMessageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                customMessageBox.ShowDialog();
                return;
            }

            string[] newWords = newWordTextBox.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (newWords.Length > 1000)
            {
                CustomMessageBox customMessageBox = new CustomMessageBox("You have exceeded the lines limit");
                customMessageBox.Owner = this;
                customMessageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                customMessageBox.ShowDialog();
                return;
            }

            List<string> lines = new List<string>();

            foreach (var newWord in newWords)
            {
                if (newWord.Length > 50)
                {
                    CustomMessageBox customMessageBox = new CustomMessageBox($"Line {Array.IndexOf(newWords, newWord) + 1}: You have exceeded the character limit");
                    customMessageBox.Owner = this;
                    customMessageBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    customMessageBox.ShowDialog();
                    return;
                }

                string modifiedText = inputTextBox.Text;

                foreach (var word in selectedWords)
                {
                    if (word != null)
                    {
                        modifiedText = modifiedText.Replace(word, newWord);
                    }
                }

                lines.Add(modifiedText);
            }

            if (outputWindow != null)
            {
                outputWindow.Close();
            }

            outputWindow = new OutputWindow(string.Join("\n", lines));
            outputWindow.Show();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter text here..." && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter text here...";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter text here..." && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void NewWordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter text here..." && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void NewWordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter text here...";
                textBox.Foreground = Brushes.Gray;
            }
        }

        private void NewWordTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter text here..." && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = "";
                textBox.Foreground = Brushes.Black;
            }
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private const int GWL_STYLE = -16;
        private const int WS_MAXIMIZEBOX = 0x10000;

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var hwnd = new WindowInteropHelper((Window)sender).Handle;
            var value = GetWindowLong(hwnd, GWL_STYLE);
            SetWindowLong(hwnd, GWL_STYLE, (int)(value & ~WS_MAXIMIZEBOX));
        }

    }
}