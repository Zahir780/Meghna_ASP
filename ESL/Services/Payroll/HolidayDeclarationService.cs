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
    public class HolidayDeclarationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public HolidayDeclarationService(
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
        public int HolidaySave(Holiday objHolidayInfo)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd;
            SqlTransaction transaction = null;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Holiday");
                string sqlInsertHolidayInfo = "insert into tbHoliday " +
                "(" +
                    "dDate,vOccasion,UserIp,UserId,vCompanyId,EntryTime" +
                ") " +
                " values" +
                "(" +
                    "@dDate,@vOccasion,@UserIp,@UserId,@vCompanyId,@EntryTime" +
                ")";

                cmd = new SqlCommand(sqlInsertHolidayInfo, con, transaction);
                cmd.Parameters.AddWithValue("@dDate", objHolidayInfo.dDate);
                cmd.Parameters.AddWithValue("@vOccasion", objHolidayInfo.vOccasion);
                cmd.Parameters.AddWithValue("@vCompanyId", "1");
                cmd.Parameters.AddWithValue("@UserId", commonService.getUserId());
                cmd.Parameters.AddWithValue("@UserIp", SD.getIp());
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                cmd.ExecuteNonQuery();
                ret = 1;
                transaction.Commit();
            }
            catch (Exception exp) {
                if (transaction != null) {
                    transaction.Rollback();
                }
            }
            finally {
                con.Close();
            }
            return ret;
        }
        public int HolidayDelete(Holiday objHolidayInfo)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd;
            SqlTransaction transaction = null;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Holiday");
                string sqlInsertHolidayInfo = "delete from tbHoliday where dDate= @dDate";
                cmd = new SqlCommand(sqlInsertHolidayInfo, con, transaction);
                cmd.Parameters.AddWithValue("@dDate", objHolidayInfo.dDate);
                cmd.ExecuteNonQuery();
                ret = 1;
                transaction.Commit();
            }
            catch (Exception exp) {
                if (transaction != null) {
                    transaction.Rollback();
                }
            }
            finally {
                con.Close();
            }
            return ret;
        }
        public IEnumerable<object> getFindData(string dFindDate)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select iAutoId,dDate,vOccasion,vCompanyId,UserId,UserIp,EntryTime " +
                    "from tbHoliday where MONTH(dDate)=MONTH(@dFindDate) and YEAR(dDate)=YEAR(@dFindDate) ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@dFindDate", dFindDate);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        iAutoId = sqlData["iAutoId"],
                        dDate = sqlData["dDate"],
                        vOccasion = sqlData["vOccasion"],
                        vCompanyId = sqlData["vCompanyId"],
                        UserId = sqlData["UserId"],
                        UserIp = sqlData["UserIp"],
                        EntryTime = sqlData["EntryTime"],
                    });
                }
            }
            catch (Exception exp)
            {
                var v = exp;
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getAll()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select iAutoId,dDate,vOccasion,vCompanyId,UserId,UserIp,EntryTime from tbHoliday";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        iAutoId = sqlData["iAutoId"],
                        dDate = sqlData["dDate"],
                        vOccasion = sqlData["vOccasion"],
                        vCompanyId = sqlData["vCompanyId"],
                        UserId = sqlData["UserId"],
                        UserIp = sqlData["UserIp"],
                        EntryTime = sqlData["EntryTime"],
                    });
                }
            }
            catch (Exception exp)
            {
                var v = exp;
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }

    }
}
