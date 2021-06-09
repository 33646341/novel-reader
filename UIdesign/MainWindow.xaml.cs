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
        #region 加载圈相关
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
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            #region
            DataContext = this;
            #endregion
        }
        #region 控件函数定义
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void textBoxId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtJD_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        #endregion
        #region 首页
        #region 搜索按钮
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnlineSearchAndRead.Form1 form = new OnlineSearchAndRead.Form1();
            String kw = keySearch.Text;
            //form.querytext = kw;

            ShowProgress = Visibility.Visible;

            // 不使用List类型，可实现自动刷新而不必切换源
            ObservableCollection<Fiction> _ltfi_Search = new ObservableCollection<Fiction>()
            {
                new Fiction() { col_fiction_id = "0", col_fiction_name = "Tim", col_fiction_author = "唐家三少",col_fiction_type="玄幻" },
                new Fiction() { col_fiction_id = "1", col_fiction_name = "Kimi", col_fiction_author = "唐家三少",col_fiction_type="玄幻" },
                new Fiction() { col_fiction_id = "2", col_fiction_name = "Amy", col_fiction_author = "唐家三少",col_fiction_type="玄幻" },
                new Fiction() { col_fiction_id = "3", col_fiction_name = "Tommy", col_fiction_author = "唐家三少",col_fiction_type="玄幻" },
                new Fiction() { col_fiction_id = "4", col_fiction_name = "Ricky", col_fiction_author = "唐家三少",col_fiction_type="玄幻" },

            };
            this.Lv_HomePage.ItemsSource = _ltfi_Search;//数据源

            new Thread(() =>
            {
                for (int i = 10000; i < 10020; i++)
                {
                    var fiction_i = new Fiction() { col_fiction_id = "5", col_fiction_name = "Richard", col_fiction_author = "天蚕土豆", col_fiction_type = "热血" };
                    fiction_i.col_fiction_id = $"{i}";
                    Thread.Sleep(200);

                    Dispatcher.Invoke(delegate ()
                    {
                        _ltfi_Search.Add(fiction_i);
                    });
                }
                ShowProgress = Visibility.Collapsed;
            }).Start();

        }
        #endregion
        #region 双击详情页
        private void SListView_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Fiction emp = (sender as ListViewItem).Content as Fiction;
            Window1 login1 = new Window1(emp.col_fiction_id, emp.col_fiction_name, emp.col_fiction_author, emp.col_fiction_type);   //Login为窗口名，把要跳转的新窗口实例化
            login1.Show();
        }   //打开新窗口
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
        #region 右键功能
        public void InfoPage(object sender, RoutedEventArgs e)
        {
            Fiction emp = (sender as ListViewItem).Content as Fiction;
            Window1 login1 = new Window1(emp.col_fiction_id, emp.col_fiction_name, emp.col_fiction_author, emp.col_fiction_type);   //Login为窗口名，把要跳转的新窗口实例化
            login1.Show();
        }
        public void BookShelf(object sender, RoutedEventArgs e)
        {

        }
        public void DownLoadBook(object sender, RoutedEventArgs e)
        {

        }

        #endregion
        #endregion
        #region 书架页
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<Student> stuList = new List<Student>()
            {
                new Student() { Id = 0, Name = "Tim", Age = 29 },
                new Student() { Id = 1, Name = "Tom", Age = 28 },
                new Student() { Id = 2, Name = "Kyle", Age = 27 },
                new Student() { Id = 3, Name = "Tony", Age = 24 },
                new Student() { Id = 4, Name = "Vina", Age = 26 },
                new Student() { Id = 5, Name = "Mike", Age = 22 },
            };
            this.Boksf_lv.ItemsSource = stuList;//数据源

            new Thread(() =>
            {
                for (int i = 10000; i < 10020; i++)
                {
                    var fiction_i = new Student() { Id = 5, Name = "Mike", Age = 22 };
                    fiction_i.Id = i;
                    Thread.Sleep(200);

                    Dispatcher.Invoke(delegate ()
                    {
                        stuList.Add(fiction_i);
                        Boksf_lv.ItemsSource = null;
                        Boksf_lv.ItemsSource = stuList;//刷新数据源
                    });
                }
                ShowProgress = Visibility.Collapsed;
            }).Start();
        }

        #endregion

        #region 下载页：正在下载中每项是进度条，可暂停可删除，下载完成放入已完成队列。
        //已完成中每项是可删除
        private void Dwning_btn_click(object sender, RoutedEventArgs e)
        {
            Novel_Spider.Form1 form = new Novel_Spider.Form1();
            form.url = "https://www.biquzhh.com/29719_29719087/";
            form.button3_Click(sender, e);//添加按钮，添加到队列开始下载
            form.button1_Click(sender, e);//下载按钮，开始下载
                                          // form.button2_Click(sender, e);//暂停按钮
                                          // MessageBox.Show($"{form.download_progress}");
                                          //MessageBox.Show(form.book[0]);
            ObservableCollection<BarValue> progress = new ObservableCollection<BarValue>()
            {
                new BarValue() { name = "0", barvalue = form.barvalue},
            };
            LV_DwnPage.ItemsSource = progress;//刷新数据源

            new Thread(() =>
            {
                while (form.barvalue < 100)
                {
                    //MessageBox.Show($"{form.download_progress}");
                    progress[0].barvalue = form.barvalue;
                    Thread.Sleep(100);
                }
            }).Start();

        }
        //执行这个方法
        private void Dwn_ctl_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Stop!");
        }   //打开新窗口
        private void Dwn_del_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Delete!");
        }   //打开新窗口
        #endregion


    }
    #region 数据源
    public class Student
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class BarValue : INotifyPropertyChanged
    {
        private double Barvalue;

        public double barvalue
        {
            get { return Barvalue; }
            set { Barvalue = value;
                NotifyPropertyChanged(nameof(barvalue));
            }
        }

        public string name { get; set; }
        //public double barvalue { get; set; }




        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
    public class Fiction
    {
        public string col_fiction_id { get; set; }
        public string col_fiction_name { get; set; }
        public string col_fiction_author { get; set; }
        public string col_fiction_type { get; set; }

    }
    #endregion

}
