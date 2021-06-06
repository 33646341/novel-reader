using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Linq;

namespace ReadTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string strPath = "";

        string folderPath = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            //路径选取
            //ImportBooks();
        }

        private void ImportBooks()
        {
            openFileDialog1.InitialDirectory = @"C:\";//默认路径，注意这里写路径时要用c:\\而不是c:\
            openFileDialog1.Filter = "TXT文件|*.txt";//过滤的文件，以|隔开，如“文本文件|*.*|Java文件|*.java”
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;//当filter有多个时，选择默认的filter，注意，下标时从1开始，如果只有一个filter可以不用写这个属性
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//这个是关键，意思是当你选择了文件后并点击了OK按钮
            {
                strPath = openFileDialog1.FileName;//获取选中文件的路径是通过FileName属性来获得
                folderPath = Path.GetDirectoryName(strPath);
                using (StreamReader sr = new StreamReader(strPath, System.Text.Encoding.UTF8))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        txtContent.AppendText(line + "\r\n");//我的文本框命名为txtList
                    }
                }
                DirectoryInfo dirs = new DirectoryInfo(folderPath);
                FileInfo[] file = dirs.GetFiles();
                int filecount = file.Count();//获得文件对象数量
                //循环文件夹
                for (int i = 0; i < filecount; i++)
                {
                    treeView1.Nodes.Add(file[i].Name.Replace(".txt",""));//将txt文件加入treeview
                }
            }
        }     

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string zj = e.Node.Text;
            SelectChapter(zj);
        }

        private void SelectChapter(string zj)
        {
            txtContent.Text = "";
            labChapter.Text = "当前章节:" + zj;
            using (StreamReader sr = new StreamReader(folderPath+"\\"+zj+".txt", System.Text.Encoding.UTF8))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    txtContent.AppendText(line + "\r\n");
                }
            }
        }

        SpeechSynthesizer reader = new SpeechSynthesizer();
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtContent.Text.Trim()!="")
            {
                reader.SpeakAsync(txtContent.Text.Trim());               
            }            
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (txtContent.Text.Trim() != "")
            {                
                SelectChapter(treeView1.Nodes[treeView1.SelectedNode.Index + 1].Text);
                treeView1.SelectedNode = treeView1.Nodes[treeView1.SelectedNode.Index + 1];
            }           
        }
        
        private void 导入书籍ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            //路径选取
            ImportBooks();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            MessageBox.Show("A");
            if (e.KeyChar.ToString() == Keys.A.ToString())
                {
                    MessageBox.Show("A");
                }
        }
    }
}
