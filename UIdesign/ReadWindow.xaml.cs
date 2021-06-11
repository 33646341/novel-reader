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
        private void SelectedColorChanged1(object sender, RoutedEventArgs e)
        {
            string color = ColorPicker.SelectedBrush.ToString();
            var color1 = (Color)ColorConverter.ConvertFromString(color);
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);
            label1.Background = BColor;
        }
        private void SelectedColorChanged2(object sender, RoutedEventArgs e)
        {
            string color = ColorPicker1.SelectedBrush.ToString();
            var color1 = (Color)ColorConverter.ConvertFromString(color);
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);
            label1.Foreground = BColor;
        }
        private void Canceled(object sender, EventArgs e)
        {
            var color1 = (Color)ColorConverter.ConvertFromString("#FFF3F1EC");
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);
            label1.Background = BColor;
        }
        private void Canceled2(object sender, EventArgs e)
        {
            var color1 = (Color)ColorConverter.ConvertFromString("#000000");
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);
            label1.Foreground = BColor;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ColorPicker.Visibility == System.Windows.Visibility.Hidden)
                ColorPicker.Visibility = System.Windows.Visibility.Visible;
            else
                ColorPicker.Visibility = System.Windows.Visibility.Hidden;

        }

        private void SecondButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorPicker1.Visibility == System.Windows.Visibility.Hidden)
                ColorPicker1.Visibility = System.Windows.Visibility.Visible;
            else
                ColorPicker1.Visibility = System.Windows.Visibility.Hidden;

        }
        private void ThirdButton_Click(object sender, RoutedEventArgs e)
        {
                ColorPicker.Visibility = System.Windows.Visibility.Hidden;
                ColorPicker1.Visibility = System.Windows.Visibility.Hidden;

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
     
            label1.Height = 578;
        }

        private void bigger_Click(object sender, RoutedEventArgs e)
        {
            label1.FontSize= label1.FontSize+3;
        }

        private void smaller_Click(object sender, RoutedEventArgs e)
        {
            if (label1.FontSize > 3)
                label1.FontSize = label1.FontSize - 3;
            else
                MessageBox.Show("字体已经不能缩小！");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
