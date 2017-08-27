using NetflixShakti.Genres;
using NetflixShakti.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.Movies
{
    public class Movie
    {
        public List<Genre> Genres { get; internal set; }
        public List<Person> Cast { get; internal set; }
        public List<Person> Directors { get; internal set; }
    }
}

namespace NetflixShakti.Genres
{
    public class Genre
    {
        public string Name { get; internal set; }
        public int Id { get; internal set; }
    }
}

namespace NetflixShakti.Persons
{
    public class Person
    {
        public string Name { get; internal set; }
        public int Id { get; internal set; }
    }
}