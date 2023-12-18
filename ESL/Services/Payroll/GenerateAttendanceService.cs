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
    public class GenerateAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public GenerateAttendanceService(
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

        public int Generate(string pGenerateDate, string pAttendanceDate)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Generate");
                string sql = "prcGenerateEmployeeAttendance";

                cmd = new SqlCommand(sql, con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pGenerateDate", pGenerateDate);
                cmd.Parameters.AddWithValue("@pAttendanceDate", pAttendanceDate);
                cmd.Parameters.AddWithValue("@pUserId", commonService.getUserId());
                cmd.Parameters.AddWithValue("@pUserIp", SD.getIp());
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
    }
}
