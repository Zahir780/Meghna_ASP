using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
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
    public class ShiftService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public ShiftService(
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
        public string getMaxShift()
        {
            return commonService.getMaxCode("vShiftId", "tbShiftInfo");
        }
        public bool nameExist(string data)
        {
            bool ret = false;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vShiftName from Shift where vShiftName like @Shift";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Shift", data);
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
        public int shiftSave(Shift obj, bool addUpdate)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd, cmdDelete, cmdUdData;
            SqlTransaction transaction = null;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("shift");

                string sqlInsertEmployeeInfo = "insert into tbShiftInfo " +
                "(" +
                    "vShiftId,vShiftName,vShiftType,dShiftStart,dShiftEnd,dLateInLimit,dEarlyOutLimit,iHHDuration,iMMDuration,iActive," +
                    "UserId,UserIp,vCompanyId,EntryTime" +
                ") " +
                " values" +
                "(" +
                    "@vShiftId,@vShiftName,@vShiftType,@dShiftStart,@dShiftEnd,@dLateInLimit,@dEarlyOutLimit,@iHHDuration,@iMMDuration,@iActive," +
                    "@UserId,@UserIp,@vCompanyId,@EntryTime" +
                ")";
                if (addUpdate)
                {
                    /* string udSql = "insert into tbUdShiftInfo " +
                     "(" +
                         "vShiftId,vShiftName,dShiftStart,dShiftEnd,dLateInLimit,dEarlyOutLimit,iActive,vUdFlag,UserId,UserIp,vCompanyId,EntryTime" +
                     ") " +
                     "select vShiftId,vShiftName,dShiftStart,dShiftEnd,dLateInLimit,dEarlyOutLimit,iActive,'UPDATE',UserId,UserIp,vCompanyId,EntryTime," +
                     "from tbEmployeeInfo where vShiftId like @vShiftId";

                     cmdUdData = new SqlCommand(udSql, con, transaction);
                     cmdUdData.Parameters.AddWithValue("@vShiftId", obj.vShiftId);
                     cmdUdData.ExecuteNonQuery();*/

                    string sqlDelete = "delete from tbShiftInfo where vShiftId like @vShiftId";
                    cmdDelete = new SqlCommand(sqlDelete, con, transaction);
                    cmdDelete.Parameters.AddWithValue("@vShiftId", obj.vShiftId);
                    cmdDelete.ExecuteNonQuery();
                }

                cmd = new SqlCommand(sqlInsertEmployeeInfo, con, transaction);
                cmd.Parameters.AddWithValue("@vShiftId", obj.vShiftId);
                cmd.Parameters.AddWithValue("@vShiftName", obj.vShiftName);
                cmd.Parameters.AddWithValue("@vShiftType", obj.vShiftType);

                cmd.Parameters.AddWithValue("@dShiftStart", obj.dShiftStart);
                cmd.Parameters.AddWithValue("@dShiftEnd", obj.dShiftEnd);
                cmd.Parameters.AddWithValue("@dLateInLimit", obj.dLateInLimit);
                cmd.Parameters.AddWithValue("@dEarlyOutLimit", obj.dEarlyOutLimit);

                cmd.Parameters.AddWithValue("@iHHDuration", obj.iHHDuration);
                cmd.Parameters.AddWithValue("@iMMDuration", obj.iMMDuration);

                cmd.Parameters.AddWithValue("@iActive", obj.iActive);
                cmd.Parameters.AddWithValue("@UserId", commonService.getUserId());
                cmd.Parameters.AddWithValue("@UserIp", SD.getIp());
                cmd.Parameters.AddWithValue("@vCompanyId", "0");
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now);

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

        public object finAllData(string iAutoId)
        {
            object returnData = new object();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select iAutoId,vShiftId,vShiftName,vShiftType," +
                    "SUBSTRING(CAST((dShiftStart) as varchar(50)),1,2)vHHShiftStart," +
                    "SUBSTRING(CAST((dShiftStart) as varchar(50)), 4, 2)vMMShiftStart," +
                    "case when SUBSTRING(CAST((dShiftStart) as varchar(50)),1,2)< 13 then 'AM' else 'PM' end vAMPMShiftStart," +

                    " SUBSTRING(CAST((dShiftEnd) as varchar(50)), 1, 2)vHHShiftEnd," +
                    "SUBSTRING(CAST((dShiftEnd) as varchar(50)), 4, 2)vMMShiftEnd," +
                    "case when SUBSTRING(CAST((dShiftEnd) as varchar(50)),1,2)< 13 then 'AM' else 'PM' end vAMPMShiftEnd," +

                    " SUBSTRING(CAST((dLateInLimit) as varchar(50)), 1, 2)vHHLateInLimit," +
                    "SUBSTRING(CAST((dLateInLimit) as varchar(50)), 4, 2)vMMLateInLimit," +
                    "case when SUBSTRING(CAST((dLateInLimit) as varchar(50)),1,2)< 13 then 'AM' else 'PM' end vAMPMLateInLimit," +

                    " SUBSTRING(CAST((dEarlyOutLimit) as varchar(50)), 1, 2)vHHEarlyOutLimit," +
                    "SUBSTRING(CAST((dEarlyOutLimit) as varchar(50)), 4, 2)vMMEarlyOutLimit," +
                    "case when SUBSTRING(CAST((dEarlyOutLimit) as varchar(50)),1,2)< 13 then 'AM' else 'PM' end vAMPMEarlyOutLimit," +

                    "iHHDuration,iMMDuration,iActive,vCompanyId,UserId,UserIp,EntryTime " +
                    "from tbShiftInfo where iAutoId like @iAutoId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@iAutoId", iAutoId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData = new
                    {
                        iAutoId = sqlData["iAutoId"].ToString(),
                        vShiftId = sqlData["vShiftId"].ToString(),
                        vShiftName = sqlData["vShiftName"].ToString(),
                        vShiftType = sqlData["vShiftType"].ToString(),

                        vHHShiftStart = sqlData["vHHShiftStart"].ToString(),
                        vMMShiftStart = sqlData["vMMShiftStart"].ToString(),
                        vAMPMShiftStart = sqlData["vAMPMShiftStart"].ToString(),

                        vHHShiftEnd = sqlData["vHHShiftEnd"].ToString(),
                        vMMShiftEnd = sqlData["vMMShiftEnd"].ToString(),
                        vAMPMShiftEnd = sqlData["vAMPMShiftEnd"].ToString(),

                        vHHLateInLimit = sqlData["vHHLateInLimit"].ToString(),
                        vMMLateInLimit = sqlData["vMMLateInLimit"].ToString(),
                        vAMPMLateInLimit = sqlData["vAMPMLateInLimit"].ToString(),

                        vHHEarlyOutLimit = sqlData["vHHEarlyOutLimit"].ToString(),
                        vMMEarlyOutLimit = sqlData["vMMEarlyOutLimit"].ToString(),
                        vAMPMEarlyOutLimit = sqlData["vAMPMEarlyOutLimit"].ToString(),

                        iHHDuration = sqlData["iHHDuration"].ToString(),
                        iMMDuration = sqlData["iMMDuration"].ToString(),

                        iActive = sqlData["iActive"].ToString(),
                        vCompanyId = sqlData["vCompanyId"].ToString(),
                        UserId = sqlData["UserId"].ToString(),
                        UserIp = sqlData["UserIp"].ToString(),
                        EntryTime = sqlData["EntryTime"].ToString()

                        /*vHHShiftStart,vMMShiftStart,vAMPMShiftStart,
                        vHHShiftEnd,vMMShiftEnd,vAMPMShiftEnd,
                        vHHLateInLimit,vMMLateInLimit,vAMPMLateInLimit,
                        vHHEarlyOutLimit,vMMEarlyOutLimit,vAMPMEarlyOutLimit,
                        iHHDuration,iMMDuration
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
            var allObj = _unitPayroll.Shift.GetAll();
            return allObj;
        }

    }
}
