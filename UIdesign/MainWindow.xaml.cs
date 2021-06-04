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

namespace UIdesign
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
            List<fiction_info> _ltfi_Search = form.button1_Click();//_ltfi_Search是列表，每一项是一个小说信息

            //为ListView设置Binding
            this.listBoxStudents.ItemsSource = _ltfi_Search;//数据源
            this.listBoxStudents.DisplayMemberPath = "col_fiction_name";//路径
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
    }
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
