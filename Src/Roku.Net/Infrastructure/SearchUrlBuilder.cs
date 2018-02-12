using System;
using System.Collections.Generic;
using System.Text;

namespace yozepi.Roku.Infrastructure
{
    internal class SearchUrlBuilder
    {
        public string Keyword { get; set; }
        public SearchType? SearchType { get; set; }
        public int? Season { get; set; }
        public int? AppId { get; set; }
        public bool Launch { get; set; }

        public string BuildSearchUrlFor(Uri rokuUri)
        {
            
            if (string.IsNullOrWhiteSpace(Keyword))
                throw new ArgumentNullException("keyword");

            if (Season != null && Season.Value < 0)
                throw new ArgumentOutOfRangeException("season", Season, "Season must be a positive value when provided.");

            var uri = rokuUri.ToString() + "search/browse?keyword=" + Uri.EscapeDataString(Keyword);

            if (SearchType != null)
            {
                string typeString = ConversionUtils.SearchTypeToQuerystringValue(SearchType.Value);
                uri += $"&type={typeString}";
                if(SearchType.Value == Roku.SearchType.TVShow)
                {
                    if (Season != null)
                    {
                        uri += $"&season={Season.Value}";
                    }
                }
            }

            if (AppId != null)
            {
                uri += $"&provider-id={AppId.Value}";
                if (Launch)
                {
                    uri += "&launch=true";
                }
            }
            return uri;
        }
    }
}
