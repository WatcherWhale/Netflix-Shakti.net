using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetflixShakti.Json.Videos;
using NetflixShakti.Json.Persons;
using Newtonsoft.Json;

namespace NetflixShakti.Json.Search
{
    public class SearchResult
    {
        public SearchValue value;
    }

    public class SearchValue
    {
        public IDictionary<string,Movies.Movie> videos;
        [JsonProperty(PropertyName = "person")]
        public IDictionary<string, Person> persons;
    }
}
