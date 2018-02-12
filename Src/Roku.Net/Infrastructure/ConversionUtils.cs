using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace yozepi.Roku.Infrastructure
{
    internal static class ConversionUtils
    {
        internal const string MOVIE_QUERYVALUE = "movie";
        internal const string PERSON_QUERYVALUE = "person";
        internal const string CHANNEL_QUERYVALUE = "channel";
        internal const string GAME_QUERYVALUE = "game";
        internal const string TVSHOW_QUERYVALUE = "tv-show";

        public static T XmlToInstance<T>(Stream xmlStream) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            var result = serializer.Deserialize(xmlStream) as T;
            return result;
        }

        public static string SearchTypeToQuerystringValue(SearchType searchType)
        {
            string typeString = null;
            switch (searchType)
            {
                case Roku.SearchType.Movie:
                    typeString = MOVIE_QUERYVALUE;
                    break;
                case Roku.SearchType.Person:
                    typeString = PERSON_QUERYVALUE;
                    break;
                case Roku.SearchType.Channel:
                    typeString = CHANNEL_QUERYVALUE;
                    break;
                case Roku.SearchType.Game:
                    typeString = GAME_QUERYVALUE;
                    break;
                case Roku.SearchType.TVShow:
                    typeString = TVSHOW_QUERYVALUE;
                    break;
                default:
                    break;
            }
            return typeString;
        }

        public static string ToRouteValue(this CommandKeys key)
        {
            return Enum.GetName(typeof(CommandKeys), key);
        }

        public static string ToRouteValue(this char key)
        {
            return Uri.EscapeDataString($"Lit_{key}");
        }
    }
}
