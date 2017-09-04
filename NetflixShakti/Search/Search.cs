using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.Search
{
    public class SearchRequest
    {
        internal Json.Search.SearchRequest request = new Json.Search.SearchRequest();

        #region AddVars

        public void AddStringVar(SearchType type, SearchName id, string varName, bool summary = false)
        {
            string summ = null;
            if (summary)
                summ = "summary";

            if (id.ID != 0 || id.ID == null)
                request.AddVar(type, id.ID, varName, summ);
            if (!String.IsNullOrEmpty(id.NAME))
                request.AddVar(type, id.NAME, varName, summ);
        }

        public void AddMultiStringVar(SearchType type, SearchName id, string[] varNames, bool summary = false)
        {
            string summ = null;
            if (summary)
                summ = "summary";

            if (id.ID != 0 || id.ID == null)
                request.AddVar(type, id.ID, varNames, summ);
            if (!String.IsNullOrEmpty(id.NAME))
                request.AddVar(type, id.NAME, varNames, summ);
        }

        public void AddImageVar(SearchType type, SearchName id, string image, string dimensions, string imageType = "webp", bool summary = false)
        {
            string[] reqStrings = new string[] { image, dimensions, imageType };

            string summ = null;
            if (summary)
                summ = "summary";

            if (id.ID != 0 || id.ID == null)
                request.AddVar(type, id.ID, reqStrings[0], reqStrings[1], reqStrings[2], summ);
            if (!String.IsNullOrEmpty(id.NAME))
                request.AddVar(type, id.NAME, reqStrings[0], reqStrings[1], reqStrings[2], summ);
        }

        public void AddSelectionVar(SearchType type, SearchName id, string varName, int from, int to, bool summary = false, params string[] requestVars)
        {
            Dictionary<string, int> selection = new Dictionary<string, int>(2)
            {
                { "from", from },
                { "to", to }
            };

            string summ = null;
            if (summary)
                summ = "summary";

            if (id.ID != 0 || id.ID == null)
                request.AddVar(type, id.ID, varName, selection, requestVars, summ);
            if (!String.IsNullOrEmpty(id.NAME))
                request.AddVar(type, id.NAME, varName, selection, requestVars, summ);
        }

        public void AddMultiSelectionVar(SearchType type, SearchName id, string[] varNames, int from, int to, bool summary = false, params string[] requestVars)
        {
            Dictionary<string, int> selection = new Dictionary<string, int>(2);
            selection.Add("from", from);
            selection.Add("to", to);

            string summ = null;
            if (summary)
                summ = "summary";

            if (id.ID != 0 || id.ID == null)
                request.AddVar(type, id.ID, varNames, selection, requestVars, summ);
            if (!String.IsNullOrEmpty(id.NAME))
                request.AddVar(type, id.NAME, varNames, selection, requestVars, summ);
        }

        #endregion

        public void RemoveVar(int index)
        {
            request.RemoveVar(index);
        }

        #region Simple Search

        public static SimpleSearch GetSimpleVideoSearch(string video)
        {
            SimpleSearch search = new SimpleSearch()
            {
                Json = ApiVars.SearchJson.Replace("{SEARCH QUERY}", video)
            };
            return search;
        }

        public static SimpleSearch GetSimpleVideoInfo(int video)
        {
            SimpleSearch search = new SimpleSearch()
            {
                Json = ApiVars.VideoInfoJson.Replace("{MOVIE ID}", video.ToString())
            };
            return search;
        }
        #endregion
    }

    public enum SearchType { Search,Videos,Person};

    public class SearchName
    {
        internal long? ID = null;
        internal string NAME = null;

        public SearchName(long id)
        {
            ID = id;
        }

        public SearchName(string name)
        {
            NAME = name;
        }
    }

    public class SimpleSearch
    {
        internal string Json { get; set; }
    }
}

namespace NetflixShakti.Json.Search
{
    using Newtonsoft.Json;

    public class SearchRequest
    {
        [JsonProperty(PropertyName = "paths")]
        List<object[]> vars = new List<object[]>();

        public void AddVar(params object[] values)
        {
            if (values[values.Length - 1] == null)
            {
                object[] array = new object[values.Length-1];

                for (int i = 0; i < array.Length -1; i++)
                {
                    array[i] = values[i];
                }
                values = array;
            }

            vars.Add(values);
        }

        public void RemoveVar(int index)
        {
            vars.RemoveAt(index);
        }
    }
}
