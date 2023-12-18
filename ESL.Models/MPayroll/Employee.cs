using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ESL.Models
{
    public class Employee
    {
        [Key]
        [Required]
        public int iAutoId { get; set; }
        [Required]
        public string vEmployeeId { get; set; }
        [Required]
		public string vEmployeeCode { get; set; }
		[Required]
		public string vFingerId { get; set; }
		[Required]
		public string vEmployeeName { get; set; }
		public string vEmployeeNameBangla { get; set; }
		public string vDesignationId { get; set; }
		public string vDesignationName { get; set; }
		public string vDepartmentId { get; set; }
		public string vDepartmentName { get; set; }
		public string vSectionId { get; set; }
		public string vSectionName { get; set; }
		public string vReligion { get; set; }
		public string vContactNo { get; set; }
		public string vEmailAddress { get; set; }
		public string vGender { get; set; }
		public DateTime dDateOfBirth { get; set; }
		public string vNationality { get; set; }
		public string vNationalIdNo { get; set; }
		public string vEmployeeType { get; set; }
		public string vServiceType { get; set; }
		public DateTime dApplicationDate { get; set; }
		public DateTime dInterviewDate { get; set; }
		public DateTime dJoiningDate { get; set; }
		public DateTime dConfirmationDate { get; set; }
		public string vProbationPeriod { get; set; }
		public string vEmployeeStatus { get; set; }
		public bool iStatus { get; set; }
		public DateTime dStatusDate { get; set; }
		public string vEmployeePhoto { get; set; }
		public string vFatherName { get; set; }
		public string vMotherName { get; set; }
		public string vPresentAddress { get; set; }
		public string vPermanentAddress { get; set; }
		public string vBloodGroup { get; set; }
		public string vMaritalStatus { get; set; }
		public DateTime dMarriageDate { get; set; }
		public string vSpouseName { get; set; }
		public string vSpouseOccupation { get; set; }
        public string iNumberOfChild { get; set; }
		public decimal mBasic { get; set; }
		public decimal mHouseRent { get; set; }
		public decimal mMedicalAllowance { get; set; }
		public decimal mGross { get; set; }
		public decimal mIncomeTax { get; set; }
		public decimal mProvidentFund { get; set; }
		public string vMoneyTransferType { get; set; }
		public string vBankId { get; set; }
		public string vBankName { get; set; }
		public string vBankBranchId { get; set; }
		public string vBankBranchName { get; set; }
		public string vAccountNo { get; set; }
		public string vRoutingNo { get; set; }
		public string vAccountMobileNo { get; set; }
		public string vCompanyId { get; set; }
		public string UserId { get; set; }
		public string UserIp { get; set; }
		public DateTime EntryTime { get; set; }
	}
}
