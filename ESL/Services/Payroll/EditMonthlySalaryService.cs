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
    public class EditMonthlySalaryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public EditMonthlySalaryService(
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
        public IEnumerable<object> GetSalary (string pSalaryDate, string pEmployeeId, string pDepartmentId, string pSectionId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vEmployeeId,vEmployeeCode,vEmployeeName,vDepartmentName,vSectionName,vServiceType," +
                    "iTotalDay,iHoliday,iWorkingDay,iLeaveDay,iPresentDay,iNetPayableDays," +

                    "mBasic,mHouseRent,mMedicalAllowance,mGrossSalary,mNetPayableSalary,iTotalOTHour,mOtRate,mFridayRate,iHolidayPresentCount,mOtAmount," +
                    "mOtherEarning,mGrossPayable,mIncomeTax,mOtherDeduction,mRevenueStamp,mAdvanceSalary,mTotalDeduction,mPayableSalary " +
                    "from tbMonthlySalary " +
                    "where dSalaryDate like @pSalaryDate and vEmployeeId like @pEmployeeId " +
                    "and vDepartmentId like @pDepartmentId and vSectionId like @pSectionId " +
                    "order by iRank,dJoiningDate ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@pSalaryDate", pSalaryDate);
                cmd.Parameters.AddWithValue("@pEmployeeId", pEmployeeId);
                cmd.Parameters.AddWithValue("@pDepartmentId", pDepartmentId);
                cmd.Parameters.AddWithValue("@pSectionId", pSectionId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                /*
                
                iRank,dJoiningDate*/
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        vEmployeeId = sqlData["vEmployeeId"].ToString(),
                        vEmployeeCode = sqlData["vEmployeeCode"].ToString(),
                        vEmployeeName = sqlData["vEmployeeName"].ToString(),
                        vDepartmentName = sqlData["vDepartmentName"].ToString(),
                        vSectionName = sqlData["vSectionName"].ToString(),
                        vServiceType = sqlData["vServiceType"].ToString(),

                        iTotalDay = sqlData["iTotalDay"].ToString(),
                        iHoliday = sqlData["iHoliday"].ToString(),
                        iWorkingDay = sqlData["iWorkingDay"].ToString(),
                        iLeaveDay = sqlData["iLeaveDay"].ToString(),
                        iPresentDay = sqlData["iPresentDay"].ToString(),
                        iNetPayableDays = sqlData["iNetPayableDays"].ToString(),

                        mBasic = sqlData["mBasic"].ToString(),
                        mHouseRent = sqlData["mHouseRent"].ToString(),
                        mMedicalAllowance = sqlData["mMedicalAllowance"].ToString(),
                        mGrossSalary = sqlData["mGrossSalary"].ToString(),
                        mNetPayableSalary = sqlData["mNetPayableSalary"].ToString(),
                        iTotalOTHour = sqlData["iTotalOTHour"].ToString(),
                        mOtRate = sqlData["mOtRate"].ToString(),
                        mFridayRate = sqlData["mFridayRate"].ToString(),
                        iHolidayPresentCount = sqlData["iHolidayPresentCount"].ToString(),
                        mOtAmount = sqlData["mOtAmount"].ToString(),
                        mOtherEarning = sqlData["mOtherEarning"].ToString(),
                        mGrossPayable = sqlData["mGrossPayable"].ToString(),
                        mIncomeTax = sqlData["mIncomeTax"].ToString(),
                        mOtherDeduction = sqlData["mOtherDeduction"].ToString(),
                        mRevenueStamp = sqlData["mRevenueStamp"].ToString(),
                        mAdvanceSalary = sqlData["mAdvanceSalary"].ToString(),
                        mTotalDeduction = sqlData["mTotalDeduction"].ToString(),
                        mPayableSalary = sqlData["mPayableSalary"].ToString(),
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
        public int updateSalary( List<Salary> objList, string pSalaryDate, string pEmployeeId, string pDepartmentId, string pSectionId )
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlTransaction transaction = null;
            SqlCommand cmd;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("Update");
                string sql = "prcSalaryUpdate";
                foreach (var item in objList)
                {
                    cmd = new SqlCommand(sql, con, transaction);
                    cmd.CommandType = CommandType.StoredProcedure;

                    /*
                    pSalaryDate,pDepartmentId,pSectionId,
                    vEmployeeId,iTotalDay,iHoliday,iLeaveDay,iPresentDay,iNetPayableDays,
                    mGrossSalary,mNetPayableSalary,iTotalOTHour,mOtRate,mFridayRate,iHolidayPresentCount,mOtAmount,
                    mOtherEarning,mGrossPayable,mIncomeTax,mOtherDeduction,mRevenueStamp,mAdvanceSalary,mTotalDeduction,mPayableSalary
                    */

                    cmd.Parameters.AddWithValue("@pSalaryDate", pSalaryDate);
                    cmd.Parameters.AddWithValue("@pDepartmentId", pDepartmentId);
                    cmd.Parameters.AddWithValue("@pSectionId", pSectionId);

                    cmd.Parameters.AddWithValue("@pEmployeeId", item.vEmployeeId);
                    cmd.Parameters.AddWithValue("@pTotalDay", item.iTotalDay);
                    cmd.Parameters.AddWithValue("@pHoliday", item.iHoliday);
                    cmd.Parameters.AddWithValue("@pLeaveDay", item.iLeaveDay);
                    cmd.Parameters.AddWithValue("@pPresentDay", item.iPresentDay);
                    cmd.Parameters.AddWithValue("@pNetPayableDays", item.iNetPayableDays);

                    cmd.Parameters.AddWithValue("@mGrossSalary", item.mGrossSalary);
                    cmd.Parameters.AddWithValue("@mNetPayableSalary", item.mNetPayableSalary);
                    cmd.Parameters.AddWithValue("@iTotalOTHour", item.iTotalOTHour);
                    cmd.Parameters.AddWithValue("@mOtRate", item.mOtRate);
                    cmd.Parameters.AddWithValue("@mFridayRate", item.mFridayRate);
                    cmd.Parameters.AddWithValue("@iHolidayPresentCount", item.iHolidayPresentCount);
                    cmd.Parameters.AddWithValue("@mOtAmount", item.mOtAmount);
                    cmd.Parameters.AddWithValue("@mOtherEarning", item.mOtherEarning);
                    cmd.Parameters.AddWithValue("@mGrossPayable", item.mGrossPayable);
                    cmd.Parameters.AddWithValue("@mIncomeTax", item.mIncomeTax);
                    cmd.Parameters.AddWithValue("@mOtherDeduction", item.mOtherDeduction);
                    cmd.Parameters.AddWithValue("@mRevenueStamp", item.mRevenueStamp);
                    cmd.Parameters.AddWithValue("@mAdvanceSalary", item.mAdvanceSalary);
                    cmd.Parameters.AddWithValue("@mTotalDeduction", item.mTotalDeduction);
                    cmd.Parameters.AddWithValue("@mPayableSalary", item.mPayableSalary);

                    cmd.Parameters.AddWithValue("@pCompanyId", "0");
                    cmd.Parameters.AddWithValue("@pUserId", commonService.getUserId());
                    cmd.Parameters.AddWithValue("@pUserIP", SD.getIp());
                    cmd.Parameters.AddWithValue("@pEntryTime", DateTime.Now);
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

        public IEnumerable<object> getSalaryDate()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();

                string sql = "select distinct top 12 cast(CONVERT(date, (SELECT DATEADD(s, -1, DATEADD(mm, DATEDIFF(m, 0, dSalaryDate) + 1, 0)))) as varchar)id," +
                        "cast(DATENAME(MONTH, dSalaryDate) as varchar) + '-' + cast(YEAR(dSalaryDate) as varchar)name " +
                        "from tbMonthlySalary order by name desc";

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
                string sql = "select distinct vDepartmentId,vDepartmentName from tbMonthlySalary " +
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
                string sql = "select distinct vSectionId,vSectionName from tbMonthlySalary " +
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
                string sql = "select distinct vEmployeeId,vEmployeeCode,(vEmployeeCode+'-'+vEmployeeName) vEmployee from tbMonthlySalary " +
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
