using Demo.Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Demo.Api.Data
{
    public class PerformanceIndicatorQueryService
    {
        public IEnumerable<PerformanceIndicator> GetPerformanceIndicators()
        {
            return ExecuteQuery();
        }


        private IEnumerable<PerformanceIndicator> ExecuteQuery()
        {
            var tenantId = ConfigurationSettings.AppSettings["tenantId"];
            var appId = ConfigurationSettings.AppSettings["appId"];
            var appSecret = ConfigurationSettings.AppSettings["appSecret"];
            var aasServerName = ConfigurationSettings.AppSettings["aasServerName"];
            var aasTokenUrl = ConfigurationSettings.AppSettings["aasTokenUrl"];

            var db = new AnalysisServicesQueryClient(tenantId, appId, appSecret, aasServerName, aasTokenUrl);
            return db.RunQuery("EVALUATE('Date')");
        }
    }
}