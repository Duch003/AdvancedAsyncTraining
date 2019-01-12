using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedAsyncTraining
{
    public static class DemoMethods
    {
        public static List<string> PrepData()
        {
            List<string> output = new List<string>();

            output.Add("https://www.yahoo.com");
            output.Add("https://www.google.com");
            output.Add("https://www.microsoft.com");
            output.Add("https://www.cnn.com");
            output.Add("https://www.amazon.com");
            output.Add("https://www.facebook.com");
            output.Add("https://www.twitter.com");
            output.Add("https://www.codeproject.com");
            output.Add("https://www.stackoverflow.com");
            output.Add("https://en.wikipedia.org/wiki/.NET_Framework");

            return output;
        }

        public static List<WebsiteDataModel> RunDownloadSync()
        {
            List<string> websites = PrepData();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();

            foreach (var site in websites)
            {
                WebsiteDataModel result = DownloadWebsite(site);
                output.Add(result);
            }

            return output;
        }

        public static List<WebsiteDataModel> RunDownloadParrarelSync()
        {
            List<string> websites = PrepData();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();

            Parallel.ForEach<string>(websites, (site) => {
                WebsiteDataModel result = DownloadWebsite(site);
                output.Add(result);
            });

            return output;
        }

        public static async Task<List<WebsiteDataModel>> RunDownloadPararellelAsyncV2(IProgress<ProgressReportModel> progress)
        {
            List<string> websites = PrepData();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();
            ProgressReportModel report = new ProgressReportModel();

            await Task.Run(() =>
                Parallel.ForEach<string>(websites, (site) =>
                {
                    WebsiteDataModel result = DownloadWebsite(site);
                    output.Add(result);

                    report.SitesDownloaded = output;
                    report.PercentageComplete = (output.Count * 100) / websites.Count;
                    progress.Report(report);
                }));

            

            return output;
        }

        public static async Task<List<WebsiteDataModel>> RunDownloadAsync(IProgress<ProgressReportModel> progress, CancellationToken ct)
        {
            List<string> websites = PrepData();
            List<WebsiteDataModel> output = new List<WebsiteDataModel>();
            ProgressReportModel report = new ProgressReportModel();

            foreach (var site in websites)
            {
                WebsiteDataModel result = await DownloadWebsiteAsync(site);
                output.Add(result);

                ct.ThrowIfCancellationRequested();

                report.SitesDownloaded = output;
                report.PercentageComplete = (output.Count * 100) / websites.Count;
                progress.Report(report);
            }

            return output;
        }

        public static async Task<List<WebsiteDataModel>> RunDownloadPararellelAsync()
        {
            List<string> websites = PrepData();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();

            foreach (var site in websites)
            {
                tasks.Add(DownloadWebsiteAsync(site));
            }

            var results = await Task.WhenAll(tasks);

            return new List<WebsiteDataModel>(results);
        }

        private static async Task<WebsiteDataModel> DownloadWebsiteAsync(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteURL);

            return output;
        }

        private static WebsiteDataModel DownloadWebsite(string websiteURL)
        {
            WebsiteDataModel output = new WebsiteDataModel();
            WebClient client = new WebClient();

            output.WebsiteUrl = websiteURL;
            output.WebsiteData = client.DownloadString(websiteURL);

            return output;
        }
    }
}
