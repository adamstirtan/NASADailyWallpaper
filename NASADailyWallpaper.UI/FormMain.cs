using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Microsoft.Win32;

using NASADailyWallpaper.Client;

namespace NASADailyWallpaper.UI
{
    public partial class FormMain : Form
    {
        private const int SPI_SETDESKWALLPAPER = 20;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public FormMain()
        {
            InitializeComponent();
        }

        private async void buttonRefresh_Click(object sender, EventArgs e)
        {
            buttonRefresh.Enabled = false;
            buttonRefresh.Text = "Loading...";

            INasaApodClient client = new NasaApodClient();

            ApodResponse apodResponse = await client.GetLatestImage();

            if (apodResponse is null)
            {
                return;
            }

            SetWallpaper(apodResponse.HDUrl);

            buttonRefresh.Enabled = true;
            buttonRefresh.Text = "Refresh now";
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.github.com/adamstirtan/NASADailyWallpaper");
            }
            catch { }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void SetWallpaper(string uri)
        {
            Stream stream = new WebClient().OpenRead(uri);
            Image img = Image.FromStream(stream);

            string tempPath = Path.Combine(Path.GetTempPath(), "wallpaper.bmp");

            img.Save(tempPath, System.Drawing.Imaging.ImageFormat.Bmp);

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);

            key.SetValue(@"WallpaperStyle", 2.ToString());
            key.SetValue(@"TileWallpaper", 0.ToString());

            _ = SystemParametersInfo(SPI_SETDESKWALLPAPER,
                0,
                tempPath,
                SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}