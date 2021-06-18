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
using OnlineSearchAndRead;
using Microsoft.VisualBasic;


namespace UIdesign
{
    /// <summary>
    /// ReadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ReadWindow : Window
    {
        string content;
        int index1;
        List<chapter_list> list1;
        public ReadWindow(string url, string number, string name, List<chapter_list> l1, int index)
        {
            InitializeComponent();
            index1 = index;
            list1 = l1;
            get_chapter_content g = new get_chapter_content();
            string url1 = "https://www.biquzhh.com" + url;
            content = g.Get_Chapter_Content(url1);
            //MessageBox.Show(content);
            //Run r = new Run();

            foreach (var p in content.Split("\r\n".ToCharArray()))
            {
                if (p.Length == 0 || p == "　　")
                //if (p.Length < 5)
                {
                    Console.WriteLine("跳过空章节！->" + p + "<-"); continue;
                }
                FlowDocument1.Blocks.Add(new Paragraph(new Run("　　" + p)));
            }
            //Paragraph paragraph1 = new Paragraph(r/*new Run(g.Get_Chapter_Content(url1))*/);
            double pvalue = (index + 1) * 100 / l1.Count;
            if (pvalue < 1) pvalue = 1;
            ProgressBar1.Value = pvalue;
            textblock1.Text = number + " " + name;
        }

        private void SelectedColorChanged1(object sender, RoutedEventArgs e)
        {
            string color = ColorPicker.SelectedBrush.ToString();
            var color1 = (Color)ColorConverter.ConvertFromString(color);
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);
            flowdocumentreader1.Background = BColor;
        }
        private void SelectedColorChanged2(object sender, RoutedEventArgs e)
        {
            string color = ColorPicker1.SelectedBrush.ToString();
            var color1 = (Color)ColorConverter.ConvertFromString(color);
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);
            flowdocumentreader1.Foreground = BColor;
        }
        private void Canceled(object sender, EventArgs e)
        {
            var color1 = (Color)ColorConverter.ConvertFromString("#FFF3F1EC");
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);

            flowdocumentreader1.Background = BColor;
        }
        private void Canceled2(object sender, EventArgs e)
        {
            var color1 = (Color)ColorConverter.ConvertFromString("#000000");
            System.Windows.Media.Brush BColor = new SolidColorBrush(color1);
            flowdocumentreader1.Foreground = BColor;
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

            flowdocumentreader1.Height = 800;
            //MessageBox.Show("Esc退出全屏");
        }
        private void Grid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)//Esc键
            {
                this.WindowState = System.Windows.WindowState.Normal;
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
            }

            flowdocumentreader1.Height = 578;
        }

        //private void bigger_Click(object sender, RoutedEventArgs e)
        //{
        //    label1.FontSize= label1.FontSize+3;
        //}

        //private void smaller_Click(object sender, RoutedEventArgs e)
        //{
        //    if (label1.FontSize > 3)
        //        label1.FontSize = label1.FontSize - 3;
        //    else
        //        MessageBox.Show("字体已经不能缩小！");
        //}

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(addnote.IsLoaded.ToString());
            //MessageBox.Show(FlowDocument1.IsLoaded.ToString());

            index1++;
            int l = 0;
            string chapter_name = "";
            string chapter_number = "";

            for (; l < list1[index1].col_chapter_name.Length; l++)
            {
                if (list1[index1].col_chapter_name[l] == '章') break;
                if (list1[index1].col_chapter_name[l] != '章' && l == list1[index1].col_chapter_name.Length - 1)
                {
                    chapter_name = list1[index1].col_chapter_name;
                    chapter_number = "章节数不规范！";
                }

            }
            if (l < list1[index1].col_chapter_name.Length)
            {
                chapter_name = list1[index1].col_chapter_name.Substring(l + 1, list1[index1].col_chapter_name.Length - l - 1);
                if (chapter_name == "") chapter_name = "章节名称不规范!";
                if (chapter_name[0] == ':') chapter_name = chapter_name.Substring(1, list1[index1].col_chapter_name.Length - l - 1);
                chapter_number = list1[index1].col_chapter_name.Substring(0, l + 1);
            }

            var chapterlist1 = new Chapterlist()
            {

                number = chapter_number,
                name = chapter_name,
                url = list1[index1].col_chapter_url
            };

            ReadWindow readWindow1 = new ReadWindow(chapterlist1.url, chapterlist1.number, chapterlist1.name, list1, index1);
            readWindow1.Show();
            Close();
            //if (this.IsLoaded)
            //{
            //    Run r = new Run(content);
            //    Paragraph paragraph1 = new Paragraph(r/*new Run(g.Get_Chapter_Content(url1))*/);
            //    //FlowDocument1.Blocks.Add(paragraph1);
            //}

        }

        bool iswriting = false;
        string s;
        private void addnote_Click(object sender, RoutedEventArgs e)
        {
            if (iswriting == false)
            {
                GroupBox1.Visibility = Visibility.Visible;
                s = Textnode.Text;
                Textnode.Focus();
                addnote.Content = "保存笔记";
                iswriting = true;
            }
            else
            {
                GroupBox1.Visibility = Visibility.Collapsed;
                addnote.Content = "添加笔记";
                iswriting = false;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            if (index1 > 0)
            {
                index1--;
                int l = 0;
                string chapter_name = "";
                string chapter_number = "";

                for (; l < list1[index1].col_chapter_name.Length; l++)
                {
                    if (list1[index1].col_chapter_name[l] == '章') break;
                    if (list1[index1].col_chapter_name[l] != '章' && l == list1[index1].col_chapter_name.Length - 1)
                    {
                        chapter_name = list1[index1].col_chapter_name;
                        chapter_number = "章节数格式不规范！";
                    }

                }
                if (l < list1[index1].col_chapter_name.Length)
                {
                    chapter_name = list1[index1].col_chapter_name.Substring(l + 1, list1[index1].col_chapter_name.Length - l - 1);
                    if (chapter_name == "") chapter_name = "章节名称格式不规范!";
                    if (chapter_name[0] == ':') chapter_name = chapter_name.Substring(1, list1[index1].col_chapter_name.Length - l - 1);
                    chapter_number = list1[index1].col_chapter_name.Substring(0, l + 1);
                }

                var chapterlist1 = new Chapterlist()
                {

                    number = chapter_number,
                    name = chapter_name,
                    url = list1[index1].col_chapter_url
                };

                ReadWindow readWindow1 = new ReadWindow(chapterlist1.url, chapterlist1.number, chapterlist1.name, list1, index1);
                readWindow1.Show();
                Close();
            }
            else
            {
                MessageBox.Show("已经是第一章！");
            }

        }
    }
}
