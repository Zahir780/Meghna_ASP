using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace ECL.Services.Payroll
{
    public class DashboardHRMService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public DashboardHRMService
        (
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor,
            ApplicationDbContext db
        )
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _db = db;
            commonService = new CommonService(_unitOfWork, _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }

        public IEnumerable<object> getEmployeeInfo()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                /*string sql = "select vServiceType vEmpType,(CAST(COUNT(vEmployeeCode) as int))vEmployee " +
                    "from tbEmployeeInfo where iStatus = 1 and vServiceType in ('Factory Worker','Office Staff','Management') " +
                    "group by vServiceType";*/


                string sql = "select vServiceType vEmpType,(CAST(COUNT(vEmployeeCode) as int))vEmployee " +
                    "from tbEmployeeInfo where iStatus = 1 " +
                    "group by vServiceType";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        vEmpType = sqlData["vEmpType"].ToString(),
                        vEmployee = sqlData["vEmployee"].ToString()
                    });
                }
            }
            catch (Exception exp) 
            {
                
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getYear()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vSalaryYear from tbMonthlySalary order by vSalaryYear desc";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["vSalaryYear"].ToString(),
                        Name = sqlData["vSalaryYear"].ToString()
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
        public IEnumerable<object> getMonthWithSalary(string year)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                
                string sql = "select MONTH(dSalaryDate)iMonth," +
                    "round" +
                    "(" +
                        "SUM" +
                        "(" +
                            "round" +
                            "(" +
                                "(" +
                                    "(" +
                                        "mGross + mPresentBonus + mTiffinAllowance" +
                                    ") - " +
                                    "(" +
                                        "mMealCharge + mLoanAmount + mAdvanceSalary + mCompansation + Isnull((mMobileAllowanceExtra), 0) +" +
                                        "round((mGross / iTotalDay) * iAbsentDay, 0) + round(mGross * 5 / 100, 0)" +
                                    ")" +
                                ") - mRevenueStamp - mIncomeTax, 0 " +
                            ")" +
                        "), 0" +
                    ") mNetSalary " +
                    "from tbMonthlySalarySEPL " +
                    "where vSalaryYear = @year " +
                    "group by MONTH(dSalaryDate) " +
                    "order by iMonth asc ";
                
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@year", year);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        iMonth = sqlData["iMonth"].ToString(),
                        mNetSalary = sqlData["mNetSalary"].ToString()
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
        public IEnumerable<object> getTypeWiseSalary(string month)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();

                string sql = "select vServiceType," +
                    "round" +
                    "(" +
                        "SUM" +
                        "(" +
                            "round" +
                            "(" +
                                "(" +
                                    "(" +
                                        "mGross + mPresentBonus + mTiffinAllowance" +
                                    ") - " +
                                    "(" +
                                        "mMealCharge + mLoanAmount + mAdvanceSalary + mCompansation + Isnull((mMobileAllowanceExtra), 0) +" +
                                        "round((mGross / iTotalDay) * iAbsentDay, 0) + round(mGross * 5 / 100, 0)" +
                                    ")" +
                                ") - mRevenueStamp - mIncomeTax, 0 " +
                            ")" +
                        "), 0" +
                    ") mNetSalary " +
                    "from tbMonthlySalarySEPL " +
                    //"where vSalaryYear = YEAR(getDate()) and MONTH(dSalaryDate)= @month " +
                    "where vSalaryYear = '2022' and MONTH(dSalaryDate)= '12' " +
                    "group by vServiceType " +
                    "order by vServiceType asc ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@month", month);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        vEmpType = sqlData["vServiceType"].ToString(),
                        mNetSalary = sqlData["mNetSalary"].ToString()
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
        public IEnumerable<object> getOverTimeChart()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select MONTH(dSalaryDate)iMonth," +
                    "SUM" +
                    "(" +
                        "round" +
                        "(" +
                            "(" +
                                "CASE " +
                                    "WHEN mGross < 4200 THEN ISNULL((mBasic * 1.71) / 208, 0) " +
                                    "WHEN mGross >= 4200 AND mGross < 5000 THEN ISNULL((mBasic * 1.70) / 208, 0) " +
                                    "WHEN mGross >= 5000 AND mGross < 5600 THEN ISNULL((mBasic * 1.69) / 208, 0) " +
                                    "WHEN mGross >= 5600 AND mGross < 6200 THEN ISNULL((mBasic * 1.68) / 208, 0) " +
                                    "WHEN mGross >= 6200 THEN ISNULL((mBasic * 1.675) / 208, 0)" +
                                    "else 0 " +
                                "END " +
                            ") *iTotalOTHour +" +
                            "(" +
                                "(" +
                                    "CASE " +
                                        "WHEN mGross< 4200 THEN ISNULL((mBasic* 1.71) / 208, 0) " +
                                        "WHEN mGross >= 4200 AND mGross< 5000 THEN ISNULL((mBasic* 1.70) / 208, 0) " +
                                        "WHEN mGross >= 5000 AND mGross< 5600 THEN ISNULL((mBasic* 1.69) / 208, 0) " +
                                        "WHEN mGross >= 5600 AND mGross< 6200 THEN ISNULL((mBasic* 1.68) / 208, 0) " +
                                        "WHEN mGross >= 6200 THEN ISNULL((mBasic* 1.675) / 208, 0) " +
                                        "else 0 " +
                                    "END" +
                                ")/ 60 " +
                            ") *iTotalOTMin,0" +
                        ") " +
                    ") mNetAmount " +
                    "from tbMonthlySalarySEPL where vSalaryYear = '2022' " +
                    "and(iTotalOTHour > 0 or iTotalOTMin > 0) " +
                    "group by MONTH(dSalaryDate) " +
                    "order by iMonth";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        iMonth = sqlData["iMonth"].ToString(),
                        mNetAmount = sqlData["mNetAmount"].ToString()
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

        public IEnumerable<object> getMealAllowChart()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                
                string sql = "select iMonth,sum(mNetAmount)mNetAmount from " +
                    "(" +
                        "select MONTH(dDate)iMonth," +
                        "(" +
                            "isnull(a.iMealDay, 0) * isnull((select mMealCharge from tbMealChargeInfo where MONTH(dDate) = MONTH(a.dDate) " +
                            "and YEAR(dDate) = YEAR(a.dDate)), 0) " +
                        ")mNetAmount " +
                        "from tbAttendanceSummary a " +
                        "inner join tbEmpOfficialPersonalInfo b on a.vEmployeeId = b.vEmployeeId " +
                        "where iMealDay> 0 and YEAR(a.dDate)= YEAR('2022-12-01') " +
                    ") as temp " +
                    "group by iMonth " +
                    "order by iMonth asc";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        iMonth = sqlData["iMonth"].ToString(),
                        mNetAmount = sqlData["mNetAmount"].ToString()
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











    }
}
