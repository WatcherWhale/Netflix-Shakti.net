using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using NetflixShakti.Json.History;
using NetflixShakti.Json.Profiles;
using NetflixShakti.Json.Lists;
using NetflixShakti.Json.Search;
using NetflixShakti.Json;
using System.Collections.Specialized;

namespace NetflixShakti
{
    public class Netflix : IDisposable
    {
        private CookieContainer _cookieJar;
        public string Id { get; set; }
        [Obsolete("Is not working at the moment")]
        public string LolomoId { get; set; }

        public ProfileContainer Profiles { get; set; }

        private Netflix(CookieContainer cookies, string id)
        {
            _cookieJar = cookies;
            Id = id;
            //LolomoId = lolomoId;
            LoadNetflixProfilesTask();
        }

        [Obsolete("Use Netflix.BuildFromSource instead")]
        public Netflix(string cookies,string id)
        {
            _cookieJar = BuildCoockieContainer(cookies);
            Id = id;
            //LolomoId = lolomoId;

            LoadNetflixProfilesTask();
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
                    WebRequest request = WebRequest.Create(ApiVars.baseAPIUrl + Id + "/viewingactivity?pg=" + pages.Count);
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
            WebRequest request = WebRequest.Create(ApiVars.baseAPIUrl + Id + "/profiles");
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
            WebRequest request = WebRequest.Create(ApiVars.baseAPIUrl + Id + "/profiles/switch?switchProfileGuid=" + prof.guid);
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

            LoadNetflixProfilesTask();
        }

        public Task<Lister> GetHomePageList()
        {
            return Task.Run(() => GetHomePageListTask());
        }

        private Lister GetHomePageListTask()
        {
            WebRequest request = WebRequest.Create(ApiVars.baseAPIUrl + Id + ApiVars.GetLists.Replace("{LOLOMOID}", LolomoId));
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = _cookieJar;

            Lister lister;

            using (HttpWebResponse res = (HttpWebResponse)webRequest.GetResponse())
            using (Stream stream = res.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();

                lister = JsonConvert.DeserializeObject<Lister>(json);
            }

            return lister;
        }

        public Task<SearchResult> Search(string query)
        {
            return Task.Run(() => SearchTask(query));
        }

        private SearchResult SearchTask(string search)
        {
            WebRequest request = WebRequest.Create(ApiVars.baseAPIUrl + Id + "/pathEvaluator?withSize=true&materialize=true&searchAPIV2=false");
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.CookieContainer = _cookieJar;

            webRequest.Method = "POST";
            webRequest.ContentType = "application/json";

            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                var json = ApiVars.SearchJson.Replace("{SEARCH QUERY}", search);
                writer.Write(json);
                writer.Flush();
            }

            SearchResult result;
            using (HttpWebResponse res = (HttpWebResponse)webRequest.GetResponse())
            using (Stream stream = res.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();

                json = JsonUtilities.RemoveProperties(json, "$size");
                json = JsonUtilities.RemoveProperties(json, "size");

                result = JsonConvert.DeserializeObject<SearchResult>(json);
            }

            return result;
        }

        public static Task<Netflix> Login(string email,string password)
        {
            return Task.Run(() => LoginTask(email, password));
        }

        public static Netflix LoginTask(string email, string password)
        {
            Netflix netflix;

            WebRequest request = WebRequest.Create(ApiVars.netflixUrl + "login");
            HttpWebRequest webRequest = request as HttpWebRequest;
            webRequest.Method = "POST";

            NameValueCollection postQuery = new NameValueCollection();

            postQuery.Add("email", email);
            postQuery.Add("password", password);
            postQuery.Add("action", "loginAction");
            postQuery.Add("mode", "login");
            postQuery.Add("rememberMe", "false");
            postQuery.Add("flow", "websiteSignUp");

            string postData = postQuery.ToString();

            using (var writer = new StreamWriter(webRequest.GetRequestStream()))
            {
                writer.Write(postData);
                writer.Flush();
            }

            using (var res = webRequest.GetResponse())
            using (var stream = res.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var webRes = res as HttpWebResponse;
                var cookies = webRes.Cookies;

                CookieContainer cc = new CookieContainer();
                cc.Add(cookies);

                string source = reader.ReadToEnd();

                netflix = Netflix.BuildFromSource(cc, source);
            }

            return netflix;
        }

        #endregion

        #region Static Functions
        public static Netflix BuildFromSource(CookieContainer cookies,string source)
        {
            string id = GetIdFromSource(source);
            //string lolomo = GetLolomoFromSource(source);

            Netflix netflix = new Netflix(cookies,id);
            return netflix;
        }

        public static string GetLolomoFromSource(string source)
        {
            string finder = "\"billboards\":{\"";
            int startIndex = source.LastIndexOf(finder) + finder.Length;

            string ender = "\":{\"data\":";
            int endIndex = source.IndexOf(ender, startIndex);

            int lenght = endIndex - startIndex;
            string lolomo = source.Substring(startIndex, lenght).Split('_')[0];

            return lolomo;
        }

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
