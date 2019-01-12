using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedAsyncTraining
{
    public class ProgressReportModel
    {
        public List<WebsiteDataModel> SitesDownloaded { get; set; } = new List<WebsiteDataModel>();

        public int PercentageComplete { get; set; } = 0;
    }
}
