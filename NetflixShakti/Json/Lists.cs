using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetflixShakti.Json.Videos;

namespace NetflixShakti.Json.Lists
{
    public class Lister
    {
        public List<List> lists;
        public List<Video> videos;
    }

    public class List
    {
        public int index;
        public string id;
        public string requestId;

        public string displayName;

        public string type;
        public string context;

        public int lenght;

        public int genreId;
        public int videoId;

        public List<string> GetVideoIds()
        {
            List<string> ids = new List<string>();

            ids.Add(v0[1]);
            ids.Add(v1[1]);
            ids.Add(v2[1]);
            ids.Add(v3[1]);
            ids.Add(v4[1]);
            ids.Add(v5[1]);
            ids.Add(v6[1]);
            ids.Add(v7[1]);

            return ids;
        }

        #region videos
        [JsonProperty(PropertyName = "0")]
        List<string> v0;
        [JsonProperty(PropertyName = "1")]
        List<string> v1;
        [JsonProperty(PropertyName = "2")]
        List<string> v2;
        [JsonProperty(PropertyName = "3")]
        List<string> v3;
        [JsonProperty(PropertyName = "4")]
        List<string> v4;
        [JsonProperty(PropertyName = "5")]
        List<string> v5;
        [JsonProperty(PropertyName = "6")]
        List<string> v6;
        [JsonProperty(PropertyName = "7")]
        List<string> v7;
        #endregion
    }
}
