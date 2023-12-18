using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Services;
using ESL.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ECL.Services.Payroll
{
    public class DesignationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public DesignationService(
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
        public string getMaxDesignation()
        {
            return commonService.getMaxCode("vDesignationId", "tbDesignation");
        }
        public bool nameExist(string data)
        {
            bool ret = false;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vDesignationName from Designation where vDesignationName like @Designation";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Designation",data);
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
        public IEnumerable<object> getRank()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct iRank,cast(iRank as varchar)+'-'+vRemarks vName from tbDesignationRank";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["iRank"].ToString(),
                        Name = sqlData["vName"].ToString()
                    });
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }


        public object finAllData(int iAutoId)
        {
            object returnData = new object();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select a.iAutoId,vDesignationID,vDesignationCode,vDesignationName,vDesignationNameBangla," +
                    "a.iRank,a.iActive,a.vCompanyId,a.UserId,a.UserIp,a.EntryTime," +
                    "cast(a.iRank as varchar)+'-'+vRemarks vRemarks from tbDesignation a " +
                    "inner join tbDesignationRank b on a.iRank = b.iRank " +
                    "where a.iAutoId like @iAutoId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@iAutoId", iAutoId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData = new
                    {
                        iAutoId = sqlData["iAutoId"].ToString(),
                        vDesignationID = sqlData["vDesignationID"].ToString(),
                        vDesignationCode = sqlData["vDesignationCode"].ToString(),
                        vDesignationName = sqlData["vDesignationName"].ToString(),
                        vDesignationNameBangla = sqlData["vDesignationNameBangla"].ToString(),
                        iRank = sqlData["iRank"].ToString(),
                        iActive = sqlData["iActive"].ToString(),
                        vCompanyId = sqlData["vCompanyId"].ToString(),
                        UserId = sqlData["UserId"].ToString(),
                        UserIp = sqlData["UserIp"].ToString(),
                        //EntryTime = sqlData["EntryTime"].ToString(),
                        vRemarks = sqlData["vRemarks"].ToString(),

                        /*
                            mBasic = SD.decFZero(sqlData["mBasic"].ToString()),
                            mEducation = SD.decFZero(sqlData["mEducation"].ToString()),
                            vTIN = sqlData["vTIN"].ToString()
                        */
                    };
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;




        }
        public IEnumerable<object> getAllData()
        {
            var allObj = _unitPayroll.Designation.GetAll();
            return allObj;
        }
    }
}
