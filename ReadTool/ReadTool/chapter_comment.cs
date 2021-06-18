using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTool
{
    class chapter_comment
    {
        public void save_comment(string novel_name, string chapter_name, string comment,string comment_path) //保存评论到文件，需要给出小说名称、章节名称、评论、保存评论的路径（给出Novel即可）
        {
            if (!Directory.Exists(comment_path + "/History/" + novel_name))
                Directory.CreateDirectory(comment_path + "/History/" + novel_name);//创建小说名文件夹
            string chapter_path = comment_path + "/History/" + novel_name + "/" + chapter_name + ".txt";
            using (FileStream fs = new FileStream(chapter_path, FileMode.Append, FileAccess.Write))
            {
                fs.Lock(0, fs.Length);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(comment);
                fs.Unlock(0, fs.Length);//一定要用在Flush()方法以前，否则抛出异常。
                sw.Flush();
            }
        }

        public string load_comment(string novel_name, string chapter_name,string comment_path)//得到评论，需要给出小说名称、章节名称
        {
            string chapter_comment = "";
            if (Directory.Exists(comment_path + "/History/" + novel_name + "/" + chapter_name + ".txt"))
            {
                using (StreamReader sr = new StreamReader(comment_path + "/History/" + novel_name + "/" + chapter_name + ".txt", Encoding.UTF8))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        chapter_comment += line;//加载评论
                    }
                }
            }
            return chapter_comment;
        }
    }
}
