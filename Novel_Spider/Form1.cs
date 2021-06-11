using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.IO.Compression;
using NovelManager;
//using System.Data.SQLite;

namespace Novel_Spider
{
    //https://www.biquzhh.com/0_4
    public partial class Form1 : Form
    {
        int index = -1;

        //double num = 0;

        //double sum = 0;

        public Queue<Chapter> chapters = new Queue<Chapter>();

        public List<string> book = new List<string>();

        public List<string> path = new List<string>();

        public double download_progress;
        //double download_progress2;

        string novel_name;

        public List<int> chapter_num = new List<int>();
        //double chapter_num2 = 0;


        public List<int> chapter_sum = new List<int>();
        //double chapter_sum2 = 0;
        public string url;

        bool tag = true;//标记是否暂停,初始为未暂停
       
        public Form1()
        {   
            InitializeComponent();
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            progressBar1.Value = 0;
            button1.Enabled = true;
            button2.Enabled = false;
        }
        public double barvalue
        {
            get
            {
                return download_progress;
            }
            set
            {
                download_progress = value;
            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;

            button1.Enabled = false;

            tag = true;

            Task task1 = new Task(() =>
                {
                    while (chapters.Count != 0)
                    {
                        if (tag == false)
                            break;

                        Chapter this_chapter = new Chapter();

                        lock (chapters)
                        {
                            this_chapter = chapters.Dequeue();
                        }

                        string[] except_char = {@"\","/",":","*","?","\"","<",">","|"};

                        string Aref_Name = this_chapter.Aref_Name; //获取章节地址

                        string file_name = this_chapter.file_name;//获取章节名

                        for(int i = 0 ;i < except_char.Length;i++)
                        {
                            file_name = file_name.Replace(except_char[i], "");
                        }

                        if(path.Count!=0)
                            Write_Novel(path[0] + "/" + file_name + ".txt", file_name, Aref_Name);

                        if (chapter_num.Count != 0 && chapter_sum.Count != 0)
                        {
                            chapter_num[0]++;

                            download_progress = 1.0 * chapter_num[0] / chapter_sum[0] * 100;
                           // MethodInvoker m = new MethodInvoker(() =>
                            //{
                               // progressBar1.Value = Convert.ToInt32(download_progress);
                            //});
                            //this.BeginInvoke(m);

                            if (chapter_sum[0] - 1 == chapter_num[0])
                            {
                                //insertDownloadRecord(path[0] + "/" + file_name + ".txt", Aref_Name, )
                                path.RemoveAt(0);
                                book.RemoveAt(0);
                                chapter_num.RemoveAt(0);
                                chapter_sum.RemoveAt(0);
                                //MethodInvoker mi = new MethodInvoker(() =>
                               //{
                                    download_progress = 100;

                                    //listBox1.Items.RemoveAt(0);

                                    if(chapter_sum.Count != 0)
                                        download_progress = 0;
                                //});
                                //this.BeginInvoke(mi);
                            }
                        }
              
                    }
                    if (tag == true)
                    {//
                        //MethodInvoker mi = new MethodInvoker(() =>
                        //{
                            download_progress = 100;
                            //progressBar1.Value = 100;
                            //MessageBox.Show("确定");
                            //this.Close();
                       // });
                        //this.BeginInvoke(mi);
                    }
                });

                Task task2 = new Task(() =>
                {
                    while (chapters.Count != 0)
                    {
                        if (tag == false)
                            break;
                        Chapter this_chapter = new Chapter();
                        lock (chapters)
                        {
                            this_chapter = chapters.Dequeue();
                        }
                        string Aref_Name = this_chapter.Aref_Name; //获取章节地址
                        string file_name = this_chapter.file_name;//获取章节名
                        if (path.Count != 0)
                            Write_Novel(path[0] + "/" + file_name + ".txt", file_name, Aref_Name);
                        if (chapter_num.Count != 0 && chapter_sum.Count != 0)
                        {
                            chapter_num[0]++;

                            download_progress = 1.0 * chapter_num[0] / chapter_sum[0] * 100;
                           // MethodInvoker m = new MethodInvoker(() =>
                           // {
                               // progressBar1.Value = Convert.ToInt32(download_progress);
                            //});
                            //this.BeginInvoke(m);

                            if (chapter_sum[0] == chapter_num[0])
                            {
                                path.RemoveAt(0);
                                book.RemoveAt(0);
                                chapter_num.RemoveAt(0);
                                chapter_sum.RemoveAt(0);
                             
                                    download_progress = 100;

                                    listBox1.Items.RemoveAt(0);

                                    if (chapter_sum.Count != 0)
                                        progressBar1.Value = 0;
                                //this.BeginInvoke(mi);
                            }
                        }
                    }
                });

            task1.Start();
            task2.Start();
            
        }

        private void insertDownloadRecord(string name, string url ,string path)
        {
            throw new NotImplementedException();
        }

        public void button2_Click(object sender, EventArgs e)
        {
            tag = false;
            button1.Enabled = true;
            button2.Enabled = false;
        }

        public void Write_Novel(string filename, string title, string url_name)
        {
            bool Novel_type = false;
            string Content_Html = HttpGet("https://www.biquzhh.com" + url_name);//获取内容页
            //string Content_Name = Regex.Match(Content_Html, "(?<=class=\"Readarea ReadAjax_content\">)([\\S\\s]*?)(?=<br />)").Value; //获取书名

            Regex Rege_Content0 = new Regex("(?<=false;</script><br />)([\\S\\s]*?)(?<=(<br /><script>))");
            Regex Rege_Content1 = new Regex("(?<=false;</script></div>)([\\S\\s]*?)(?=(<script>))");
            string Result_Content = Rege_Content0.Match(Content_Html).Value;
            Result_Content = Rege_Content1.Match(Result_Content).Value;

            if (Result_Content == "")
                {
                Novel_type = true;
                Rege_Content0 = new Regex("(?<=();</script><br />)([\\S\\s]*?)(?<=(<br /><script>))");
                Rege_Content1 = new Regex("(?<=();</script></div>)([\\S\\s]*?)(?=(<script>))");
                Result_Content = Rege_Content0.Match(Content_Html).Value;
                Result_Content = Rege_Content1.Match(Result_Content).Value;
                Result_Content = Result_Content.Replace("&nbsp;", "");
            }

            Regex Regex_Main = new Regex(@"(&nbsp;&nbsp;&nbsp;&nbsp;)(.*)");
            string Rsult_Main = Regex_Main.Match(Result_Content).Value; //正文

            string Screen_Content;
            if (Novel_type || Rsult_Main == "")
            {
                Screen_Content = Result_Content.Replace("<br />", "\r\n");
                Screen_Content = Screen_Content.Replace("&nbsp;", "");
            }
            else
            {
                Screen_Content = Rsult_Main.Replace("&nbsp;", "").Replace("<br />", "\r\n");
                Screen_Content = Regex.Unescape(Screen_Content); //字符串转意
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
            Web_Request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1";
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

       public class Chapter
        {
            public string file_name;
            public string Aref_Name;
        }

        public void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void button3_Click(object sender, EventArgs e)
        {
            string html;

            string Novel_Name;

            string Picture_Url;

            string[] except_char = { @"\", "/", ":", "*", "?", "\"", "<", ">", "|" };

            bool OLD = book.Contains(url);

            if (!OLD)
            {
                index++;

                book.Add(url);

                html = HttpGet(book[index]);

                Novel_Name = Regex.Match(html, @"(?<=<h1>)([\S\s]*?)(?=</h1>)").Value;

                for (int i = 0; i < except_char.Length; i++)
                {
                    Novel_Name = Novel_Name.Replace(except_char[i], "");
                }

                Picture_Url = Regex.Match(html, "<meta property=\"og:image\" content=\"https://.*?/>").Value.Replace("<meta property=\"og:image\" content=\"", "").Replace("\"/>", "");

                // 数据库开始
                var novelDAL = new NovelManager.NovelDAL();
                if (novelDAL.exsitsNovel(Novel_Name)==0)
                {
                    novelDAL.addNovel(Novel_Name, book[index]);
                }
                var Novel_id = novelDAL.exsitsNovel(Novel_Name);
                Console.WriteLine(Novel_id);
                // 数据库结束

                //novel_name1 = Novel_Name;//获取书名

                path.Add(System.AppDomain.CurrentDomain.BaseDirectory + "/Novel/" + Novel_Name);

                
                if (!Directory.Exists(path[index]))
                {
                    Directory.CreateDirectory(path[index]);
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


                if (chapters.Count == 0 || book.Count > 1)
                {
                    var counter = 0;
                    foreach (Match NextMatch in Matches)
                    {
                        Chapter this_chapter = new Chapter();

                        this_chapter.file_name = Regex.Match(NextMatch.Value, "(?<=\">)([\\S\\s]*?)(?=</a>)").Value; //获取章节名

                        for (int i = 0; i < except_char.Length; i++)
                        {
                            this_chapter.file_name = this_chapter.file_name.Replace(except_char[i], "");
                        }

                        this_chapter.Aref_Name = Regex.Match(NextMatch.Value, "(?<=<a href =\")([\\S\\s]*?)(?=\">)").Value; //获取章节地址

                        // 数据库开始
                        novelDAL.addChapter(Novel_id, counter, this_chapter.file_name, this_chapter.Aref_Name);
                        counter++;
                        // 数据库结束

                        chapters.Enqueue(this_chapter);
                    }
                }
                chapter_sum.Add(Matches.Count);

                chapter_num.Add(0);

                download_picture(Picture_Url, Novel_Name);

                listBox1.Items.Add(Novel_Name);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void download_picture(string Picture_url, string Novel_Name)
        {
            if (!Directory.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "/Novel_Picture"))
            {
                Directory.CreateDirectory(System.AppDomain.CurrentDomain.BaseDirectory + "/Novel_Picture");
            }

            string fileName = System.AppDomain.CurrentDomain.BaseDirectory + "/Novel_Picture/" + Novel_Name + ".jpg";

            //数据库开始
            var novelDAL = new NovelManager.NovelDAL();
            if (novelDAL.exsitsNovel(Novel_Name) == 0)
            {
                novelDAL.addNovel(Novel_Name, book[index]);
            }
            var Novel_id = novelDAL.exsitsNovel(Novel_Name);
            Console.WriteLine(Novel_id);
            novelDAL.updateNovel(Novel_id, "image",fileName);
            //数据库结束

            WebClient webClient = new WebClient();
            //下载url链接文件，并指定到本地的文件夹路径和文件名称
            webClient.DownloadFile(Picture_url, fileName);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            url = textBox1.Text;
        }
    }
}
