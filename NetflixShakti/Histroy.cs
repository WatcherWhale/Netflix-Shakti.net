﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetflixShakti.History
{
    public class ViewHistory
    {
        public int page;
        public int size;

        public List<ViewItem> viewedItems;
    }
        
    public class ViewItem
    {
        public string title;
        public string country;
        public string dateStr;
        public string estRating;

        public string seriesTitle;
        public int series;

        public int movieID;
        public long date;
        public double duration;

        public int index;
        public int bookmark;
        public int deviceType;
    }
}