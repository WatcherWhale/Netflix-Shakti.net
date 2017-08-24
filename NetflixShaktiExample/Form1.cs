using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using NetflixShakti;
using NetflixShakti.History;
using NetflixShakti.Profiles;
using System.Net;

namespace NetflixShaktiExample
{
    public partial class Form1 : Form
    {
        Netflix _netflix;

        public Form1()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Url.AbsolutePath == "/browse")
            {
                // '/browse' path means that the user is logged in
                webBrowser1.DocumentCompleted -= webBrowser1_DocumentCompleted;

                string source = webBrowser1.DocumentText;

                string id = Netflix.GetIdFromSource(source);
                CookieContainer container = Netflix.BuildCoockieContainer(webBrowser1.Document.Cookie);

                SetupNetflix(container, id);
            }
        }

        private async void SetupNetflix(CookieContainer container, string id)
        {
            _netflix = new Netflix(container, id);

            var stream = await _netflix.Profiles.active.GetAvatarImageStream(AvatarSizes.Size64);
            profilesDropDown.Image = Image.FromStream(stream);
            activeUser.Text = _netflix.Profiles.active.firstName;

            Task.Run(() => LoadHistory());

            foreach (Profile prof in _netflix.Profiles.profiles)
            {
                if (prof.rawFirstName == "Kids") continue;

                var task = prof.GetAvatarImageStream(AvatarSizes.Size64);

                var dropItem = profilesDropDown.DropDownItems.Add(prof.firstName);
                dropItem.Image = Image.FromStream(await task);

                dropItem.Click += DropItem_Click;
            }
        }

        private async void LoadHistory()
        {
            Invoke(new MethodInvoker(delegate { dataGridView1.Rows.Clear(); }));

            var pages = await _netflix.GetViewHistory();
            var history = await _netflix.GetViewHistoryFromPages(pages);

            foreach (ViewItem vi in history.viewedItems)
            {
                string title = vi.title;
                if (vi.seriesTitle != "" && vi.seriesTitle != null) title = vi.seriesTitle + ": " + title;

                string rating = vi.estRating == null ? "/" : (double.Parse(vi.estRating) / 10).ToString();

                var ts = TimeSpan.FromSeconds(vi.duration);
                string dur = ts.Hours > 0 ? ts.Hours + "h " + ts.Minutes + "m" : ts.Minutes + "m";

                object[] row = new object[] { vi.dateStr, title, dur, rating };
                Invoke(new MethodInvoker(delegate { dataGridView1.Rows.Add(row); }));
            }

            TimeSpan duration = await _netflix.GetTotalWatchTime();
            watchTime.Text = "Total Watch Time: " + duration.Days + " days " + duration.Hours + " hours " + duration.Minutes + " Minutes " + duration.Seconds + " Secconds";
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
    }
}
