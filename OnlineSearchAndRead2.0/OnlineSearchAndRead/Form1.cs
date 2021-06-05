using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace OnlineSearchAndRead
{
    public partial class Form1 : Form
    {
        private form_fiction_content anotherForm;
        //小说查找
        fiction_search _cfs = new fiction_search();
        List<fiction_info> _ltfi_Search;
        public string te = "";

        //操作线程
        //查找小说线程
        Thread _thread_Search;
        public Form1()
        {
              //InitializeComponent();
              //anotherForm = new form_fiction_content();
        }

        public List<fiction_info> Thread_Fiction_Search()
        {
            _ltfi_Search = _cfs._o_Get_Fiction_Info_By_KeyWord(te);//关键字得到信息
            return _ltfi_Search;
           
        }
        public void show_text_box(string msg)
        {
            MessageBox.Show(msg);
            
        }
        //显示查找列表
        public void Show_Search_List(List<fiction_info> _ltfi)
        {
            Lv_HomePage.Items.Clear();
            if (_ltfi_Search != null && _ltfi_Search.Count > 0)
            {
                foreach (fiction_info _tfi in _ltfi_Search)
                {
                    ListViewItem _lvi = new ListViewItem(_tfi.col_fiction_type);
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

        //提示信息

        public List<fiction_info> button1_Click()
        {
            return Thread_Fiction_Search();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Lv_HomePage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //
                fiction_info _clc = Lv_HomePage.SelectedItems[0].Tag as fiction_info;//
                string url_new = _clc.col_update_chapter_url;
                //获取章节信息和章节地址
                anotherForm = new form_fiction_content(new chapter_list()
                {
                    col_chapter_url = url_new,
                    col_chapter_name = _clc.col_update_chapter
                });//_tfdi_All_Info._ltcl_Chapter.Count() -
                anotherForm.Owner = this;
                anotherForm.Show();
            }
            catch
            {
                
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
