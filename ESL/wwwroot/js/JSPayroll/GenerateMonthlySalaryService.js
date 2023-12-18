$(document).ready(function () {
    init();
    btnAction();

});

function btnAction() {
    $("#btnGenerate").click(function () {
        if (checkValidation()) {
            if (!checkSalary()) {
                generate();
            }
            else {
                warningNotify("Salary Already Exist!!");
            }
        }
    });
    $("#pDepartmentId").click(function () {
        var pDepartmentId = $("#pDepartmentId").select2('data');
        if (!isEmpty(pDepartmentId)) {
            getSection(pDepartmentId.id);
        }
    });
    $("#pSectionId").click(function () {
        var pDepartmentId = $("#pDepartmentId").select2('data');
        var pSectionId = $("#pSectionId").select2('data');
        if (!isEmpty(pDepartmentId)) {
            if (!isEmpty(pSectionId)) {
                getEmployee(pDepartmentId.id, pSectionId.id);
            }
        }
    });
    $("#pSalaryMonthDate").change(function () {
        getMonthRelatedData();
    });
}
function generate() {
    var pGenerateDate = getBdToDbFormat($("#pGenerateDate").val());
    var pSalaryMonthDate = getBdToDbFormat($("#pSalaryMonthDate").val());
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pRevenueStamp = $("#pRevenueStamp").val();

    var jsonData =
    {
        pGenerateDate: pGenerateDate,
        pSalaryMonthDate: pSalaryMonthDate,
        pEmployeeId: pEmployeeId.id,
        pDepartmentId: pDepartmentId.id,
        pSectionId: pSectionId.id,
        pRevenueStamp: pRevenueStamp 
    }
    console.log(jsonData);
    $("#btnGenerate").hide();
    $("#btnGenerateLoad").show();
    $.ajax({
        type: "POST",
        url: "/Payroll/GenerateMonthlySalary/Generate",
        data: jsonData,
        async: false,
        success: function (res) {
            //console.log(JSON.stringify(res.data));
            if (res.success) {
                $("#btnGenerate").show();
                $("#btnGenerateLoad").hide();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}

function checkValidation() {
    var pGenerateDate = $("#pGenerateDate").val();
    var pSalaryMonthDate = $("#pSalaryMonthDate").val();
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pRevenueStamp = $("#pRevenueStamp").val();

    if (!isEmpty(pGenerateDate))
    {
        if (!isEmpty(pSalaryMonthDate))
        {
            if (pDepartmentId.id != "0")
            {
                if (pSectionId.id != "0")
                {
                    if (pEmployeeId.id != "0")
                    {
                        if (!isEmpty(pRevenueStamp))
                        {
                            return true;
                        }
                        else {
                            warningNotify("Please Choose Revenue Stamp");
                            return false;
                        }
                    }
                    else {
                        warningNotify("Please Choose Employee");
                        return false;
                    }
                }
                else {
                    warningNotify("Please Choose Section");
                    return false;
                }
            }
            else {
                warningNotify("Please Choose Department");
                return false;
            }
        }
        else {
            warningNotify("Please Choose Salary Month");
            return false;
        }
    }
    else {
        warningNotify("Please Choose Generate Date");
        return false;
    }
}
function init() {
    clear();
    getMonthRelatedData();
}
function clear() {
    $("#pGenerateDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#pSalaryMonthDate").val(getCDayLast() + '-' + getCMonth() + '-' + getCYear());

    getDepartment();

    $("#pTotalDays").prop('disabled', true);
    $("#pFridays").prop('disabled', true);
    $("#pHolidays").prop('disabled', true);
    $("#pWorkingDays").prop('disabled', true);
    $("#pTotalDays").val("");
    $("#pFridays").val("");
    $("#pHolidays").val("");
    $("#pWorkingDays").val("");
    $("#pRevenueStamp").val("10");

    $("#btnGenerateLoad").hide();
    //$("#btnGenerate").prop('disabled', true);
}
function getDepartment() {
    var url = "/Payroll/GenerateMonthlySalary/getDepartment";
    console.log("getDepartment: " + url);
    $("#pDepartmentId").select2('data', { id: '0', text: '==Choose a Department==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Department==</option>'
        item += '<option value="' + '%' + '">All</option>'
        $("#pDepartmentId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pDepartmentId").html(item);
    });
}
function getSection(pDepartmentId) {
    var url = "/Payroll/GenerateMonthlySalary/getSection?pDepartmentId=" + pDepartmentId;
    console.log("getSection: " + url);
    $("#pSectionId").select2('data', { id: '0', text: '==Choose a Section==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Section==</option>'
        item += '<option value="' + '%' + '">All</option>'
        $("#pSectionId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pSectionId").html(item);
    });
}
function getEmployee(pDepartmentId, pSectionId) {
    var url = "/Payroll/GenerateMonthlySalary/getEmployee?pDepartmentId=" + pDepartmentId + "&pSectionId=" + pSectionId;
    console.log("getEmployee: " + url);
    $("#pEmployeeId").select2('data', { id: '0', text: '==Choose a Section==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Section==</option>'
        item += '<option value="' + '%' + '">All</option>'
        $("#pEmployeeId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pEmployeeId").html(item);
    });
}

function checkSalary()
{
    var pSalaryMonthDate = getBdToDbFormat($("#pSalaryMonthDate").val());
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');

    var url = "/Payroll/GenerateMonthlySalary/checkSalary?pSalaryMonthDate=" + pSalaryMonthDate + "&pEmployeeId=" + pEmployeeId.id +
        "&pDepartmentId=" + pDepartmentId.id + "&pSectionId=" + pSectionId.id;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.data;
        }
    });
    return result;
}

function getMonthRelatedData()
{
    var pSalaryMonthDate = getBdToDbFormat($("#pSalaryMonthDate").val());
    if (pSalaryMonthDate != "" && pSalaryMonthDate != null && pSalaryMonthDate != undefined) {
        $.ajax({
            url: "/Payroll/GenerateMonthlySalary/getMonthRelatedData?pSalaryMonthDate=" + pSalaryMonthDate,
            data: { pSalaryMonthDate: pSalaryMonthDate },
            async: false,
            success: function (res) {
                console.log("find:" + JSON.stringify(res.data));
                if (res.isFind) {
                    if (!isEmpty(res.data)) {
                        $("#pTotalDays").val(res.data.pTotalDays);
                        $("#pFridays").val(res.data.pFridays);
                        $("#pHolidays").val(res.data.pHolidays);
                        $("#pWorkingDays").val(res.data.pWorkingDays);
                    }
                }
                else {
                    errorNotify("Data not found!");
                }
            }
        });
    }

}


