using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Compression;

namespace Novel_Spider
{
    public partial class Form1 : Form
    {
        double download_progress;
        double chapter_num = 0;
        double chapter_sum = 0;
        public Form1()
        {
            InitializeComponent();
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = 100;
            this.progressBar1.Value = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string html = HttpGet(Url_Txt.Text);

            string Novel_Name = Regex.Match(html, @"(?<=<h1>)([\S\s]*?)(?=</h1>)").Value; //获取书名

            string path = System.AppDomain.CurrentDomain.BaseDirectory + "/Novel/" + Novel_Name;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }//创建小说名文件夹

            string strregex = "(?<=<dt>《" + Novel_Name + "》正文卷)([\\S\\s]*?).+?(?=list3())";
            //string strregex = @"(?<=<dt>《小世界其乐无穷》正文卷)([\S\s]*?).+?(?=list3())";
            Regex Regex_Menu = new Regex(strregex);
            string Result_Menu = Regex_Menu.Match(html).Value; //获取列表内容


            MatchCollection Matches = Regex.Matches(
            Result_Menu,
            "(?<=<dd>)([\\S\\s]*?)(?=</dd>)",
            RegexOptions.IgnoreCase |         //忽略大小写  
            RegexOptions.ExplicitCapture    //提高检索效率   
            );

            chapter_sum = Matches.Count;

            foreach (Match NextMatch in Matches)
            {
                string Aref_Name = Regex.Match(NextMatch.Value, "(?<=<a href =\")([\\S\\s]*?)(?=\">)").Value; //获取书名
                string file_name = Regex.Match(NextMatch.Value, "(?<=\">)([\\S\\s]*?)(?=</a>)").Value; //获取书名
                Write_Novel(path + "/" + file_name + ".txt", file_name, Aref_Name);
                chapter_num++;
                download_progress = chapter_num / chapter_sum * 100;
                this.progressBar1.Value = Convert.ToInt32(download_progress);
            }

            MessageBox.Show("完成!");
            this.Close();
        }

        private void Write_Novel(string filename, string title, string url_name)
        {
            bool Novel_type = false;
            string Content_Html = HttpGet("https://www.biquzhh.com" + url_name);//获取内容页
            //string Content_Name = Regex.Match(Content_Html, "(?<=class=\"Readarea ReadAjax_content\">)([\\S\\s]*?)(?=<br />)").Value; //获取书名
            Regex Rege_Content0 = new Regex("(?<=false;</script><br />)([\\S\\s]*?)(?<=(<br /><script>))");
            Regex Rege_Content1 = new Regex("(?<=false;</script></div>)([\\S\\s]*?)(?=(<script>))");
            string Result_Content = Rege_Content0.Match(Content_Html).Value;
            Result_Content = Rege_Content1.Match(Result_Content).Value;//获取文章内容

            if (Result_Content == "")
                {
                Novel_type = true;
                Rege_Content0 = new Regex("(?<=();</script><br />)([\\S\\s]*?)(?<=(<br /><script>))");
                Rege_Content1 = new Regex("(?<=();</script></div>)([\\S\\s]*?)(?=(<script>))");
                Result_Content = Rege_Content0.Match(Content_Html).Value;
                Result_Content = Rege_Content1.Match(Result_Content).Value;//获取文章内容
            }

            Regex Regex_Main = new Regex(@"(&nbsp;&nbsp;&nbsp;&nbsp;)(.*)");
            string Rsult_Main = Regex_Main.Match(Result_Content).Value; //正文

            string Screen_Content;
            if (Novel_type || Rsult_Main == "")
            {
                Screen_Content = Result_Content.Replace("<br />", "\r\n");
            }
            else
            {
                Screen_Content = Rsult_Main.Replace("&nbsp;", "").Replace("<br />", "\r\n");
                Screen_Content = System.Text.RegularExpressions.Regex.Unescape(Screen_Content); //字符串转意
            }

            using (FileStream fsWrite = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fsWrite.Seek(0, SeekOrigin.Begin);
                byte[] novel = Encoding.UTF8.GetBytes(title + "\r\n" + Screen_Content);
                fsWrite.Write(novel, 0, novel.Length);
                fsWrite.Close();
            }
        }

        /// <summary>
        /// 抓取网页并转码
        /// </summary>
        /// <param name="url"></param>
        /// <param name="post_parament"></param>
        /// <returns></returns>
        public string HttpGet(string url)
        {
            string html;
            HttpWebRequest Web_Request = (HttpWebRequest)WebRequest.Create(url);
            Web_Request.Timeout = 30000;
            Web_Request.Method = "GET";
            Web_Request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:21.0) Gecko/20100101 Firefox/21.0";
            Web_Request.Headers.Add("Accept-Encoding", "gzip, deflate");
            //Web_Request.Credentials = CredentialCache.DefaultCredentials;
            //设置代理属性WebProxy-------------------------------------------------
            //WebProxy proxy = new WebProxy("111.13.7.120", 80);
            ////在发起HTTP请求前将proxy赋值给HttpWebRequest的Proxy属性
            //Web_Request.Proxy = proxy;

            HttpWebResponse Web_Response = (HttpWebResponse)Web_Request.GetResponse();

            if (Web_Response.ContentEncoding.ToLower() == "gzip")  // 如果使用了GZip则先解压
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (var Zip_Stream = new GZipStream(Stream_Receive, CompressionMode.Decompress))
                    {
                        using (StreamReader Stream_Reader = new StreamReader(Zip_Stream, Encoding.Default))
                        {
                            html = Stream_Reader.ReadToEnd();
                        }
                    }
                }
            }
            else
            {
                using (Stream Stream_Receive = Web_Response.GetResponseStream())
                {
                    using (StreamReader Stream_Reader = new StreamReader(Stream_Receive, Encoding.Default))
                    {
                        html = Stream_Reader.ReadToEnd();
                    }
                }
            }

            return html;
        }
    }
}
