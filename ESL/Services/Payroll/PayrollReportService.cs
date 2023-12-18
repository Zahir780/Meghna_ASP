using ESL.DataAccess.Data;
using ESL.DataAccess.Repository;
using ESL.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ESL.Services.Payroll
{
    public class PayrollReportService
    {
        private readonly UnitPayrollInterface _unitPayroll;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _accessor;
        string sqlCon;
        List<object> returnData;
        SqlConnection connection;
        SqlCommand command;
        SqlDataReader reader;

        public PayrollReportService
        (
            UnitPayrollInterface unitPayroll,
            IUnitOfWork unitOfWork,
            ApplicationDbContext db,
            IHttpContextAccessor accessor
        )
        {
            _unitPayroll = unitPayroll;
            _unitOfWork = unitOfWork;
            _db = db;
            _accessor = accessor;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }
        public IEnumerable<object> getServiceType(string type, string dDate, string vDepartmentId, string vSectionId)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if(type == "RptDailyAttendanceStatement" || type == "RptDailyAbsentStatement" || type == "RptDailyLateInEmployeeList" || type == "RptDailyEarlyOutEmployeeList")
                {
                    sql = "select distinct b.vServiceType vName from tbEmployeeAttendanceFinal a " +
                        "inner join tbEmployeeInfo b on a.vEmployeeID = b.vEmployeeId " +
                        "where a.dDate='" + dDate + "' and a.vDepartmentId like'" + vDepartmentId + "' and a.vSectionId like '" + vSectionId + "' " +
                        "order by b.vServiceType ";
                }
                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vName"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getEmployee(string type, string vStatus, string dDate, string vMonth, string vDepartmentId, string vSectionId, string vSalaryType)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if(type == "RptEmployeeList")
                {
                    sql = "select distinct vEmployeeId vId,vEmployeeCode+'-'+vEmployeeName vName,cast(vEmployeeCode as int) from tbEmployeeInfo " +
                        "where iStatus like '" + vStatus + "' and vDepartmentId like '" + vDepartmentId + "' and vSectionId like '" + vSectionId + "' " +
                        "order by cast(vEmployeeCode as int) ";
                }
                else if
                (
                    type == "RptEmployeeProfileBioData" || type == "RptEditEmployeeInfo" ||
                    type == "RptApplicationAndAppointmentLetter" || type == "RptCertification"
                )
                {
                    sql = "select distinct vEmployeeId vId,vEmployeeCode+'-'+vEmployeeName vName,cast(vEmployeeCode as int) from tbEmployeeInfo " +
                        "order by cast(vEmployeeCode as int) ";
                }
                else if (type == "RptEmployeeList")
                {
                    sql = "select distinct vEmployeeId vId,vEmployeeCode+'-'+vEmployeeName vName,cast(vEmployeeCode as int) from tbEmployeeInfo " +
                        "where iStatus like '" + vStatus + "' " +
                        "order by cast(vEmployeeCode as int)";
                }
                else if
                (
                    type == "RptIndividualEmployeeAttendance" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance"
                )
                {
                    sql = "select distinct vEmployeeId vId,vEmployeeCode+'-'+vEmployeeName vName,cast(vEmployeeCode as int) from tbEmployeeAttendanceFinal " +
                        "where YEAR(dDate)=YEAR('" + vMonth + "') and MONTH(dDate)=MONTH('" + vMonth + "') " +
                        "and vDepartmentId like '" + vDepartmentId + "' and vSectionId like '" + vSectionId + "'  " +
                        "order by cast(vEmployeeCode as int)";
                }

                else if
               (
                   type == ("RptMonthlyDutyRoster")
               )
                {
                    sql = "select distinct vEmployeeId vId,vEmployeeCode+'-'+vEmployeeName vName,cast(vEmployeeCode as int) from tbDutyRoster " +
                        "where YEAR(dFromDate)=YEAR('" + vMonth + "') and MONTH(dFromDate)=MONTH('" + vMonth + "') " +
                        "and vDepartmentId like '" + vDepartmentId + "' and vSectionId like '" + vSectionId + "'  " +
                        "order by cast(vEmployeeCode as int)";
                }


                else if(type == "RptMonthlySalary" || type == "RptPaySlip")
                {
                    sql = "select distinct vEmployeeId vId,vEmployeeCode+'-'+vEmployeeName vName,cast(vEmployeeCode as int) from tbMonthlySalary " +
                        "where YEAR(dSalaryDate) = YEAR('" + vMonth + "') and MONTH(dSalaryDate)= MONTH('" + vMonth + "') " +
                        "and vDepartmentId like '" + vDepartmentId + "' and vSectionId like '" + vSectionId + "' and ";

                    if (vSalaryType == "Office Staff")
                    {
                        sql = sql + " vServiceType in ('Management','Office Staff') order by cast(vEmployeeCode as int) ";
                    }
                    else if (vSalaryType == "Factory")
                    {
                        sql = sql + " vServiceType in ('Supervisor','Driver','Factory Worker') order by cast(vEmployeeCode as int) ";
                    }
                    else if (vSalaryType == "Security")
                    {
                        sql = sql + " vServiceType in ('Security') order by cast(vEmployeeCode as int) ";
                    }
                }
                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vId"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }

            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getMonth(string type,string vSalaryType)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if
                (
                    type == "RptIndividualEmployeeAttendance" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance"
                )
                {
                    sql = "select distinct top 12 cast(CONVERT(date, (SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,dDate)+1,0)))) as varchar)vId," +
                        "cast(DATENAME(MONTH, dDate) as varchar) + '-' + cast(YEAR(dDate) as varchar)vName,YEAR(dDate),MONTH(dDate) " +
						"from tbEmployeeAttendanceFinal order by vId desc ";
                }
                else if
                (
                    type == "RptMonthlySalary"  
                )
                {
                    sql = "select distinct top 24 cast(CONVERT(date, (SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,dSalaryDate)+1,0)))) as varchar)vId," +
                        "cast(DATENAME(MONTH, dSalaryDate) as varchar) + '-' + cast(YEAR(dSalaryDate) as varchar)vName,YEAR(dSalaryDate),MONTH(dSalaryDate) " +
                        "from tbMonthlySalary ";

                    if (vSalaryType== "Office Staff") {
                        sql = sql + " where vServiceType in ('Management','Office Staff') order by vId desc ";
                    }
                    else if (vSalaryType== "Factory") {
                        sql = sql + " where vServiceType in ('Supervisor','Driver','Factory Worker') order by vId descc ";
                    }
                    else if (vSalaryType== "Security") {
                        sql = sql + " where vServiceType in ('Security') order by vId desc ";
                    }
                    
                }
                else if
                (
                    type == "RptBankAdviceWithForwardingLatter" || type == "RptNoteRequisition" || type == "RptPaySlip"
                )
                {
                    sql = "select distinct top 24 cast(CONVERT(date, (SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,dSalaryDate)+1,0)))) as varchar)vId," +
                        "cast(DATENAME(MONTH, dSalaryDate) as varchar) + '-' + cast(YEAR(dSalaryDate) as varchar)vName,YEAR(dSalaryDate),MONTH(dSalaryDate) " +
						"from tbMonthlySalary order by vId desc ";
                }

                else if
               (
                   type == "RptMonthlyDutyRoster"
               )
                {
                    sql = "select distinct top 24 cast(CONVERT(date, (SELECT DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,dFromDate)+1,0)))) as varchar)vId," +
                        "cast(DATENAME(MONTH,dFromDate ) as varchar) + '-' + cast(YEAR(dFromDate) as varchar)vName,YEAR(dFromDate),MONTH(dFromDate) " +
						"from tbDutyRoster order by vId desc ";
                }

                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vId"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }

            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getDepartment(string type, string dDate, string vMonth, string vStatus, string vSalaryType)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if
                (
                    type == "RptEmployeeList"
                )
                {
                    sql = "select distinct vDepartmentId vId,vDepartmentName vName from tbEmployeeInfo " +
                        "where iStatus like '"+vStatus+"' " +
                        "order by vDepartmentName";
                }
                else if
                (
                    type == "RptDailyAttendanceStatement" || type == "RptDailyAbsentStatement" || type == "RptDailyLateInEmployeeList" ||
                    type == "RptDailyEarlyOutEmployeeList"
                )
                {
                    sql = "select distinct vDepartmentId vId,vDepartmentName vName from tbEmployeeAttendanceFinal " +
                        "where dDate = '" + dDate + "' " +
                        "order by vDepartmentName";
                }
                else if
                (
                    type == "RptIndividualEmployeeAttendance" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance"
                )
                {
                    sql = "select distinct vDepartmentId vId,vDepartmentName vName from tbEmployeeAttendanceFinal " +
                        "where YEAR(dDate)=YEAR('" + vMonth + "') and MONTH(dDate)=MONTH('" + vMonth + "')  " +
                        "order by vDepartmentName";
                }



                else if (type == "RptMonthlyDutyRoster")

                {
                    sql = "select distinct vDepartmentId vId,vDepartmentName vName from tbDutyRoster " +
                        "where YEAR(dFromDate)=YEAR('" + vMonth + "') and MONTH(dFromDate)=MONTH('" + vMonth + "')  " +
                        "order by vDepartmentName";
                }



                else if
                (
                    type == "RptMonthlySalary" || type == "RptPaySlip"
                )
                {
                    sql = "select distinct vDepartmentId vId,vDepartmentName vName from tbMonthlySalary " +
                        "where YEAR(dSalaryDate) = YEAR('" + vMonth + "') and MONTH(dSalaryDate)= MONTH('" + vMonth + "') and ";

                    if (vSalaryType == "Office Staff")
                    {
                        sql = sql + " vServiceType in ('Management','Office Staff') order by vDepartmentName ";
                    }
                    else if (vSalaryType == "Factory")
                    {
                        sql = sql + " vServiceType in ('Supervisor','Driver','Factory Worker') order by vDepartmentName ";
                    }
                    else if (vSalaryType == "Security")
                    {
                        sql = sql + " vServiceType in ('Security') order by vDepartmentName ";
                    }


                }
                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vId"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getSection(string type, string dDate, string vMonth, string vDepartmentId, string vStatus, string vSalaryType)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if
                (
                    type == "RptEmployeeList"
                )
                {
                    sql = "select distinct vSectionId vId,vSectionName vName from tbEmployeeInfo " +
                        "where iStatus like '" + vStatus + "' and vDepartmentId like'" + vDepartmentId + "' " +
                        "order by vSectionName";
                }
                else if
                (
                    type == "RptDailyAttendanceStatement" || type == "RptDailyAbsentStatement" || type == "RptDailyLateInEmployeeList" ||
                    type == "RptDailyEarlyOutEmployeeList"
                )
                {
                    sql = "select distinct vSectionId vId,vSectionName vName from tbEmployeeAttendanceFinal " +
                        "where dDate='" + dDate + "' and vDepartmentId like'" + vDepartmentId + "' " +
                        "order by vSectionName";
                }
                else if
                (
                    type == "RptIndividualEmployeeAttendance" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance"
                )
                {
                    sql = "select distinct vSectionId vId,vSectionName vName from tbEmployeeAttendanceFinal " +
                        "where YEAR(dDate)=YEAR('" + vMonth + "') and MONTH(dDate)=MONTH('" + vMonth + "')  and vDepartmentId like'" + vDepartmentId + "' " +
                        "order by vSectionName";
                }


                else if (type == "RptMonthlyDutyRoster")
             
                {
                    sql = "select distinct vSectionId vId,vSectionName vName from tbDutyRoster " +
                        "where YEAR(dFromDate)=YEAR('" + vMonth + "') and MONTH(dFromDate)=MONTH('" + vMonth + "')  and vDepartmentId like'" + vDepartmentId + "' " +
                        "order by vSectionName";
                }




                else if
                (
                    type == "RptMonthlySalary" || type == "RptPaySlip"
                )
                {
                    sql = "select distinct vSectionId vId,vSectionName vName from tbMonthlySalary " +
                        "where YEAR(dSalaryDate) = YEAR('" + vMonth + "') and MONTH(dSalaryDate)= MONTH('" + vMonth + "')  and vDepartmentId like'" + vDepartmentId + "' and ";

                    if (vSalaryType == "Office Staff")
                    {
                        sql = sql + " vServiceType in ('Management','Office Staff') order by vSectionName ";
                    }
                    else if (vSalaryType == "Factory")
                    {
                        sql = sql + " vServiceType in ('Supervisor','Driver','Factory Worker') order by vSectionName ";
                    }
                    else if (vSalaryType == "Security")
                    {
                        sql = sql + " vServiceType in ('Security') order by vSectionName ";
                    }
                }
                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vId"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getBank(string type, string vMonth)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if( type == "RptBankAdviceWithForwardingLatter" )
                {
                    //sql = "select distinct vBankId vId,vBankName vName from tbMonthlySalary " +
                    //    "where ISNULL(vBankId,'')!='' and YEAR(dSalaryDate)=YEAR('" + vMonth + "') and MONTH(dSalaryDate)=MONTH('" + vMonth + "') " +
                    //    "order by vName ";


                    sql = "select distinct Id vId,BankName vName from tbBankName order by vName";

                }
                else if( type == "RptNoteRequisition" )
                {
                    sql = "select distinct Id vId,BankName vName from tbBankName order by vName";
                }
                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vId"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }

            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }
        public IEnumerable<object> getBankBranch(string type, string vMonth)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if
                (
                    type == "RptBankAdviceWithForwardingLatter" 
                )
                {
                    //sql = "select distinct vBankBranchId vId,vBankBranchName vName from tbMonthlySalary " +
                    //    "where ISNULL(vBankId,'')!='' and YEAR(dSalaryDate)=YEAR('" + vMonth + "') and MONTH(dSalaryDate)=MONTH('" + vMonth + "') " +
                    //    "order by vName ";

                    sql = "select distinct Id vId,BranchName vName from tbBankBranch ";
                }

                else if
                (
                    type == ""
                )
                {
                    sql = "";
                }
                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vId"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }

            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }

        public IEnumerable<object> getDesignation(string type, string vCertifiedById)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if
                (
                    type == "RptCertification"
                )
                {
                    sql = "select vDesignationName from tbEmployeeInfo where vEmployeeId='"+ vCertifiedById + "'  ";
                }

                else if
                (
                    type == ""
                )
                {
                    sql = "";
                }

                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vDesignationName"].ToString(),
                            Name = reader["vDesignationName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }

        public IEnumerable<object> getCertifiedBy(string type)
        {
            returnData = new List<object>();
            connection = new SqlConnection(sqlCon);
            try
            {
                connection.Open();
                string sql = "";
                if
                (
                    type == "RptCertification"
                )
                {
                    sql = "select distinct vEmployeeId vId,vEmployeeName vName from tbEmployeeInfo order by vEmployeeName ";
                }

                if (sql != "")
                {
                    command = new SqlCommand(sql, connection);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        returnData.Add(new
                        {
                            Id = reader["vId"].ToString(),
                            Name = reader["vName"].ToString()
                        });
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return returnData;
        }
















    }
}





















