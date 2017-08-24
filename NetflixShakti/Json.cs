using Newtonsoft.Json;
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

namespace NetflixShakti.Profiles
{
    public class ProfileContainer
    {
        public List<Profile> profiles = new List<Profile>();
        public Profile active;

        public Profile GetProfileByName(string name)
        {
            foreach (Profile prof in profiles)
            {
                if (prof.firstName == name)
                    return prof;
            }
            return null;
        }

        public Profile GetProfileByGuid(string guid)
        {
            foreach (Profile prof in profiles)
            {
                if (prof.guid == guid)
                    return prof;
            }
            return null;
        }
    }

    public class Profile
    {
        public string firstName;
        public string rawFirstName;
        public string guid;
        public string experience;
        public string avatarName;

        public bool isAccountOwner;
        public bool isFirstUse;
        public bool isActive;
        public bool isDefault;
        public bool canEdit;

        public Task<System.IO.Stream> GetAvatarImageStream(int size)
        {
            return Task.Run(() => GetAvatarImageStreamTask(size));
        }

        private System.IO.Stream GetAvatarImageStreamTask(int size)
        {
            int icon = int.Parse(avatarName.Substring("icon".Length));
            string url = $"https://secure.netflix.com/ffe/profiles/avatars_v2/{size}x{size}/PICON_0{icon}.png";

            byte[] imageData = new System.Net.WebClient().DownloadData(url);

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(imageData);
            return memoryStream;
        }
    }

    public class AvatarSizes
    {
        public const int Size32 = 32;
        public const int Size50 = 50;
        public const int Size64 = 64;
        public const int Size80 = 80;
        public const int Size100 = 100;
        public const int Size112 = 112;
        public const int Size160 = 160;
        public const int Size200 = 200;
        public const int Size320 = 320;
    }
}