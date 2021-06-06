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
        #region
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
        #endregion
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnlineSearchAndRead.Form1 form = new OnlineSearchAndRead.Form1();
            String kw = keySearch.Text;
            form.te = kw;

            ShowProgress = Visibility.Visible;

            List<fiction_info> _ltfi_Search = new List<fiction_info>();
            List<Student> stuList = new List<Student>()
            {
                new Student() { Id = 0, Name = "Tim", Age = 29 },
                new Student() { Id = 1, Name = "Tom", Age = 28 },
                new Student() { Id = 2, Name = "Kyle", Age = 27 },
                new Student() { Id = 3, Name = "Tony", Age = 24 },
                new Student() { Id = 4, Name = "Vina", Age = 26 },
                new Student() { Id = 5, Name = "Mike", Age = 22 },
            };
            this.Lv_HomePage.ItemsSource = stuList;//数据源

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
                        Lv_HomePage.ItemsSource = null;
                        Lv_HomePage.ItemsSource = stuList;//刷新数据源
                    });
                }
                ShowProgress = Visibility.Collapsed;
            }).Start();

        }

        private void SListView_ItemDoubleClick(object sender, RoutedEventArgs e)
        {
            Student emp = (sender as ListViewItem).Content as Student;

            Window1 login1 = new Window1(emp.Name, emp.Id, emp.Age);   //Login为窗口名，把要跳转的新窗口实例化
            login1.Show();
        }   //打开新窗口

        #region
        //单击表头排序
        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void Sort_Click(object sender,RoutedEventArgs e)
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
    }
    #region
    public class Student
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
    
    #endregion

}
