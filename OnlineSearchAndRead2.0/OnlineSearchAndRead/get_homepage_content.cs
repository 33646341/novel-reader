using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OnlineSearchAndRead
{
    public class get_homepage_content
    {
        string _url_Homepage = "";

        public get_homepage_content() { }
        public get_homepage_content(string _url)
        {
            _url_Homepage = _url;
        }

        public fiction_info _o_Get_Fiction_Detail()
        {
            //判断链接是否存在
            if (_url_Homepage == "")
                return null;

            fiction_info _tfi_ret = new fiction_info();

            HtmlWeb _web_Main = new HtmlWeb();
            _web_Main.OverrideEncoding = Encoding.GetEncoding("gb2312");
            HtmlAgilityPack.HtmlDocument _doc_Main = new HtmlAgilityPack.HtmlDocument();
            _doc_Main = _web_Main.Load(_url_Homepage);
            //判断是否有数据
            if (_doc_Main.Text == "")
                return null;

            //获取小说名
            HtmlNodeCollection _hnc_Title = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'info')]/h1");
            if (_hnc_Title.Count == 0)
                return null;

            _tfi_ret.col_fiction_name = _hnc_Title[0].InnerText;

            //小说主页链接

            _tfi_ret.col_url_homepage = _url_Homepage;

            //获取小说信息
            HtmlNodeCollection _hnc_Info = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'info')]");//"//div[starts-with(@id,'maininfo')]/div[starts-with(@id,'info')]"
            if (_hnc_Info.Count == 0)
                return null;
            //获取小说作者
            var _rg_Author = Regex.Matches(_hnc_Info[0].InnerHtml.Replace("&nbsp;", ""), @">作者：([^<]+)<");
            if (_rg_Author.Count > 0)
            {
                _tfi_ret.col_fiction_author = _rg_Author[0].Value.Replace(">作者：", "")
                    .Replace("<", "");
            }
            HtmlNodeCollection _hnc_update_time = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:update_time')]");
            //获取最后更新时间
            _tfi_ret.col_update_time = DateTime.Parse(_hnc_update_time[0].Attributes["content"].Value);       

            //获取最后更新章节及链接
            HtmlNodeCollection _hnc_Chapter = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:latest_chapter_name')]");
            _tfi_ret.col_update_chapter = _hnc_Chapter[0].Attributes["content"].Value;
            HtmlNodeCollection _hnc_Chapter_Url = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:latest_chapter_url')]");
            _tfi_ret.col_update_chapter_url = _hnc_Chapter_Url[0].Attributes["content"].Value;

            //获取小说简介
            HtmlNodeCollection _hnc_Intro = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'maininfo')]/div[starts-with(@id,'intro')]/p");
            _hnc_Intro.RemoveAt(1);
            if (_hnc_Intro.Count > 0)
            {
                _tfi_ret.col_fiction_introduction = _hnc_Intro[0].InnerText.Trim();
            }

            //小说封皮链接
            HtmlNodeCollection _hnc_Poster_URL = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'fmimg')]/img");
            if (_hnc_Poster_URL.Count > 0)
            {
                _tfi_ret.col_url_poster = "https://www.biquzhh.com" + _hnc_Poster_URL[0].Attributes["src"].Value;
            }//https://www.biquzhh.com/files/article/image/40047/40047833/40047833s.jpg


            //设置来源
            _tfi_ret.col_fiction_source = "笔趣阁";

            return _tfi_ret;
        }

        public List<chapter_list> _o_Get_Chapter_Content()
        {
            //判断链接是否存在
            if (_url_Homepage == "")
                return null;
            List<chapter_list> _ltfi_ret = new List<chapter_list>();
            fiction_info _tfi_ret = new fiction_info();

            HtmlWeb _web_Main = new HtmlWeb();
            _web_Main.OverrideEncoding = Encoding.GetEncoding("gb2312");
            try
            {
                HtmlAgilityPack.HtmlDocument _doc_Main = new HtmlAgilityPack.HtmlDocument();
                _doc_Main = _web_Main.Load(_url_Homepage);
                //判断是否有数据
                if (_doc_Main.Text == "")
                    return null;

                try
                { 
                //获取小说章节信息
                //获取查询列表列表名,把查询列表展示在UI
                HtmlNodeCollection _hnc_Content_List = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@class,'listmain')]/dl/dd");
                    //查询列表第一项为表头，所有查询项数据需要大于1
                    for (int i = 0; i < 12; i++)
                        _hnc_Content_List.RemoveAt(0);

                    foreach (HtmlNode _hn in _hnc_Content_List)
                {
                    HtmlAgilityPack.HtmlDocument _doc_One = new HtmlAgilityPack.HtmlDocument();
                    _doc_One.LoadHtml(_hn.InnerHtml);
                    chapter_list _tfi = new chapter_list();
                    //获取章节名称及章节链接
                    HtmlNodeCollection _hnc_Fiction_Name_URL = _doc_One.DocumentNode.SelectNodes("//a");
                    if (_hnc_Fiction_Name_URL != null && _hnc_Fiction_Name_URL.Count > 0)
                    {
                        _tfi.col_chapter_name = _hnc_Fiction_Name_URL[0].InnerText.Trim();
                        _tfi.col_chapter_url = _hnc_Fiction_Name_URL[0].Attributes["href"].Value;
                    }
                    _ltfi_ret.Add(_tfi);
                }
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

            return _ltfi_ret;
        }


        public  Tuple<fiction_info,List<chapter_list>> TupleDetail()
        {


            //加载网页，爬取章节内容
            //判断链接是否存在
            if (_url_Homepage == "")
                return null;
            List<chapter_list> _ltfi_ret = new List<chapter_list>();
            fiction_info _tfi_ret = new fiction_info();

            HtmlWeb _web_Main = new HtmlWeb();
            _web_Main.OverrideEncoding = Encoding.GetEncoding("gb2312");

            //加载章节信息以及小说信息
            try
            {
                HtmlAgilityPack.HtmlDocument _doc_Main = new HtmlAgilityPack.HtmlDocument();
                _doc_Main = _web_Main.Load(_url_Homepage);
                //判断是否有数据
                if (_doc_Main.Text == "")
                    return null;

                try
                {
                 //获取小说章节信息
                    HtmlNodeCollection _hnc_Content_List = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@class,'listmain')]/dl/dd");
                    //查询列表第一项为表头，所有查询项数据需要大于1
                    for (int i = 0; i < 12; i++)
                        _hnc_Content_List.RemoveAt(0);

                    foreach (HtmlNode _hn in _hnc_Content_List)
                    {
                        HtmlAgilityPack.HtmlDocument _doc_One = new HtmlAgilityPack.HtmlDocument();
                        _doc_One.LoadHtml(_hn.InnerHtml);
                        chapter_list _tfi = new chapter_list();
                        //获取章节名称及章节链接
                        HtmlNodeCollection _hnc_Fiction_Name_URL = _doc_One.DocumentNode.SelectNodes("//a");
                        if (_hnc_Fiction_Name_URL != null && _hnc_Fiction_Name_URL.Count > 0)
                        {
                            _tfi.col_chapter_name = _hnc_Fiction_Name_URL[0].InnerText.Trim();
                            _tfi.col_chapter_url = _hnc_Fiction_Name_URL[0].Attributes["href"].Value;
                        }
                        _ltfi_ret.Add(_tfi);
                    }

                //获取小说信息
                    //获取小说名
                    HtmlNodeCollection _hnc_Title = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'info')]/h1");
                    if (_hnc_Title.Count == 0)
                        return null;

                    _tfi_ret.col_fiction_name = _hnc_Title[0].InnerText;

                    //小说主页链接

                    _tfi_ret.col_url_homepage = _url_Homepage;

                    //获取小说信息
                    HtmlNodeCollection _hnc_Info = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'info')]");//"//div[starts-with(@id,'maininfo')]/div[starts-with(@id,'info')]"
                    if (_hnc_Info.Count == 0)
                        return null;
                    //获取小说作者
                    var _rg_Author = Regex.Matches(_hnc_Info[0].InnerHtml.Replace("&nbsp;", ""), @">作者：([^<]+)<");
                    if (_rg_Author.Count > 0)
                    {
                        _tfi_ret.col_fiction_author = _rg_Author[0].Value.Replace(">作者：", "")
                            .Replace("<", "");
                    }
                    HtmlNodeCollection _hnc_update_time = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:update_time')]");
                    //获取最后更新时间
                    _tfi_ret.col_update_time = DateTime.Parse(_hnc_update_time[0].Attributes["content"].Value);

                    HtmlNodeCollection _hnc_fiction_type = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:category')]");
                    //获取小说类型
                    _tfi_ret.col_fiction_type = _hnc_fiction_type[0].Attributes["content"].Value;

                    HtmlNodeCollection _hnc_fiction_stata = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:status')]");
                    //获取小说状态
                    _tfi_ret.col_fiction_stata = _hnc_fiction_stata[0].Attributes["content"].Value;


                    //获取最后更新章节及链接
                    HtmlNodeCollection _hnc_Chapter = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:latest_chapter_name')]");
                    _tfi_ret.col_update_chapter = _hnc_Chapter[0].Attributes["content"].Value;
                    HtmlNodeCollection _hnc_Chapter_Url = _doc_Main.DocumentNode.SelectNodes("//meta[starts-with(@property,'og:novel:latest_chapter_url')]");
                    _tfi_ret.col_update_chapter_url = _hnc_Chapter_Url[0].Attributes["content"].Value;

                    //获取小说简介
                    HtmlNodeCollection _hnc_Intro = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'maininfo')]/div[starts-with(@id,'intro')]/p");
                    _hnc_Intro.RemoveAt(1);
                    if (_hnc_Intro.Count > 0)
                    {
                        _tfi_ret.col_fiction_introduction = _hnc_Intro[0].InnerText.Trim();
                    }

                    //小说封皮链接
                    HtmlNodeCollection _hnc_Poster_URL = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@id,'fmimg')]/img");
                    if (_hnc_Poster_URL.Count > 0)
                    {
                        _tfi_ret.col_url_poster = "https://www.biquzhh.com" + _hnc_Poster_URL[0].Attributes["src"].Value;
                    }//https://www.biquzhh.com/files/article/image/40047/40047833/40047833s.jpg


                    //设置来源
                    _tfi_ret.col_fiction_source = "笔趣阁";

                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

            //将两类信息放在一个元祖上
            Tuple<fiction_info, List<chapter_list>> tup = new Tuple<fiction_info, List<chapter_list>>(_tfi_ret, _ltfi_ret);
            return tup;
        }
        public List<chapter_list> _o_Get_Chapter_Content(string _url)
        {
            _url_Homepage = _url;
            return _o_Get_Chapter_Content();
        }

        public fiction_info _o_Get_Fiction_Detail(string _url)
        {
            _url_Homepage = _url;
            return _o_Get_Fiction_Detail();
        }

        public Tuple<fiction_info, List<chapter_list>> TupleDetail(string _url)
        {
            _url_Homepage = _url;
            return TupleDetail();
        }
    }
}
