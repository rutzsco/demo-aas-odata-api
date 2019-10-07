using Demo.Api.Models;
using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Api.Data
{
    public class AnalysisServicesQueryClient
    {
        private readonly string _tenantId;
        private readonly string _appId;
        private readonly string _appSecret;
        private readonly string _aasServerName;
        private readonly string _aasTokenUrl;

        public AnalysisServicesQueryClient(string tenantId, string appId, string appSecret, string aasServerName, string aasTokenUrl)
        {
            _tenantId = tenantId;
            _appId = appId;
            _appSecret = appSecret;
            _aasServerName = aasServerName;
            _aasTokenUrl = aasTokenUrl;
        }

        public IEnumerable<PerformanceIndicator> RunQuery(string query)
        {
            if (string.IsNullOrEmpty(_aasServerName) || _aasServerName == "VALUE_GOES_HERE")
            {
                yield return new PerformanceIndicator() { Id = 0, Name = "RecordCount", Value = "100" };
                yield return new PerformanceIndicator() { Id = 1, Name = "FieldCount", Value = "20" };
            }
            else
            {
                var token = GetAccessToken(_aasTokenUrl);
                var connectionString = $"Provider=MSOLAP;Data Source={_aasServerName};Initial Catalog=adventureworks;User ID=;Password={token};Persist Security Info=True;Impersonation Level=Impersonate";
                ConnectionManager.Initialize(connectionString);
                var connection = ConnectionManager.GetConnection();

                using (AdomdCommand command = new AdomdCommand(query, connection))
                {
                    using (var results = command.ExecuteReader())
                    {
                        int count = 0;
                        int fieldcount = 0;
                        foreach (var record in results)
                        {
                            count++;
                            fieldcount = record.FieldCount;
                        }
                        yield return new PerformanceIndicator() { Id = 0, Name = "RecordCount", Value = count.ToString() };
                        yield return new PerformanceIndicator() { Id = 1, Name = "FieldCount", Value = fieldcount.ToString() };
                    }
                }
                ConnectionManager.ReturnConnection(connection);
            }
        }

        private string GetAccessToken(string aasUrl)
        {
            string authorityUrl = $"https://login.microsoftonline.com/{_tenantId}";
            var authContext = new AuthenticationContext(authorityUrl);

            // Config for OAuth client credentials 
            var clientCred = new ClientCredential(_appId, _appSecret);
            AuthenticationResult authenticationResult =  authContext.AcquireTokenAsync(aasUrl, clientCred).Result;

            //get access token
            return authenticationResult.AccessToken;
        }
    }
}