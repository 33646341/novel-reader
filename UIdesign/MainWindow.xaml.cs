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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OnlineSearchAndRead.Form1 form = new OnlineSearchAndRead.Form1();
            String kw = keySearch.Text;
            form.te = kw;
            List<fiction_info> _ltfi_Search = form.button1_Click();
           /* Lv_HomePage.BeginInvoke(new Action(() =>
            {
                if (_ltfi_Search == null || _ltfi_Search.Count == 0)
                {
                    Lv_HomePage.Items.Clear();
                }
                else
                {
                    Show_Search_List(_ltfi_Search);
                }
                Lv_HomePage.Enabled = true;
            }));
           */
        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

      
        /*
        public void Show_Search_List(List<fiction_info> _ltfi_Search)
        {
            Lv_HomePage.Items.Clear();
            if (_ltfi_Search != null && _ltfi_Search.Count > 0)
            {
                foreach (fiction_info _tfi in _ltfi_Search)
                {

                    ListViewItem _lvi = new ListViewItem();
                    _lvi.
                    _lvi.SubItems.Add(_tfi.col_fiction_name);
                    _lvi.SubItems.Add(_tfi.col_fiction_author);
                    _lvi.SubItems.Add(_tfi.col_update_chapter);
                    _lvi.SubItems.Add(_tfi.col_update_time.ToString("yyyy-MM-dd"));
                    _lvi.SubItems.Add(_tfi.col_fiction_stata);
                    _lvi.SubItems.Add(_tfi.col_click_count);
                    _lvi.Tag = _tfi;

                    Lv_HomePage.Items.Add(_lvi);
                }
            }
        }
         */
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Lv_HomePage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LV_Item_Selected(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
