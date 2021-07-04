using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIdesign.Entity
{
    public class Book
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string workDirection { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string catId { get; set; }
        /// <summary>
        /// 网游竞技
        /// </summary>
        public string catName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string picUrl { get; set; }
        /// <summary>
        /// 暗黑破坏神之毁灭
        /// </summary>
        public string bookName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string authorId { get; set; }
        /// <summary>
        /// 第七重奏01
        /// </summary>
        public string authorName { get; set; }
        /// <summary>
        /// 　　百族亲王的奇幻暗黑大陆战（jie）斗(cao)毁灭史。
        /// </summary>
        public string bookDesc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string score { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string bookStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string visitCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string wordCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string commentCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string yesterdayBuy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastIndexId { get; set; }
        /// <summary>
        /// 第三千八百四十九章 明白人
        /// </summary>
        public string lastIndexName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string lastIndexUpdateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isVip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string crawlSourceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string crawlBookId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string crawlLastTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string crawlIsStop { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public string pageNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Book> list { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }

}
