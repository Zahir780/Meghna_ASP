using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ESL.Utility;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace ESL.Services.Payroll
{
    public class EmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UnitPayrollInterface _unitPayroll;
        private IHttpContextAccessor _accessor;
        ILogger<EmployeeService> _logger;
        private ApplicationDbContext _db;
        private CommonService commonService;
        string sqlCon;
        public EmployeeService
        (
            UnitPayrollInterface unitPayroll,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor accessor, 
            ILogger<EmployeeService> logger,
            ApplicationDbContext db
        )
        {
            _unitPayroll = unitPayroll;
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _logger = logger;
            _db = db;
            commonService = new CommonService(_unitOfWork, _accessor, _db);
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }
        public string getMaxEmployee()
        {
            return commonService.getMaxCode("vEmployeeId", "tbEmployeeInfo");
        }
        public string getMaxEmployeeCode()
        {
            return commonService.getMaxCode("vEmployeeCode", "tbEmployeeInfo");
        }
        public bool employeeCodeExist(string data)
        {
            bool ret = false;
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vEmployeeCode from tbEmployeeInfo where vEmployeeCode like @vEmployeeCode";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@vEmployeeCode", data);
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
        public IEnumerable<object> getDesignation()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vDesignationId,vDesignationName from tbDesignation";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["vDesignationId"].ToString(),
                        Name = sqlData["vDesignationName"].ToString()
                    });
                }
            }
            finally{con.Close();}
            return returnData;
        }
        public IEnumerable<object> getDepartment()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vDepartmentId,vDepartmentName from tbDepartment";

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
            finally{con.Close();}
            return returnData;
        }
        public IEnumerable<object> getSection()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select distinct vSectionId,vSectionName from tbSection";

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
            finally{con.Close();}
            return returnData;
        }
        public IEnumerable<object> getBank()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select id, bankName from tbBankName";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["id"].ToString(),
                        Name = sqlData["bankName"].ToString()
                    });
                }
            }
            finally{con.Close();}
            return returnData;
        }
        public IEnumerable<object> getBankBranch()
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select id, branchName from tbBankBranch";

                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        Id = sqlData["id"].ToString(),
                        Name = sqlData["branchName"].ToString()
                    });
                }
            }
            finally{con.Close();}
            return returnData;
        }
        public int employeeSave(Employee objEmployeeInfo, List<EmpEducation> objEducation, List<EmpExperience> objExperience, bool addUpdate, string attachment)
        {
            int ret = 0;
            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd, cmdDeleteEmployeeInfo, cmdDeleteEducationInfo, cmdDeleteExperienceInfo, cmdEducation, cmdExperience, cmdUdData;
            SqlTransaction transaction = null;
            try
            {
                con.Open();
                transaction = con.BeginTransaction("employee");

                /*
                vEmployeeId,vEmployeeCode,vFingerId,vEmployeeName,vEmployeeNameBangla,vDesignationId,vDesignationName,vDepartmentId,
                vDepartmentName,vSectionId,vSectionName,vReligion,vContactNo,vEmailAddress,vGender,dDateOfBirth,vNationality,vNationalIdNo,
                vEmployeeType,vServiceType,dApplicationDate,dInterviewDate,dJoiningDate,dConfirmationDate,vProbationPeriod,
                vEmployeeStatus,iStatus,dStatusDate,vEmployeePhoto,vFatherName,vMotherName,vPresentAddress,vPermanentAddress,vBloodGroup,
                vMaritalStatus,dMarriageDate,vSpouseName,vSpouseOccupation,iNumberOfChild,
                vBankId,vBankName,vBankBranchId,vBankBranchName,vAccountNo,vMoneyTransferType,vRoutingNo,vAccountMobileNo,
                mBasic,mHouseRent,mMedicalAllowance,mGross,mIncomeTax,mProvidentFund 
                */


                string sqlInsertEmployeeInfo = "insert into tbEmployeeInfo " +
                "(" +
                    "vEmployeeId,vEmployeeCode,vFingerId,vEmployeeName,vEmployeeNameBangla,vDesignationId,vDesignationName,vDepartmentId," +
                    "vDepartmentName,vSectionId,vSectionName,vReligion,vContactNo,vEmailAddress,vGender,dDateOfBirth,vNationality,vNationalIdNo," +
                    "vEmployeeType,vServiceType,dApplicationDate,dInterviewDate,dJoiningDate,dConfirmationDate,vProbationPeriod," +
                    "vEmployeeStatus,iStatus,dStatusDate,vEmployeePhoto,vFatherName,vMotherName,vPresentAddress,vPermanentAddress,vBloodGroup," +
                    "vMaritalStatus,dMarriageDate,vSpouseName,vSpouseOccupation,iNumberOfChild," +
                    "vBankId,vBankName,vBankBranchId,vBankBranchName,vAccountNo,vMoneyTransferType,vRoutingNo,vAccountMobileNo," +
                    "mBasic,mHouseRent,mMedicalAllowance,mGross,mIncomeTax,mProvidentFund,UserIp,UserId,vCompanyId,EntryTime" +
                ") " +
                " values" +
                "(" +
                    "@vEmployeeId,@vEmployeeCode,@vFingerId,@vEmployeeName,@vEmployeeNameBangla,@vDesignationId,@vDesignationName,@vDepartmentId," +
                    "@vDepartmentName,@vSectionId,@vSectionName,@vReligion,@vContactNo,@vEmailAddress,@vGender,@dDateOfBirth,@vNationality,@vNationalIdNo," +
                    "@vEmployeeType,@vServiceType,@dApplicationDate,@dInterviewDate,@dJoiningDate,@dConfirmationDate,@vProbationPeriod," +
                    "@vEmployeeStatus,@iStatus,@dStatusDate,@vEmployeePhoto,@vFatherName,@vMotherName,@vPresentAddress,@vPermanentAddress,@vBloodGroup," +
                    "@vMaritalStatus,@dMarriageDate,@vSpouseName,@vSpouseOccupation,@iNumberOfChild," +
                    "@vBankId,@vBankName,@vBankBranchId,@vBankBranchName,@vAccountNo,@vMoneyTransferType,@vRoutingNo,@vAccountMobileNo," +
                    "@mBasic,@mHouseRent,@mMedicalAllowance,@mGross,@mIncomeTax,@mProvidentFund,@UserIp,@UserId,@vCompanyId,@EntryTime" +
                ")";
                if (addUpdate)
                {
                    string udSql = "insert into tbUdEmployeeInfo " +
                    "(" +
                        "vEmployeeId,vEmployeeCode,vFingerId,vEmployeeName,vEmployeeNameBangla,vDesignationId,vDesignationName," +
                        "vDepartmentId,vDepartmentName,vSectionId,vSectionName,vReligion,vContactNo,vEmailAddress,vGender,dDateOfBirth,vNationality," +
                        "vNationalIdNo,vEmployeeType,vServiceType,dApplicationDate,dInterviewDate,dJoiningDate,dConfirmationDate,vProbationPeriod," +
                        "vEmployeeStatus,iStatus,dStatusDate,vEmployeePhoto,vFatherName,vMotherName,vPresentAddress,vPermanentAddress,vBloodGroup," +
                        "vMaritalStatus,dMarriageDate,vSpouseName,vSpouseOccupation,iNumberOfChild,vBankId,vBankName,vBankBranchId,vBankBranchName," +
                        "vAccountNo,vMoneyTransferType,vRoutingNo,vAccountMobileNo,mBasic,mHouseRent,mMedicalAllowance,mGross,mIncomeTax,mProvidentFund," +
                        "vUdFlag,UserIp,UserId,vCompanyId,EntryTime" +
                    ") " +
                    "select vEmployeeId,vEmployeeCode,vFingerId,vEmployeeName,vEmployeeNameBangla,vDesignationId,vDesignationName," +
                    "vDepartmentId,vDepartmentName,vSectionId,vSectionName,vReligion,vContactNo,vEmailAddress,vGender,dDateOfBirth,vNationality," +
                    "vNationalIdNo,vEmployeeType,vServiceType,dApplicationDate,dInterviewDate,dJoiningDate,dConfirmationDate,vProbationPeriod," +
                    "vEmployeeStatus,iStatus,dStatusDate,vEmployeePhoto,vFatherName,vMotherName,vPresentAddress,vPermanentAddress,vBloodGroup," +
                    "vMaritalStatus,dMarriageDate,vSpouseName,vSpouseOccupation,iNumberOfChild,vBankId,vBankName,vBankBranchId,vBankBranchName," +
                    "vAccountNo,vMoneyTransferType,vRoutingNo,vAccountMobileNo,mBasic,mHouseRent,mMedicalAllowance,mGross,mIncomeTax,mProvidentFund," +
                    "'UPDATE',UserIp,UserId,vCompanyId,EntryTime from tbEmployeeInfo where vEmployeeId like @vEmployeeId";

                    cmdUdData = new SqlCommand(udSql, con, transaction);
                    cmdUdData.Parameters.AddWithValue("@vEmployeeId", objEmployeeInfo.vEmployeeId);
                    cmdUdData.ExecuteNonQuery();

                    string sqlDeleteEmployeeInfo = "delete from tbEmployeeInfo where vEmployeeId like @vEmployeeId";
                    cmdDeleteEmployeeInfo = new SqlCommand(sqlDeleteEmployeeInfo, con, transaction);
                    cmdDeleteEmployeeInfo.Parameters.AddWithValue("@vEmployeeId", objEmployeeInfo.vEmployeeId);
                    cmdDeleteEmployeeInfo.ExecuteNonQuery();

                    /*Education Start*/
                    string sqlDeleteEducationInfo = "delete from tbEmpEducationInfo where vEmployeeId like @vEmployeeId";
                    cmdDeleteEducationInfo = new SqlCommand(sqlDeleteEducationInfo, con, transaction);
                    cmdDeleteEducationInfo.Parameters.AddWithValue("@vEmployeeId", objEmployeeInfo.vEmployeeId);
                    cmdDeleteEducationInfo.ExecuteNonQuery();
                    /*Education Start*/

                    /*Experience Start*/
                    string sqlDeleteExperienceInfo = "delete from tbEmpExperienceInfo where vEmployeeId like @vEmployeeId";
                    cmdDeleteExperienceInfo = new SqlCommand(sqlDeleteExperienceInfo, con, transaction);
                    cmdDeleteExperienceInfo.Parameters.AddWithValue("@vEmployeeId", objEmployeeInfo.vEmployeeId);
                    cmdDeleteExperienceInfo.ExecuteNonQuery();
                    /*Experience Start*/
                }

                cmd = new SqlCommand(sqlInsertEmployeeInfo, con, transaction);
                cmd.Parameters.AddWithValue("@vEmployeeId", objEmployeeInfo.vEmployeeId);
                cmd.Parameters.AddWithValue("@vEmployeeCode", objEmployeeInfo.vEmployeeCode);
                cmd.Parameters.AddWithValue("@vFingerId", objEmployeeInfo.vFingerId);
                cmd.Parameters.AddWithValue("@vEmployeeName", objEmployeeInfo.vEmployeeName);
                cmd.Parameters.AddWithValue("@vEmployeeNameBangla", objEmployeeInfo.vEmployeeNameBangla == null ? "" : objEmployeeInfo.vEmployeeNameBangla);
                cmd.Parameters.AddWithValue("@vDesignationId", objEmployeeInfo.vDesignationId);
                cmd.Parameters.AddWithValue("@vDesignationName", objEmployeeInfo.vDesignationName);
                cmd.Parameters.AddWithValue("@vDepartmentId", objEmployeeInfo.vDepartmentId);
                cmd.Parameters.AddWithValue("@vDepartmentName", objEmployeeInfo.vDepartmentName);

                cmd.Parameters.AddWithValue("@vSectionId", objEmployeeInfo.vSectionId == "0" ? "" : objEmployeeInfo.vSectionId);
                cmd.Parameters.AddWithValue("@vSectionName", objEmployeeInfo.vSectionId == "0" ? "" : objEmployeeInfo.vSectionName);

                cmd.Parameters.AddWithValue("@vReligion", objEmployeeInfo.vReligion);
                cmd.Parameters.AddWithValue("@vContactNo", objEmployeeInfo.vContactNo);
                cmd.Parameters.AddWithValue("@vEmailAddress", objEmployeeInfo.vEmailAddress == null ? "" : objEmployeeInfo.vEmailAddress);
                cmd.Parameters.AddWithValue("@vGender", objEmployeeInfo.vGender);
                cmd.Parameters.AddWithValue("@dDateOfBirth", objEmployeeInfo.dDateOfBirth);
                cmd.Parameters.AddWithValue("@vNationality", objEmployeeInfo.vNationality);
                cmd.Parameters.AddWithValue("@vNationalIdNo", objEmployeeInfo.vNationalIdNo == null ? "" : objEmployeeInfo.vNationalIdNo);
                cmd.Parameters.AddWithValue("@vEmployeeType", objEmployeeInfo.vEmployeeType);
                cmd.Parameters.AddWithValue("@vServiceType", objEmployeeInfo.vServiceType);
                cmd.Parameters.AddWithValue("@dApplicationDate", objEmployeeInfo.dApplicationDate);
                cmd.Parameters.AddWithValue("@dInterviewDate", objEmployeeInfo.dInterviewDate);
                cmd.Parameters.AddWithValue("@dJoiningDate", objEmployeeInfo.dJoiningDate);
                cmd.Parameters.AddWithValue("@dConfirmationDate", objEmployeeInfo.dConfirmationDate);
                cmd.Parameters.AddWithValue("@vProbationPeriod", objEmployeeInfo.vProbationPeriod);

                cmd.Parameters.AddWithValue("@vEmployeeStatus", objEmployeeInfo.vEmployeeStatus);
                cmd.Parameters.AddWithValue("@iStatus", objEmployeeInfo.vEmployeeStatus == "Continue" ? true : false);
                cmd.Parameters.AddWithValue("@dStatusDate", objEmployeeInfo.dStatusDate);

                cmd.Parameters.AddWithValue("@vEmployeePhoto", attachment);
                cmd.Parameters.AddWithValue("@vFatherName", objEmployeeInfo.vFatherName);
                cmd.Parameters.AddWithValue("@vMotherName", objEmployeeInfo.vMotherName);
                cmd.Parameters.AddWithValue("@vPresentAddress", objEmployeeInfo.vPresentAddress == null ? "" : objEmployeeInfo.vPresentAddress);
                cmd.Parameters.AddWithValue("@vPermanentAddress", objEmployeeInfo.vPermanentAddress == null ? "" : objEmployeeInfo.vPermanentAddress);
                cmd.Parameters.AddWithValue("@vBloodGroup", objEmployeeInfo.vBloodGroup == "0" ? "" : objEmployeeInfo.vBloodGroup);

                cmd.Parameters.AddWithValue("@vMaritalStatus", objEmployeeInfo.vMaritalStatus);
                cmd.Parameters.AddWithValue("@dMarriageDate", objEmployeeInfo.dMarriageDate);
                cmd.Parameters.AddWithValue("@vSpouseName", objEmployeeInfo.vSpouseName == null ? "" : objEmployeeInfo.vSpouseName);
                cmd.Parameters.AddWithValue("@vSpouseOccupation", objEmployeeInfo.vSpouseOccupation == null ? "" : objEmployeeInfo.vSpouseOccupation);
                cmd.Parameters.AddWithValue("@iNumberOfChild", objEmployeeInfo.iNumberOfChild == null ? "0" : objEmployeeInfo.iNumberOfChild);

                cmd.Parameters.AddWithValue("@mBasic", objEmployeeInfo.mBasic);
                cmd.Parameters.AddWithValue("@mHouseRent", objEmployeeInfo.mHouseRent);
                cmd.Parameters.AddWithValue("@mMedicalAllowance", objEmployeeInfo.mMedicalAllowance);
                cmd.Parameters.AddWithValue("@mGross", objEmployeeInfo.mGross);
                cmd.Parameters.AddWithValue("@mIncomeTax", objEmployeeInfo.mIncomeTax);
                cmd.Parameters.AddWithValue("@mProvidentFund", objEmployeeInfo.mProvidentFund);
                
                cmd.Parameters.AddWithValue("@vBankId", objEmployeeInfo.vBankId == "0" ? "" : objEmployeeInfo.vBankId);
                cmd.Parameters.AddWithValue("@vBankName", objEmployeeInfo.vBankId == "0" ? "" : objEmployeeInfo.vBankName);
                cmd.Parameters.AddWithValue("@vBankBranchId", objEmployeeInfo.vBankBranchId == "0" ? "" : objEmployeeInfo.vBankBranchId);
                cmd.Parameters.AddWithValue("@vBankBranchName", objEmployeeInfo.vBankBranchId == "0" ? "" : objEmployeeInfo.vBankBranchName);
                cmd.Parameters.AddWithValue("@vAccountNo", objEmployeeInfo.vAccountNo == null ? "" : objEmployeeInfo.vAccountNo);
                cmd.Parameters.AddWithValue("@vMoneyTransferType", objEmployeeInfo.vMoneyTransferType);
                cmd.Parameters.AddWithValue("@vRoutingNo", objEmployeeInfo.vRoutingNo == null ? "" : objEmployeeInfo.vRoutingNo);
                cmd.Parameters.AddWithValue("@vAccountMobileNo", objEmployeeInfo.vAccountMobileNo == null ? "" : objEmployeeInfo.vAccountMobileNo);

                cmd.Parameters.AddWithValue("@vCompanyId", "0");
                cmd.Parameters.AddWithValue("@UserId", commonService.getUserId());
                cmd.Parameters.AddWithValue("@UserIp", SD.getIp());
                cmd.Parameters.AddWithValue("@EntryTime", DateTime.Now);



                /*EmpEducation Start*/
                foreach (EmpEducation item in objEducation)
                {
                    string sqlEducation = "insert into tbEmpEducationInfo " +
                    "(" +
                        "vEmployeeId,vEmployeeName,vExamp,vGroup,vInstitute,vBoard,vDivOrClass,iYear,UserIp,UserId,vCompanyId,EntryTime" +
                    ")" +
                    "values" +
                    "(" +
                        "@vEmployeeId,@vEmployeeName,@vExamp,@vGroup,@vInstitute,@vBoard,@vDivOrClass,@iYear,@UserIp,@UserId,@vCompanyId,@EntryTime" +
                    ")";

                    cmdEducation = new SqlCommand(sqlEducation, con, transaction);
                    cmdEducation.Parameters.AddWithValue("@vEmployeeId", objEmployeeInfo.vEmployeeId);
                    cmdEducation.Parameters.AddWithValue("@vEmployeeName", objEmployeeInfo.vEmployeeName);
                    cmdEducation.Parameters.AddWithValue("@vExamp", item.vExamp);
                    cmdEducation.Parameters.AddWithValue("@vGroup", item.vGroup);
                    cmdEducation.Parameters.AddWithValue("@vInstitute", item.vInstitute);
                    cmdEducation.Parameters.AddWithValue("@vBoard", item.vBoard);
                    cmdEducation.Parameters.AddWithValue("@vDivOrClass", item.vDivOrClass);
                    cmdEducation.Parameters.AddWithValue("@iYear", item.iYear);
                    cmdEducation.Parameters.AddWithValue("@vCompanyId", "0");
                    cmdEducation.Parameters.AddWithValue("@UserId", commonService.getUserId());
                    cmdEducation.Parameters.AddWithValue("@UserIp", SD.getIp());
                    cmdEducation.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                    cmdEducation.ExecuteNonQuery();

                    _logger.LogInformation("sqlEducation:" + sqlEducation);
                }
                /*EmpEducation End*/

                /*EmpExperience Start*/
                foreach (EmpExperience item in objExperience)
                {
                    string sqlExperience = "insert into tbEmpExperienceInfo " +
                    "(" +
                        "vEmployeeId,vEmployeeName,vPostOrDesignation,vCompanyName,dFrom,dTo,vMajorResponsibility,UserIp,UserId,vCompanyId,EntryTime" +
                    ")" +
                    "values" +
                    "(" +
                        "@vEmployeeId,@vEmployeeName,@vPostOrDesignation,@vCompanyName,@dFrom,@dTo,@vMajorResponsibility,@UserIp,@UserId,@vCompanyId,@EntryTime" +
                    ")";

                    cmdExperience = new SqlCommand(sqlExperience, con, transaction);
                    cmdExperience.Parameters.AddWithValue("@vEmployeeId", objEmployeeInfo.vEmployeeId);
                    cmdExperience.Parameters.AddWithValue("@vEmployeeName", objEmployeeInfo.vEmployeeName);
                    cmdExperience.Parameters.AddWithValue("@vPostOrDesignation", item.vPostOrDesignation);
                    cmdExperience.Parameters.AddWithValue("@vCompanyName", item.vCompanyName);
                    cmdExperience.Parameters.AddWithValue("@dFrom", item.dFrom);
                    cmdExperience.Parameters.AddWithValue("@dTo", item.dTo);
                    cmdExperience.Parameters.AddWithValue("@vMajorResponsibility", item.vMajorResponsibility);
                    cmdExperience.Parameters.AddWithValue("@vCompanyId", "0");
                    cmdExperience.Parameters.AddWithValue("@UserId", commonService.getUserId());
                    cmdExperience.Parameters.AddWithValue("@UserIp", SD.getIp());
                    cmdExperience.Parameters.AddWithValue("@EntryTime", DateTime.Now);
                    cmdExperience.ExecuteNonQuery();

                    //_logger.LogInformation("sqlExperience:" + sqlExperience);
                }
                /*EmpExperience End*/

                cmd.ExecuteNonQuery();

                _logger.LogInformation("sqlInsertEmployeeInfo:" + sqlInsertEmployeeInfo);


                ret = 1;
                transaction.Commit();
            }
            catch (Exception exp)
            {
                _logger.LogInformation("Exception :" + exp);
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
        public IEnumerable<object> findEducationData(string vEmployeeId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vEmployeeId,vEmployeeName,vExamp,vGroup,vInstitute,vBoard,vDivOrClass,iYear " +
                    "from tbEmpEducationInfo where vEmployeeId like @vEmployeeId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@vEmployeeId", vEmployeeId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        vEmployeeId = sqlData["vEmployeeId"].ToString(),
                        vEmployeeName = sqlData["vEmployeeName"].ToString(),
                        vExamp = sqlData["vExamp"].ToString(),
                        vGroup = sqlData["vGroup"].ToString(),
                        vInstitute = sqlData["vInstitute"].ToString(),
                        vBoard = sqlData["vBoard"].ToString(),
                        vDivOrClass = sqlData["vDivOrClass"].ToString(),
                        iYear = sqlData["iYear"].ToString()
                    });
                }
            }
            catch (Exception exp)
            {
                _logger.LogInformation("findEducationData: " + exp);
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public IEnumerable<object> findExperienceData(string vEmployeeId)
        {
            List<object> returnData = new List<object>();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select vEmployeeId,vEmployeeName,vPostOrDesignation,vCompanyName,dFrom,dTo,vMajorResponsibility " +
                    "from tbEmpExperienceInfo where vEmployeeId like @vEmployeeId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@vEmployeeId", vEmployeeId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData.Add(new
                    {
                        vEmployeeId = sqlData["vEmployeeId"].ToString(),
                        vEmployeeName = sqlData["vEmployeeName"].ToString(),
                        vPostOrDesignation = sqlData["vPostOrDesignation"].ToString(),
                        vCompanyName = sqlData["vCompanyName"].ToString(),
                        dFrom = sqlData["dFrom"].ToString(),
                        dTo = sqlData["dTo"].ToString(),
                        vMajorResponsibility = sqlData["vMajorResponsibility"].ToString()
                    });
                }
            }
            catch (Exception exp)
            {
                _logger.LogInformation("findExperienceData: " + exp);
            }
            finally
            {
                con.Close();
            }
            return returnData;
        }
        public object findAll(string iAutoId)
        {
            object returnData = new object();
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select iAutoId,vEmployeeId,vEmployeeCode,vFingerId,vEmployeeName,vEmployeeNameBangla,vDesignationId,vDesignationName," +
                    "vDepartmentId,vDepartmentName,vSectionId,vSectionName,vReligion,vContactNo,vEmailAddress,vGender," +
                    "convert(varchar, dDateOfBirth, 105)dDateOfBirth,vNationality,vNationalIdNo,vEmployeeType,vServiceType," +
                    "convert(varchar, dApplicationDate, 105)dApplicationDate,convert(varchar, dInterviewDate, 105)dInterviewDate," +
                    "convert(varchar, dJoiningDate, 105)dJoiningDate,convert(varchar, dConfirmationDate, 105)dConfirmationDate,vProbationPeriod," +
                    "vEmployeeStatus,iStatus,convert(varchar, dStatusDate, 105)dStatusDate,vEmployeePhoto,vFatherName,vMotherName,vPresentAddress," +
                    "vPermanentAddress,vBloodGroup,vMaritalStatus,convert(varchar, dMarriageDate, 105)dMarriageDate,vSpouseName,vSpouseOccupation," +
                    "iNumberOfChild,vBankId,vBankName,vBankBranchId,vBankBranchName,vAccountNo,vMoneyTransferType,vRoutingNo,vAccountMobileNo," +
                    "mBasic,mHouseRent,mMedicalAllowance,mGross,mIncomeTax,mProvidentFund " +
                    "from tbEmployeeInfo where iAutoId like @iAutoId";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@iAutoId", iAutoId);
                SqlDataReader sqlData = cmd.ExecuteReader();
                while (sqlData.Read())
                {
                    returnData = new
                    {
                        iAutoId = sqlData["iAutoId"].ToString(),
                        vEmployeeId = sqlData["vEmployeeId"].ToString(),
                        vEmployeeCode = sqlData["vEmployeeCode"].ToString(),
                        vFingerId = sqlData["vFingerId"].ToString(),
                        vEmployeeName = sqlData["vEmployeeName"].ToString(),
                        vEmployeeNameBangla = sqlData["vEmployeeNameBangla"].ToString(),
                        vDesignationId = sqlData["vDesignationId"].ToString(),
                        vDesignationName = sqlData["vDesignationName"].ToString(),
                        vDepartmentId = sqlData["vDepartmentId"].ToString(),
                        vDepartmentName = sqlData["vDepartmentName"].ToString(),
                        vSectionId = sqlData["vSectionId"].ToString(),
                        vSectionName = sqlData["vSectionName"].ToString(),
                        vReligion = sqlData["vReligion"].ToString(),
                        vContactNo = sqlData["vContactNo"].ToString(),
                        vEmailAddress = sqlData["vEmailAddress"].ToString(),
                        vGender = sqlData["vGender"].ToString(),
                        dDateOfBirth = sqlData["dDateOfBirth"].ToString(),
                        vNationality = sqlData["vNationality"].ToString(),
                        vNationalIdNo = sqlData["vNationalIdNo"].ToString(),
                        vEmployeeType = sqlData["vEmployeeType"].ToString(),
                        vServiceType = sqlData["vServiceType"].ToString(),
                        dApplicationDate = sqlData["dApplicationDate"].ToString(),
                        dInterviewDate = sqlData["dInterviewDate"].ToString(),
                        dJoiningDate = sqlData["dJoiningDate"].ToString(),
                        dConfirmationDate = sqlData["dConfirmationDate"].ToString(),
                        vProbationPeriod = sqlData["vProbationPeriod"].ToString(),
                        vEmployeeStatus = sqlData["vEmployeeStatus"].ToString(),
                        iStatus = sqlData["iStatus"].ToString(),
                        dStatusDate = sqlData["dStatusDate"].ToString(),
                        vEmployeePhoto = sqlData["vEmployeePhoto"].ToString(),

                        vFatherName = sqlData["vFatherName"].ToString(),
                        vMotherName = sqlData["vMotherName"].ToString(),
                        vPresentAddress = sqlData["vPresentAddress"].ToString(),
                        vPermanentAddress = sqlData["vPermanentAddress"].ToString(),
                        vBloodGroup = sqlData["vBloodGroup"].ToString(),
                        vMaritalStatus = sqlData["vMaritalStatus"].ToString(),
                        dMarriageDate = sqlData["dMarriageDate"].ToString(),
                        vSpouseName = sqlData["vSpouseName"].ToString(),
                        vSpouseOccupation = sqlData["vSpouseOccupation"].ToString(),
                        iNumberOfChild = sqlData["iNumberOfChild"].ToString(),

                        mBasic = SD.decFZero(sqlData["mBasic"].ToString()),
                        mHouseRent = SD.decFZero(sqlData["mHouseRent"].ToString()),
                        mMedicalAllowance = SD.decFZero(sqlData["mMedicalAllowance"].ToString()),
                        mGross = SD.decFZero(sqlData["mGross"].ToString()),
                        mIncomeTax = SD.decFZero(sqlData["mIncomeTax"].ToString()),
                        mProvidentFund = SD.decFZero(sqlData["mProvidentFund"].ToString()),

                        vBankId = sqlData["vBankId"].ToString(),
                        vBankName = sqlData["vBankName"].ToString(),
                        vBankBranchId = sqlData["vBankBranchId"].ToString(),
                        vBankBranchName = sqlData["vBankBranchName"].ToString(),
                        vAccountNo = sqlData["vAccountNo"].ToString(),
                        vMoneyTransferType = sqlData["vMoneyTransferType"].ToString(),
                        vRoutingNo = sqlData["vRoutingNo"].ToString(),
                        vAccountMobileNo = sqlData["vAccountMobileNo"].ToString(),
                        Attachment = sqlData["vEmployeePhoto"].ToString()
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
            var allObj = _unitPayroll.Employee.GetAll();
            return allObj;
        }
    }
}
