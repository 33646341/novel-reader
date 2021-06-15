﻿using System;
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
using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using HandyControl.Controls;

namespace UIdesign
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : System.Windows.Window, INotifyPropertyChanged
    {
        #region 加载圈相关
        public event PropertyChangedEventHandler PropertyChanged;
        private Visibility menustat = Visibility.Visible;
        public Visibility Conmenu
        {
            get { return menustat; }
            set
            {
                menustat = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(Conmenu)));
            }
        }
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
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            #region
            DataContext = this;
            #endregion
            DemoModel = new PropertyGridModel
            {
                账户名 = "000001",
                主题 = Gender.天蓝色,
                读者号 = "2019305232130",

            };

            LV_loadedPage.ItemsSource = loaded;

            // 数据库开始
            NovelDAL novelDAL = new NovelDAL();
            foreach (var obj in novelDAL.getDownloadedNovels())
            {
                Fiction fiction = new Fiction()
                {
                    Id = obj[0].ToString(),
                    Name = obj[1].ToString(),
                    Author = obj[2].ToString(),
                    Url = obj[3].ToString()
                };
                loaded.Add(fiction);
            }
            // 数据库结束

        }

        #region 控件函数定义
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void textBoxId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        private void Boksf_lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LV_DwnPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void LV_loadedPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void content_key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Button_Click(sender, e);
            }
        }
        #endregion
        #region 首页
        #region 搜索按钮
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnlineSearchAndRead.Form1 form = new OnlineSearchAndRead.Form1();
            String kw = keySearch.Text;
            form.querytext = kw;
            List<fiction_info> _fic_info;
            // 不使用List类型，可实现自动刷新而不必切换源

            this.Lv_HomePage.ItemsSource = _ltfi_Search;//数据源
            ShowProgress = Visibility.Visible;
            //textstat(sender, "加载中");
            new Thread(() =>
            {

                _fic_info = form.Search_Result();
                if (_fic_info != null)
                {
                    Dispatcher.Invoke(delegate ()
                    {
                        _ltfi_Search.Clear();
                        searchPanel.Visibility = Visibility.Collapsed;
                        searchPanel2.Visibility = Visibility.Collapsed;
                        DockPanel.SetDock(searchPanel, Dock.Top);
                        searchPanel.Visibility = Visibility.Visible;
                        searchPanel2.Visibility = Visibility.Visible;
                    });
                    //Thread.Sleep(2000);
                    //Dispatcher.Invoke(delegate ()
                    //{

                    //});

                    for (int i = 0; i < _fic_info.Count; i++)
                    {
                        var fiction_i = new Fiction()
                        {
                            Id = _fic_info[i].col_fiction_id,
                            Name = _fic_info[i].col_fiction_name,
                            Author = _fic_info[i].col_fiction_author,
                            Url = _fic_info[i].col_url_homepage,
                        };
                        //MessageBox.Show(_fic_info[i].col_url_homepage);
                        //Thread.Sleep(200);

                        Dispatcher.Invoke(delegate ()
                        {
                            _ltfi_Search.Add(fiction_i);
                        });
                    }
                    ShowProgress = Visibility.Collapsed;

                    //加载一个空的小说详情页网站，提高再次访问时速度
                    HtmlWeb _web_Main = new HtmlWeb();
                    _web_Main.OverrideEncoding = Encoding.GetEncoding("gb2312");
                    HtmlAgilityPack.HtmlDocument _doc_Main = new HtmlAgilityPack.HtmlDocument();
                    //_doc_Main = _web_Main.Load("https://www.biquzhh.com/13_13134/");

                    //textstat(sender, "就绪");
                }
                else
                {
                    ShowProgress = Visibility.Collapsed;
                    //textstat(sender, "无结果！");
                }


            }).Start();
        }
        //public void textstat(object sender,string stat)
        //{
        //    var fe = (FrameworkElement)sender;
        //    BindingOperations.ClearBinding(search_stat, TextBlock.TextProperty);
        //    var myDataObject = new MyData(stat);
        //    var myBinding = new Binding("MyDataProperty") { Source = myDataObject };
        //    search_stat.SetBinding(TextBlock.TextProperty, myBinding);
        //}
        #endregion
        #region 双击详情页
        private void SListView_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Lv_HomePage.SelectedItem is Fiction emp) toinfopage(emp);
        }

        //打开新窗口
        private void toinfopage(Fiction emp)
        {
            //Fiction emp = (sender as ListViewItem).Content as Fiction;
            get_homepage_content content = new get_homepage_content();
            //MessageBox.Show(emp.col_fiction_url);

            ShowProgress = Visibility.Visible;
            new Thread(() =>
            {
                // 本地调试开始
                var localServerResult = Settings.ReadSetting("tempChaptersWeb", out string localServer);
                if (!localServerResult)
                {
                    Settings.AddUpdateAppSettings("tempChaptersWeb", "https://static.avosapps.us/chapters.htm");
                    Settings.ReadSetting("tempChaptersWeb", out localServer);
                }
                if (localServer != "")
                {
                    //////////////////////////////emp.Url = localServer;
                }
                // 本地调试结束

                Tuple<fiction_info, List<chapter_list>> result = content.TupleDetail(emp.Url);
                List<chapter_list> lis = result.Item2;
                //MessageBox.Show(lis[1].col_chapter_content);
                fiction_info li = result.Item1;
                //MessageBox.Show(li.col_fiction_introduction);
                ShowProgress = Visibility.Collapsed;
                Dispatcher.Invoke(delegate ()
                {
                    Window1 login1 = new Window1(lis, li);   //Login为窗口名，把要跳转的新窗口实例化
                    login1.Show();
                });
            }).Start();








        }
        #endregion
        #region 排序代码
        //单击表头排序
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void Sort_Click(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }
        private void Sort(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView =
              CollectionViewSource.GetDefaultView(Lv_HomePage.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
        #endregion
        #endregion
        #region 右键功能 查看详情，添加/移除/删除书架，添加/删除下载
        #region 首页右键，查看详情，添加书架，添加下载
        public void InfoPage(object sender, RoutedEventArgs e)
        {
            object sen = this.Lv_HomePage.SelectedItems[0];
            Fiction emp = sen as Fiction;
            toinfopage(emp);
        }
        public void BookShelf(object sender, RoutedEventArgs e)
        {
            object sen = this.Lv_HomePage.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Add_Bksf(sender, e, emp);
        }
        public void DownLoadBook(object sender, RoutedEventArgs e)
        {

            object sen = this.Lv_HomePage.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Down_Load(sender, e, emp);
        }
        #endregion
        #region 书架页右键 查看详情，移除书架，添加下载
        public void Bok_InfoPage(object sender, RoutedEventArgs e)
        {
            object sen = this.Boksf_lv.SelectedItems[0];
            Fiction emp = sen as Fiction;
            toinfopage(emp);
        }
        public void RemoveBk(object sender, RoutedEventArgs e)
        {
            object sen = this.Boksf_lv.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Remove_Bksf(sender, e, emp);
        }
        public void Bok_DownLoadBook(object sender, RoutedEventArgs e)
        {

            object sen = this.Boksf_lv.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Down_Load(sender, e, emp);
        }

        #endregion
        #region 下载页右键 查看详情，添加书架，删除本书
        public void Dwn_InfoPage(object sender, RoutedEventArgs e)
        {
            object sen = this.LV_loadedPage.SelectedItems[0];
            Fiction emp = sen as Fiction;
            toinfopage(emp);
        }
        public void Del_bk(object sender, RoutedEventArgs e)
        {
            object sen = this.LV_loadedPage.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Del_Loaded(sender, e, emp);
        }
        public void Dwn_BookShelf(object sender, RoutedEventArgs e)
        {
            object sen = this.LV_loadedPage.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Add_Bksf(sender, e, emp);
        }
        #endregion
        ObservableCollection<CardModel> boksf = new ObservableCollection<CardModel>();//书架页
        ObservableCollection<Fiction> progress = new ObservableCollection<Fiction>();//正下载
        ObservableCollection<Fiction> loaded = new ObservableCollection<Fiction>();//已下载
        ObservableCollection<Fiction> _ltfi_Search = new ObservableCollection<Fiction>();//搜索页
        private void Remove_Bksf(object sender, RoutedEventArgs e, Fiction fic)
        {
            //Boksf_lv.ItemsSource = boksf;
            //new Thread(() =>//前端添加下载项，无限制
            //{
            //    Dispatcher.Invoke(delegate ()
            //    {
            //        boksf.Remove(fic);
            //    });

            //}).Start();
        }
        private void Add_Bksf(object sender, RoutedEventArgs e, Fiction fic)
        {
            get_homepage_content content = new get_homepage_content();
            Tuple<fiction_info, List<chapter_list>> result = content.TupleDetail(fic.Url);
            string urlposter = result.Item1.col_url_poster;
            BitmapImage img = new BitmapImage(new Uri(urlposter));
            CardModel bok = new CardModel() { Cover = img, Fiction = fic.Name, Writer = fic.Author };
            
            Boksf_lb.ItemsSource = boksf;
            new Thread(() =>//前端添加下载项，无限制
            {
                Dispatcher.Invoke(delegate ()
                {
                    boksf.Add(bok);
                });

            }).Start();
        }
        private void Del_Loaded(object sender, RoutedEventArgs e, Fiction fic)
        {
            LV_loadedPage.ItemsSource = loaded;
            new Thread(() =>//前端添加下载项，无限制
            {
                
                Dispatcher.Invoke(delegate ()
                {
                    loaded.Remove(fic);
                });

            }).Start();
        }
        #endregion 
        #region 下载页：正在下载中每项是进度条，可暂停可删除，下载完成放入已完成队列。
        //已完成中每项是可删除
        Novel_Spider.Form1 form = new Novel_Spider.Form1();
        bool is_prepared = false;
        private void Down_Load(object sender, RoutedEventArgs e, Fiction fic)
        {

            LV_DwnPage.ItemsSource = progress;//绑定数据源
            form.url = fic.Url;
            form.novel_name = fic.Name;
            new Thread(() =>//后端添加到下载队列
            {
                form.button3_Click(sender, e);//添加按钮，添加到队列开始下载
                is_prepared = true;
            }).Start();
            new Thread(() =>//前端添加下载项，无限制
            {
                Fiction fiction = new Fiction(fic.Name, form.barvalue, fic.Author, fic.Url);
                if (progress != null)
                {

                    fiction.Barvalue = 0;
                }
                Dispatcher.Invoke(delegate ()
                {
                    progress.Add(fiction);

                });

            }).Start();
        }
        private void Dwn_start_Click(object sender, RoutedEventArgs e)
        {

            new Thread(() =>//后端开始下载--当且仅当当前小说是第一个时
            {
                if (is_prepared == true)
                {
                    is_prepared = false;
                    Dispatcher.Invoke(delegate ()
                    {
                        Dwn_start.IsEnabled = false;
                        Dwn_stop.IsEnabled = true;
                    });
                    form.button1_Click(sender, e);
                }



            }).Start();

            new Thread(() =>//前端下载进度显示，显示第一条
            {
                //MessageBox.Show($"{form.barvalue}");

                while (form.barvalue < 100)
                {
                    //MessageBox.Show($"{form.barvalue}");
                    progress[0].Barvalue = form.barvalue;
                    Thread.Sleep(100);
                }
                Dispatcher.Invoke(delegate ()
                {
                    //MessageBox.Show($"{form.barvalue}");
                    loaded.Add(progress[0]);
                    //Thread.Sleep(2000);
                    progress.Remove(progress[0]);
                    /// 放置数据库代码progress[0]，其中的四个属性

                });
            }).Start();
        }
        private void Dwn_stop_Click(object sender, RoutedEventArgs e)
        {
            Dwn_stop.IsEnabled = false;
            form.button2_Click(sender, e);
            Dwn_start.IsEnabled = true;
        }

        private void ItemClick(object sender, MouseButtonEventArgs e)
        {
            //Dwn_start.Visibility = Visibility.Visible;
            //Dwn_stop.Visibility = Visibility.Visible;
        }
        #endregion
        #region 设置页 设置存放路径

        #endregion




        // 2021.6.12 新增
        /// <summary>
        /// 切换主题
        /// </summary>
        /// <param name="skin"></param>
        public void UpdateSkin(SkinType skin)
        {
            SharedResourceDictionary.SharedDictionaries.Clear();
            Resources.MergedDictionaries.Add(ResourceHelper.GetSkin(skin));
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });
            this?.OnApplyTemplate();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var theme = (sender as RadioButton)?.Content.ToString();
            Growl.InfoGlobal(theme);
            switch (theme)
            {
                case "天蓝主题":
                    UpdateSkin(SkinType.Default);
                    break;
                case "紫色主题":
                    UpdateSkin(SkinType.Violet);
                    break;
            }
        }
        #region  设置模式
        public class PropertyGridModel
        {
            [Category("Category1")]
            public string 账户名 { get; set; }

            [Category("Category1")]
            public Gender 主题 { get; set; }

            [Category("Category2")]
            public int 数字 { get; set; }

            [Category("Category2")]
            public string 读者号 { get; set; }

            public ImageSource DownLoadPath { get; set; }
        }
        public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
            "DemoModel", typeof(PropertyGridModel), typeof(MainWindow), new PropertyMetadata(default(PropertyGridModel)));
        public PropertyGridModel DemoModel
        {
            get => (PropertyGridModel)GetValue(DemoModelProperty);
            set => SetValue(DemoModelProperty, value);
        }

        #endregion


    }




    #region 数据源

    public class Fiction : INotifyPropertyChanged
    {
        private string fic_name;
        private double barvalue;
        private string fic_author;
        private string fic_url;
        private string id;
        public Fiction()
        {
        }
        public string Author
        {
            get { return fic_author; }
            set
            {
                fic_author = value;
                OnPropertyChanged(Author);
            }
        }
        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(Id);
            }
        }
        public string Url
        {
            get { return fic_url; }
            set
            {
                fic_url = value;
                OnPropertyChanged(Url);
            }
        }
        public Fiction(string na, double va, string author, string url)
        {
            fic_name = na;
            barvalue = va;
            fic_author = author;
            fic_url = url;
        }
        public string Name
        {
            get { return fic_name; }
            set
            {
                fic_name = value;
                OnPropertyChanged(Name);
            }
        }
        public double Barvalue
        {
            get { return barvalue; }
            set
            {
                barvalue = value;
                OnPropertyChanged(nameof(Barvalue));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }

    public enum Gender
    {
        天蓝色,
        浅紫色
    }
    public class CardModel
    {
        private string fiction;

        private BitmapImage cover;
        private string writer;
        public string Writer
        {
            get { return writer; }
            set
            {
                writer = value;
                OnPropertyChanged(Writer);
            }
        }
        
        public BitmapImage Cover
        {
            get { return   cover; }
            set
            {
                cover = value;
                CoverPropertyChanged(nameof(Cover));
            }
        }
        public string Fiction
        {
            get { return fiction; }
            set
            {
                fiction = value;
                OnPropertyChanged(Fiction);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        private void CoverPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }

   
    #endregion

}
