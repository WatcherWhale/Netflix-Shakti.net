using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetflixShakti.Json.Videos;

namespace NetflixShakti.Json.Search
{
    public class SearchResult
    {
        public SearchValue value;
    }

    public class SearchValue
    {
        public IDictionary<string,Video> videos;
    }
}
