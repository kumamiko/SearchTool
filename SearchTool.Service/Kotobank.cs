using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using SearchTool.Service.Models;
using Flurl.Http;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace SearchTool.Service
{
    public class Kotobank : ISearch
    {
        public async Task<Result> GetResult(string keyword)
        {
            try
            {
                var rep = await $"https://kotobank.jp/word/{WebUtility.UrlEncode(keyword)}".GetAsync();
                var location = rep.RequestMessage.RequestUri;

                var html = await location.ToString().GetStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                Result res = new Result { Keyword = keyword, Meanings = new List<Meaning>() };

                HtmlNode TitleNode = doc.DocumentNode.SelectSingleNode("/html/body//div[@id='mainTitle']");
                HtmlNode ArticleNode = doc.DocumentNode.SelectSingleNode("/html/body//article[@class='dictype cf']");

                var title1 = TitleNode?.SelectSingleNode("h1[@class='grid02 grid1 left']")?.InnerHtml ?? string.Empty;
                var title2 = TitleNode?.SelectSingleNode("//span")?.InnerHtml ?? string.Empty;

                res.Word = $"{title1} {title2}";

                HtmlNodeCollection MeaningNodes = ArticleNode?.SelectNodes("div[@class='ex cf']");

                foreach (var item in MeaningNodes)
                {
                    var head = item.SelectSingleNode("h3") ;
                    var body = item.SelectSingleNode("section");
                    if(head != null && body != null)
                    {
                        var headafterstring = head.InnerHtml.Replace("<br>", "  ");
                        HtmlDocument headdocafter = new HtmlDocument();
                        headdocafter.LoadHtml(headafterstring);
                        var headafter = headdocafter.DocumentNode;

                        var bodyafterstring = body.InnerHtml.Replace("\n", string.Empty).Trim(' ').Replace("<br>", Environment.NewLine);
                        HtmlDocument bodydocafter = new HtmlDocument();
                        bodydocafter.LoadHtml(bodyafterstring);
                        var bodyafter = bodydocafter.DocumentNode;

                        res.Meanings.Add(new Meaning { Head = headafter.InnerText, Body = bodyafter.InnerText});
                    }
                }

                return res;
            }
            catch (Exception)
            {
                return new Result();
            }
        }
    }

    public static class StringEx
    {
        public static string Utf8ToUnicode(this string srcString)
        {
            byte[] arraydefalut = Encoding.UTF8.GetBytes(srcString);
            byte[] arrayunicode = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, arraydefalut);

            return Encoding.Unicode.GetString(arrayunicode);
        }
    }
}

