using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using HtmlAgilityPack;
using SearchTool.Data;

namespace SearchTool.Service
{
    public class Iciba : ISearch
    {
        public async Task<Result> GetResult(string keyword)
        {
            try
            {
                var html = await $"http://www.iciba.com/{keyword}".WithTimeout(TimeSpan.FromSeconds(10)).GetStringAsync();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                Result res = new Result { Keyword = keyword, Meanings = new List<Meaning>() };

                HtmlNode contentNode = doc.DocumentNode.SelectSingleNode("//div[@class='in-base-top clearfix']");

                if (contentNode == null) return new Result();

                HtmlNode wordNode = contentNode.SelectSingleNode("h1");
                HtmlNode pronounceNode = contentNode.SelectSingleNode("//div[@class='base-speak']");
                HtmlNodeCollection contentNodes = contentNode.SelectNodes("//li[@class='clearfix']");


                StringBuilder content = new StringBuilder();
                if(contentNodes != null)
                {
                    foreach (var item in contentNodes)
                    {
                        content.AppendLine(new System.Text.RegularExpressions.Regex("[\\s]+").Replace(item.InnerText, " ").Replace("\n", " "));
                    }
                }

                res.Meanings.Add(
                    new Meaning
                    {
                        Info = "词霸",
                        Word = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(wordNode.InnerText, " ").Replace("\n", " ").Trim(),
                        Pronounce = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(pronounceNode.InnerText, " ").Replace("\n", " ").Trim(),
                        Content = content.ToString()
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
