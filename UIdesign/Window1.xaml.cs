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
using System.Threading;
using System.Net;
using System.Text.RegularExpressions;
using ReadTool;




namespace UIdesign
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>

    public partial class Window1 : Window
    {

        ObservableCollection<Chapterlist> alllist = new ObservableCollection<Chapterlist>();
        ObservableCollection<note> notelist = new ObservableCollection<note>();
        Fiction f1 = new Fiction();
        List<chapter_list> l2;
        This_chapter_list pro = new This_chapter_list();
        int seed = 0;
        bool isonline1;
        public Window1(List<chapter_list> l1, fiction_info a,Fiction f,Boolean isonline, This_chapter_list prolist)
        {
            InitializeComponent();
            l2 = l1;
            pro = prolist;
            isonline1 = isonline;
            if (!isonline)
            {
                Fiction_name.Text = a.col_fiction_name;
                Author_name.Text = a.col_fiction_author + " | " + a.col_fiction_type;
                Total_number.Text = $"小说 | " + a.col_fiction_stata;
                this.detaillist.ItemsSource = alllist;
                this.notelist1.ItemsSource = notelist;
                //this.introduction.Text = a.col_fiction_introduction;
                this.surfaceimg.Source = new BitmapImage(new Uri(a.col_url_poster));
                f1 = f;
                for (int i = 0; i < l1.Count; i++)
                {
                    int l = 0;
                    string chapter_name = "";
                    string chapter_number = "";

                    for (; l < l1[i].col_chapter_name.Length; l++)
                    {
                        if (l1[i].col_chapter_name[l] == '章') break;
                        if (l1[i].col_chapter_name[l] != '章' && l == l1[i].col_chapter_name.Length - 1)
                        {
                            chapter_name = l1[i].col_chapter_name;
                            chapter_number = "章节数格式不规范！";
                        }

                    }
                    if (l < l1[i].col_chapter_name.Length)
                    {
                        chapter_name = l1[i].col_chapter_name.Substring(l + 1, l1[i].col_chapter_name.Length - l - 1);
                        if (chapter_name == "") chapter_name = "[无章节名]";
                        //if (chapter_name[0] == ':') chapter_name = chapter_name.Substring(1, l1[i].col_chapter_name.Length - l - 1);
                        chapter_number = l1[i].col_chapter_name.Substring(0, l + 1);
                    }

                    Random rd = new Random(seed);  //无参即为使用系统时钟为种子
                    string ss = rd.Next(100).ToString();
                    int v = int.Parse(ss);
                    seed = seed + 5;
                    var chapterlist1 = new Chapterlist()
                    {

                        number = chapter_number,
                        name = chapter_name,
                        url = l1[i].col_chapter_url,
                        value = v,
                        content=""
                    };

                    alllist.Add(chapterlist1);
                }

            }
            else
            {
                MessageBox.Show(prolist.chapter_name.Count.ToString());
                Fiction_name.Text = f.fic_name;
                Author_name.Text = f.fic_author + " | " ;
                Total_number.Text = $"小说 | 已下载 ";
                this.detaillist.ItemsSource = alllist;
                this.notelist1.ItemsSource = notelist;
                //this.introduction.Text = a.col_fiction_introduction;
                //this.surfaceimg.Source = new BitmapImage(new Uri(a.col_url_poster));
                f1 = f;
                for (int i = 0; i < prolist.chapter_name.Count; i++)
                {
                    int l = 0;
                    string chapter_name = "";
                    string chapter_number = "";

                    for (; l < prolist.chapter_name[i].Length; l++)
                    {
                        if (prolist.chapter_name[i][l] == '章') break;
                        if (prolist.chapter_name[i][l] != '章' && l == prolist.chapter_name[i].Length - 1)
                        {
                            chapter_name = prolist.chapter_name[i];
                            chapter_number = "章节数格式不规范！";
                        }

                    }
                    if (l < prolist.chapter_name[i].Length)
                    {
                        chapter_name = prolist.chapter_name[i].Substring(l + 1, prolist.chapter_name[i].Length - l - 1);
                        if (chapter_name == "") chapter_name = "[无章节名]";
                        //if (chapter_name[0] == ':') chapter_name = chapter_name.Substring(1, l1[i].col_chapter_name.Length - l - 1);
                        chapter_number = prolist.chapter_name[i].Substring(0, l + 1);
                    }

                    Random rd = new Random(seed);  //无参即为使用系统时钟为种子
                    string ss = rd.Next(100).ToString();
                    int v = int.Parse(ss);
                    seed = seed + 5;
                    var chapterlist1 = new Chapterlist()
                    {

                        number = chapter_number,
                        name = chapter_name,
                        url = "",
                        value = v,
                        content = prolist.chapter_content[i]
                    };

                    alllist.Add(chapterlist1);
                }
            }
            
            for (int l = 0; l < 4; l++)
            {
                var note1 = new note()
                { 
                    index = l,
                    context = "abababab"
                };
                notelist.Add(note1);
            }

        }
        #region 添加笔记项
        public void addnewnote(string s)
        {

            var note1 = new note()
            {
                index = notelist.Count,
                context = s
            };
            notelist.Add(note1);

        }
        #endregion


        #region 立即阅读
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            var firstint = alllist.First();
            var listsize = alllist.Count;
            //MessageBox.Show(firstint.url);
            ReadWindow readWindow1 = new ReadWindow(firstint.url, firstint.number, firstint.name, l2,0,isonline1,pro);
            readWindow1.Show();
            
        }
        #endregion

        // 数据库添加已阅读的记录
        void saveReadRecord(Chapterlist fic,int index)
        {
            // 数据库开始
            var novelDAL = new NovelManager.NovelDAL();
            if (novelDAL.exsitsNovel(fic.name) == 0)
            {
                novelDAL.addNovel(fic.name, fic.url);
            }
            var Novel_id = novelDAL.exsitsNovel(fic.name);
            Console.WriteLine("章节写入阅读记录 Novel_id = " + Novel_id);
            novelDAL.addChapter(Novel_id, index, fic.name, fic.url);
            var Chapter_id = novelDAL.exsitsChapter(fic.url);
            novelDAL.updateChapter(Chapter_id, "readTimes", "1");
            // 数据库结束
        }


        // 数据库添加已阅读的记录

        #region 双击item阅读
        private void SListView_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Chapterlist emp = detaillist.SelectedItem as Chapterlist;
            int index = detaillist.SelectedIndex;
            saveReadRecord(emp, index);
            var listsize = alllist.Count;
            Console.WriteLine(listsize);
            //int index = this.detaillist.Items.IndexOf(emp);
            ReadWindow readWindow1 = new ReadWindow(emp.url, emp.number, emp.name,l2,index,isonline1,pro);
            readWindow1.Show();
        }
        #endregion

        #region 点击下载

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //foreach(Window a in Application.Current.Windows)
            //{
            //    Console.WriteLine("Title = " + a.Title);
            //}

            //Console.WriteLine("[1] Title = " + Application.Current.Windows[2]);

                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.Down_Load(sender, e, f1);
        }

       
        #endregion

        #region 加入书架
        ObservableCollection<CardModel> boksf = new ObservableCollection<CardModel>();//书架页
        private void Button_Click(object sender, RoutedEventArgs e)
        {
                MainWindow mainwin = Application.Current.MainWindow as MainWindow;
                mainwin.Add_Bksf(sender, e, f1);
        }

        //public void Add_Bksf(object sender, RoutedEventArgs e, Fiction fic)
        //{
        //    BitmapImage bi = new BitmapImage();
        //    // BitmapImage.UriSource must be in a BeginInit/EndInit block.
        //    bi.BeginInit();
        //    bi.UriSource = new Uri(@"..\..\img\emp.jpg", UriKind.RelativeOrAbsolute);
        //    bi.EndInit();

        //    CardModel bok = new CardModel() { Cover = bi, Fiction = fic.Name, Writer = fic.Author, Url = fic.Url };


        //    boksf.Add(bok);

        //    // 数据库开始
        //    var novelDAL = new NovelManager.NovelDAL();
        //    if (novelDAL.exsitsNovel(fic.Name) == 0)
        //    {
        //        novelDAL.addNovel(fic.Name, fic.Url);
        //    }
        //    var Novel_id = novelDAL.exsitsNovel(fic.Name);
        //    Console.WriteLine(Novel_id);
        //    novelDAL.updateNovel(Novel_id, "starred", "1");
        //    // 数据库结束




        //    new Thread(() =>
        //    {
        //        WebClient webClient = new WebClient();
        //        var html = webClient.DownloadString(fic.Url.Replace("www", "m"));
        //        var matchResult = Regex.Match(html, @"og:image"" content=""(.*)""/>");
        //        var imageUrl = "";
        //        if (matchResult.Success)
        //        {
        //            imageUrl = matchResult.Groups[1].Value;
        //            //HandyControl.Controls.MessageBox.Show(imageUrl);

        //            novelDAL.updateNovel(Novel_id, "imageURL", imageUrl); // 写入数据库

        //            Dispatcher.Invoke(delegate ()
        //            {
        //                BitmapImage img = new BitmapImage(new Uri(imageUrl));
        //                bok.Cover = img;
        //            });
        //        }
        //    }).Start();
        //}


        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void detaillist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        #region
        public void AddNote(object sender, RoutedEventArgs e, String s)
        {

        }
        #endregion
    }



    #region 数据源
    public class Chapterlist
    {
        public string number { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public int value { get; set; }
        public string content { get; set; }
    }

    public class note
    {
        public int index { get; set; }
        public string context { get; set; }
    }
    #endregion
}
