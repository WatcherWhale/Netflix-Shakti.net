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
        private List<StringVar> stringVars = new List<StringVar>();
        private List<ImageVar> imageVars = new List<ImageVar>();
        private List<SelectionVar> selectionVars = new List<SelectionVar>();
        private List<MultipleSelectionVar> multipleSelectionVars = new List<MultipleSelectionVar>();

        #region AddVars

        public void AddStringVar(SearchType type, SearchName id, string varName, bool summary = false)
        {
            StringVar var = new StringVar()
            {
                Type = type.ToString().ToLower(),
                Id = id,
                Request = varName,
                Summary = summary
            };

            stringVars.Add(var);
        }

        public void AddImageVar(SearchType type, SearchName id, string image, string dimensions, string imageType = "webp", bool summary = false)
        {
            string[] request = new string[] { image, dimensions, imageType };
            ImageVar var = new ImageVar()
            {
                Type = type.ToString().ToLower(),
                Id = id,
                Request = request,
                Summary = summary
            };
            imageVars.Add(var);
        }

        public void AddSelectionRequest(SearchType type, SearchName id, string varName, int from, int to, bool summary = false, params string[] requestVars)
        {
            Dictionary<string, int> selection = new Dictionary<string, int>(2);
            selection.Add("from", from);
            selection.Add("to", to);

            SelectionVar var = new SelectionVar()
            {
                Type = type.ToString().ToLower(),
                Id = id,
                Request = varName,
                Selection = selection,
                RequestVars = requestVars,
                Summary = summary
            };
            selectionVars.Add(var);
        }

        public void AddMultipleSelectionRequest(SearchType type, SearchName id, string[] varNames, int from, int to, bool summary = false, params string[] requestVars)
        {
            Dictionary<string, int> selection = new Dictionary<string, int>(2);
            selection.Add("from", from);
            selection.Add("to", to);

            MultipleSelectionVar var = new MultipleSelectionVar()
            {
                Type = type.ToString().ToLower(),
                Id = id,
                Request = varNames,
                Selection = selection,
                RequestVars = requestVars,
                Summary = summary
            };
            multipleSelectionVars.Add(var);
        }

        #endregion

        internal Json.Search.SearchRequest Build()
        {
            var request = new Json.Search.SearchRequest();

            foreach (var sVar in stringVars)
            {
                string summary = null;
                if(sVar.Summary)
                    summary = "summary";

                if(sVar.Id.ID != 0 || sVar.Id.ID == null)
                    request.AddVar(sVar.Type,sVar.Id.ID,sVar.Request,summary);
                if (!String.IsNullOrEmpty(sVar.Id.NAME))
                    request.AddVar(sVar.Type, sVar.Id.NAME, sVar.Request, summary);
            }

            foreach (var iVar in imageVars)
            {
                string summary = null;
                if (iVar.Summary)
                    summary = "summary";

                if (iVar.Id.ID != 0 || iVar.Id.ID == null)
                    request.AddVar(iVar.Type, iVar.Id.ID, iVar.Request[0], iVar.Request[1], iVar.Request[2], summary);
                if (!String.IsNullOrEmpty(iVar.Id.NAME))
                    request.AddVar(iVar.Type, iVar.Id.NAME, iVar.Request[0], iVar.Request[1], iVar.Request[2], summary);
            }

            foreach (var sVar in selectionVars)
            {
                string summary = null;
                if (sVar.Summary)
                    summary = "summary";

                if (sVar.Id.ID != 0 || sVar.Id.ID == null)
                    request.AddVar(sVar.Type, sVar.Id.ID, sVar.Request, sVar.Selection, sVar.RequestVars, summary);
                if (!String.IsNullOrEmpty(sVar.Id.NAME))
                    request.AddVar(sVar.Type, sVar.Id.NAME, sVar.Request, sVar.Selection, sVar.RequestVars, summary);
            }

            foreach (var sVar in multipleSelectionVars)
            {
                string summary = null;
                if (sVar.Summary)
                    summary = "summary";

                if (sVar.Id.ID != 0 || sVar.Id.ID == null)
                    request.AddVar(sVar.Type, sVar.Id.ID, sVar.Request, sVar.Selection, sVar.RequestVars, summary);
                if (!String.IsNullOrEmpty(sVar.Id.NAME))
                    request.AddVar(sVar.Type, sVar.Id.NAME, sVar.Request, sVar.Selection, sVar.RequestVars, summary);
            }

            return request;
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

    #region VarClasses

    public class StringVar
    {
        public string Type { get; set; }
        public SearchName Id { get; set; }
        public string Request { get; set; }
        public bool Summary { get; set; }
    }

    public class ImageVar
    {
        public string Type { get; set; }
        public SearchName Id { get; set; }
        public string[] Request { get; set; }
        public bool Summary { get; set; }
    }

    public class SelectionVar 
    {
        public string Type { get; set; }
        public SearchName Id { get; set; }
        public string Request { get; set; }
        public Dictionary<string, int> Selection;
        public string[] RequestVars;
        public bool Summary { get; set; }
    }

    public class MultipleSelectionVar
    {
        public string Type { get; set; }
        public SearchName Id { get; set; }
        public string[] Request { get; set; }
        public Dictionary<string, int> Selection;
        public string[] RequestVars;
        public bool Summary { get; set; }
    }

    public class SearchName
    {
        internal long? ID = null;
        internal string NAME = null;

        public SearchName(int id)
        {
            ID = id;
        }

        public SearchName (string name)
        {
            NAME = name;
        }
    }

    #endregion

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
            vars.Add(values);
        }
    }
}
