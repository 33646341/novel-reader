using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineSearchAndRead
{
    public partial class Form_detail_content : Form
    {
        private form_fiction_content anotherForm;
        //小说查找
        get_homepage_content _cfs = new get_homepage_content();
        List<chapter_list> _ltfi_Search;


        //操作线程

        public Form_detail_content()
        {
            InitializeComponent();
            anotherForm = new form_fiction_content();
        }
        fiction_info _fcl_Now;
        public Form_detail_content(fiction_info _fcl)
        {
            InitializeComponent();
            anotherForm = new form_fiction_content();
            //显示表格操作
            _fcl_Now = _fcl;
            Tuple<fiction_info, List<chapter_list>> tup = _cfs.TupleDetail(_fcl_Now.col_url_homepage);
            //_fcl_Now = _cfs._o_Get_Fiction_Detail(_fcl_Now.col_url_homepage);
            Show_TextBox(tup.Item1);
            //_ltfi_Search = _cfs._o_Get_Chapter_Content(_fcl_Now.col_url_homepage);//关键字得到信息
            Show_Search_List(tup.Item2);
            
            /*_thread_Search_Fiction = new Thread(Thread_Detail_Search);
            _thread_Search_Fiction.IsBackground = true;
            _thread_Search_Fiction.Start();

            _thread_Search_Chapter = new Thread(Thread_Chapter_Search);
            _thread_Search_Chapter.IsBackground = true;
            _thread_Search_Chapter.Start();
            */
        }
        public void Thread_Detail_Search(object _fcl)// fiction_info
        {
            _fcl_Now = _cfs._o_Get_Fiction_Detail(_fcl_Now.col_url_homepage);//关键字得到信息
            listView1.BeginInvoke(new Action(() =>
            {
                if (_fcl_Now == null)
                {
                    //UI设计的同学可考虑添加控件
                    listView1.Items.Clear();
                }
                else
                {
                    Show_TextBox(_fcl_Now);
                }
                listView1.Enabled = true;
            }));
            //return _fcl_Now;

        }
        public void Thread_Chapter_Search(object _fcl)// List<fiction_info>
        {
            _ltfi_Search = _cfs._o_Get_Chapter_Content(_fcl_Now.col_url_homepage);//关键字得到信息
            listView1.BeginInvoke(new Action(() =>
            {
                if (_ltfi_Search == null || _ltfi_Search.Count == 0)
                {
                    //UI设计的同学可考虑添加控件
                    listView1.Items.Clear();
                }
                else
                {
                    Show_Search_List(_ltfi_Search);
                }
                listView1.Enabled = true;
            }));
            //return _ltfi_Search;

        }

        //显示查找列表
        public void Show_Search_List(List<chapter_list> _ltfi)
        {
            listView1.Items.Clear();
            if (_ltfi != null && _ltfi.Count > 0)
            {
                foreach (chapter_list _tfi in _ltfi)
                {
                    ListViewItem _lvi = new ListViewItem(_tfi.col_chapter_url);
                    _lvi.SubItems.Add(_tfi.col_chapter_name);
                    _lvi.Tag = _tfi;
                    listView1.Items.Add(_lvi);
                }
            }
        }

        public void Show_TextBox(fiction_info _ftfi)
        {
            textBox1.Clear();
            textBox1.Text = "小说名: " + _ftfi.col_fiction_name + "小说主页链接  " + _ftfi.col_url_homepage + " 小说作者 " + _ftfi.col_fiction_author + "最后更新时间  " + _ftfi.col_update_time + "最新章节名  " + _ftfi.col_update_chapter + "最新章节链接  " + _ftfi.col_update_chapter_url + "小说简介： " + _ftfi.col_fiction_introduction + "封皮链接：" + _ftfi.col_url_poster;
            /*if (_ltfi_Search != null && _ltfi_Search.Count > 0)
            {
                foreach (chapter_list _tfi in _ltfi_Search)
                {
                    ListViewItem _lvi = new ListViewItem(_tfi.col_chapter_url);
                    _lvi.SubItems.Add(_tfi.col_chapter_name);
                    _lvi.Tag = _tfi;
                    listView1.Items.Add(_lvi);
                }
            }
            */
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //
                //fiction_info _clc = listView1.SelectedItems[0].Tag as fiction_info;//
                //string url_new = _clc.col_update_chapter_url;
                //尝试跳进小说详情页
                chapter_list _clc = listView1.SelectedItems[0].Tag as chapter_list;//
                string url_new = "https://www.biquzhh.com" + _clc.col_chapter_url;

                //获取章节信息和章节地址
                anotherForm = new form_fiction_content(new chapter_list()
                {
                    col_chapter_url = url_new,
                    col_chapter_name = _clc.col_chapter_name//_clc.col_url_homepage//
                });//_tfdi_All_Info._ltcl_Chapter.Count() 
                anotherForm.Owner = this;
                anotherForm.Show();
            }
            catch
            {

            }
        }






    }
}
