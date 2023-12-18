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
    public class DeleteDutyRosterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public DeleteDutyRosterService(
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

        public int deleteSalary(string pSalaryDate, string pEmployeeId, string pDepartmentId, string pSectionId)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Delete");
                string sql = "prcDeleteDutyRoster";

                cmd = new SqlCommand(sql, con, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pSalaryDate", pSalaryDate);
                cmd.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                cmd.Parameters.AddWithValue("@pDepartmentId", pDepartmentId);
                cmd.Parameters.AddWithValue("@pSectionId", pSectionId);
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
        public IEnumerable<object> getSalaryDate()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct top 12 cast(CONVERT(date, (SELECT DATEADD(s, -1, DATEADD(mm, DATEDIFF(m, 0, dSalaryDate) + 1, 0)))) as varchar)id," +
                        "cast(DATENAME(MONTH, dSalaryDate) as varchar) + '-' + cast(YEAR(dSalaryDate) as varchar)name " +
                        "from tbDutyRoster order by name desc";
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
        public IEnumerable<object> getDepartment(string pSalaryDate)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vDepartmentId,vDepartmentName from tbDutyRoster " +
                    "where dSalaryDate ='" + pSalaryDate + "' ";

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
        public IEnumerable<object> getSection(string pSalaryDate, string pDepartmentId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vSectionId,vSectionName from tbDutyRoster " +
                    "where dSalaryDate ='" + pSalaryDate + "' and vDepartmentId like '" + pDepartmentId + "'";
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
        public IEnumerable<object> getEmployee(string pSalaryDate, string pDepartmentId, string pSectionId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vEmployeeId,vEmployeeCode,(vEmployeeCode+'-'+vEmployeeName) vEmployee from tbDutyRoster " +
                    "where dSalaryDate ='" + pSalaryDate + "' and vDepartmentId like '" + pDepartmentId + "' " +
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
