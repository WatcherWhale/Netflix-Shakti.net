using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetflixShakti.Search;
using NetflixShakti.PathEval;
using NetflixShakti.Json.PathEval;
using Newtonsoft.Json;

namespace NetflixShakti.Search
{
    public interface ISearchRequest
    {
        string Build();
    }


    public class SearchRequest : ISearchRequest
    {
        List<Path> pathList = new List<Path>();
        public string Term { get; private set; }
        public string SearchFor { get; private set; }


        public SearchRequest(string term, string searchFor)
        {
            Term = term;
            SearchFor = searchFor;
        }

        public string Build()
        {
            List<Path> paths = new List<Path>();

            Path path = new Path(PathType.Search, "byTerm", Term, SearchFor, 48);
            path.AddAttribute("referenceId", "id", "length", "name", "trackIds", "requestId");

            var pathsobj = Path.BuildPathList(paths);
            PathEvalRequest req = new PathEvalRequest(pathsobj);

            var json = JsonConvert.SerializeObject(req);

            return json;
        }
    }
}
