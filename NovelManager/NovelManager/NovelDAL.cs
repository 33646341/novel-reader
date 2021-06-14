using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections;

namespace NovelManager
{
    public class NovelDAL
    {
        public NovelDAL()
        {
            System.Diagnostics.StackTrace ss = new System.Diagnostics.StackTrace(true);
            System.Reflection.MethodBase mb = ss.GetFrame(1).GetMethod();
            var ori_ns = mb.DeclaringType.Namespace; // 取得父方法命名空间
            Console.WriteLine(ori_ns);
            if(ori_ns == "Novel_Spider")
            {
                dbName = @"..\..\..\ndb";
            }
        }

        private string dbName = @"..\..\..\ndb";
        //private string dbName = @"D:\Richard\C#project\Test\ndb";

        public static void Main()
        {
            var novelDAL = new NovelDAL();
            
            Console.WriteLine($"1 isStarred? {novelDAL.isStarred(1)}");
            Console.WriteLine($"2 isStarred? {novelDAL.isStarred(2)}");

            Console.WriteLine($"cid=1 的阅读进度? {novelDAL.readingProgress(1)}");
            Console.WriteLine($"cid=2 的阅读进度? {novelDAL.readingProgress(2)}");

            Console.WriteLine($"collect nid 1? {novelDAL.collectNovel(1)}");

            //Console.WriteLine($"updateNovel nid 1? {novelDAL.updateNovel(1, "号", "www.ggg.vom", "分类")}");

            //增删查测试();

            Console.ReadLine();
        }

        public IEnumerable<SqliteDataReader> getDownloadedNovels()
        {
            return ExecuteReader($@"
                SELECT `nid`,`name`,`author`,`url`
                FROM novel
                WHERE downloadPath is not NULL
                order by nid
            ");
        }

        public IEnumerable<SqliteDataReader> getChapters(int nid)
        {
            return ExecuteReader($@"
                SELECT cno,cname,downloadPath
                FROM chapter
                WHERE nid = {nid}
                order by cno
            ");
        }

        public int addChapter(int nid, int cno, string cname, string curl)
        {
            try
            {
                return ExecuteNonQuery($@"
                    INSERT INTO chapter
                    ('nid','cno','cname','curl') VALUES ({nid},{cno},'{cname}','{curl}');
                ");
            }
            catch
            {
                return 0; // 违反唯一性约束
            }
        }

        public int updateNovel(int nid, string propName, string value)
        {
            if (exsitsNovel(nid) > 0)
            {
                return ExecuteNonQuery($@"
                    UPDATE novel
                    set {propName} = '{value}'
                    where nid = {nid}
                ");
            }
            return 0;
        }

        /// <summary>
        /// 查询小说是否已存在小说数据库中
        /// </summary>
        /// <param name="name">小说名</param>
        /// <returns>小说标识符nid</returns>
        public int exsitsNovel(string name)
        {
            // 此处低调的foreach实则是GetEnumerator()的优雅形式。
            foreach (var obj in ExecuteReader($@"
                SELECT novel.nid 
                FROM novel 
                WHERE novel.name = '{name}';
            "))
            {
                if (!obj.IsDBNull(0))
                {
                    return obj.GetInt32(0); // 成功返回
                }
            }
            return 0; // 失败返回
        }

        /// <summary>
        /// 查询小说是否已存在小说数据库中
        /// </summary>
        /// <param name="nid">小说id</param>
        /// <returns></returns>
        public int exsitsNovel(int nid)
        {
            // 此处低调的foreach实则是GetEnumerator()的优雅形式。
            foreach (var obj in ExecuteReader($@"
                SELECT novel.nid 
                FROM novel 
                WHERE novel.nid = {nid};
            "))
            {
                if (!obj.IsDBNull(0))
                {
                    return obj.GetInt32(0); // 成功返回
                }
            }
            return 0; // 失败返回
        }

        public int collectNovel(int nid)
        {
            try
            {
                return ExecuteNonQuery($@"
                    INSERT INTO `collection`
                    ('nid') VALUES ({nid});
                ");
            }
            catch
            {
                return 0; // 违反唯一性约束，已收藏的小说重复收藏了
            }
        }

        public bool isStarred(int nid)
        {
            return ExecuteReader($@"
                SELECT `cid`,`nid`
                FROM collection
                where nid = {nid};
            ").Count() > 0;
        }

        public double readingProgress(int cid)
        {
            // 此处低调的foreach实则是GetEnumerator()的优雅形式。
            foreach (var obj in ExecuteReader($@"
                SELECT readWords/totalWords
                FROM chapter
                where chaid = {cid}
            "))
            {
                if (!obj.IsDBNull(0))
                {
                    return obj.GetDouble(0); // 成功返回
                }
            }
            return 0; // 失败返回
        }

        #region "增删查的功能函数与测试"
        public int addNovel(string name, string url, string category = "默认分类")
        {
            return ExecuteNonQuery($@"
                INSERT INTO `novel` 
                (`name`, `url`,`category`) VALUES ('{name}', '{url}','{category}');
            ");
        }

        public ArrayList getNovel2()
        {
            ArrayList al = new ArrayList();

            using (var connection = new SqliteConnection($@"Data Source={dbName}.db;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                        SELECT `nid`,`name`,`url`
                        FROM novel
                        order by `name`
                    ";

                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("打开数据库连接成功");
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var zh = reader.GetString(1);
                        var en = reader.GetString(2);
                        al.Add(new object[] { id, zh, en });
                        //Console.WriteLine(zh);
                        //Console.WriteLine(en);
                    }
                }
            }
            Console.WriteLine("关闭数据库连接成功");
            return al;
        }

        public IEnumerable<SqliteDataReader> getNovel()
        {
            return ExecuteReader(@"
                SELECT `nid`,`name`,`url`
                FROM novel
                order by `name`
            ");
        }

        public int delNovel(int nid)
        {
            return ExecuteNonQuery($@"
                DELETE FROM `novel` WHERE rowid = {nid};
            ");
        }

        static void 增删查测试()
        {
            var novelDAL = new NovelDAL();
            Console.WriteLine("你好，数据库连接测试");

            // 删除10号小说记录，返回是否成功

            var re = novelDAL.delNovel(10);
            Console.WriteLine($"删除10号记录！影响了{re}行");

            // 添加小说(小说名，小说链接)，返回是否成功

            re = novelDAL.addNovel("测试小说", "www.login.com");
            Console.WriteLine($"添加一行记录！影响了{re}行");

            // 查询库中所有小说

            foreach (var obj in novelDAL.getNovel())
            {
                Console.WriteLine($"小说编号 : {obj[0]}");
                Console.WriteLine($"小说名字 : {obj[1]}");
                Console.WriteLine($"小说链接 : {obj[2]}");
            }
        }
        #endregion


        /*
        private IEnumerator words;

        public NovelDAL()
        {
            words = new Words().GetEnumerator();
        }

        public class Words : IEnumerable
        {
            //public ToolStripLabel tip { get; set; }
            private string dbName = "ndb";

            public IEnumerator GetEnumerator()
            {
                using (var connection = new SqliteConnection($@"Data Source={dbName}.db;"))
                {
                    connection.Open();
                    //tip.Text = ("打开数据库连接成功");

                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                        SELECT `name`,`url`
                        FROM novel
                        order by `name`
                    ";

                    using (var reader = command.ExecuteReader())
                    {
                        Console.WriteLine("打开数据库连接成功");
                        while (reader.Read())
                        {
                            var zh = reader.GetString(0);
                            var en = reader.GetString(1);
                            yield return new string[] { zh, en };
                        }
                    }
                }
                Console.WriteLine("关闭数据库连接成功");
                //tip.Text = ("关闭数据库连接成功");
            }
        }

        private string anwser = "";

        private void get()
        {
            if (words.MoveNext())
            {
                var word = (string[])words.Current;
                var wd = word[0];
                anwser = word[1];
                Console.WriteLine(wd);
                Console.WriteLine(anwser);
            }
            else
            {
                Console.WriteLine("单词用完了，按'go'重新开始");
                //tip.Text = "单词用完了，按'go'重新开始";
                //wd.Text = "WIN!";
                //anwser = "";
                words = new Words().GetEnumerator();
            }
        }

        */

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (input.Text == anwser)
        //    {
        //        if (anwser.Length > 0) tip.Text = ("回答正确！");
        //        input.Text = "";
        //        if (words.MoveNext())
        //        {
        //            var word = (string[])words.Current;
        //            wd.Text = word[0];
        //            anwser = word[1];
        //        }
        //        else
        //        {
        //            tip.Text = "单词用完了，按'go'重新开始";
        //            wd.Text = "WIN!";
        //            anwser = "";
        //            words = new Words() { tip = tip }.GetEnumerator();
        //        }
        //    }
        //    else
        //    {
        //        tip.Text = ("回答错误，重新输入");
        //        Console.WriteLine(anwser);
        //    }
        //    input.Focus();
        //}

        //private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (e.KeyChar == 13)
        //    {
        //        e.Handled = true;
        //        button1.PerformClick();
        //    }
        //}

        /// <summary>
        /// 查询语句的一般封装形式
        /// </summary>
        /// <param name="SQLStatement">要请求的SQL语句</param>
        /// <returns>SqliteDataReader</returns>
        private IEnumerable<SqliteDataReader> ExecuteReader(string SQLStatement)
        {
            using (var connection = new SqliteConnection($@"Data Source={dbName}.db;"))
            {
                Console.WriteLine($@"Data Source='{dbName}.db';");
                //SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_dynamic_cdecl());
                SQLitePCL.Batteries.Init();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = SQLStatement;
                using (var reader = command.ExecuteReader())
                {
                    //Console.WriteLine("打开数据库连接成功");
                    while (reader.Read())
                    {
                        yield return reader;
                    }
                }
            }
            //Console.WriteLine("关闭数据库连接成功");
        }

        /// <summary>
        /// 无需返回查询结果的查询语句一般封装形式
        /// </summary>
        /// <param name="SQLStatement">要请求的SQL语句</param>
        /// <returns>受影响的行数，一般不为零表示成功调用</returns>
        private int ExecuteNonQuery(string SQLStatement)
        {
            var rows = 0;
            using (var connection = new SqliteConnection($@"Data Source={dbName}.db;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = SQLStatement;
                rows = command.ExecuteNonQuery();
            }
            return rows;
        }
    }
}
