using FastReport.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Utility
{
    public interface IReport
    {
        public WebReport GetReport(IHttpContextAccessor _accessor, string reportpath, List<string> sqls);
    }
}
