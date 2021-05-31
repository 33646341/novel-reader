using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Collections;

namespace NovelManager
{
    class NovelDAL
    {
        public static void Main()
        {
            Console.WriteLine("hi");

            var novelDAL = new NovelDAL();

            var re = novelDAL.delNovel(10);
            Console.WriteLine(re);

            re = novelDAL.addNovel("测试小说", "www.login.com");
            Console.WriteLine(re);

            foreach (var obj in novelDAL.getNovel())
            {
                var ns = obj as object[];
                Console.WriteLine($"小说编号 : {ns[0]}");
                Console.WriteLine($"小说名字 : {ns[1]}");
                Console.WriteLine($"小说链接 : {ns[2]}");
            }
            //while (true)
            //{
            //    novelDAL.get();
            //    Console.ReadLine();
            //}

            Console.ReadLine();

        }

        private string dbName = "ndb";

        public int addNovel(string name,string url)
        {
            var rows = 0;
            using (var connection = new SqliteConnection($@"Data Source={dbName}.db;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                $@"
                    INSERT INTO `novel` 
                    (`name`, `url`) VALUES ('{name}', '{url}');
                 ";
                rows = command.ExecuteNonQuery();
            }
            return rows;
        }

        public ArrayList getNovel()
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
                        al.Add(new object[] { id,zh, en });
                        //Console.WriteLine(zh);
                        //Console.WriteLine(en);
                    }
                }
            }
            Console.WriteLine("关闭数据库连接成功");
            return al;
        }

        public int delNovel(int nid)
        {
            var rows = 0;
            using (var connection = new SqliteConnection($@"Data Source={dbName}.db;"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                $@"
                    DELETE FROM `novel` WHERE rowid = {nid}
                 ";
                rows = command.ExecuteNonQuery();
            }
            return rows;
        }

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
    }
}
