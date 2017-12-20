using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.Json.History
{
    public class ViewHistory
    {
        internal int page;
        public int size;

        public TimeSpan totalWatchTime;

        public List<ViewItem> viewedItems;
    }
        
    public class ViewItem
    {
        public string title;
        public string country;
        public string dateStr;
        public string estRating
        {
            get
            {
                return estratingstr;
            }
            set
            {
                estratingstr = value;

                if (double.TryParse(value, out rating))
                {
                    ratingPercentage = rating / 50;
                }
            }
        }

        private string estratingstr;
        public double rating;
        public double ratingPercentage;


        public string seriesTitle;
        public int series;

        public long movieID;
        public long date;
        public double duration;

        public int index;
        public int bookmark;
        public int deviceType;
    }
}