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
    public class DeleteAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public DeleteAttendanceService(
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

        public int deleteAttendance(string dDate, string pEmployeeId)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmdUdData,cmdDeleteAttendanceInfo;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Delete");

                string udSql = "insert into tbUDEmployeeAttendance " +
                "(" +
                    "dDate, vEmployeeID, vEmployeeCode, vFingerID, vEmployeeName, vDesignationID, vDesignation," +
                    "vDepartmentID, vDepartmentName, vSectionID, vSectionName, dAttDate, dAttInTime, dAttOutTime, permittedBy, vReason," +
                    "vShiftID, udFlag, vPAFlag, vUserID, vUserIP, dEntryTime" +
                ")" +
                "select dDate, vEmployeeID, vEmployeeCode, vFingerID, vEmployeeName, vDesignationID, vDesignationName," +
                "vDepartmentID, vDepartmentName, vSectionId, vSectionName, dDate, dInTimeFirst, dOutTimeFirst, 'N/A', 'N/A'," +
                "vShiftID,'DELETE',vAttendFlag,vUserID,vUserIp,dEntryTime " +
                "from tbEmployeeAttendanceFinal " +
                "where vEmployeeID = @pEmployeeId and dDate = @dDate ";

                cmdUdData = new SqlCommand(udSql, con, transaction);
                cmdUdData.Parameters.AddWithValue("@dDate", dDate);
                cmdUdData.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                cmdUdData.ExecuteNonQuery();

                /*DeleteAttendanceInfo Start*/
                string sqlDeleteAttendanceInfo = "delete from tbEmployeeAttendanceFinal where vEmployeeId = @pEmployeeId and dDate = @dDate";
                cmdDeleteAttendanceInfo = new SqlCommand(sqlDeleteAttendanceInfo, con, transaction);
                cmdDeleteAttendanceInfo.Parameters.AddWithValue("@dDate", dDate);
                cmdDeleteAttendanceInfo.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                cmdDeleteAttendanceInfo.ExecuteNonQuery();
                /*DeleteAttendanceInfo Start*/


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
        public IEnumerable<object> getDepartment(string dDate)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vDepartmentId vId,vDepartmentName vName from tbEmployeeAttendanceFinal " +
                        "where dDate = '" + dDate + "' " +
                        "order by vDepartmentName";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["vId"].ToString(),
                        Name = sqlData["vName"].ToString()
                    });
                }
            }
            finally { con.Close(); }
            return returnData;
        }
        public IEnumerable<object> getSection(string dDate, string pDepartmentId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vSectionId vId,vSectionName vName from tbEmployeeAttendanceFinal " +
                        "where dDate = '" + dDate + "' and vDepartmentId like '" + pDepartmentId + "' " +
                        "order by vName";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["vId"].ToString(),
                        Name = sqlData["vName"].ToString()
                    });
                }
            }
            finally { con.Close(); }
            return returnData;
        }
        public IEnumerable<object> getEmployee(string dDate, string pDepartmentId, string pSectionId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vEmployeeId,vEmployeeCode,(vEmployeeCode+'-'+vEmployeeName) vEmployee from tbEmployeeAttendanceFinal " +
                    "where dDate ='" + dDate + "' and vDepartmentId like '" + pDepartmentId + "' " +
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
