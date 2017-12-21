using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetflixShakti.Search.Json;

namespace NetflixShakti.Search
{
    public interface ISearchRequest
    {
        string Build();
    }

    public class ByTermSearchRequest : ISearchRequest
    {
        private const string searchJson = "{\"paths\":[[\"search\",\"byTerm\",\"{SEARCHQUERY}\",\"titles\",48,{\"from\":0,\"to\":48},\"reference\",[\"summary\",\"title\",\"userRating\"]],[\"search\",\"byTerm\",\"{SEARCHQUERY}\",\"titles\",48,{\"from\":0,\"to\":48},\"reference\",\"boxarts\",\"_342x192\",\"jpg\"],[\"search\",\"byTerm\",\"{SEARCHQUERY}\",\"titles\",48,{\"from\":0,\"to\":48},\"summary\"],[\"search\",\"byTerm\",\"{SEARCHQUERY}\",\"titles\",48,[\"referenceId\",\"id\",\"length\",\"name\",\"trackIds\",\"requestId\"]],[\"search\",\"byTerm\",\"{SEARCHQUERY}\",\"suggestions\",20,{\"from\":0,\"to\":20},\"summary\"],[\"search\",\"byTerm\",\"{SEARCHQUERY}\",\"suggestions\",20,[\"length\",\"referenceId\",\"trackIds\"]]]}";
        private string json;

        public ByTermSearchRequest(string query)
        {
            json = searchJson.Replace("{SEARCHQUERY}", query);
        }

        public string Build()
        {
            return json;
        }
    }
}
