var commonUrl = "https://localhost:44359";
/*Report View Action Start*/
function callReport() {
    var type = document.forms.myForm.reportMenu.value;
    console.log(type);
    switch (type) {
        case 'RptEmployeeProfileBioData':
            EmployeeProfileBioDataReport();
            break;
        case 'RptEmployeeList':
            EmployeeListReport();
            break;
        case 'RptEditEmployeeInfo':
            EditEmployeeInfoReport();
            break;
        case 'RptApplicationAndAppointmentLetter':
            ApplicationAndAppointmentLetterReport();
            break;
        case 'RptCertification':
            CertificationReport();
            break;

        case 'RptMonthlyDutyRoster':
            MonthlyDutyRoster();
            break;

        case 'RptDailyAttendanceStatement':
            DailyAttendanceStatementReport();
            break;
        case 'RptDailyAbsentStatement':
            DailyAbsentStatementReport();
            break;
        case 'RptDailyLateInEmployeeList':
            DailyLateInEmployeeListReport();
            break;
        case 'RptDailyEarlyOutEmployeeList':
            DailyEarlyOutEmployeeListReport();
            break;
        case 'RptIndividualEmployeeAttendance':
            IndividualEmployeeAttendanceReport();
            break;
        case 'RptDailyAbsentStatement':
            DailyAbsentStatementReport();
            break;
        case 'RptMonthlyAttendanceSummary':
            MonthlyAttendanceSummaryReport();
            break;
        case 'RptShortViewOfAttendance':
            ShortViewOfAttendanceReport();
            break;
        case 'RptMonthlySalary':
            MonthlySalaryReport();
            break;
        case 'RptPaySlip':
            PaySlipReport();
            break;
        case 'RptBankAdviceWithForwardingLatter':
            BankAdviceWithForwardingLatterReport();
            break;
        case 'RptNoteRequisition':
            NoteRequisitionReport();
            break;
        

    }
}
/*Report View Action End*/

//Employee List Servicing Report
function EmployeeProfileBioDataReport()
{
    var vEmployeeId = $("#vEmployeeId").select2('data');
    if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
        var url = "/Payroll/PayrollReport/RptEmployeeProfileBioData?vEmployeeId=" + encodeURI(vEmployeeId.id);
        console.log("EmployeeProfileBioDataReport: " + commonUrl + url);
        reportView(url);
    }
    else {
        warningNotify("Please Select Employee ID");
    }
}
//Employee List Report
function EmployeeListReport() {
    var vStatus = $("input[name='vStatus']:checked").val();
    var vWithOrWithOutGross = $("input[name='vWithOrWithOutGross']:checked").val();
    var vEmployeeId = $("#vEmployeeId").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
        var url = "/Payroll/PayrollReport/RptEmployeeList?vEmployeeId=" + encodeURI(vEmployeeId.id) + "&vStatus=" + encodeURI(vStatus) +
            "&vDepartmentId=" + encodeURI(vDepartmentId.id) + "&vSectionId=" + encodeURI(vSectionId.id) + "&vWithOrWithOutGross=" + encodeURI(vWithOrWithOutGross);
        reportView(url);
    }
    else {
        warningNotify("Please Select Employee ID");
    }

}

//Edit Employee Info
function EditEmployeeInfoReport() {
    var vEmployeeId = $("#vEmployeeId").select2('data');
    if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
        var url = "/Payroll/PayrollReport/RptEditEmployeeInfo?vEmployeeId=" + encodeURI(vEmployeeId.id);
        console.log(commonUrl + url);
        reportView(url);
      /*  console.log("HI");*/
    }
    else {
        warningNotify("Please Select Employee");
    }

}
//Certification
function CertificationReport() {
    var vEmployeeId = $("#vEmployeeId").select2('data');
    var vCertifiedById = $("#vCertifiedById").select2('data');
    var inpDesignationCertifiedBy = $("#inpDesignationCertifiedBy").val();
    if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
        if (vCertifiedById.id != "" && vCertifiedById.id != '0') {
                var url = "/Payroll/PayrollReport/RptCertification?vEmployeeId=" + encodeURI(vEmployeeId.id) +
                    "&vCertifiedById=" + encodeURI(vCertifiedById.text) 
                    + "&inpDesignationCertifiedBy=" + encodeURI(inpDesignationCertifiedBy);
            console.log(commonUrl + url);
                reportView(url);
          /*  console.log("Hello");*/
                }
            
    else {
        warningNotify("Please Select Employee");
    }

        }
        else {
            warningNotify("Please Select  Certify  ");
        }

    }
  

//Attendence

function MonthlyDutyRoster() {
    var vMonth = $("#vMonth").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vEmployeeId = $("#vEmployeeId").select2('data');

    if (vMonth.id != "" && vMonth.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0") {
                if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
                    var url = "/Payroll/PayrollReport/RptMonthlyDutyRoster?vMonth=" + encodeURI(vMonth.id) +
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) + "&vSectionId=" + encodeURI(vSectionId.id) + "&vEmployeeId=" + encodeURI(vEmployeeId.id);
                    console.log(commonUrl + url);
                    reportView(url);
                }
                else {
                    warningNotify("Please Select Employee ID");
                }
            }
            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Date");
    }
}



function DailyAttendanceStatementReport()
{
    var dDate = getBdToDbFormat($("#dDate").val());
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vServiceType = $("#vServiceType").select2('data');
    var vEmployeeType = $("input[name='vEmployeeType']:checked").val();
    if (dDate.id != "" && dDate.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0")
            {
                if (vServiceType.id != "" && vServiceType.id != "0") {
                    if (vEmployeeType != "") {
                        var url = "/Payroll/PayrollReport/RptDailyAttendanceStatement?dDate=" + encodeURI(dDate) +
                            "&vDepartmentId=" + encodeURI(vDepartmentId.id) +
                            "&vSectionId=" + encodeURI(vSectionId.id) +
                            "&vServiceType=" + encodeURI(vServiceType.id) +
                            "&vEmployeeType=" + encodeURI(vEmployeeType);
                        console.log(commonUrl + url);
                        reportView(url);
                    }
                    else {
                        warningNotify("Please select Employee Type");
                    }
                }
                else {
                    warningNotify("Please select Service Type");
                }
            }
            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Date");
    }
}
function DailyAbsentStatementReport() {
    var dDate = getBdToDbFormat($("#dDate").val());    
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vServiceType = $("#vServiceType").select2('data');
    if (dDate.id != "" && dDate.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0")
            {
                if (vServiceType.id != "" && vServiceType.id != "0")
                {
                    var url = "/Payroll/PayrollReport/RptDailyAbsentStatement?dDate=" + encodeURI(dDate) +
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) +
                        "&vSectionId=" + encodeURI(vSectionId.id) + 
                        "&vServiceType=" + encodeURI(vServiceType.id);
                    console.log(commonUrl + url);
                    reportView(url);
                }
                else {
                    warningNotify("Please select Service Type");
                }
            }
            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Date");
    }
}
function DailyLateInEmployeeListReport() {
    var dDate = getBdToDbFormat($("#dDate").val());
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vServiceType = $("#vServiceType").select2('data');
    if (dDate.id != "" && dDate.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0")
            {
                if (vServiceType.id != "" && vServiceType.id != "0") {
                    var url = "/Payroll/PayrollReport/RptDailyLateInEmployeeList?dDate=" + encodeURI(dDate) +
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) +
                        "&vSectionId=" + encodeURI(vSectionId.id) +
                        "&vServiceType=" + encodeURI(vServiceType.id);
                    reportView(url);
                }
                else {
                    warningNotify("Please select Service Type");
                }
            }
            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Date");
    }
}
function DailyEarlyOutEmployeeListReport() {
    var dDate = getBdToDbFormat($("#dDate").val());
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vServiceType = $("#vServiceType").select2('data');
    if (dDate.id != "" && dDate.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0")
            {
                if (vServiceType.id != "" && vServiceType.id != "0") {
                    var url = "/Payroll/PayrollReport/RptDailyEarlyOutEmployeeList?dDate=" + encodeURI(dDate) +
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) +
                        "&vSectionId=" + encodeURI(vSectionId.id) +
                        "&vServiceType=" + encodeURI(vServiceType.id);
                    reportView(url);
                }
                else {
                    warningNotify("Please select Service Type");
                }
            }
            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Date");
    }
}

function IndividualEmployeeAttendanceReport() {
    var vMonth = $("#vMonth").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data'); 
    var vEmployeeId = $("#vEmployeeId").select2('data');

    if (vMonth.id != "" && vMonth.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0") {
                if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
                    var url = "/Payroll/PayrollReport/RptIndividualEmployeeAttendance?vMonth=" + encodeURI(vMonth.id) + 
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) + "&vSectionId=" + encodeURI(vSectionId.id) + "&vEmployeeId=" + encodeURI(vEmployeeId.id);
                    console.log(commonUrl + url);
                    reportView(url);
                }
                else {
                    warningNotify("Please Select Employee ID");
                }
            }
            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Date");
    }
}
function MonthlyAttendanceSummaryReport() {
    var vMonth = $("#vMonth").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vEmployeeId = $("#vEmployeeId").select2('data');

    if (vMonth.id != "" && vMonth.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0") {
                if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
                    var url = "/Payroll/PayrollReport/RptMonthlyAttendanceSummary?vMonth=" + encodeURI(vMonth.id) + 
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) + "&vSectionId=" + encodeURI(vSectionId.id) + "&vEmployeeId=" + encodeURI(vEmployeeId.id);
                    console.log(commonUrl + url);
                    reportView(url);
                }
                else {
                    warningNotify("Please Select Employee ID");
                }
            }
            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Date");
    }
}

function MonthlySalaryReport() {
    var vMonth = $("#vMonth").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vEmployeeId = $("#vEmployeeId").select2('data');
    var vSalaryType = $("input[name='vSalaryType']:checked").val();

    if (vMonth.id != "" && vMonth.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0") {
                if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
                    var url = "/Payroll/PayrollReport/RptMonthlySalary?vMonth=" + encodeURI(vMonth.id) + "&vMonthName=" + encodeURI(vMonth.text) +
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) + "&vSectionId=" + encodeURI(vSectionId.id) + "&vEmployeeId=" + encodeURI(vEmployeeId.id) +
                        "&vSalaryType=" + encodeURI(vSalaryType);
                    console.log(commonUrl + url);
                    reportView(url);
                }
                else {
                    warningNotify("Please Select Employee ID");
                }
            }

            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Month");
    }
}
function PaySlipReport() {
    var vMonth = $("#vMonth").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vEmployeeId = $("#vEmployeeId").select2('data');
/*    var vSalaryType = $("input[name='vSalaryType']:checked").val();*/

    if (vMonth.id != "" && vMonth.id != '0') {
        if (vDepartmentId.id != "" && vDepartmentId.id != "0") {
            if (vSectionId.id != "" && vSectionId.id != "0") {
                if (vEmployeeId.id != "" && vEmployeeId.id != '0') {
                    var url = "/Payroll/PayrollReport/RptPaySlip?vMonth=" + encodeURI(vMonth.id) + "&vMonthName=" + encodeURI(vMonth.text) +
                        "&vDepartmentId=" + encodeURI(vDepartmentId.id) +
                        "&vSectionId=" + encodeURI(vSectionId.id) + "&vEmployeeId=" + encodeURI(vEmployeeId.id);
                    console.log(commonUrl + url);
                  /*  console.log("Hello")*/
                    reportView(url);
                }
                else {
                    warningNotify("Please Select Employee ID");
                }
            }

            else {
                warningNotify("Please select Section");
            }
        }
        else {
            warningNotify("Please Select Department");
        }
    }
    else {
        warningNotify("Please Select Month");
    }
}
function NoteRequisitionReport() {
    var dDate = getBdToDbFormat($("#dDate").val());
    var vMonth = $("#vMonth").select2('data');
    var vBankId = $("#vBankId").select2('data');
/*    console.log(vBankId);*/
    var inpDesignation = $("#inpDesignation").val();
    var inpBranchName = $("#inpBranchName").val();
    var inpAddress = $("#inpAddress").val();
    var inpAccountNo = $("#inpAccountNo").val();

    if (dDate.id != "" && dDate.id != '0') {
        if (vMonth.id != "" && vMonth.id != '0') {
            if (vBankId.name != "" && vBankId.id != "0") {
                {
                    var url = "/Payroll/PayrollReport/RptNoteRequisition?dDate=" + encodeURI(dDate) + "&vMonth=" + encodeURI(vMonth.id) +
                        "&vMonthName=" + encodeURI(vMonth.text) + "&vBankId=" + encodeURI(vBankId.text) + "&inpDesignation=" +
                        inpDesignation + "&inpBranchName=" + inpBranchName + "&inpAddress=" + inpAddress + "&inpAccountNo=" + inpAccountNo;
                    console.log(commonUrl + url);
                    reportView(url);
                }
            }

            else {
                warningNotify("Please select Bank Name");
            }
        }
        else {
            warningNotify("Please Select Month");
        }
    }
    else {
        warningNotify("Please Select Date");
    }

}


/*Common Part for Report Preview*/
async function reportView(url) {
    $('#reportView').innerHTML = '';
    await $('#reportView').load(url);

    $.magnificPopup.open({
        items: {
            src: '#reportViewer',
            type: 'inline'
        },
        preloader: false,
        modal: true,
    });
    showLoader();
}
function showLoader() {
    $("<div/>").addClass('loader').appendTo($("#reportView"));
}
/*Common Part for Report Preview*/