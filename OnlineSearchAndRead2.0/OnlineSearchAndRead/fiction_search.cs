using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSearchAndRead
{
    public class fiction_search
    {
        const string _url_Search = "https://so.biqusoso.com/s.php?ie=utf-8&siteid=zanghaihuatxt.com&q=";//https://sou.xanbhx.com/search?siteid=qula&q=
        string _str_KeyWord = "";
        public fiction_search() { }
        public fiction_search(string _keyword)
        {
            _str_KeyWord = _keyword;
        }

        /// <summary>
        /// 根据关键词获取小说列表
        /// </summary>
        /// <returns></returns>
        public List<fiction_info> _o_Get_Fiction_Info_By_KeyWord()
        {
            //判断关键字
            if (_str_KeyWord == "")
                return null;

            List<fiction_info> _ltfi_ret = new List<fiction_info>();

            HtmlWeb _web_Main = new HtmlWeb();
            _web_Main.OverrideEncoding = Encoding.UTF8;
            try
            {
                HtmlAgilityPack.HtmlDocument _doc_Main = new HtmlAgilityPack.HtmlDocument();
                _doc_Main = _web_Main.Load(_url_Search + _str_KeyWord);
                //判断是否有数据
                if (_doc_Main.Text == "")
                    return null;

                //获取查询列表列表名,把查询列表展示在UI
                HtmlNodeCollection _hnc_Search_List = _doc_Main.DocumentNode.SelectNodes("//div[starts-with(@class,'search-list')]/ul/li");
                //查询列表第一项为表头，所有查询项数据需要大于1
                if (_hnc_Search_List.Count == 1)
                    return null;
                //移除表头,即列表名操作每一个元祖
                _hnc_Search_List.RemoveAt(0);

                foreach (HtmlNode _hn in _hnc_Search_List)
                {
                    HtmlAgilityPack.HtmlDocument _doc_One = new HtmlAgilityPack.HtmlDocument();
                    _doc_One.LoadHtml(_hn.InnerHtml);
                    fiction_info _tfi = new fiction_info();
                    //获取小说序号
                    HtmlNodeCollection _hnc_Fiction_Number = _doc_One.DocumentNode.SelectNodes("//span[starts-with(@class,'s1')]");
                    if (_hnc_Fiction_Number != null && _hnc_Fiction_Number.Count > 0)
                    {
                        _tfi.col_fiction_id = _hnc_Fiction_Number[0].InnerText.Replace("[", "").Replace("]", "");
                    }
                    //获取小说名称及主页链接
                    HtmlNodeCollection _hnc_Fiction_Name_URL = _doc_One.DocumentNode.SelectNodes("//span[starts-with(@class,'s2')]/a");
                    if (_hnc_Fiction_Name_URL != null && _hnc_Fiction_Name_URL.Count > 0)
                    {
                        _tfi.col_fiction_name = _hnc_Fiction_Name_URL[0].InnerText.Trim();
                        _tfi.col_url_homepage = _hnc_Fiction_Name_URL[0].Attributes["href"].Value;
                    }
                    //获取最新章节及链接
                    HtmlNodeCollection _hnc_Update_Chapter_URL = _doc_One.DocumentNode.SelectNodes("//span[starts-with(@class,'s3')]/a");
                    if (_hnc_Update_Chapter_URL != null && _hnc_Update_Chapter_URL.Count > 0)
                    {
                        _tfi.col_update_chapter = _hnc_Update_Chapter_URL[0].InnerText;
                        _tfi.col_update_chapter_url = _hnc_Update_Chapter_URL[0].Attributes["href"].Value;
                        //后面会用到链接，点击获得得到对应的文章
                        
                    }
                    //获取小说作者
                    HtmlNodeCollection _hnc_Fiction_Author = _doc_One.DocumentNode.SelectNodes("//span[starts-with(@class,'s4')]");
                    if (_hnc_Fiction_Author != null && _hnc_Fiction_Author.Count > 0)
                    {
                        _tfi.col_fiction_author = _hnc_Fiction_Author[0].InnerText;
                    }
                    //获取点击数
                    HtmlNodeCollection _hnc_Click_Count = _doc_One.DocumentNode.SelectNodes("//span[starts-with(@class,'s5')]");
                    if (_hnc_Click_Count != null && _hnc_Click_Count.Count > 0)
                    {
                        _tfi.col_click_count = _hnc_Click_Count[0].InnerText;
                    }
                    //获取更新时间
                    HtmlNodeCollection _hnc_Update_Time = _doc_One.DocumentNode.SelectNodes("//span[starts-with(@class,'s6')]");
                    if (_hnc_Update_Time != null && _hnc_Update_Time.Count > 0)
                    {
                        _tfi.col_update_time = DateTime.Parse(_hnc_Update_Time[0].InnerText);
                    }
                    //获取小说状态
                    HtmlNodeCollection _hnc_Fiction_Stata = _doc_One.DocumentNode.SelectNodes("//span[starts-with(@class,'s7')]");
                    if (_hnc_Fiction_Stata != null && _hnc_Fiction_Stata.Count > 0)
                    {
                        _tfi.col_fiction_stata = _hnc_Fiction_Stata[0].InnerText;
                    }
                    _tfi.col_fiction_source = "笔趣阁";

                    if(_tfi.col_fiction_stata !="连载"&& _tfi.col_fiction_stata != "完成")
                        _ltfi_ret.Add(_tfi);
                }
                return _ltfi_ret;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 根据关键词获取小说列表
        /// </summary>
        /// <param name="_s_kw">关键词</param>
        /// <returns></returns>
        public List<fiction_info> _o_Get_Fiction_Info_By_KeyWord(string _s_kw)
        {
            _str_KeyWord = _s_kw;
            return _o_Get_Fiction_Info_By_KeyWord();
        }


      
    }

}
