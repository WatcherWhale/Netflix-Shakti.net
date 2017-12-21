using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.ComponentModel;

using Newtonsoft.Json;

using NetflixShakti.Json;
using NetflixShakti.Json.History;
using NetflixShakti.Json.Profiles;
using NetflixShakti.Json.Lists;
using NetflixShakti.Json.RatingHistory;

using NetflixShakti.Search;

namespace NetflixShakti
{
    public class Netflix : IDisposable
    {
        private CookieContainer _cookieJar;
        public string Id { get; set; }
        
        private string LolomoId { get; set; }

        public ProfileContainer Profiles { get; set; }

        [Description("Netflix constructor needs a cookie container and a user id that can be obtained from the source.")]
        public Netflix(CookieContainer cookies, string id)
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
        [Description("Get the whole viewing history of the currently active profile.")]
        public Task<ViewHistory> GetViewHistory()
        {
            return Task.Run(() => GetViewHistoryTask());
        }

        private ViewHistory GetViewHistoryTask()
        {
            try
            {
                List<ViewHistory> pages = new List<ViewHistory>();
                bool loading = true;

                while (loading)
                {
                    var res = WebRequester.DoRequest(ApiVars.baseAPIUrl + Id + "/viewingactivity?pg=" + pages.Count, _cookieJar);

                    var page = res.DeserializeResponse<ViewHistory>();

                    if (page.viewedItems.Count == 0)
                    {
                        loading = false;
                    }
                    else
                    {
                        pages.Add(page);
                    }
                }

                return GetViewHistoryFromPages(pages);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private ViewHistory GetViewHistoryFromPages(List<ViewHistory> pages)
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

            vh.totalWatchTime = GetTotalWatchTimeTask(vh);

            return vh;
        }

        [Description("Get the whole rating history of the currently active profile.")]
        public Task<RatingList> GetRatingHistory()
        {
            return Task.Run(() => GetRatingHistoryTask());
        }

        public RatingList GetRatingHistoryTask()
        {
            List<RatingList> pages = new List<RatingList>();
            bool loading = true;

            while(loading)
            {
                var res = WebRequester.DoRequest(ApiVars.baseAPIUrl + Id + "/ratinghistory?pg=" + pages.Count,_cookieJar);
                var page = res.DeserializeResponse<RatingList>();

                if (page.ratingItems.Count == 0)
                {
                    loading = false;
                }
                else
                {
                    pages.Add(page);
                }
            }

            RatingList list = new RatingList();

            foreach (RatingList page in pages)
            {
                foreach (Rating rat in page.ratingItems)
                {
                    list.ratingItems.Add(rat);
                }
            }

            list.totalRatings = list.ratingItems.Count;
            list.size = list.totalRatings;
            list.page = 0;

            return list;
        }

        [Description("Get a timespan of how long the currently active profile has watched.")]
        public Task<TimeSpan> GetTotalWatchTime()
        {
            return Task.Run(() => GetTotalWatchTimeTask());
        }

        private TimeSpan GetTotalWatchTimeTask()
        {
            var history = GetViewHistoryTask();

            double totalTime = 0;
            foreach (var item in history.viewedItems)
            {
                totalTime += item.duration;
            }

            return TimeSpan.FromSeconds(totalTime);
        }

        [Description("Get a timespan of how long the currently active profile has watched.")]
        public Task<TimeSpan> GetTotalWatchTime(ViewHistory history)
        {
            return Task.Run(() => GetTotalWatchTimeTask(history));
        }

        private TimeSpan GetTotalWatchTimeTask(ViewHistory history)
        {
            double totalTime = 0;
            foreach (var item in history.viewedItems)
            {
                totalTime += item.duration;
            }

            return TimeSpan.FromSeconds(totalTime);
        }

        [Description("(Re)loads the netflix profiles associated with the current account.")]
        public Task LoadNetflixProfiles()
        {
            return Task.Run(() => LoadNetflixProfilesTask());
        }

        private void LoadNetflixProfilesTask()
        {
            var res = WebRequester.DoRequest(ApiVars.baseAPIUrl + Id + "/profiles",_cookieJar);
            Profiles = res.DeserializeResponse<ProfileContainer>();
        }

        [Description("Switches the active profile.")]
        public Task SwitchProfile(Profile prof)
        {
            return Task.Run(() => SwitchProfileTask(prof));
        }

        private void SwitchProfileTask(Profile prof)
        {
            var res = WebRequester.DoRequest(ApiVars.baseAPIUrl + Id + "/profiles/switch?switchProfileGuid=" + prof.guid, _cookieJar);

            CookieContainer cc = new CookieContainer();
            foreach (Cookie cookie in res.Cookies)
            {
                cc.Add(cookie);
            }

            _cookieJar = cc;

            LoadNetflixProfilesTask();
        }

        [Description("Gets a list of list of movies that loads on the netflix homepage.")]
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

        public Task Search(ISearchRequest request)
        {
            return new Task(() => SearchTask(request));
        }

        private void SearchTask(ISearchRequest request)
        {
            var res = WebRequester.DoPostRequest(ApiVars.baseAPIUrl + Id + "/pathEvaluator?withSize=true&materialize=true&canWatchBranchingTitles=false&isWatchlistEnabled=false",
                _cookieJar, "Accept: application/json, text/javascript, */*", request.Build());
        }

        #endregion

        #region Static Functions
        [Description("Builds the netflix class from a browser source and cookies.")]
        public static Netflix BuildFromSource(CookieContainer cookies,string source)
        {
            string id = GetIdFromSource(source);
            //string lolomo = GetLolomoFromSource(source);

            Netflix netflix = new Netflix(cookies,id);
            return netflix;
        }

        private static string GetLolomoFromSource(string source)
        {
            string finder = "\"billboards\":{\"";
            int startIndex = source.LastIndexOf(finder) + finder.Length;

            string ender = "\":{\"data\":";
            int endIndex = source.IndexOf(ender, startIndex);

            int lenght = endIndex - startIndex;
            string lolomo = source.Substring(startIndex, lenght).Split('_')[0];

            return lolomo;
        }

        [Description("Gets the user id from the browser source.")]
        public static string GetIdFromSource(string browserSource)
        {
            int StartIndex = browserSource.LastIndexOf("\"BUILD_IDENTIFIER\":\"") + "\"BUILD_IDENTIFIER\":\"".Length;
            return browserSource.Substring(StartIndex, 8);
        }

        [Description("Builds a cookiecontainer from a cookie string.")]
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

        public static Task<Netflix> Login(string email, string password)
        {
            System.Windows.Forms.WebBrowser browser = new System.Windows.Forms.WebBrowser
            {
                ScriptErrorsSuppressed = true,
                Visible = false
            };

            return Task.Run(() => LoginTask(email, password,browser));
        }

        public static Netflix LoginTask(string email, string password, System.Windows.Forms.WebBrowser browser)
        {
            bool wait = true;
            Netflix netflix = null;

            browser.Navigate(ApiVars.netflixUrl + "/login");
            browser.DocumentCompleted += (object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e) => 
            {
                if(browser.Url.AbsolutePath == "/login")
                {
                    browser.Document.GetElementById("email").InnerText = email;
                    browser.Document.GetElementById("password").InnerText = password;

                    foreach (System.Windows.Forms.HtmlElement btn in browser.Document.GetElementsByTagName("button"))
                    {
                        if(btn.GetAttribute("class").Contains("login-button"))
                        {
                            btn.InvokeMember("click");
                        }
                    }
                }
                else if(browser.Url.AbsolutePath == "/browse")
                { 
                    netflix = Netflix.BuildFromSource(Netflix.BuildCoockieContainer(browser.Document.Cookie), browser.DocumentText);
                    wait = false;
                }
            };

            while(wait)
            {
                //Wait for it ~ Barney Stinson
            }

            return netflix;
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
