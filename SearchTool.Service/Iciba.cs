using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;
using SearchTool.Service.Models;

namespace SearchTool.Service
{
    public class Iciba : ISearch
    {
        public async Task<Result> GetResult(string keyword)
        {
            try
            {
                var html = await $"http://www.iciba.com/{keyword}".GetStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                Result res = new Result { Keyword = keyword, Meanings = new List<Meaning>() };

                HtmlNode contentNode = doc.DocumentNode.SelectSingleNode("/html/body//div[@class='in-base']");

                HtmlNode headNode = contentNode.SelectSingleNode("div[@class='in-base-top clearfix']");
                HtmlNodeCollection bodyNodes = contentNode.SelectNodes("//li[@class='clearfix']");


                StringBuilder body = new StringBuilder();
                foreach (var item in bodyNodes)
                {
                    body.AppendLine(new System.Text.RegularExpressions.Regex("[\\s]+").Replace(item.InnerText, " ").Replace("\n", " "));
                }

                res.Meanings.Add(
                    new Meaning
                    {
                        Head = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(headNode.InnerText, " ").Replace("\n", " ").Trim(),
                        Body = body.ToString()
                    }
                );

                return res;
            }
            catch
            {
                return new Result();
            }
        }
    }
}
