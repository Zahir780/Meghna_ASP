
$(document).ready(function () {
    init();
    btnAction();

});

function btnAction() {
    $("#btnDelete").click(function () {
        if (checkValidation()) {
            deleteAttendance();
        }
    });

    $("#dDate").change(function () {
        var dDate = getBdToDbFormat($("#dDate").val());
        if (dDate != "" && dDate != "0") {
            getDepartment(dDate);
        }
    });
    $("#pDepartmentId").click(function () {
        var dDate = getBdToDbFormat($("#dDate").val());
        var pDepartmentId = $("#pDepartmentId").select2('data');
        if (dDate != "0" && pDepartmentId.id != "0") {
            getSection(dDate, pDepartmentId.id);
        }
    });
    $("#pSectionId").click(function () {
        var dDate = getBdToDbFormat($("#dDate").val());
        var pDepartmentId = $("#pDepartmentId").select2('data');
        var pSectionId = $("#pSectionId").select2('data');
        if (dDate != "0" && pDepartmentId.id != "0" && pSectionId.id != "0")
        {
            getEmployee(dDate, pDepartmentId.id, pSectionId.id);
        }
    });
}
function deleteAttendance() {
    var dDate = getBdToDbFormat($("#dDate").val());
    var pEmployeeId = $("#pEmployeeId").select2('data');

    var jsonData =
    {
        dDate: dDate,
        pEmployeeId: pEmployeeId.id
    }
    //console.log(jsonData);

    $.ajax({
        type: "POST",
        url: "/Payroll/DeleteAttendance/deleteAttendance",
        data: jsonData,
        async: false,
        success: function (res)
        {
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
    var dDate = $("#dDate").val();
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');

    if (dDate != "0") {
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
        warningNotify("Please Choose Attendance Date");
        return false;
    }
}
function init() {
    clear();
}
function clear() {
    var dDate = getCDay() + '-' + getCMonth() + '-' + getCYear();
    $("#dDate").val(dDate);
    getDepartment(getBdToDbFormat($("#dDate").val()));
    $('#pDepartmentId').select2('data', { id: "0", text: "==Choose a Department==" });
    $('#pSectionId').select2('data', { id: "0", text: "==Choose a Section==" });
    $('#pEmployeeId').select2('data', { id: "0", text: "==Choose a Employee==" });
    $("#btnDeleteLoad").hide();
}
function getDepartment(dDate) {
    var url = "/Payroll/DeleteAttendance/getDepartment?dDate=" + dDate;
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
function getSection(dDate,pDepartmentId) {
    var url = "/Payroll/DeleteAttendance/getSection?dDate=" + dDate + "&pDepartmentId=" + pDepartmentId;
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
function getEmployee(dDate, pDepartmentId, pSectionId) {
    var url = "/Payroll/DeleteAttendance/getEmployee?dDate=" + dDate + "&pDepartmentId=" + pDepartmentId + "&pSectionId=" + pSectionId;
    console.log("getEmployee: " + url);
    $("#pEmployeeId").select2('data', { id: '0', text: '==Choose a Section==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Section==</option>'
        $("#pEmployeeId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pEmployeeId").html(item);
    });
}


