$(document).ready(function () {
    init();
    btnAction();

});

function btnAction() {
    $("#btnDelete").click(function () {
        if (checkValidation()) {
            deleteDutyRoster();
        }
    });
    $("#pSalaryDate").click(function () {
        var pSalaryDate = $("#pSalaryDate").select2('data');
        if (!isEmpty(pSalaryDate)) {
            getDepartment(pSalaryDate.id);
        }
    });
    $("#pDepartmentId").click(function () {
        var pSalaryDate = $("#pSalaryDate").select2('data');
        var pDepartmentId = $("#pDepartmentId").select2('data');
        if (pSalaryDate.id != "0" && pDepartmentId.id != "0") {
            getSection(pSalaryDate.id, pDepartmentId.id);
        }
    });
    $("#pSectionId").click(function () {
        var pSalaryDate = $("#pSalaryDate").select2('data');
        var pDepartmentId = $("#pDepartmentId").select2('data');
        var pSectionId = $("#pSectionId").select2('data');
        if (pSalaryDate.id != "0" && pDepartmentId.id != "0" && pSectionId.id != "0")
        {
            getEmployee(pSalaryDate.id, pDepartmentId.id, pSectionId.id);
        }
    });
}
function deleteDutyRoster() {
    var pSalaryDate = $("#pSalaryDate").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');

    var jsonData =
    {
        pSalaryDate: pSalaryDate.id,
        pEmployeeId: pEmployeeId.id,
        pDepartmentId: pDepartmentId.id,
        pSectionId: pSectionId.id
    }
    $("#btnDelete").hide();
    $("#btnDeleteLoad").show();
    $.ajax({
        type: "POST",
        url: "/Payroll/DeleteDutyRoster/deleteDutyRoster",
        data: jsonData,
        async: false,
        success: function (res) {
            //console.log(JSON.stringify(res.data));
            if (res.success) {
                $("#btnDelete").show();
                $("#btnDeleteLoad").hide();
                clear();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}

function checkValidation() {
    var pSalaryDate = $("#pSalaryDate").val();
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');

    if (!isEmpty(pSalaryDate)) {
        if (pDepartmentId.id != "0") {
            if (pSectionId.id != "0") {
                if (pEmployeeId.id != "0") {
                    return true;
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
        warningNotify("Please Choose Salary Date");
        return false;
    }
}
function init() {
    clear();
}
function clear() {
    getSalaryDate();
    $('#pDepartmentId').select2('data', { id: "0", text: "==Choose a Department==" });
    $('#pSectionId').select2('data', { id: "0", text: "==Choose a Section==" });
    $('#pEmployeeId').select2('data', { id: "0", text: "==Choose a Employee==" });
    $("#btnDeleteLoad").hide();
}
function getSalaryDate() {
    var url = "/Payroll/DeleteDutyRoster/getSalaryDate";
    console.log("getSalaryDate: " + url);
    $("#pSalaryDate").select2('data', { id: '0', text: '==Choose a Date==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Date==</option>'
        $("#pSalaryDate").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pSalaryDate").html(item);
    });
}
function getDepartment(pSalaryDate) {
    var url = "/Payroll/DeleteDutyRoster/getDepartment?pSalaryDate=" + pSalaryDate;
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
function getSection(pSalaryDate,pDepartmentId) {
    var url = "/Payroll/DeleteDutyRoster/getSection?pSalaryDate=" + pSalaryDate + "&pDepartmentId=" + pDepartmentId;
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
function getEmployee(pSalaryDate, pDepartmentId, pSectionId) {
    var url = "/Payroll/DeleteDutyRoster/getEmployee?pSalaryDate=" + pSalaryDate + "&pDepartmentId=" + pDepartmentId + "&pSectionId=" + pSectionId;
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


