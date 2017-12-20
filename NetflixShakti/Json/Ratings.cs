using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.Json.RatingHistory
{
    public class RatingList
    {
        internal int page;
        public int size;
        public int totalRatings;

        public List<Rating> ratingItems = new List<Rating>();
    }

    public enum RatingType { Thumb, Star}

    public class Rating
    {
        public string title;
        public long movieID;

        internal string ratingType;
        public RatingType RatingType
        {
            get
            {
                if(ratingType == Json.RatingHistory.RatingType.Thumb.ToString().ToLower())
                {
                    return Json.RatingHistory.RatingType.Thumb;
                }
                else
                {
                    return Json.RatingHistory.RatingType.Star;
                }
            }
            set
            {
                ratingType = value.ToString().ToLower();
            }
        }

        public int YourRating
        {
            get
            {
                if(ratingType == "thumb")
                {
                    return yourRating - 1;
                }
                else
                {
                    return yourRating;
                }
            }
            set
            {
                if (ratingType == "thumb")
                {
                    yourRating = value + 1;
                }
                else
                {
                    yourRating = value;
                }
            }
        }
        internal int yourRating;

        public string date;
        public long timestamp;
        public long comparableDate;
    }
}
