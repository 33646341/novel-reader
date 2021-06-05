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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OnlineSearchAndRead;
using Novel_Spider;
using NovelManager;
using System.ComponentModel;
using System.Threading;
using System.Collections.ObjectModel;

namespace UIdesign
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Visibility showProgress = Visibility.Collapsed;

        public Visibility ShowProgress
        {
            get { return showProgress; }
            set
            {
                showProgress = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(ShowProgress)));
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void textBoxId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnlineSearchAndRead.Form1 form = new OnlineSearchAndRead.Form1();
            String kw = keySearch.Text;
            form.te = kw;

            ShowProgress = Visibility.Visible;

            List<fiction_info> _ltfi_Search = new List<fiction_info>();

            this.listBoxStudents.ItemsSource = _ltfi_Search;//数据源
            this.listBoxStudents.DisplayMemberPath = "col_fiction_name";//路径

            new Thread(() =>
            {
                for (int i = 10000; i < 10020; i++)
                {
                    var fiction_i = new fiction_info();
                    fiction_i.col_fiction_name = i.ToString();
                    Thread.Sleep(200);

                    Dispatcher.Invoke(delegate ()
                    {
                        _ltfi_Search.Add(fiction_i);
                        listBoxStudents.ItemsSource = null;
                        listBoxStudents.ItemsSource = _ltfi_Search;//刷新数据源
                    });
                    //listBoxStudents.ItemsSource = _ltfi_Search;//数据源
                    //PropertyChanged(this, new PropertyChangedEventArgs(nameof(_ltfi_Search)));
                }
                ShowProgress = Visibility.Collapsed;
            }).Start();
            //List<fiction_info> _ltfi_Search = form.button1_Click();//_ltfi_Search是列表，每一项是一个小说信息

            //为ListView设置Binding

            //ListView.ItemsSourceProperty.
            //为TextBox设置Binding
            //Binding binding = new Binding("SelectedItem.col_fiction_name") { Source = this.listBoxStudents };
            //this.textBoxId.SetBinding(TextBox.TextProperty, binding);

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void listBoxStudents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void SListBox_ItemDoubleClick(object sender, RoutedEventArgs e)
        {
            Window1 login1 = new Window1();   //Login为窗口名，把要跳转的新窗口实例化
            login1.Show();   //打开新窗口
            //this.Close();
        }
    }
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
