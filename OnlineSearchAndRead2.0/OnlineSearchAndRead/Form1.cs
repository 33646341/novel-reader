using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineSearchAndRead
{
    public partial class Form1 : Form
    {
        private Form_detail_content anotherForm;
        //小说查找
        fiction_search _cfs = new fiction_search();
        List<fiction_info> _ltfi_Search;


        //操作线程
        //查找小说线程
        Thread _thread_Search;
        public Form1()
        {
            InitializeComponent();
            anotherForm = new Form_detail_content();
        }

        public void Thread_Fiction_Search(object _s_kw)// List<fiction_info>
        {
            _ltfi_Search = _cfs._o_Get_Fiction_Info_By_KeyWord(_s_kw.ToString());//关键字得到信息
            Lv_HomePage.BeginInvoke(new Action(() =>
            {
                if (_ltfi_Search == null || _ltfi_Search.Count == 0)
                {
                    //UI设计的同学可考虑添加控件
                    //Show_Btm_Msg("无匹配查询结果，请更换关键词后重试！", 0);
                    //测试专用
                    //show_text_box("失败");
                    Lv_HomePage.Items.Clear();
                }
                else
                {
                    //show_text_box("成功");
                    //Show_Btm_Msg("查找成功，相关数据【" + _ltfi_Search.Count + "】条！", 0);
                    Show_Search_List(_ltfi_Search);
                }
                Lv_HomePage.Enabled = true;
            }));
            //return _ltfi_Search;

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
                    ListViewItem _lvi = new ListViewItem(_tfi.col_fiction_id);
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

        private void button1_Click(object sender, EventArgs e)
        {
            //设计时暂时不考虑线程冲突问题，可以设计msgbox解决
            //标识线程正在执行，需要关闭线程
           // if (_thread_Search != null && _thread_Search.IsAlive)
            {
                //_thread_Search.Abort();
                //_thread_Search.Join();
                //_thread_Search = null;
                _thread_Search = new Thread(Thread_Fiction_Search);
                _thread_Search.IsBackground = true;
                _thread_Search.Start(textBox1.Text);
                
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Lv_HomePage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //
                //fiction_info _clc = Lv_HomePage.SelectedItems[0].Tag as fiction_info;//
                //string url_new = _clc.col_update_chapter_url;
                //尝试跳进小说详情页
                fiction_info _clc = Lv_HomePage.SelectedItems[0].Tag as fiction_info;//
                string url_new = _clc.col_url_homepage;//_clc.col_url_homepage;//文章内容 https://www.biquzhh.com/62531_62531024/669296227.html

                //获取章节信息和章节地址
                anotherForm = new Form_detail_content(new fiction_info()
                {
                    col_url_homepage = url_new,
                    //col_chapter_name = "zzz"//_clc.col_url_homepage//col_update_chapter
                }) ;//_tfdi_All_Info._ltcl_Chapter.Count() 
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
    }
}
