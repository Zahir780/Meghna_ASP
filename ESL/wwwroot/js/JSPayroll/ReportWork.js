var commonUrl = "https://localhost:44359";
$(document).ready(function () {
    init();
    visibility();
    $(".form-group").css("display", "none");
    $('input[type=radio][name=reportMenu]').change(function () {
        //console.log("reportMenu");
        visibility();
        reportMenuAction($(this).val());
    });
    othersAction();
});
function init() {
    clear();
}
function visibility() {
    document.getElementById('rdoStatus').style.display = "none";
    document.getElementById('pDate').style.display = "none";
    document.getElementById('pBankBranchId').style.display = "none";
    document.getElementById('pBankId').style.display = "none";
    document.getElementById('pDepartmentId').style.display = "none";
    document.getElementById('pSectionId').style.display = "none";
    document.getElementById('comServiceType').style.display = "none";
    document.getElementById('comEmployeeId').style.display = "none";
    document.getElementById('pSalaryType').style.display = "none";
    document.getElementById('pWithOrWithOutGross').style.display = "none";
    document.getElementById('pMonthId').style.display = "none";
    document.getElementById('pEmployeeType').style.display = "none";
    document.getElementById('rdoMoneyTransferTypeId').style.display = "none";
    document.getElementById('pDesignation').style.display = "none";
    document.getElementById('pBranchName').style.display = "none";
    document.getElementById('pAddress').style.display = "none";
    document.getElementById('comAccountNo').style.display = "none";
    document.getElementById('comReferenceNo').style.display = "none";
    document.getElementById('comCertifiedBy').style.display = "none";
    document.getElementById('comDesignation').style.display = "none";

    
}
function removeModalClass() {
    document.getElementById("reportViewer").classList.remove("landscapeA4");
    document.getElementById("reportViewer").classList.remove("landscapeA42");
    document.getElementById("reportViewer").classList.remove("portraitA4");
}
function reportMenuAction(type) {
    /*Master Report Part End*/
    if (type == "RptEmployeeProfileBioData") {
        $("#ReportTitle").text('Employee Profile (Bio-Data)');
        RptEmployeeProfileBioDataWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptEmployeeList") {
        $("#ReportTitle").text('Employee List');
        RptEmployeeWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptEditEmployeeInfo") {
        $("#ReportTitle").text('Edit Employee Info');
        RptEditEmployeeInfoWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptApplicationAndAppointmentLetter") {
        $("#ReportTitle").text('Application And Appointment Letter');
        RptApplicationAndAppointmentLetterWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptCertification") {
        $("#ReportTitle").text('Certification');
        RptCertificationWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    /*Master Report Part End*/

    /*Attendance Report Part Start*/

    if (type == "RptMonthlyDutyRoster") {
        $("#ReportTitle").text('Monthly Duty Roster');
        RptMonthlyDutyRoster();
        document.getElementById("reportViewer").className += " portraitA4";
    }


    if (type == "RptDailyAttendanceStatement") {
        $("#ReportTitle").text('Daily Attendance Statement');
        RptDailyAttendanceStatementWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptDailyAbsentStatement") {
        $("#ReportTitle").text('Daily Absent Statement');
        RptDailyAbsentStatementWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptDailyLateInEmployeeList") {
        $("#ReportTitle").text('Daily LateIn Employee List');
        RptDailyLateInEmployeeListWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptDailyEarlyOutEmployeeList") {
        $("#ReportTitle").text('Daily Early Out Employee List');
        RptDailyEarlyOutEmployeeListWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptIndividualEmployeeAttendance") {
        $("#ReportTitle").text('Individual Employee Attendance');
        RptIndividualEmployeeAttendanceWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptMonthlyAttendanceSummary") {
        $("#ReportTitle").text('Monthly Attendance Summary');
        RptMonthlyAttendanceSummaryWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptShortViewOfAttendance") {
        $("#ReportTitle").text('Short View Of Attendance');
        RptShortViewOfAttendanceWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    /*Attendance Report Part End*/
    
    /*Salary Report Part Satrt*/
    if (type == "RptMonthlySalary") {
        $("#ReportTitle").text('Monthly Salary');
        RptMonthlySalaryWork();
        document.getElementById("reportViewer").className += " landscapeA4";
    }
    if (type == "RptPaySlip") {
        $("#ReportTitle").text('PaySlip');
        RptPaySlipWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptBankAdviceWithForwardingLatter") {
        $("#ReportTitle").text('Bank Advice With Forwarding Latter');
        RptBankAdviceWithForwardingLatterWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    if (type == "RptNoteRequisition") {
        $("#ReportTitle").text('Note Requisition');
        RptNoteRequisitionWork();
        document.getElementById("reportViewer").className += " portraitA4";
    }
    /*Salary Report Part End*/
}


/*Master Report Data Load Action Start*/
function RptEmployeeProfileBioDataWork() {
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
    var type = "RptEmployeeProfileBioData";
    $("#type").val(type);
    if (type != "") {
        getEmployee(type, "0", "0", "0", "0", "0", "0");

    }
}
function RptEmployeeWork() {
    document.getElementById('rdoStatus').style.display = "block";
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comEmployeeId').style.display = "block";
    document.getElementById('pWithOrWithOutGross').style.display = "block";
    clear();
    var type = "RptEmployeeList";
    $("#type").val(type);
    var vStatus = $("input[name='vStatus']:checked").val();
    if (vStatus=="Active") {
        vStatus = "1";
    }
    else if (vStatus=="Inactive") {
        vStatus = "0";
    }
    else if (vStatus=="All") {
        vStatus = "%";
    }
    if (type != "") {
        getDepartment(type, "0", "0", vStatus,"0");
    } 
}
function RptEditEmployeeInfoWork() {
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
    var type = "RptEditEmployeeInfo";
    $("#type").val(type);
    if (type != "") {
        getEmployee(type,"0","0","0","0","0","0");
    } 
}
function RptApplicationAndAppointmentLetterWork() {
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
    var type = "RptApplicationAndAppointmentLetter";
    $("#type").val(type);
    if (type != "") {
        getEmployee(type,"0","0","0","0","0","0");
    } 
}
function RptCertificationWork() {
    document.getElementById('comEmployeeId').style.display = "block";
    document.getElementById('comCertifiedBy').style.display = "block";
    document.getElementById('comDesignation').style.display = "block";
    clear();
    var type = "RptCertification";
    $("#type").val(type);
    if (type != "") {
        getEmployee(type, "0", "0", "0", "0", "0", "0");
        getCertifiedBy(type);
    } 
}
/*Master Report Data Load Action End*/

/*Attendance Report Data Load Action  Start*/

function RptMonthlyDutyRoster() {
    document.getElementById('pMonthId').style.display = "block";
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
    var type = "RptMonthlyDutyRoster";
    $("#type").val(type);
    if (type != "") {
        getMonth(type, "0");
    }
}




function RptDailyAttendanceStatementWork() {
    document.getElementById('pDate').style.display = "block";
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comServiceType').style.display = "block";
    document.getElementById('pEmployeeType').style.display = "block";
    clear();
    var type = "RptDailyAttendanceStatement";
    var dDate = getBdToDbFormat($("#dDate").val());
    $("#type").val(type);
    if (type != "") {
        if (dDate !="") {
            getDepartment(type, dDate, "0", "0", "0");
        }
    }
}

function RptDailyAbsentStatementWork() {
    document.getElementById('pDate').style.display = "block";
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comServiceType').style.display = "block";
    clear();
    var type = "RptDailyAbsentStatement";
    var dDate = getBdToDbFormat($("#dDate").val());
    $("#type").val(type);
    if (type != "") {
        if (dDate != "") {
            getDepartment(type, dDate, "0", "0", "0");
        }
    }
}

function RptDailyLateInEmployeeListWork() {
    document.getElementById('pDate').style.display = "block";    
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comServiceType').style.display = "block";
    clear();
    var type = "RptDailyLateInEmployeeList";
    var dDate = getBdToDbFormat($("#dDate").val());
    $("#type").val(type);
    if (type != "") {
        if (dDate != "") {
            getDepartment(type, dDate, "0", "0", "0");
        }
    }
}

function RptDailyEarlyOutEmployeeListWork() {
    document.getElementById('pDate').style.display = "block";    
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comServiceType').style.display = "block";
    clear();
    var type = "RptDailyEarlyOutEmployeeList";
    var dDate = getBdToDbFormat($("#dDate").val());
    $("#type").val(type);
    if (type != "") {
        if (dDate != "") {
            getDepartment(type, dDate, "0", "0", "0");
        }
    }
}

function RptIndividualEmployeeAttendanceWork() {
    document.getElementById('pMonthId').style.display = "block";
    
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
    var type = "RptIndividualEmployeeAttendance";
    $("#type").val(type);
    if (type != "") {
        getMonth(type, "0");
    }
}

function RptMonthlyAttendanceSummaryWork() {
    document.getElementById('pMonthId').style.display = "block";    
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
    var type = "RptMonthlyAttendanceSummary";
    $("#type").val(type);
    if (type != "") {
        getMonth(type, "0");
    }
}
function RptShortViewOfAttendanceWork() {
    document.getElementById('pMonthId').style.display = "block";
    
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
    var type = "RptShortViewOfAttendance";
    $("#type").val(type);
    if (type != "") {
        getMonth(type, "0");
    }
}
/*Attendance Report Data Load Action End*/

/*Salary Report Data Load Action Start*/
function RptMonthlySalaryWork() {
    document.getElementById('pMonthId').style.display = "block";    
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comEmployeeId').style.display = "block";
    document.getElementById('pSalaryType').style.display = "block";
    clear();
    var type = "RptMonthlySalary";
    var vSalaryType = $("input[name='vSalaryType']:checked").val();

    $("#type").val(type);
    if (type != "") {
        getMonth(type, vSalaryType);

    }
}
function RptPaySlipWork() {
    document.getElementById('pMonthId').style.display = "block";    
    document.getElementById('pDepartmentId').style.display = "block";
    document.getElementById('pSectionId').style.display = "block";
    document.getElementById('comEmployeeId').style.display = "block";
    clear();
//    var type = "RptPaySlip";
//    $("#type").val(type);

//    var vSalaryType = $("input[name='vSalaryType']:checked").val();

//    if (type != "")
//    {
//        if (vSalaryType != "")
//        {
//            getMonth(type, vSalaryType);
//        }
//    }
//}
    var type = "RptPaySlip";
    $("#type").val(type);
    if (type != "") {
        getMonth(type, "0");
    }
}



function RptBankAdviceWithForwardingLatterWork() {
    document.getElementById('rdoMoneyTransferTypeId').style.display = "block";
    document.getElementById('pDate').style.display = "block";
    document.getElementById('pMonthId').style.display = "block"
    document.getElementById('pBankId').style.display = "block";
    document.getElementById('pBankBranchId').style.display = "block";
    document.getElementById('comAccountNo').style.display = "block";
    document.getElementById('comReferenceNo').style.display = "block";
    clear();
    var type = "RptBankAdviceWithForwardingLatter";
    $("#type").val(type);
    if (type != "") {
        getMonth(type, "0");
    }
}
function RptNoteRequisitionWork() {
    document.getElementById('pDate').style.display = "block";
    document.getElementById('pMonthId').style.display = "block"
    document.getElementById('pDesignation').style.display = "block";
    document.getElementById('pBankId').style.display = "block";
    document.getElementById('pBranchName').style.display = "block";
    document.getElementById('pAddress').style.display = "block";
    document.getElementById('comAccountNo').style.display = "block";

    clear();
    var type = "RptNoteRequisition";
    $("#type").val(type);
    if (type != "") {
        getMonth(type, "0");
    }
}
/*Master Report Data Load Action End*/


// action for option group, select option, Radio etc action Start
function othersAction() {
    var vType = "";
    $('input[type=radio][name=vStatus]').change(function () {
        vType = $("#type").val();
        var vStatus = $("input[name='vStatus']:checked").val();

        if (vType != "") {
            if (vStatus == "Active") {
                vStatus = "1";
            }
            else if (vStatus == "Inactive") {
                vStatus = "0";
            }
            else if (vStatus == "All") {
                vStatus = "%";
            }
            if (vType == "RptEmployeeList") {
                getDepartment(vType, "0", "0", vStatus, "0");
            }
        }
    });

    $("#dDate").change(function () {
        var dDate = getBdToDbFormat($("#dDate").val());
        var type = $("#type").val();
        if (type != "") {
            if (dDate != "" && dDate != "0")
            {
                if (
                    type == "RptDailyAttendanceStatement" || type == "RptDailyAbsentStatement" ||
                    type == "RptDailyLateInEmployeeList" || type == "RptDailyEarlyOutEmployeeList"
                ) {
                    getDepartment(type, dDate, "0", "0", "0");
                }
            }
        }
    });

    $('input[type=radio][name=vSalaryType]').change(function () {
        vType = $("#type").val();
        var vSalaryType = $("input[name='vSalaryType']:checked").val();

        if (vType != "") {
            if (vSalaryType != "") {
                getMonth(vType, vSalaryType);
            }

        }
    });

    $("#vMonth").change(function () {
        var type = $("#type").val();
        var vMonth = $("#vMonth").select2('data');
        var vSalaryType = $("input[name='vSalaryType']:checked").val();
        if (type != "") {
            if (vMonth.id != "" && vMonth.id != "0")
            {
                if ( type == "RptMonthlySalary" || type == "RptPaySlip" )
                {
                    if (vSalaryType != "") {
                        getDepartment(type, "0", vMonth.id, "0", vSalaryType);
                    }
                }
                else if (type == "RptIndividualEmployeeAttendance" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance" || type == "RptMonthlyDutyRoster")
                {
                    getDepartment(type, "0", vMonth.id, "0", "0");
                }
                else if (
                    type == "RptBankAdviceWithForwardingLatter" || type == "RptNoteRequisition" 
                )
                {
                    getBank(type, vMonth.id);
                }
            }
        }
    });

    $("#vCertifiedById").change(function () {
        var type = $("#type").val();
        var vCertifiedById = $("#vCertifiedById").select2('data');
       
        if (type != "") {
            if (vCertifiedById.id != "" && vCertifiedById.id != "0") {
                if (type == "RptCertification" ) {
                    getDesignation(type, vCertifiedById.id)
                }
            }
        }
    });

    $("#vBankId").change(function () {
        var type = $("#type").val();
        var vMonth = $("#vMonth").select2('data');
        var vBankId = $("#vBankId").select2('data');
        if (type != "") {
            if (vMonth.id != "" && vMonth.id != "0" && vBankId.id != "" && vBankId.id != "0")
            {
                if (
                    type == "RptBankAdviceWithForwardingLatter" || type == "RptNoteRequisition" 
                )
                {
                    getBankBranch(type, vMonth.id, vBankId.id);
                }
            }
        }
    });

    $("#vDepartmentId").change(function () {
        var type = $("#type").val();
        var dDate = $("#dDate").val();
        var vMonth = $("#vMonth").select2('data');
        var vDepartmentId = $("#vDepartmentId").select2('data');
        var vStatus = $("input[name='vStatus']:checked").val();
        if (vStatus == "Active") {
            vStatus = "1";
        }
        else if (vStatus == "Inactive") {
            vStatus = "0";
        }
        else if (vStatus == "All") {
            vStatus = "%";
        }
        var vSalaryType = $("input[name='vSalaryType']:checked").val();

        if (type != "") {
            if (type == "RptEmployeeList")
            {
                if (vDepartmentId != "" && vDepartmentId != "0")
                {            
                    getSection(type, "0", "0", vDepartmentId.id, vStatus,"0");
                }
            }
            if (
                type == "RptDailyAttendanceStatement" || type == "RptDailyAbsentStatement" ||
                type == "RptDailyLateInEmployeeList" || type == "RptDailyEarlyOutEmployeeList" 
            ) {
                
                if (
                    (dDate != "" && dDate != "0") && (vDepartmentId != "" && vDepartmentId != "0")
                )
                {            
                    var dDate = getBdToDbFormat(dDate);
                    getSection(type, dDate, "0", vDepartmentId.id, "0", "0");
                }
            }
            else if (
                type == "RptMonthlySalary" || type == "RptPaySlip"
            )
            {
                if (
                    (vMonth.id != "" && vMonth.id != "0") && (vDepartmentId != "" && vDepartmentId != "0")
                ) {
                    getSection(type, "0", vMonth.id, vDepartmentId.id, "0", vSalaryType);
                }
            }
            else if (
                type == "RptIndividualEmployeeAttendance" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance" || type == "RptMonthlyDutyRoster" 
            )
            {
                if (
                    (vMonth.id != "" && vMonth.id != "0") && (vDepartmentId != "" && vDepartmentId != "0")
                ) {
                    getSection(type, "0", vMonth.id, vDepartmentId.id, "0", "0");
                }
            }
        }
    });

    $("#vSectionId").change(function () {
        var type = $("#type").val();
        var vMonth = $("#vMonth").select2('data');
        var vDepartmentId = $("#vDepartmentId").select2('data');
        var vSectionId = $("#vSectionId").select2('data');
        var vStatus = $("input[name='vStatus']:checked").val();
        var dDate = $("#dDate").val();

        if (vStatus == "Active") {
            vStatus = "1";
        }
        else if (vStatus == "Inactive") {
            vStatus = "0";
        }
        else if (vStatus == "All") {
            vStatus = "%";
        }

        var vSalaryType = $("input[name='vSalaryType']:checked").val();

        if (type != "")
        {
            if (type == "RptEmployeeList")
            {
                if ( (vDepartmentId != "" && vDepartmentId != "0") && (vSectionId != "" && vSectionId != "0")) {
                    getEmployee(type, vStatus, "0", vMonth.id, vDepartmentId.id, vSectionId.id, "0");
                }
            }
            else if (type == "RptMonthlySalary" || type == "RptPaySlip")
            {
                if (
                    (vMonth.id != "" && vMonth.id != "0") && (vDepartmentId != "" && vDepartmentId != "0") && 
                    (vSectionId != "" && vSectionId != "0") && (vSalaryType != "" && vSalaryType != "0")
                ) {
                    getEmployee(type, "0", "0", vMonth.id, vDepartmentId.id, vSectionId.id, vSalaryType);
                }
            }
            else if (type == "RptIndividualEmployeeAttendance" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance" || type == "RptMonthlyDutyRoster" )
            {
                if (
                    (vMonth.id != "" && vMonth.id != "0") &&
                    (vDepartmentId != "" && vDepartmentId != "0") &&
                    (vSectionId != "" && vSectionId != "0")
                ) {
                    getEmployee(type, "0", "0", vMonth.id, vDepartmentId.id, vSectionId.id, "0");
                }
            }
            else if (type == "RptDailyAttendanceStatement" || type == "RptDailyAbsentStatement" || type == "RptDailyLateInEmployeeList" || type == "RptDailyEarlyOutEmployeeList")
            {
                if ((dDate != "" && dDate != "0") && (vDepartmentId != "" && vDepartmentId != "0") && (vSectionId != "" && vSectionId != "0")) {
                    var dDate = getBdToDbFormat(dDate);
                    getServiceType(type, dDate, vDepartmentId.id, vSectionId.id);
                }
            }
        }
    });
    
}
// action for option group, select option, Radio etc action End

/*Data Load Action Start*/

function getServiceType(type, dDate, vDepartmentId, vSectionId)
{
    var url = "/Payroll/PayrollReport/getServiceType?type=" + type + "&dDate=" + dDate + "&vDepartmentId=" + vDepartmentId + "&vSectionId=" + vSectionId;
    console.log("getServiceType " + type +" : "+ commonUrl + url);
    $('#vServiceType').select2('data', { id: '0', text: '==Choose a Service Type==' });
    $.getJSON(url, function (data) {
        var item = "";
        $("#vServiceType").empty();
        item += '<option value="' + '0' + '">==Choose a Service Type==</option>'     
        if (type == "RptDailyAttendanceStatement" || type == "RptDailyAbsentStatement" || type == "RptDailyLateInEmployeeList" || type == "RptDailyEarlyOutEmployeeList") {
            item += '<option value="' + '%' + '">All</option>'
        }
        $("#vServiceType").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vServiceType").html(item);
    });
}
function getEmployee(type, vStatus, dDate, vMonth, vDepartmentId, vSectionId, vSalaryType)
{
    var url = "/Payroll/PayrollReport/getEmployee?type=" + type + "&vStatus=" + vStatus + "&dDate=" + dDate + "&vMonth=" + vMonth +
        "&vDepartmentId=" + vDepartmentId + "&vSectionId=" + vSectionId + "&vSalaryType=" + vSalaryType ;
    console.log("getEmployee " + type +" : "+ commonUrl + url);
    $('#vEmployeeId').select2('data', { id: '0', text: '==Choose a Employee==' });
    $.getJSON(url, function (data) {
        var item = "";
        $("#vEmployeeId").empty();
        item += '<option value="' + '0' + '">==Choose a Employee==</option>'     
        if (
            type == "RptEmployeeList" || type == "RptMonthlyAttendanceSummary" || type == "RptShortViewOfAttendance" ||
            type == "RptMonthlySalary" || type == "RptPaySlip" || type == "RptMonthlyDutyRoster"
        ) {
            item += '<option value="' + '%' + '">All</option>'
        }
        $("#vEmployeeId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vEmployeeId").html(item);
     
    });
}

function getMonth(type, vSalaryType) {
    var url = "/Payroll/PayrollReport/getMonth?type=" + type + "&vSalaryType=" + vSalaryType;
    console.log("getMonth " + type +" : "+ commonUrl + url);
    $("#vMonth").select2('data', { id: '0', text: '==Choose a Month==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Month==</option>'
        $("#vMonth").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vMonth").html(item);
    });
}

function getCertifiedBy(type) {
    var url = "/Payroll/PayrollReport/getCertifiedBy?type=" + type ;
    console.log("getCertifiedBy " + type + " : " + commonUrl + url);
    $("#vCertifiedById").select2('data', { id: '0', text: '=Choose a Certified By==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Certified By==</option>'
        $("#vCertifiedById").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vCertifiedById").html(item);
    });
}

function getDesignation(type, vCertifiedById) {
    var url = "/Payroll/PayrollReport/getDesignation?type=" + type + "&vCertifiedById=" + vCertifiedById;
    console.log("getDesignation" + type + " : " + commonUrl + url);
    $.getJSON(url, function (data)
    {
        $.each(data, function (i, opt) {
            $('#inpDesignationCertifiedBy').val(opt.text);
        });
    });
}

function getBank(type, vMonth) {
    var url = "/Payroll/PayrollReport/getBank?type=" + type + "&vMonth=" + vMonth ;
    console.log("getBank " + type +" : "+ commonUrl+url);
    $("#vBankId").select2('data', { id: '0', text: '==Choose a Bank==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Bank==</option>'
        $("#vBankId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vBankId").html(item);
    });
}
function getDepartment(type, dDate, vMonth, vStatus, vSalaryType) {
    var url = "/Payroll/PayrollReport/getDepartment?type=" + type + "&dDate=" + dDate + "&vMonth=" + vMonth + "&vStatus=" + vStatus + "&vSalaryType=" + vSalaryType;
    console.log("getDepartment " + type +" : "+ commonUrl + url);
    $("#vDepartmentId").select2('data', { id: '0', text: '==Choose a Department==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Department==</option>'
        item += '<option value="' + '%' + '">All</option>'
        $("#vDepartmentId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vDepartmentId").html(item);
    });
}

function getSection(type, dDate, vMonth, vDepartmentId, vStatus, vSalaryType)
{
    var url = "/Payroll/PayrollReport/getSection?type=" + type + "&dDate=" + dDate + "&vMonth=" + vMonth + "&vDepartmentId=" +
        vDepartmentId + "&vStatus=" + vStatus + "&vSalaryType=" + vSalaryType;

    console.log("getSection " + type +" : "+ commonUrl + url);
    $("#vSectionId").select2('data', { id: '0', text: '==Choose a Section==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Section==</option>'
         item += '<option value="' + '%' + '">All</option>'
        $("#vSectionId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vSectionId").html(item);
    });
}
function getBankBranch(type, vMonth) {
    var url = "/Payroll/PayrollReport/getBankBranch?type=" + type + "&vMonth=" + vMonth 
    console.log("getBankBranch " + type +" : "+ commonUrl + url);
    $("#mBranchId").select2('data', { id: '0', text: '==Choose a Branch==' });
  /*  console.log("HI");*/
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Branch==</option>'
        $("#mBranchId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });

        $("#mBranchId").html(item);
    });
}




/*Data Load Action End*/



//Clear Function
function clear() {
    var date = getCDay() + '-' + getCMonth() + '-' + getCYear();
    $("#type").val("");
    $('#vEmployeeId').select2('data', { id: '0', text: '==Choose a Employee==' });
    $("input[name=vStatus][value='Active']").prop("checked", true);
    $("input[name=vEmployeeType][value='Present Employee']").prop("checked", true);
    $("input[name=vWithOrWithOutGross][value='Without Gross']").prop("checked", true);
    $("#vCertifiedById").select2('data', { id: '0', text: '=Choose a Certified By==' });
    $("#vDesignationId").select2('data', { id: '0', text: '==Choose a designation==' })
    $("#dDate").val(date);
    $("#vMonth").select2('data', { id: '0', text: '==Choose a Month==' });
    $("#vDepartmentId").select2('data', { id: '0', text: '==Choose a Department==' });
    $("#vSectionId").select2('data', { id: '0', text: '==Choose a Section==' });
    $("#inpAccountNo").val("");
    $("#inpAddress").val("");
    $("#inpReferenceNo").val("");
}