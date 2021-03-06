using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace OnlineSearchAndRead
{
    public class get_chapter_content
    {
        string _url_Chapter = "";
        fiction_info _tfi_Main;
        List<Task> _ltask = new List<Task>();

        /// <summary>
        /// 设置小说实体
        /// </summary>
        public fiction_info Tfi_Main { get => _tfi_Main; set => _tfi_Main = value; }

        /// <summary>
        /// 实例化爬取章节内容
        /// </summary>
        public get_chapter_content(fiction_info _tfi = null)
        {
            _tfi_Main = _tfi;
        }
        /// <summary>
        /// 实例化爬取章节内容
        /// </summary>
        /// <param name="_url">章节URL</param>
        public get_chapter_content(string _url, fiction_info _tfi = null)
        {
            _url_Chapter = _url;
            _tfi_Main = _tfi;
        }
        /// <summary>
        /// 获取单章节内容
        /// </summary>
        /// <returns>章节内容</returns>
        public string Get_Chapter_Content()
        {
            string _s_ret = "";
            //判断链接是否存在
            if (_url_Chapter == "")
                return _s_ret;

            HtmlWeb _web_Main = new HtmlWeb();
            _web_Main.OverrideEncoding = Encoding.GetEncoding("gb2312");
            //_web_Main.OverrideEncoding = Encoding.;
            try
            {
                HtmlAgilityPack.HtmlDocument _doc_Main = new HtmlAgilityPack.HtmlDocument();
                _doc_Main = _web_Main.Load(_url_Chapter);
                //判断是否有数据
                if (_doc_Main.Text == "")
                    return _s_ret;

                HtmlNodeCollection _hnc_Chapter_Content = null;
                

                //获取章节内容，存在部分章节内容为null
                if (_doc_Main.DocumentNode
                 .SelectNodes("//div[starts-with(@id,'content')]")!=null)
                    _hnc_Chapter_Content = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'content')]");///reader-maindiv[starts-with(@class,'box_con')]/div[starts-with(@id,'content')]
                //未实例化，查看原网页解决，原网页给了超链接，但打不开，直接置为没有
                else
                    return "抱歉,该内容暂时缺失";
                
                if (_hnc_Chapter_Content.Count == 0)
                        return _s_ret;

                foreach (var div in _doc_Main.DocumentNode.Descendants("div").ToArray())
                    
                {
                    div.Remove();
                    foreach (var script in _doc_Main.DocumentNode.Descendants("script").ToArray())
                        script.Remove();
                }
                _s_ret = _hnc_Chapter_Content[0].InnerHtml.Trim()
                    .Replace("<br />", "\r\n")
                    .Replace("<br>", "\r\n")
                    .Replace("&nbsp", "")
                    .Replace("<!-", "")
                    .Replace("->", "")
                    .Replace("https://www.biquzhh.com/", "")
                    .Replace("0", "")
                    .Replace("1", "")
                    .Replace("2", "")
                    .Replace("3", "")
                    .Replace("4", "")
                    .Replace("5", "")
                    .Replace("6", "")
                    .Replace("7", "")
                    .Replace("8", "")
                    .Replace("9", "")
                    .Replace("_", "")
                    .Replace(".html", "")
                    .Replace("/", "")
                    .Replace("  ", "")
                    .Replace("天才一秒记住本站地址：www.biquzhh.com。笔趣阁手机版阅读网址：m.biquzhh.com", "")
                    .Replace(";", "");
                

                return _s_ret;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取单章节内容
        /// </summary>
        /// <param name="_url">章节URL</param>
        /// <returns>章节内容</returns>
        public string Get_Chapter_Content(string _url)
        {
            _url_Chapter = _url;
            return Get_Chapter_Content();
        }
    }

}
