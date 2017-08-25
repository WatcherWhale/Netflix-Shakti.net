using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti
{
    static class ApiVars
    {
        public const string baseAPIUrl = "https://www.netflix.com/api/shakti/";
        public const string netflixUrl = "https://www.netflix.com/";
        public const string GetLists = "/preflight?batchImages=true&lolomoid={LOLOMOID}_ROOT&fromRow=4&toRow=50&opaqueImageExtension=webp&transparentImageExtension=webp";

        public const string SearchJson = "{\"paths\":[[\"search\",\"{SEARCH QUERY}\",\"titles\",{\"from\":0,\"to\":48},[\"summary\",\"title\"]],[\"search\",\"{SEARCH QUERY}\",\"titles\",{\"from\":0,\"to\":48},\"boxarts\",\"_342x192\",\"webp\"],[\"search\",\"{SEARCH QUERY}\",\"titles\",[\"id\",\"length\",\"name\",\"trackIds\",\"requestId\",\"referenceId\"]],[\"search\",\"{SEARCH QUERY}\",\"person\",{\"from\":0,\"to\":10},\"person\",\"summary\"],[\"search\",\"{SEARCH QUERY}\",\"person\",{\"from\":0,\"to\":10},\"referenceId\"],[\"search\",\"{SEARCH QUERY}\",\"person\",[\"summary\",\"reference\",\"trackId\"]],[\"search\",\"{SEARCH QUERY}\",\"suggestions\",{\"from\":0,\"to\":20},[\"referenceId\",\"summary\"]],[\"search\",\"{SEARCH QUERY}\",\"suggestions\",[\"summary\",\"reference\",\"trackId\"]]]}";
    }
}
