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
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        List<topic> topiclist = new List<topic>()
            {
                new topic(){number = 1,name="第一章名",content="内容1" },
            new topic() { number = 2, name = "第二章名", content = "内容2" },
            new topic() { number = 3, name = "第三章名", content = "内容3" },
            new topic() { number = 4, name = "第四章名", content = "内容4" },
            new topic() { number = 5, name = "第五章名", content = "内容5" }
            };
        
        public Window1(string fiction_name,int author_name,int total_number)
        {
            InitializeComponent();
            Fiction_name.Text = fiction_name;
            //Author_name.Text = Convert.ToString(author_name);
            Total_number.Text = $"小说|完结|{total_number}字";
            this.detaillist.ItemsSource = topiclist;
        }
        

        #region 立即阅读
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var firstint = topiclist.First();
            var listsize = topiclist.Count;
            MessageBox.Show(listsize.ToString());
            int propotion = 1;
            ReadWindow readWindow1 = new ReadWindow(firstint.number,firstint.name,firstint.content,propotion);
            readWindow1.Show();
            this.Close();
        }
        #endregion

        #region 双击item阅读
        private void SListView_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            topic emp = detaillist.SelectedItem as topic;
            var listsize = topiclist.Count;
            Console.WriteLine(listsize);
            int propotion = (emp.number - 1) *100 / listsize;
            ReadWindow readWindow1 = new ReadWindow(emp.number,emp.name,emp.content,propotion);
            readWindow1.Show();
        }
        #endregion

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }


    #region 数据源
    public class topic
    {
        public int number { get; set; }
        public string name { get; set; }
        public string content { get; set; }
    }
    #endregion
}
