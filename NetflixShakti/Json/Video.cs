using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.Json.Videos
{
    public class Video
    {
        public string title;
        public VideoSummary summary;
        public Boxarts boxarts;

        public bool hasSensitiveMetadata;
        public string regularSynopsis;
        public string synopsis;
    }

    public class VideoSummary
    {
        public int id;
        public string type;

        public bool isNSRE;
        public bool isOriginal;
    }

    public class Boxarts
    {
        public WebpImage Image { get { return imageContainer.webp; } }

        [JsonProperty(PropertyName = "_342x192")]
        BannerImageContainer imageContainer;
    }

    public class BannerImageContainer
    {
        public WebpImage webp;
    }

    public class WebpImage
    {
        public string url;
        public string image_key;
    }
}
