using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using NetflixShakti.History;
using NetflixShakti.Profiles;

namespace NetflixShakti
{
    public class Netflix : IDisposable
    {
        private CookieContainer _cookieJar;
        readonly string apiUrl = "https://www.netflix.com/api/shakti/";

        public string Id { get; set; }
        public ProfileContainer Profiles { get; set; }

        public Netflix(CookieContainer cookies,string id)
        {
            _cookieJar = cookies;

            LoadNetflixProfiles();
        }

        public Netflix(string cookies,string id)
        {
            _cookieJar = BuildCoockieContainer(cookies);
            LoadNetflixProfiles();
        }

        #region Tasks
        public Task<List<ViewHistory>> GetViewHistory()
        {
            return Task.Run(() => GetViewHistoryTask());
        }

        private List<ViewHistory> GetViewHistoryTask()
        {
            try
            {
                List<ViewHistory> pages = new List<ViewHistory>();
                bool loading = true;

                while (loading)
                {
                    WebRequest request = WebRequest.Create(apiUrl + Id + "/viewingactivity?pg=" + pages.Count);
                    HttpWebRequest webRequest = request as HttpWebRequest;
                    webRequest.CookieContainer = _cookieJar;

                    using (var res = webRequest.GetResponse())
                    using (var resStream = res.GetResponseStream())
                    using (var reader = new StreamReader(resStream))
                    {
                        string json = reader.ReadToEnd();
                        var page = JsonConvert.DeserializeObject<ViewHistory>(json);

                        if (page.viewedItems.Count == 0)
                        {
                            loading = false;
                        }
                        else
                        {
                            pages.Add(page);
                        }
                    }
                }

                return pages;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public Task<ViewHistory> GetViewHistoryFromPages(List<ViewHistory> pages)
        {
            return Task.Run(() => GetViewHistoryFromPagesTask(pages));
        }

        private ViewHistory GetViewHistoryFromPagesTask(List<ViewHistory> pages)
        {
            ViewHistory vh = new ViewHistory()
            {
                viewedItems = new List<ViewItem>(),
                page = 1
            };

            foreach (ViewHistory page in pages)
            {
                foreach (ViewItem item in page.viewedItems)
                {
                    vh.viewedItems.Add(item);
                }
            }

            vh.size = vh.viewedItems.Count;

            return vh;
        }

        public Task<TimeSpan> GetTotalWatchTime()
        {
            return Task.Run(() => GetTotalWatchTimeTask());
        }

        private TimeSpan GetTotalWatchTimeTask()
        {
            var pages = GetViewHistoryTask();
            var history = GetViewHistoryFromPagesTask(pages);

            double totalTime = 0;
            foreach (var item in history.viewedItems)
            {
                totalTime += item.duration;
            }

            return TimeSpan.FromSeconds(totalTime);
        }

        public Task LoadNetflixProfiles()
        {
            return Task.Run(() => LoadNetflixProfilesTask());
        }

        private void LoadNetflixProfilesTask()
        {
            WebRequest request = WebRequest.Create(apiUrl + Id + "/profiles");
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = _cookieJar;

            using (var res = webRequest.GetResponse())
            {
                using (Stream resStream = res.GetResponseStream())
                {
                    using (StreamReader read = new StreamReader(resStream))
                    {
                        string json = read.ReadToEnd();
                        Profiles = JsonConvert.DeserializeObject<ProfileContainer>(json);
                    }
                }
            }
        }

        public Task SwitchProfile(Profile prof)
        {
            return Task.Run(() => SwitchProfileTask(prof));
        }

        private void SwitchProfileTask(Profile prof)
        {
            WebRequest request = WebRequest.Create(apiUrl + Id + "/profiles/switch?switchProfileGuid=" + prof.guid);
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = _cookieJar;

            using (HttpWebResponse res = (HttpWebResponse)webRequest.GetResponse())
            {
                CookieContainer cc = new CookieContainer();
                foreach (Cookie cookie in res.Cookies)
                {
                    cc.Add(cookie);
                }

                _cookieJar = cc;
            }
        }
        #endregion

        #region Static Functions
        public static string GetIdFromSource(string browserSource)
        {
            int StartIndex = browserSource.LastIndexOf("\"BUILD_IDENTIFIER\":\"") + "\"BUILD_IDENTIFIER\":\"".Length;
            return browserSource.Substring(StartIndex, 8);
        }

        public static CookieContainer BuildCoockieContainer(string cookies)
        {
            CookieContainer cookieJar = new CookieContainer();

            foreach (string cookie in cookies.Split(';'))
            {
                string name = cookie.Split('=')[0];
                string value = cookie.Substring(name.Length + 1);
                string path = "/";
                string domain = ".netflix.com";
                cookieJar.Add(new Cookie(name.Trim(), value.Trim(), path, domain));
            }

            return cookieJar;
        }
        #endregion

        public void Dispose()
        {
            _cookieJar = null;
            Profiles = null;
            Id = "";
        }
    }
}
