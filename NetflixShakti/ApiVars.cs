using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti
{
    internal static class ApiVars
    {
        public const string baseAPIUrl = "https://www.netflix.com/api/shakti/";
        public const string netflixUrl = "https://www.netflix.com/";
        public const string GetLists = "/preflight?batchImages=true&lolomoid={LOLOMOID}_ROOT&fromRow=4&toRow=50&opaqueImageExtension=webp&transparentImageExtension=webp";

        public const string SearchJson = "{\"paths\":[[\"search\",\"{SEARCH QUERY}\",\"titles\",{\"from\":0,\"to\":48},[\"summary\",\"title\"]],[\"search\",\"{SEARCH QUERY}\",\"titles\",{\"from\":0,\"to\":48},\"boxarts\",\"_342x192\",\"webp\"],[\"search\",\"{SEARCH QUERY}\",\"titles\",[\"id\",\"length\",\"name\",\"trackIds\",\"requestId\",\"referenceId\"]],[\"search\",\"{SEARCH QUERY}\",\"person\",{\"from\":0,\"to\":10},\"person\",\"summary\"],[\"search\",\"{SEARCH QUERY}\",\"person\",{\"from\":0,\"to\":10},\"referenceId\"],[\"search\",\"{SEARCH QUERY}\",\"person\",[\"summary\",\"reference\",\"trackId\"]],[\"search\",\"{SEARCH QUERY}\",\"suggestions\",{\"from\":0,\"to\":20},[\"referenceId\",\"summary\"]],[\"search\",\"{SEARCH QUERY}\",\"suggestions\",[\"summary\",\"reference\",\"trackId\"]]]}";
        public const string VideoInfoJson = "{\"paths\":[[\"videos\",{MOVIE ID},[\"requestId\",\"hasSensitiveMetadata\",\"regularSynopsis\",\"evidence\"]],[\"videos\",{MOVIE ID},\"trailers\",\"summary\"],[\"videos\",{MOVIE ID},\"bb2OGLogo\",\"_550x124\",\"webp\"],[\"videos\",{MOVIE ID},\"genres\",{\"from\":0,\"to\":2},[\"id\",\"name\"]],[\"videos\",{MOVIE ID},\"genres\",\"summary\"],[\"videos\",{MOVIE ID},\"tags\",{\"from\":0,\"to\":9},[\"id\",\"name\"]],[\"videos\",{MOVIE ID},\"tags\",\"summary\"],[\"videos\",{MOVIE ID},[\"cast\",\"directors\",\"creators\"],{\"from\":0,\"to\":4},[\"id\",\"name\"]],[\"videos\",{MOVIE ID},[\"cast\",\"directors\",\"creators\"],\"summary\"],[\"videos\",{MOVIE ID},\"artWorkByType\",\"BILLBOARD\",\"_1280x720\",\"webp\"],[\"videos\",{MOVIE ID},\"interestingMoment\",\"_665x375\",\"webp\"]]}";
    }
}
