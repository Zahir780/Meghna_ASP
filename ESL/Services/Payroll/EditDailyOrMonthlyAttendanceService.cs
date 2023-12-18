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
    public class EditDailyOrMonthlyAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public EditDailyOrMonthlyAttendanceService
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
        public IEnumerable<object> GetAttendance (string vAttendanceBy, string dDate, string pDepartmentId, string pSectionId, string pEmployeeId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vEmployeeId,vEmployeeCode,vEmployeeName,vDepartmentName,txtDay," +
                    "cast(dAttendanceDate as varchar(150))dAttendanceDate,inHour,inMinute," +
                    "cast(dOutDate as varchar(150))dOutDate,outHour,outMinute," +
                    "''vPermittedBy,''vReason " +
                    "from funEditDailyOrMonthlyEmployeeAttendance ( @pAttendanceBy, @pDate, @pDepartmentId, @pSectionId, @pEmployeeId ) " +
                    "order by vDepartmentName,vEmployeeCode,dAttendanceDate ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@pAttendanceBy", vAttendanceBy);
                cmd.Parameters.AddWithValue("@pDate", dDate);
                cmd.Parameters.AddWithValue("@pDepartmentId", pDepartmentId);
                cmd.Parameters.AddWithValue("@pSectionId", pSectionId);
                cmd.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                SqlDataReader sqlData = cmd.ExecuteReader();

                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        vEmployeeId = sqlData["vEmployeeId"].ToString(),
                        vEmployeeCode = sqlData["vEmployeeCode"].ToString(),
                        vEmployeeName = sqlData["vEmployeeName"].ToString(),
                        vDepartmentName = sqlData["vDepartmentName"].ToString(),
                        txtDay = sqlData["txtDay"].ToString(),

                        dAttendanceDate = sqlData["dAttendanceDate"].ToString(),
                        inHour = sqlData["inHour"].ToString(),
                        inMinute = sqlData["inMinute"].ToString(),

                        dOutDate = sqlData["dOutDate"].ToString(),
                        outHour = sqlData["outHour"].ToString(),
                        outMinute = sqlData["outMinute"].ToString(),

                        vPermittedBy = sqlData["vPermittedBy"].ToString(),
                        vReason = sqlData["vReason"].ToString()
                        /*vEmployeeId,vEmployeeCode,vEmployeeName,vDepartmentName,dAttendanceDate,inHour,inMinute,dOutDate,outHour,outMinute,vPermittedBy,vReason*/
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
        public int updateAttendance (List<UpdateAttendance> objList)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Attendance");
                string sql = "prcEditDailyOrMonthlyAttendance";
                foreach (var item in objList)
                {
                    cmd = new SqlCommand(sql, con, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;

                    /*dFromDate,dToDate,inTime,outtime,vEmployeeId,pDepartmentId,pSectionId,vReason,pCompanyId,pUserId,pUserIP*/

                    cmd.Parameters.AddWithValue("@pEmployeeId", item.vEmployeeId);
                    cmd.Parameters.AddWithValue("@dAttendanceDate", item.dAttendanceDate);
                    cmd.Parameters.AddWithValue("@inTime", item.inTime);
                    cmd.Parameters.AddWithValue("@outTime", item.outTime);
                    cmd.Parameters.AddWithValue("@vPermittedBy", item.vPermittedBy);
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

        public IEnumerable<object> getMonth()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();

                string sql = "select distinct top 12 cast(CONVERT(date, (SELECT DATEADD(s, -1, DATEADD(mm, DATEDIFF(m, 0, dDate) + 1, 0)))) as varchar)id," +
                        "cast(DATENAME(MONTH, dDate) as varchar) + '-' + cast(YEAR(dDate) as varchar)name " +
                        "from tbEmployeeAttendanceFinal order by id desc";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["id"].ToString(),
                        Name = sqlData["name"].ToString()
                    });
                }
            }
            finally { con.Close(); }
            return returnData;
        }
        public IEnumerable<object> getDepartment(string vAttendanceBy, string dDate)
        {
            string whereOption = "";
            if (vAttendanceBy == "Date") { whereOption = " dDate='"+dDate+"' "; }
            else { whereOption = " YEAR(dDate)=YEAR('" + dDate + "') and MONTH(dDate)=MONTH('" + dDate + "') "; }

            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vDepartmentId,vDepartmentName from tbEmployeeAttendanceFinal where " + whereOption+ " order by vDepartmentName ";

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
        public IEnumerable<object> getSection(string vAttendanceBy, string dDate, string pDepartmentId)
        {
            string whereOption = "";
            if (vAttendanceBy == "Date") { whereOption = " dDate='"+dDate+"' "; }
            else { whereOption = " YEAR(dDate)=YEAR('" + dDate + "') and MONTH(dDate)=MONTH('" + dDate + "') "; }

            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vSectionId,vSectionName from tbEmployeeAttendanceFinal " +
                    "where " + whereOption + " and vDepartmentId like '" + pDepartmentId + "'";
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
        public IEnumerable<object> getEmployee(string vAttendanceBy, string dDate, string pDepartmentId, string pSectionId)
        {
            string whereOption = "";
            if (vAttendanceBy == "Date") { whereOption = " dDate='"+dDate+"' "; }
            else { whereOption = " YEAR(dDate)=YEAR('" + dDate + "') and MONTH(dDate)=MONTH('" + dDate + "') "; }

            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vEmployeeId,vEmployeeCode,(vEmployeeCode+'-'+vEmployeeName) vEmployee from tbEmployeeAttendanceFinal " +
                    "where " + whereOption + " and vDepartmentId like '" + pDepartmentId + "' " +
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
