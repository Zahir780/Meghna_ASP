using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ECL.Services.Payroll
{
    public class SectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public SectionService(
            UnitPayrollInterface unitPayroll, 
            IUnitOfWork unitOfWork, 
            IHttpContextAccessor accessor, ApplicationDbContext db)
        {
            _unitPayroll = unitPayroll;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _db = db;
            commonService = new CommonService(_unitOfWork,_accessor,_db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }
        public string getMaxSection()
        {
            return commonService.getMaxCode("vSectionId", "tbSection");
        }
        public bool nameExist(string data)
        {
            bool ret = false;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vSectionName from Section where vSectionName like @Section";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Section",data);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if(sqlData.Read())
                {
                    if(sqlData.HasRows)
                    {
                        ret = true;
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        public object finAllData(int iAutoId)
        {
            var obj = _unitPayroll.Section.GetFirstOrDefault(x => x.iAutoId == iAutoId);
            return obj;
        }
        public IEnumerable<object> getAllData()
        {
            var allObj = _unitPayroll.Section.GetAll();
            return allObj;
        }
    }
}
