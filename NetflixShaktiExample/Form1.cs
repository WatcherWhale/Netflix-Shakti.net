﻿using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NetflixShakti;
using NetflixShakti.Json.History;
using NetflixShakti.Json.Profiles;
using NetflixShakti.Search;

namespace NetflixShaktiExample
{
    public partial class Form1 : Form
    {
        Netflix _netflix;

        public Form1()
        {
            InitializeComponent();
        }

        private async void Login(string username, string password)
        {
            _netflix = await Netflix.Login(username, password);

            SetupNetflix(_netflix);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            /*if (webBrowser1.Url.AbsolutePath == "/browse")
            {
                // '/browse' path means that the user is logged in
                webBrowser1.DocumentCompleted -= webBrowser1_DocumentCompleted;

                Task.Run(() =>
                {
                    //System.Threading.Thread.Sleep(5000);
                    string source ="";
                    Invoke(new MethodInvoker(delegate { source = webBrowser1.DocumentText; }));

                    string cookies = "";
                    Invoke(new MethodInvoker(delegate { cookies = webBrowser1.Document.Cookie; }));

                    CookieContainer container = Netflix.BuildCoockieContainer(cookies);

                    

                    SetupNetflix(container, source);
                });
            }*/
        }

        private async void SetupNetflix(Netflix _netflix)
        {
            //await _netflix.LoadNetflixProfiles();
            var stream = await _netflix.Profiles.active.GetAvatarImageStream(AvatarSizes.Size64);
            profilesDropDown.Image = Image.FromStream(stream);
            activeUser.Text = _netflix.Profiles.active.firstName;

            Task.Run(() => LoadHistory());

            foreach (Profile prof in _netflix.Profiles.profiles)
            {
                if (prof.rawFirstName == "Kids") continue;

                var task = prof.GetAvatarImageStream(AvatarSizes.Size64);

                ToolStripItem dropItem = null;
                Invoke(new MethodInvoker(delegate { dropItem = profilesDropDown.DropDownItems.Add(prof.firstName); }));
                Invoke(new MethodInvoker(async delegate { dropItem.Image = Image.FromStream(await task); }));

                dropItem.Click += DropItem_Click;
            }

            //_netflix.GetHomePageList();
        }

        private async void LoadHistory()
        {
            Invoke(new MethodInvoker(delegate { dataGridView1.Rows.Clear(); }));

            var history = await _netflix.GetViewHistory();

            TimeSpan duration = history.totalWatchTime;
            watchTime.Text = "Total Watch Time: " + duration.Days + " days " + duration.Hours + " hours " + duration.Minutes + " Minutes " + duration.Seconds + " Secconds";

            foreach (ViewItem vi in history.viewedItems)
            {
                string title = vi.title;
                if (vi.seriesTitle != "" && vi.seriesTitle != null) title = vi.seriesTitle + ": " + title;

                string rating = vi.estRating == null ? "/" : (vi.ratingPercentage * 10).ToString() + "/10";

                var ts = TimeSpan.FromSeconds(vi.duration);
                string dur = ts.Hours > 0 ? ts.Hours + "h " + ts.Minutes + "m" : ts.Minutes + "m";

                object[] row = new object[] { vi.dateStr, title, dur, rating };
                Invoke(new MethodInvoker(delegate { dataGridView1.Rows.Add(row); }));
            }
        }

        private async void DropItem_Click(object sender, EventArgs e)
        {
            var dropItem = (ToolStripItem)sender;
            await _netflix.SwitchProfile(_netflix.Profiles.GetProfileByName(dropItem.Text));

            var stream = await _netflix.Profiles.active.GetAvatarImageStream(AvatarSizes.Size64);
            profilesDropDown.Image = Image.FromStream(stream);

            activeUser.Text = _netflix.Profiles.active.firstName;
            watchTime.Text = "Loading...";

            Task.Run(() => LoadHistory());
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            /*listBox1.Items.Clear();            

            var output = await _netflix.Search(SearchRequest.GetSimpleVideoSearch(textBox2.Text));

            foreach (var video in output.value.videos)
            {
                listBox1.Items.Add(video.Value.title);
            }*/

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            NetflixShakti.Search.SearchRequest req = new SearchRequest("Doctor Who", "titles");
            await _netflix.Search(req);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Login(textBox1.Text, textBox3.Text);
        }
    }
}
