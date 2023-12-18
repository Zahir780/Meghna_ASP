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
    public class DateBetweenAttendanceManuallyMachineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public DateBetweenAttendanceManuallyMachineService
        (
            UnitPayrollInterface unitPayroll,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor, ApplicationDbContext db)
        {
            _unitPayroll = unitPayroll;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _db = db;
            commonService = new CommonService(_unitOfWork, _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }
        public IEnumerable<object> GetAttendance (
            string pEmployeeId, string pDepartmentId, string pSectionId 
        )
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vEmployeeId,vEmployeeCode,vEmployeeName,vDepartmentName,vSectionName,''vReason," +
                    "(select top 1 iRank from tbDesignation where vDesignationID = a.vDesignationId )iRank " +
                    "from tbEmployeeInfo a where iStatus = 1 and vEmployeeId like @pEmployeeId " +
                    "and vDepartmentId like @pDepartmentId and vSectionId like @pSectionId " +
                    "order by iRank,dJoiningDate ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                cmd.Parameters.AddWithValue("@pDepartmentId", pDepartmentId);
                cmd.Parameters.AddWithValue("@pSectionId", pSectionId);
                SqlDataReader sqlData = cmd.ExecuteReader();

                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        vEmployeeId = sqlData["vEmployeeId"].ToString(),
                        vEmployeeCode = sqlData["vEmployeeCode"].ToString(),
                        vEmployeeName = sqlData["vEmployeeName"].ToString(),
                        vDepartmentName = sqlData["vDepartmentName"].ToString(),
                        vSectionName = sqlData["vSectionName"].ToString(),
                        vReason = sqlData["vReason"].ToString()
                    });
                }
            }
            catch (Exception exp) { }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public int generateAttendance (List<Attendance> objList, string dFromDate, string dToDate, string inTime, string outtime)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Attendance");
                string sql = "prcMonthlyEmployeeAttendanceDateBetween";
                foreach (var item in objList)
                {
                    cmd = new SqlCommand(sql, con, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;

                    /*dFromDate,dToDate,inTime,outtime,vEmployeeId,pDepartmentId,pSectionId,vReason,pCompanyId,pUserId,pUserIP*/

                    cmd.Parameters.AddWithValue("@dFromDate", dFromDate);
                    cmd.Parameters.AddWithValue("@dToDate", dToDate);
                    cmd.Parameters.AddWithValue("@inTime", inTime);
                    cmd.Parameters.AddWithValue("@outtime", outtime);

                    cmd.Parameters.AddWithValue("@pEmployeeId", item.vEmployeeId);
                    cmd.Parameters.AddWithValue("@vReason", item.vReason);
                    cmd.Parameters.AddWithValue("@pCompanyId", "0");
                    cmd.Parameters.AddWithValue("@pUserId", commonService.getUserId());
                    cmd.Parameters.AddWithValue("@pUserIP", SD.getIp());
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
                ret = 1;
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
                string sql = "select distinct vDepartmentId,vDepartmentName from tbEmployeeInfo " +
                    "where iStatus = 1 ";

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
                    "where iStatus = 1 and vDepartmentId like '" + pDepartmentId + "'";
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
                    "where iStatus = 1 and vDepartmentId like '" + pDepartmentId + "' " +
                    "and vSectionId like '" + pSectionId + "' " +
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
    }
}
