using NetflixShakti.Json.Genres;
using NetflixShakti.Json.Persons;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.Json.Movies
{
    public class MovieSerilizer
    {
        [JsonProperty(PropertyName = "value")]
        public MovieInfo info;
    }

    public class MovieInfo
    {
        Dictionary<long, Movie> movies = new Dictionary<long, Movie>();
        [JsonProperty(PropertyName = "person")]
        Dictionary<long, Person> persons = new Dictionary<long, Person>();
        Dictionary<long, Genre> genres = new Dictionary<long, Genre>();
    }

    public class Movie
    {
        public bool hasSensitiveMetadata;
        public string regularSynopsis;

    }
}
