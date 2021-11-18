using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Themes;
using HandyControl.Tools;
using Newtonsoft.Json;
using NovelManager;
using OnlineSearchAndRead;
using ReadTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UIdesign.Entity;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;

namespace UIdesign
{
    /// <summary>
    /// ReadingOnline.xaml 的交互逻辑
    /// </summary>
    public partial class ReadingOnline : System.Windows.Window, INotifyPropertyChanged
    {
        public ReadingOnline()
        {
            InitializeComponent();

            // 数据源绑定
            DataContext = this;

            // 搜索框聚焦
            keySearch_TextBox.Focus();

            // 设置面板 值初始化
            SettingModel = new PropertyGridModel
            {
                账户名 = "000001",
                读者号 = "2019305232130",
                昵称 = "探险家",
                主题 = Gender.天蓝色,
            };

            // 绑定数据源
            downloaded_DataGrid.ItemsSource = loaded;
            bookshelf_ListBox.ItemsSource = boksf;
            //searchResult_DataGrid.ItemsSource = searchResultBooks;

            #region 数据库开始

            // 读取已下载的小说列表
            NovelDAL novelDAL = new NovelDAL();
            foreach (var obj in novelDAL.getDownloadedNovels())
            {
                Book fiction = new Book()
                {
                    id = obj[0].ToString(),
                    bookName = obj[1].ToString(),
                    authorName = obj[2].ToString()
                    //Url = obj[3].ToString()
                };
                loaded.Add(fiction);
            }

            // 读取已收藏的小说列表
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

                MarkedBook bok = new MarkedBook()
                {
                    CoverImage = bi,
                    bookName = obj[1].ToString(),
                    authorName = obj[2].ToString(),
                    id = obj[3].ToString()
                };
                boksf.Add(bok);
            }

            #endregion 数据库结束

            dwnPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            dwnpath.Text = dwnPath;
        }

        #region 回车触发搜索
        private void content_key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search_Btn_Click(sender, e);
            }
        }
        #endregion
        #region 首页：搜索按钮，排序，双击

        private System.Collections.Concurrent.ConcurrentDictionary<Book, Tuple<fiction_info, List<chapter_list>>> bookResultCacheCollection = new System.Collections.Concurrent.ConcurrentDictionary<Book, Tuple<fiction_info, List<chapter_list>>>();

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Btn_Click(object sender, RoutedEventArgs e)
        {
            // 界面上转换为加载中
            ShowProgress = Visibility.Visible;
            Dispatcher.Invoke(delegate ()
            {
                var color1 = (Color)ColorConverter.ConvertFromString("#3d6633");
                state.Foreground = new SolidColorBrush(color1);
                state.Content = $"Loading...";
            });

            // 开一个搜索线程
            new Thread(() =>
            {
                WebClient webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                string searchResultData = webClient.DownloadString($"http://47.106.243.172:8888/book/searchByPage?curr=1&limit=20&keyword={KeyWord}");
                Root rt = JsonConvert.DeserializeObject<Root>(searchResultData);
                ObservableCollection<Book> books = new ObservableCollection<Entity.Book>(rt.data.list);//.Take(10).ToList()
                //HandyControl.Controls.MessageBox.Info(rt.data.list[0].bookName);

                //List<fiction_info> _fic_info;

                if (books != null)
                {
                    Dispatcher.Invoke(() =>
                    {
                        SearchResultBooks.Clear();
                        SearchResultBooks = books;
                    });
                    bookResultCacheCollection.Clear();
                    #region
                    /*
                     
                    for (int i = 0; i < books.Count; i++)
                    {
                        // 预加载开始
                        new Thread(() =>
                        {
                            get_homepage_content content = new get_homepage_content();
                            Tuple<fiction_info, List<chapter_list>> result;
                            // 先查快表
                            if (!bookResultCacheCollection.Keys.Contains(fiction_i))
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
                                bookResultCacheCollection.TryAdd(fiction_i, result); // 加入快表
                                Console.WriteLine($"{fiction_i.Id} Done!");
                                Dispatcher.Invoke(delegate ()
                                {
                                    searchResultCollection.Add(fiction_i);

                                    // 等到预加载成功的小说超过五个时，才展示出来
                                    if (_fic_info.Count > 0 && searchResultCollection.Count == Math.Min(5, _fic_info.Count))
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


                                    if (bookResultCacheCollection.Count < _fic_info.Count)
                                    {
                                        state.Content = $"{(bookResultCacheCollection.Count * 100 / _fic_info.Count)}%";
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
                    */
                    #endregion



                    Dispatcher.Invoke(delegate ()
                    {
                        // 等到预加载成功的小说超过五个时，才展示出来
                        if (true || (books.Count > 0 && SearchResultBooks.Count == Math.Min(5, books.Count)))
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

                        if (bookResultCacheCollection.Count < books.Count)
                        {
                            state.Content = $"{(bookResultCacheCollection.Count * 100 / books.Count)}%";
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

        #region 双击详情页
        private void SListView_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //打开新窗口
            if (searchResult_DataGrid.SelectedItem is Book emp) toinfopage(emp);
        }
        #endregion

        #region 排序代码
        //单击表头排序
        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        private void Sort_Click(object sender, RoutedEventArgs e)
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
              CollectionViewSource.GetDefaultView(searchResult_DataGrid.ItemsSource);
            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }
        #endregion
        #endregion
        #region 右键功能的定义函数 查看详情，添加/移除书架，添加/删除下载
        #region 首页右键，查看详情，添加书架，添加下载
        public void InfoPage(object sender, RoutedEventArgs e)
        {
            object sen = this.searchResult_DataGrid.SelectedItems[0];
            Book emp = sen as Book;
            toinfopage(emp);
        }
        public void BookShelf(object sender, RoutedEventArgs e)
        {
            object sen = this.searchResult_DataGrid.SelectedItems[0];
            Book emp = sen as Book;
            Add_Bksf((MarkedBook)emp);
        }
        public void DownLoadBook(object sender, RoutedEventArgs e)
        {
            object sen = this.searchResult_DataGrid.SelectedItems[0];
            Book emp = sen as Book;
            Down_Load(sender, e, emp);
        }
        #endregion
        #region 书架页右键 查看详情，移除书架，添加下载
        public void Bok_InfoPage(object sender, RoutedEventArgs e)
        {
            object sen = this.bookshelf_ListBox.SelectedItems[0];
            Book emp = sen as Book;
            toinfopage(emp);
        }
        public void RemoveBk(object sender, RoutedEventArgs e)
        {
            object sen = this.bookshelf_ListBox.SelectedItems[0];
            Book emp = sen as Book;
            Remove_Bksf(sender, e, emp);
        }
        public void Bok_DownLoadBook(object sender, RoutedEventArgs e)
        {

            object sen = this.bookshelf_ListBox.SelectedItems[0];
            Book emp = sen as Book;
            Down_Load(sender, e, emp);
        }

        #endregion
        #region 下载页右键 查看详情，添加书架，删除本书
        //////////// 下载
        public void Dwn_InfoPage(object sender, RoutedEventArgs e)
        {
            //    object sen = this.downloaded_DataGrid.SelectedItems[0];
            //    Book emp = sen as Book;
            //    this_chapter_list t1 = new this_chapter_list();
            //    This_chapter_list l1 = new This_chapter_list();
            //    l1 = t1.Get_chapter(dwnPath + "\\Novel\\" + emp.Name);

            //    Window1 login1 = new Window1(null, null, emp, true, l1);  //Login为窗口名，把要跳转的新窗口实例化
            //    login1.Show();
        }
        public void Del_bk(object sender, RoutedEventArgs e)
        {
            object sen = this.downloaded_DataGrid.SelectedItems[0];
            Book emp = sen as Book;
            Del_Loaded(sender, e, emp);
        }
        public void Dwn_BookShelf(object sender, RoutedEventArgs e)
        {
            object sen = this.downloaded_DataGrid.SelectedItems[0];
            Book emp = sen as Book;
            Add_Bksf((MarkedBook)emp);
        }
        #endregion
        #endregion
        #region 定义集合
        private ObservableCollection<Book> boksf = new ObservableCollection<Book>();//书架页
        private ObservableCollection<Book> progress = new ObservableCollection<Book>();//正下载
        private ObservableCollection<Book> loaded = new ObservableCollection<Book>();//已下载
        #endregion
        #region 功能函数

        /// <summary>
        /// //////移除书架
        /// </summary>
        /// <param name="fic"></param>
        private void Remove_Bksf(object sender, RoutedEventArgs e, Book fic)
        {
            //    bookshelf_ListBox.ItemsSource = boksf;
            //    new Thread(() =>//前端删除下载项，无限制
            //    {
            //        Dispatcher.Invoke(delegate ()
            //        {
            //            boksf.Remove(fic);
            //        });
            //    }).Start();

            //    // 数据库开始
            //    var novelDAL = new NovelManager.NovelDAL();
            //    if (novelDAL.exsitsNovel(fic.Name) == 0)
            //    {
            //        novelDAL.addNovel(fic.Name, fic.Url);
            //    }
            //    var Novel_id = novelDAL.exsitsNovel(fic.Name);
            //    Console.WriteLine(Novel_id);
            //    novelDAL.updateNovel(Novel_id, "starred", "0");
            //    // 数据库结束
        }
        public void Add_Bksf(MarkedBook fic)
        {
            //BitmapImage bi = new BitmapImage();
            //// BitmapImage.UriSource must be in a BeginInit/EndInit block.
            //bi.BeginInit();
            //bi.UriSource = new Uri(@"..\..\img\emp.jpg", UriKind.RelativeOrAbsolute);
            //bi.EndInit();
            //boksf.Add(fic);

            //// 数据库开始
            //var novelDAL = new NovelManager.NovelDAL();
            //if (novelDAL.exsitsNovel(fic.bookName) == 0)
            //{
            //    novelDAL.addNovel(fic.bookName, fic.Url);
            //}
            //var Novel_id = novelDAL.exsitsNovel(fic.Name);
            //Console.WriteLine(Novel_id);
            //novelDAL.updateNovel(Novel_id, "starred", "1");
            //novelDAL.updateNovel(Novel_id, "author", fic.authorName);
            //// 数据库结束
            //new Thread(() =>
            //{
            //    WebClient webClient = new WebClient();
            //    var html = webClient.DownloadString(fic.Url.Replace("www", "m"));
            //    var matchResult = Regex.Match(html, @"og:image"" content=""(.*)""/>");
            //    var imageUrl = "";
            //    if (matchResult.Success)
            //    {
            //        imageUrl = matchResult.Groups[1].Value;
            //        //HandyControl.Controls.MessageBox.Show(imageUrl);

            //        novelDAL.updateNovel(Novel_id, "imageURL", imageUrl); // 写入数据库

            //        Dispatcher.Invoke(delegate ()
            //        {
            //            BitmapImage img = new BitmapImage(new Uri(imageUrl));
            //            fic.c = img;
            //        });
            //    }
            //}).Start();
        }
        private void Del_Loaded(object sender, RoutedEventArgs e, Book fic)
        {
            downloaded_DataGrid.ItemsSource = loaded;
            new Thread(() =>//前端删除下载项，无限制
            {

                Dispatcher.Invoke(delegate ()
                {
                    loaded.Remove(fic);
                });

            }).Start();
        }

        /// <summary>
        /// //////转详情
        /// </summary>
        private void toinfopage(Book emp)
        {
            //    //Fiction emp = (sender as ListViewItem).Content as Fiction;

            //    get_homepage_content content = new get_homepage_content();
            //    //MessageBox.Show(emp.col_fiction_url);

            //    ShowProgress = Visibility.Visible;
            //    new Thread(() =>
            //    {
            //        // 本地调试开始
            //        var localServerResult = Settings.ReadSetting("tempChaptersWeb", out string localServer);
            //        if (!localServerResult)
            //        {
            //            Settings.AddUpdateAppSettings("tempChaptersWeb", "https://static.avosapps.us/chapters.htm");
            //            Settings.ReadSetting("tempChaptersWeb", out localServer);
            //        }
            //        if (localServer != "")
            //        {
            //            //////////////////////////////emp.Url = localServer;
            //        }
            //        // 本地调试结束


            //        Tuple<fiction_info, List<chapter_list>> result;

            //        if (bookResultCacheCollection.Keys.Contains(emp))
            //        {
            //            result = bookResultCacheCollection[emp];
            //        }
            //        else if (!searchResultBooks.Contains(emp))
            //        {
            //            // 如果不是从搜索页发出的请求，那就直接查
            //            result = content.TupleDetail(emp.Url);
            //            try
            //            {
            //                bookResultCacheCollection.TryAdd(emp, result); // 加入快表

            //            }
            //            catch (Exception e)
            //            {
            //                HandyControl.Controls.MessageBox.Show(e.Message);
            //            }

            //        }
            //        else
            //        {
            //            //思路二，等待快表加载完，直到快表有才结束，不贸然加载（也没必要）
            //            while (!bookResultCacheCollection.Keys.Contains(emp))
            //            {
            //                Thread.Sleep(100);
            //            }
            //            result = bookResultCacheCollection[emp];
            //        }





            //        /*
            //        // 先查快表
            //        if (fictionResultCache.Keys.Contains(emp))
            //        {
            //            result = fictionResultCache[emp];
            //        }
            //        else
            //        {
            //            result = content.TupleDetail(emp.Url);
            //            try
            //            {
            //                fictionResultCache.Add(emp, result); // 加入快表

            //            }catch(Exception e)
            //            {
            //                HandyControl.Controls.MessageBox.Show(e.Message);
            //            }
            //        }
            //        // 查快表结束
            //        */

            //        if (result == null)
            //        {
            //            HandyControl.Controls.MessageBox.Info("您的请求过于频繁，请稍候再试。");
            //            return;
            //        }

            //        List<chapter_list> lis = result.Item2;
            //        //MessageBox.Show(lis[1].col_chapter_content);
            //        fiction_info li = result.Item1;
            //        //MessageBox.Show(li.col_fiction_introduction);
            //        ShowProgress = Visibility.Collapsed;
            //        Dispatcher.Invoke(delegate ()
            //        {
            //            This_chapter_list provisionallist = new This_chapter_list();
            //            Window1 login1 = new Window1(lis, li, emp, false, provisionallist);  //Login为窗口名，把要跳转的新窗口实例化

            //            login1.Show();
            //        });
            //    }).Start();

        }
        #endregion
        #region 下载页：正在下载中每项是进度条，可暂停可删除，下载完成放入已完成队列。
        //已完成中每项是可删除
        #region 连接后台程序代码，进度条设置
        private Novel_Spider.Spider dwn = new Novel_Spider.Spider();
        private bool is_prepared = false;

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
        #endregion
        public void Down_Load(object sender, RoutedEventArgs e, Book fic)
        {
            LV_DwnPage.ItemsSource = progress;//绑定数据源
            dwn.novel_name = fic.bookName;
            if (!dwn.down_or_not())
            {
                search_stat.Text = "";
                DwnProgress = Visibility.Visible;
                string dpath = dwnPath;
                new Thread(() =>//开启线程调用后台程序，添加下载
            {
                dwn.download_add(fic.id, dpath);
            }).Start();
                new Thread(() =>//显示添加下载状态
                {
                    while (true)
                    {
                        if (dwn.down_or_not())
                        {
                            DwnProgress = Visibility.Collapsed;
                            Dwnum++;
                            this.Dispatcher.Invoke(
               new Action(
                    delegate
                    {
                        search_stat.Text = "";
                    }
               ));
                            is_prepared = true;
                            break;
                        };
                    }
                }
                ).Start();

                DownloadingBook downloadingBook = (DownloadingBook)fic;
                if (progress != null)
                {
                    downloadingBook.DownloadingProgress = 0;
                }
                progress.Add(downloadingBook);
            }

        }
        #region 开始下载按钮

        private void Dwn_start_Click(object sender, RoutedEventArgs e)
        {
            /*
    #region 后台开始下载
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
    #endregion
    new Thread(() =>//前端下载进度显示，显示第一条
    {
        while (dwn.tag && progress.Count > 0)
        {
            if (dwn.barvalue <= 100)
            {
                progress[0].Barvalue = dwn.barvalue;
                //Thread.Sleep(100);
            }
            if (progress[0].Barvalue >= 100 && progress.Count > 0)
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
            */
        }


        #endregion
        #region 停止下载按钮
        private void Dwn_stop_Click(object sender, RoutedEventArgs e)
        {
            Dwn_stop.IsEnabled = false;
            dwn.download_pause();
            is_prepared = true;
            Dwn_start.IsEnabled = true;
        }
        #endregion
        private void ItemClick(object sender, MouseButtonEventArgs e)
        {
            //Dwn_start.Visibility = Visibility.Visible;
            //Dwn_stop.Visibility = Visibility.Visible;
        }
        #endregion
        #region 设置页 设置存放路径
        private void 选择路径_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog { SelectedPath = dwnPath };
            var res = dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK;
            if (!res) return;
            dwnPath = dlg.SelectedPath;
            dwnpath.IsEnabled = true;
            dwnpath.Text = dwnPath;
        }
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
            public Gender 主题 { get; set; }


        }
        public static readonly DependencyProperty DemoModelProperty = DependencyProperty.Register(
            "DemoModel", typeof(PropertyGridModel), typeof(ReadingOnline), new PropertyMetadata(default(PropertyGridModel)));
        public PropertyGridModel SettingModel
        {
            get => (PropertyGridModel)GetValue(DemoModelProperty);
            set => SetValue(DemoModelProperty, value);
        }
        /// <summary> 文件格式过滤器。
        /// </summary>
        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty PathProperty =
        DependencyProperty.Register("Path", typeof(string), typeof(ReadingOnline), new PropertyMetadata(string.Empty));
        /// <summary> 选择的路径
        /// </summary>
        public string dwnPath
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }
        public static readonly DependencyProperty FilterProperty =
        DependencyProperty.Register("Filter", typeof(string), typeof(ReadingOnline), new PropertyMetadata("All|*.*"));

        #endregion

        #region 加载条 模型相关
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

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private string keyWord;

        public string KeyWord { get => keyWord; set => SetProperty(ref keyWord, value); }


        private ObservableCollection<Book> searchResultBooks = new ObservableCollection<Book>();//搜索页
        public ObservableCollection<Book> SearchResultBooks { get => searchResultBooks; 
            set => SetProperty(ref searchResultBooks, value); }
        #endregion
    }
    #region 数据源

    public class DownloadingBook : Book, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private int downloadingProgress;

        public int DownloadingProgress { get => downloadingProgress; set => SetProperty(ref downloadingProgress, value); }
    }

    public class MarkedBook : Book, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private BitmapImage coverImage;

        public BitmapImage CoverImage { get => coverImage; set => SetProperty(ref coverImage, value); }
    }

    /*
    public class Book : INotifyPropertyChanged
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
        public Book()
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
        public Book(string na, double va, string author, string url)
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
    */

    public enum Gender
    {
        天蓝色,
        浅紫色
    }
    #endregion
}
