using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guitarist_Helper
{
    internal class WebScraper
    {
#pragma warning disable CS8603 // Possible null refence return.
        public string GetLink(string html)
        {
            //Load html for html agility pack
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            try
            {
                return htmlDoc.DocumentNode.SelectNodes("//ul/li/a").First().Attributes["href"].Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
