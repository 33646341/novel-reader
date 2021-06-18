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
using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using System.Windows.Media.Animation;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using HandyControl.Controls;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using ReadTool;

namespace UIdesign
{
    /// <summary>
    /// ReadingOnline.xaml 的交互逻辑
    /// </summary>
    public partial class ReadingOnline : System.Windows.Window, INotifyPropertyChanged
    {
        #region 加载圈相关
        public event PropertyChangedEventHandler PropertyChanged;
        private int dwnnum = 0;
        public int Dwnum
        {
            get { return dwnnum; }
            set
            {
                dwnnum = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(dwnnum)));
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
        public ReadingOnline()
        {
            InitializeComponent();
            keySearch.Focus();

            #region
            DataContext = this;
            #endregion
            DemoModel = new PropertyGridModel
            {
                账户名 = "000001",
                读者号 = "2019305232130",
                昵称="探险家",

            };

            LV_loadedPage.ItemsSource = loaded;
            Boksf_lb.ItemsSource = boksf;
            this.Lv_HomePage.ItemsSource = _ltfi_Search;//数据源

            // 数据库开始

            // 读取已下载

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

            // 读取已收藏

            foreach (var obj in novelDAL.getStarredNovels())
            {
                var imageURL = obj["imageURL"].ToString();
                if (imageURL == "")
                {
                    imageURL = @"..\..\img\emp.jpg";
                }

                BitmapImage bi = new BitmapImage();
                // BitmapImage.UriSource must be in a BeginInit/EndInit block.
                bi.BeginInit();
                bi.UriSource = new Uri(imageURL, UriKind.RelativeOrAbsolute);
                bi.EndInit();

                Fiction bok = new Fiction()
                {
                    Cover = bi,
                    Name = obj[1].ToString(),
                    Author = obj[2].ToString(),
                    Url = obj[3].ToString()
                };
                boksf.Add(bok);
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
            //this.Lv_HomePage.ItemsSource = _ltfi_Search;//数据源

            ShowProgress = Visibility.Visible;

            Dispatcher.Invoke(delegate ()
            {
                var color1 = (Color)ColorConverter.ConvertFromString("#3d6633");
                state.Foreground = new SolidColorBrush(color1);
                state.Content = $"Loading...";
            });

            new Thread(() =>
            {

                _fic_info = form.Search_Result();

                if (_fic_info != null)
                {
                    _fic_info = _fic_info.Take(10).ToList();

                    Dispatcher.Invoke(delegate ()
                    {
                        _ltfi_Search.Clear();

                        // 2021.6.16 转移

                        //searchPanel.Visibility = Visibility.Collapsed;
                        //searchPanel2.Visibility = Visibility.Collapsed;
                        //DockPanel.SetDock(searchPanel, Dock.Top);
                        //searchPanel.Visibility = Visibility.Visible;
                        //searchPanel2.Visibility = Visibility.Visible;
                    });
                    //Thread.Sleep(2000);
                    //Dispatcher.Invoke(delegate ()
                    //{

                    //});
                    fictionResultCache.Clear();
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
                            //_ltfi_Search.Add(fiction_i);
                        });

                        // 预加载开始
                        new Thread(() =>
                        {
                            get_homepage_content content = new get_homepage_content();
                            Tuple<fiction_info, List<chapter_list>> result;
                            // 先查快表
                            if (!fictionResultCache.Keys.Contains(fiction_i))
                            {
                                result = content.TupleDetail(fiction_i.Url);
                                int liveTimes = 5;
                                while (liveTimes > 0 && result == null)
                                {
                                    liveTimes -= 1;
                                    Console.WriteLine("liveTimes = " + liveTimes);
                                    Thread.Sleep(5000);
                                    Console.WriteLine("王继承的函数报空值异常，但是他又没去做处理，可能导致错误结果，点击确认尝试再次加载（可以略等一会，防止服务器繁忙）");
                                    //HandyControl.Controls.MessageBox.Info("王继承的函数报空值异常，但是他又没去做处理，可能导致错误结果，点击确认尝试再次加载（可以略等一会，防止服务器繁忙）");
                                    result = content.TupleDetail(fiction_i.Url);
                                }
                                fictionResultCache.TryAdd(fiction_i, result); // 加入快表
                                Console.WriteLine($"{fiction_i.Id} Done!");
                                Dispatcher.Invoke(delegate ()
                                {
                                    _ltfi_Search.Add(fiction_i);

                                    // 等到预加载成功的小说超过五个时，才展示出来
                                    if (_fic_info.Count > 0 && _ltfi_Search.Count == Math.Min(5, _fic_info.Count))
                                    {

                                        if (DockPanel.GetDock(searchPanel) != Dock.Top)
                                        {
                                            logo.Visibility = Visibility.Collapsed;
                                            searchPanel.Visibility = Visibility.Collapsed;
                                            searchPanel2.Visibility = Visibility.Collapsed;
                                            DockPanel.SetDock(searchPanel, Dock.Top);
                                            searchPanel.Visibility = Visibility.Visible;
                                            searchPanel2.Visibility = Visibility.Visible;
                                        }


                                        ShowProgress = Visibility.Collapsed;
                                    }


                                    if (fictionResultCache.Count < _fic_info.Count)
                                    {
                                        state.Content = $"{(fictionResultCache.Count * 100 / _fic_info.Count)}%";
                                    }
                                    else
                                    {
                                        var color1 = (Color)ColorConverter.ConvertFromString("#90c981");
                                        state.Foreground = new SolidColorBrush(color1);
                                        state.Content = $"完毕";
                                        ShowProgress = Visibility.Collapsed;

                                    }


                                });
                            }
                        }).Start();
                        // 预加载结束


                    }

                    //加载一个空的小说详情页网站，提高再次访问时速度
                    //HtmlWeb _web_Main = new HtmlWeb();
                    //_web_Main.OverrideEncoding = Encoding.GetEncoding("gb2312");
                    //HtmlAgilityPack.HtmlDocument _doc_Main = new HtmlAgilityPack.HtmlDocument();
                    //_doc_Main = _web_Main.Load("https://www.biquzhh.com/13_13134/");

                    //textstat(sender, "就绪");
                }
                else
                {
                    Dispatcher.Invoke(delegate ()
                    {
                        state.Foreground = new SolidColorBrush(Colors.DarkRed);
                        state.Content = $"找不到";

                    });
                    ShowProgress = Visibility.Collapsed;
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
        System.Collections.Concurrent.ConcurrentDictionary<Fiction, Tuple<fiction_info, List<chapter_list>>> fictionResultCache = new System.Collections.Concurrent.ConcurrentDictionary<Fiction, Tuple<fiction_info, List<chapter_list>>>();
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


                Tuple<fiction_info, List<chapter_list>> result;

                if (fictionResultCache.Keys.Contains(emp))
                {
                    result = fictionResultCache[emp];
                }
                else if (!_ltfi_Search.Contains(emp))
                {
                    // 如果不是从搜索页发出的请求，那就直接查
                    result = content.TupleDetail(emp.Url);
                    try
                    {
                        fictionResultCache.TryAdd(emp, result); // 加入快表

                    }
                    catch (Exception e)
                    {
                        HandyControl.Controls.MessageBox.Show(e.Message);
                    }

                }
                else
                {
                    //思路二，等待快表加载完，直到快表有才结束，不贸然加载（也没必要）
                    while (!fictionResultCache.Keys.Contains(emp))
                    {
                        Thread.Sleep(100);
                    }
                    result = fictionResultCache[emp];
                }





                /*
                // 先查快表
                if (fictionResultCache.Keys.Contains(emp))
                {
                    result = fictionResultCache[emp];
                }
                else
                {
                    result = content.TupleDetail(emp.Url);
                    try
                    {
                        fictionResultCache.Add(emp, result); // 加入快表

                    }catch(Exception e)
                    {
                        HandyControl.Controls.MessageBox.Show(e.Message);
                    }
                }
                // 查快表结束
                */

                if (result == null)
                {
                    HandyControl.Controls.MessageBox.Info("您的请求过于频繁，请稍候再试。");
                    return;
                }

                List<chapter_list> lis = result.Item2;
                //MessageBox.Show(lis[1].col_chapter_content);
                fiction_info li = result.Item1;
                //MessageBox.Show(li.col_fiction_introduction);
                ShowProgress = Visibility.Collapsed;
                Dispatcher.Invoke(delegate ()
                {
                    This_chapter_list provisionallist = new This_chapter_list();
                    Window1 login1 = new Window1(lis, li, emp, false, provisionallist);  //Login为窗口名，把要跳转的新窗口实例化

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
            object sen = this.Boksf_lb.SelectedItems[0];
            Fiction emp = sen as Fiction;
            toinfopage(emp);
        }
        public void RemoveBk(object sender, RoutedEventArgs e)
        {
            object sen = this.Boksf_lb.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Remove_Bksf(sender, e, emp);
        }
        public void Bok_DownLoadBook(object sender, RoutedEventArgs e)
        {

            object sen = this.Boksf_lb.SelectedItems[0];
            Fiction emp = sen as Fiction;
            Down_Load(sender, e, emp);
        }

        #endregion
        #region 下载页右键 查看详情，添加书架，删除本书
        public void Dwn_InfoPage(object sender, RoutedEventArgs e)
        {
            object sen = this.LV_loadedPage.SelectedItems[0];
            Fiction emp = sen as Fiction;
            this_chapter_list t1 = new this_chapter_list();
            This_chapter_list l1 = new This_chapter_list();
            l1 = t1.Get_chapter(@".\Novel\" + emp.Name);
            
            Window1 login1 = new Window1(null,null, emp, true, l1);  //Login为窗口名，把要跳转的新窗口实例化
            login1.Show();
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
        ObservableCollection<Fiction> boksf = new ObservableCollection<Fiction>();//书架页
        ObservableCollection<Fiction> progress = new ObservableCollection<Fiction>();//正下载
        ObservableCollection<Fiction> loaded = new ObservableCollection<Fiction>();//已下载
        ObservableCollection<Fiction> _ltfi_Search = new ObservableCollection<Fiction>();//搜索页
        private void Remove_Bksf(object sender, RoutedEventArgs e, Fiction fic)
        {
            Boksf_lb.ItemsSource = boksf;
            new Thread(() =>//前端删除下载项，无限制
            {
                Dispatcher.Invoke(delegate ()
                {
                    boksf.Remove(fic);
                });
            }).Start();

            // 数据库开始
            var novelDAL = new NovelManager.NovelDAL();
            if (novelDAL.exsitsNovel(fic.Name) == 0)
            {
                novelDAL.addNovel(fic.Name, fic.Url);
            }
            var Novel_id = novelDAL.exsitsNovel(fic.Name);
            Console.WriteLine(Novel_id);
            novelDAL.updateNovel(Novel_id, "starred", "0");
            // 数据库结束
        }
        public void Add_Bksf(object sender, RoutedEventArgs e, Fiction fic)
        {
            BitmapImage bi = new BitmapImage();
            // BitmapImage.UriSource must be in a BeginInit/EndInit block.
            bi.BeginInit();
            bi.UriSource = new Uri(@"..\..\img\emp.jpg", UriKind.RelativeOrAbsolute);
            bi.EndInit();
            boksf.Add(fic);

            // 数据库开始
            var novelDAL = new NovelManager.NovelDAL();
            if (novelDAL.exsitsNovel(fic.Name) == 0)
            {
                novelDAL.addNovel(fic.Name, fic.Url);
            }
            var Novel_id = novelDAL.exsitsNovel(fic.Name);
            Console.WriteLine(Novel_id);
            novelDAL.updateNovel(Novel_id, "starred", "1");
            novelDAL.updateNovel(Novel_id, "author", fic.Author);
            // 数据库结束
            new Thread(() =>
            {
                WebClient webClient = new WebClient();
                var html = webClient.DownloadString(fic.Url.Replace("www", "m"));
                var matchResult = Regex.Match(html, @"og:image"" content=""(.*)""/>");
                var imageUrl = "";
                if (matchResult.Success)
                {
                    imageUrl = matchResult.Groups[1].Value;
                    //HandyControl.Controls.MessageBox.Show(imageUrl);

                    novelDAL.updateNovel(Novel_id, "imageURL", imageUrl); // 写入数据库

                    Dispatcher.Invoke(delegate ()
    {
        BitmapImage img = new BitmapImage(new Uri(imageUrl));
        fic.Cover = img;
    });
                }
            }).Start();
        }
        private void Del_Loaded(object sender, RoutedEventArgs e, Fiction fic)
        {
            LV_loadedPage.ItemsSource = loaded;
            new Thread(() =>//前端删除下载项，无限制
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
        Novel_Spider.Spider dwn = new Novel_Spider.Spider();
        bool is_prepared = false;
        
        private Visibility dwnProgress = Visibility.Collapsed;
        public Visibility DwnProgress
        {
            get { return dwnProgress; }
            set
            {
                dwnProgress = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(DwnProgress)));
            }
        }

        public void Down_Load(object sender, RoutedEventArgs e, Fiction fic)
        {

            LV_DwnPage.ItemsSource = progress;//绑定数据源
            dwn.novel_name = fic.Name;
            if (!dwn.down_or_not())
            {//未成功时进入while循环 ，成功时退出循环
             //添加中

                search_stat.Text = "";
                DwnProgress = Visibility.Visible;


                new Thread(() =>//显示添加下载状态
            {
                //Dispatcher.Invoke(delegate ()
                //{
                //    search_stat.Text = "添加中...";
                //});
                //System.Windows.MessageBox.Show("添加中！");
                dwn.download_add(fic.Url, "C://Users//ASW//Desktop//down");
                //System.Windows.MessageBox.Show("添加成功！");
                //Dispatcher.Invoke(delegate ()
                //{
                //    search_stat.Text = "添加成功";
                //});
                //添加进度条
            }).Start();
                if (dwn.down_or_not())
                {
                    DwnProgress = Visibility.Collapsed;
                    Thread.Sleep(1000);

                    //
                    Dwnum++;
                    Thread.Sleep(1000);
                    search_stat.Text = "";
                    is_prepared = true;
                };
                Fiction fiction = new Fiction(fic.Name, dwn.barvalue, fic.Author, fic.Url);
                if (progress != null)
                {
                    fiction.Barvalue = 0;
                }
                progress.Add(fiction);
            }

        }

        private void Dwn_start_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() =>
            {
                if (is_prepared == true)
                {
                    is_prepared = false;

                    Dispatcher.Invoke(delegate ()
                    {
                    Dwn_start.IsEnabled = false;
                    Dwn_stop.IsEnabled = true;
                    });
                    dwn.download_novel();
                }
            }).Start();

            new Thread(() =>//前端下载进度显示，显示第一条
            {
                while (dwn.tag && progress.Count > 0)
                {
                    if (dwn.barvalue <= 100)
                    {
                        progress[0].Barvalue = dwn.barvalue;
                        //Thread.Sleep(100);
                    }
                    if (progress[0].Barvalue >= 100)
                    {
                        progress[0].Barvalue = 0;
                        //System.Windows.Forms.MessageBox.Show("睡眠态！");
                        Dispatcher.Invoke(delegate ()
                        {
                            //System.Windows.Forms.MessageBox.Show($"实际：{dwn.barvalue}");
                            //System.Windows.Forms.MessageBox.Show($"显示：{progress[0].Barvalue}");
                            loaded.Add(progress[0]);
                            progress.Remove(progress[0]);
                            // System.Windows.Forms.MessageBox.Show("删除表项第一个！");
                        });
                    }
                }


            }).Start();
        }
        private void Dwn_stop_Click(object sender, RoutedEventArgs e)
        {
            Dwn_stop.IsEnabled = false;
            dwn.download_pause();
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
        #region 切换主题
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
        #endregion

        #region  设置模式
        public class PropertyGridModel
        {
            [Category("个人信息")]
            public string 账户名 { get; set; }
            public string 读者号 { get; set; }
            public string 昵称 { get; set; }


        }
        public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
            "DemoModel", typeof(PropertyGridModel), typeof(ReadingOnline), new PropertyMetadata(default(PropertyGridModel)));
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
        public string fic_name;
        public double barvalue;
        public string fic_author;
        private string fic_url;
        private string id;
        private BitmapImage cover;
        public BitmapImage Cover
        {
            get { return cover; }
            set
            {
                cover = value;
                CoverPropertyChanged(nameof(Cover));
            }
        }
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
        private void CoverPropertyChanged(string info)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }

    //public enum Gender
    //{
    //    天蓝色,
    //    浅紫色
    //}
    #endregion
}
