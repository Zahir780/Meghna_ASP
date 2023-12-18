using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Services;
using ESL.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ECL.Services.Payroll
{
    public class GenerateMonthlySalaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public GenerateMonthlySalaryService(
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

        public int Generate(
            string pGenerateDate, string pSalaryMonthDate, string pEmployeeId,
            string pDepartmentId, string pSectionId, string pRevenueStamp
        )
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Generate");
                string sql = "prcCalcMonthlySalaryMachine";

                cmd = new SqlCommand(sql, con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pGenerateDate", pGenerateDate);
                cmd.Parameters.AddWithValue("@pSalaryMonthDate", pSalaryMonthDate);
                cmd.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                cmd.Parameters.AddWithValue("@pDepartmentId", pDepartmentId);
                cmd.Parameters.AddWithValue("@pSectionId", pSectionId);
                cmd.Parameters.AddWithValue("@pRevenueStamp", pRevenueStamp);
                cmd.Parameters.AddWithValue("@pUserId", commonService.getUserId());
                cmd.Parameters.AddWithValue("@pUserIP", SD.getIp());
                cmd.ExecuteNonQuery();

                ret = 1;
                transaction.Commit();
            }
            catch (Exception exp)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        public IEnumerable<object> getDepartment()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vDepartmentId,vDepartmentName from tbEmployeeInfo ";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["vDepartmentId"].ToString(),
                        Name = sqlData["vDepartmentName"].ToString()
                    });
                }
            }
            finally { con.Close(); }
            return returnData;
        }
        public IEnumerable<object> getSection(string pDepartmentId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vSectionId,vSectionName from tbEmployeeInfo " +
                    "where vDepartmentId like '" + pDepartmentId + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["vSectionId"].ToString(),
                        Name = sqlData["vSectionName"].ToString()
                    });
                }
            }
            finally { con.Close(); }
            return returnData;
        }
        public IEnumerable<object> getEmployee(string pDepartmentId, string pSectionId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vEmployeeId,vEmployeeCode,(vEmployeeCode+'-'+vEmployeeName) vEmployee from tbEmployeeInfo " +
                    "where vDepartmentId like '" + pDepartmentId + "' and vSectionId like '" + pSectionId + "' " +
                    "order by vEmployeeCode";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["vEmployeeId"].ToString(),
                        Name = sqlData["vEmployee"].ToString()
                    });
                }
            }
            finally { con.Close(); }
            return returnData;
        }
        public object getMonthRelatedData(string pSalaryMonthDate)
        {
            object returnData = new object();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select pTotalDays,pFridays,pHolidays,pWorkingDays from funGetMonthRelatedData('" + pSalaryMonthDate + "')";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData = new
                    {
                        pTotalDays = sqlData["pTotalDays"].ToString(),
                        pFridays = sqlData["pFridays"].ToString(),
                        pHolidays = sqlData["pHolidays"].ToString(),
                        pWorkingDays = sqlData["pWorkingDays"].ToString(),
                    };
                }
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public bool checkSalary(string pSalaryMonthDate, string pEmployeeId, string pDepartmentId, string pSectionId)
        {
            bool ret = false;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vEmployeeId from tbMonthlySalary " +
                    "where dSalaryDate=@pSalaryMonthDate and vEmployeeId like @pEmployeeId " +
                    "and vDepartmentId like @pDepartmentId and vSectionId like @pSectionId ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@pSalaryMonthDate", pSalaryMonthDate);
                cmd.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                cmd.Parameters.AddWithValue("@pDepartmentId", pDepartmentId);
                cmd.Parameters.AddWithValue("@pSectionId", pSectionId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
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
    }
}
