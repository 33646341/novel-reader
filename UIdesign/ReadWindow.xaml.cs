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

namespace UIdesign
{
    /// <summary>
    /// ReadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ReadWindow : Window
    {
        public ReadWindow(int number,string name,string content,int propotion)
        {
            InitializeComponent();
            label1.Content = content;
            ProgressBar1.Value = propotion;
        }
        char i = '0';
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (i == '0') i = '1';
            else if (i == '1') i = '2';
            else if (i == '2') i = '3';
            else if (i == '3') i = '0';
            switch (i)
            {
                case '0':
                    {
                        var color = (Color)ColorConverter.ConvertFromString("#FFF3F1EC");
                        var color1 = (Color)ColorConverter.ConvertFromString("#000000");
                        System.Windows.Media.Brush BColor = new SolidColorBrush(color);
                        System.Windows.Media.Brush CColor = new SolidColorBrush(color1);
                        label1.Background = BColor;
                        label1.Foreground = CColor;
                    }
                    break;
                case '1':
                    {
                        var color = (Color)ColorConverter.ConvertFromString("#FF3D3A3D");
                        var color1 = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                        System.Windows.Media.Brush BColor = new SolidColorBrush(color);
                        System.Windows.Media.Brush CColor = new SolidColorBrush(color1);
                        label1.Background = BColor;
                        label1.Foreground = CColor;
                    }
                    break;
                case '2':
                    {

                        var color = (Color)ColorConverter.ConvertFromString("#4ea397");
                        System.Windows.Media.Brush BColor = new SolidColorBrush(color);
                        label1.Background = BColor;
                    };
                    break;
                case '3':
                    {
                        var color = (Color)ColorConverter.ConvertFromString("#e6c6cf");
                        System.Windows.Media.Brush BColor = new SolidColorBrush(color);
                        label1.Background = BColor;
                    };
                    break;
            }

        }

        int l = 0;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //l += 1;
            //System.Timers.Timer t = new System.Timers.Timer(600);
            //t.Interval = 600;

            //t.Elapsed += (s, ee) => { t.Enabled = false; l = 0; };
            //t.Enabled = true;
            //if (l % 2 == 0)
            //{
            //    t.Enabled = false;
            //    l = 0;
            //}

            this.Topmost = true;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.WindowState = System.Windows.WindowState.Maximized;
            pop3.IsOpen = true;
            label1.Height = 800;
            //MessageBox.Show("Esc退出全屏");
        }
        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)//Esc键
            {
                this.WindowState = System.Windows.WindowState.Normal;
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            }
            pop3.Visibility = System.Windows.Visibility.Hidden;
            label1.Height = 513;
        }

        Boolean isfont = false;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!isfont)
            {
                bigger.Visibility = System.Windows.Visibility.Visible;
                smaller.Visibility = System.Windows.Visibility.Visible;
                fontimg.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                bigger.Visibility = System.Windows.Visibility.Hidden;
                smaller.Visibility = System.Windows.Visibility.Hidden;
                fontimg.Visibility = System.Windows.Visibility.Visible;

            }
            isfont = !isfont;
        }

        private void bigger_Click(object sender, RoutedEventArgs e)
        {
            label1.FontSize++;
        }

        private void smaller_Click(object sender, RoutedEventArgs e)
        {
            label1.FontSize--;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
