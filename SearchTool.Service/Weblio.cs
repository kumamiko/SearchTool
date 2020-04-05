using Flurl.Http;
using HtmlAgilityPack;
using SearchTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchTool.Service
{
    public class Weblio : ISearch
    {
        public async Task<Result> GetResult(string keyword)
        {
            try
            {
                var html = await $"https://www.weblio.jp/content/{keyword}".WithTimeout(TimeSpan.FromSeconds(10)).GetStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                Result res = new Result { Keyword = keyword, Meanings = new List<Meaning>() };

                HtmlNodeCollection pbarT_items = doc.DocumentNode.SelectNodes("//div[@class='pbarT']");
                HtmlNodeCollection kijiWrp_items = doc.DocumentNode.SelectNodes("//div[@class='kijiWrp']");

                if(pbarT_items != null)
                {
                    for (int i = 0; i < pbarT_items.Count; i++)
                    {
                        string info = string.Empty;

                        HtmlNode infoNode = pbarT_items[i].SelectSingleNode("div[@class='pbarTLW']/div[@class='pbarTL']");
                        if (infoNode != null) info = infoNode.InnerText;

                        HtmlNodeCollection NetDicHead_items = kijiWrp_items[i].SelectNodes("div[@class='kiji']/div[@class='NetDicHead']");
                        HtmlNodeCollection NetDicBody_items = kijiWrp_items[i].SelectNodes("div[@class='kiji']/div[@class='NetDicBody']");

                        if (NetDicHead_items == null || NetDicBody_items == null)
                        {
                            HtmlNode kijiNode = kijiWrp_items[i].SelectSingleNode("div[@class='kiji']");

                            if (kijiNode == null) continue;
                            HtmlNode node_1 = kijiNode.SelectSingleNode("h2[@class='midashigo']");
                            HtmlNode node_2 = kijiNode.SelectSingleNode("div[1]");

                            if (node_1 == null || node_2 == null) continue;

                            res.Meanings.Add(new Meaning
                            {
                                Word = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(node_1.InnerText, " ").Trim(),
                                Content = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(node_2.InnerText, " ").Trim(),
                                Pronounce = string.Empty,
                                Info = info
                            });
                        }
                        else
                        {
                            for (int j = 0; j < NetDicHead_items.Count; j++)
                            {
                                HtmlNode wordNode = NetDicHead_items[j].SelectSingleNode(".");
                                HtmlNode contentNode = NetDicBody_items[j].SelectSingleNode(".");

                                if (wordNode == null || contentNode == null) continue;

                                res.Meanings.Add(new Meaning {
                                    Word = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(wordNode.InnerText, " ").Trim(),
                                    Content = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(contentNode.InnerText, " ").Trim(),
                                    Pronounce = string.Empty,
                                    Info = info
                                });
                            }
                        }

                    }
                }
                

                return res;
            }
            catch
            {
                return new Result();
            }
        }
    }
}
