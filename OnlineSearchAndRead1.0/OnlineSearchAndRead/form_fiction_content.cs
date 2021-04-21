using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace OnlineSearchAndRead
{
    public partial class form_fiction_content : Form
    {
        public form_fiction_content()
        {
            InitializeComponent();
        }
        get_chapter_content _cgcc = new get_chapter_content();
        
        int _i_Index = 0;//编号
        public form_fiction_content(chapter_list _tcl)
        {
            InitializeComponent();
            _tcl_Now = _tcl;
            string content = _cgcc.Get_Chapter_Content(_tcl_Now.col_chapter_url);
            this.textBox1.Text = content;

        }
        chapter_list _tcl_Now;
        public chapter_list Tcl_Now
        {
            get => _tcl_Now;
            set
            {
                _tcl_Now = value;
               
            }
        }
        //*要改动的部分
        public void Thread_Get_Chapter_Content(object _o)
        {
            //实体为null或者内容为空，才需要获取内容
            if (_tcl_Now == null || _tcl_Now.IsDownload == false)
            {
                _tcl_Now.col_chapter_content = _cgcc.Get_Chapter_Content(_tcl_Now.col_chapter_url);
            }
            this.BeginInvoke(new Action(() =>
            {
                if (_tcl_Now != null)
                {
                    this.textBox1.Clear();// = "";
                    this.textBox1.SelectionStart = 0;
                    this.textBox1.Text = _tcl_Now.col_chapter_content;
                }
            }));
        }
        public int I_Index { get => _i_Index; set => _i_Index = value; }


 

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void form_fiction_content_Load(object sender, EventArgs e)
        {

        }
    }
}





