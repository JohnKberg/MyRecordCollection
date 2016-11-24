using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyRecordCollection
{
    public static class MyHtmlHelper
    {
        public static IHtmlString DisplayLargeText(string text, int? maxLength = null, string ellipsis = "...")
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new HtmlString(string.Empty);
            }

            string htmlString;

            if (maxLength.HasValue && maxLength.Value > 0)
            {
                if (text.Length <= maxLength.Value)
                {
                    htmlString = text;
                }
                else
                {
                    htmlString = text.Substring(0, maxLength.Value - ellipsis.Length);
                    var lastWordPosition = htmlString.LastIndexOf(' ');

                    if (lastWordPosition < 0)
                    {
                        lastWordPosition = 0;
                    }
                    htmlString = htmlString.Substring(0, lastWordPosition).Trim(new[] { '.', ',', '!', '?' }) + ellipsis;
                } 
            }
            else
            {
                htmlString = text;
            }

            htmlString = htmlString.Replace("\n", "<br />");

            return new HtmlString(htmlString);
        }
    }
}
