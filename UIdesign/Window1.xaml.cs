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
using OnlineSearchAndRead;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace UIdesign
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>

    public partial class Window1 : Window
    {

        ObservableCollection<Chapterlist> alllist = new ObservableCollection<Chapterlist>();
        string url = "";
        public Window1(List<chapter_list> l1, fiction_info a)
        {
            InitializeComponent();
            Fiction_name.Text = a.col_fiction_name;
            Author_name.Text = a.col_fiction_author + " | " + a.col_fiction_type;
            Total_number.Text = $"小说 | " + a.col_fiction_stata;
            this.detaillist.ItemsSource = alllist;
            this.introduction.Text = a.col_fiction_introduction;
            this.surfaceimg.Source = new BitmapImage(new Uri(a.col_url_poster));

            for (int i = 0; i < l1.Count; i++)
            {
                int l = 0;
                string chapter_name = "";
                string chapter_number = "";
                url = l1[i].col_chapter_url;
                for (; l < l1[i].col_chapter_name.Length; l++)
                {
                    if (l1[i].col_chapter_name[l] == '章') break;
                    if (l1[i].col_chapter_name[l] != '章' && l == l1[i].col_chapter_name.Length - 1)
                    {
                        chapter_name = "断更";
                    }
                }
                if (l < l1[i].col_chapter_name.Length)
                {
                    chapter_name = l1[i].col_chapter_name.Substring(l + 1, l1[i].col_chapter_name.Length - l - 1);
                    chapter_number = l1[i].col_chapter_name.Substring(0, l + 1);
                }

                var chapterlist1 = new Chapterlist()
                {

                    number = chapter_number,
                    name = chapter_name,
                    content = l1[i].col_chapter_content
                };

                alllist.Add(chapterlist1);

            }

        }


        #region 立即阅读
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var firstint = alllist.First();
            var listsize = alllist.Count;
            MessageBox.Show(listsize.ToString());
            int propotion = 1;
            ReadWindow readWindow1 = new ReadWindow(url, firstint.number, firstint.name, firstint.content, propotion);
            readWindow1.Show();
            this.Close();
        }
        #endregion

        #region 双击item阅读
        private void SListView_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Chapterlist emp = detaillist.SelectedItem as Chapterlist;
            var listsize = alllist.Count;
            Console.WriteLine(listsize);
            //int index = this.detaillist.Items.IndexOf(emp);
            int propotion =  /*index*100 / listsize;*/80;
            ReadWindow readWindow1 = new ReadWindow(url, emp.number, emp.name, emp.content, propotion);
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

        private void detaillist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


    #region 数据源
    public class Chapterlist
    {
        public string number { get; set; }
        public string name { get; set; }
        public string content { get; set; }
    }
    #endregion
}
