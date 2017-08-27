using System;
using System.Collections.Generic;
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

        public void AddStringVar(SearchType type, long id, string varName, bool summary = false)
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

        public void AddImageVar(SearchType type, long id, string image, string dimensions, string imageType = "webp", bool summary = false)
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

        public void AddSelectiveRequest(SearchType type, long id, string varName, int from, int to, bool summary = false, params string[] requestVars)
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

        internal Json.Search.SearchRequest Build()
        {
            var request = new Json.Search.SearchRequest();

            foreach (var sVar in stringVars)
            {
                string summary = null;
                if(sVar.Summary)
                    summary = "summary";

                request.AddVar(sVar.Type,sVar.Id,sVar.Request,summary);
            }

            foreach (var iVar in imageVars)
            {
                string summary = null;
                if (iVar.Summary)
                    summary = "summary";


                request.AddVar(iVar.Type, iVar.Id, iVar.Request[0], iVar.Request[1], iVar.Request[2], summary);
            }

            foreach (var sVar in selectionVars)
            {
                string summary = null;
                if (sVar.Summary)
                    summary = "summary";

                request.AddVar(sVar.Type, sVar.Id, sVar.Request, sVar.Selection, sVar.RequestVars, summary);
            }

            return request;
        }
    } 

    public enum SearchType { Videos,Person};

    public class StringVar
    {
        public string Type { get; set; }
        public long Id { get; set; }
        public string Request { get; set; }
        public bool Summary { get; set; }
    }

    public class ImageVar
    {
        public string Type { get; set; }
        public long Id { get; set; }
        public string[] Request { get; set; }
        public bool Summary { get; set; }
    }

    public class SelectionVar 
    {
        public string Type { get; set; }
        public long Id { get; set; }
        public string Request { get; set; }
        public Dictionary<string, int> Selection;
        public string[] RequestVars;
        public bool Summary { get; set; }
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
