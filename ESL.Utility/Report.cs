using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using FastReport.Data;
using FastReport.Utils;
using FastReport.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ESL.Utility
{
    public class Report : IReport
    {
        private readonly ILogger<Report> _logger;
        private ESLDbConnection connection = null;
        public Report(ILogger<Report> logger)
        {
            _logger = logger;
            try
            {
                RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));
                FastReport.Utils.Config.FontListFolder = "C:\\FontFolder";
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"{"appsettings.json"}");
                var str = System.IO.File.ReadAllText(folderDetails);
                connection = JsonConvert.DeserializeObject<ESLDbConnection>(str);
            }
            catch (Exception) { }
        }

        public WebReport GetReport(IHttpContextAccessor _accessor,string reportpath,List<string> sqls)
        {
            ISession _session = _accessor.HttpContext.Session;
            if (connection != null)
            {
                try
                {
                    WebReport webReport = new WebReport();
                    webReport.ShowPreparedReport = false;
                    webReport.Report.Load(reportpath);
                    webReport.SinglePage = true;
                    //MsSqlDataConnection sqlConnection = new MsSqlDataConnection();
                    //sqlConnection.ConnectionString = connection.reportDbConnection;//"Data Source=ESL12;AttachDbFilename=;Initial Catalog=ESLInventory;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=123";
                    //webReport.Report.Dictionary.Connections.Add(sqlConnection);
                    webReport.Report.Dictionary.Connections[0].ConnectionString = connection.reportDbConnection;//"Data Source=ESL12;AttachDbFilename=;Initial Catalog=ESLInventory;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=123";
                
                    webReport.Report.SetParameterValue("userName", _session.GetString("userName"));
                    webReport.Report.SetParameterValue("userIp", _session.GetString("userIp"));
                    webReport.Report.SetParameterValue("developer", _session.GetString("developer"));
                    webReport.Report.SetParameterValue("phone", _session.GetString("phone"));
                    webReport.Report.SetParameterValue("companyName", _session.GetString("companyName"));
                    webReport.Report.SetParameterValue("companyAddress", _session.GetString("companyAddress"));
                    int i = 0;
                    foreach (string sql in sqls)
                    {
                        webReport.Report.SetParameterValue("sql" + i, sql);
                        i++;
                    }
                    return webReport;
                }
                catch (Exception e) { _logger.LogInformation(e.Message); return null; }
            }
            return null;

        }
    }
}
