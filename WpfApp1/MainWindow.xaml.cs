using System;
using System.Windows;
using System.Windows.Navigation;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Window1 w1 = new Window1();
            //w1.Show();

            NavigationWindow window = new NavigationWindow();
            window.Source = new Uri("Page1.xaml", UriKind.Relative);
            window.Show();
            //window.content = new Page1();

            //NavigationWindow window = new NavigationWindow();
            //window.Source = new Uri("Page1.xaml", UriKind.Relative);
            //window.Show();
        }

        private void mycheck_Copy_Checked(object sender, RoutedEventArgs e)
        {
            lbl.Content = "已选择";
        }

        private void mycheck_Copy_Unchecked(object sender, RoutedEventArgs e)
        {
            lbl.Content = "已取消";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Novel_Spider.Form1 f = new Novel_Spider.Form1();
            f.Show();
            //f.run("https://www.biquzhh.com/29719_29719087/");
        }
    }
}
