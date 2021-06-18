using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NovelManager;

namespace ReadTool
{
    public class This_chapter_list
    {
        public List<string> chapter_name = new List<string>();//章节名称
        public List<string> chapter_content = new List<string>();//章节内容
    }

    class this_chapter_list
    {
       
        public This_chapter_list Get_chapter(string novel_path)
        {
            This_chapter_list Chapter_List = new This_chapter_list();
            // 数据库开始
            DirectoryInfo dirs = new DirectoryInfo(novel_path);
            var novelDAL = new NovelManager.NovelDAL();
            var nid = novelDAL.exsitsNovel(dirs.Name);
            foreach (var obj in novelDAL.getChapters(nid))
            {
                Chapter_List.chapter_name.Add(obj[1].ToString());
            }
            // 数据库结束

            for (int i = 0; i < Chapter_List.chapter_name.Count; i++)
            {
                using (StreamReader sr = new StreamReader(novel_path + "/" + Chapter_List.chapter_name[i] + ".txt", Encoding.UTF8))
                {
                    Chapter_List.chapter_name.Add("");
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Chapter_List.chapter_content[i] += (line + "\r\n");
                    }
                }
            }

            return Chapter_List;//返回类，里面有章节名称list和章节内容list
        }
    }
}
