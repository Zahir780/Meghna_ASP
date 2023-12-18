$(document).ready(function () {
    init();
    btnAction();

});

function btnAction() {
    $("#btnGenerate").click(function () {
        if (checkValidation()) {
            /*if (!checkDutyRoster()) {
                generate();
            }
            else {
                warningNotify("Data Already Exist!!");
            }*/
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
}

function generate() {
    var pDate = getBdToDbFormat($("#pDate").val());
    var pFromDate = getBdToDbFormat($("#pFromDate").val());
    var pToDate = getBdToDbFormat($("#pToDate").val());
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pShiftId = $("#pShiftId").select2('data');

    //pDate,pFromDate,pToDate,pEmployeeId,pDepartmentId,pSectionId,pShiftId

    var jsonData =
    {
        pDate: pDate,
        pFromDate: pFromDate,
        pToDate: pToDate,
        pEmployeeId: pEmployeeId.id,
        pDepartmentId: pDepartmentId.id,
        pSectionId: pSectionId.id,
        pShiftId: pShiftId.id,
    }
    //console.log(jsonData);
    $("#btnGenerate").hide();
    $("#btnGenerateLoad").show();
    $.ajax({
        type: "POST",
        url: "/Payroll/DutyRoster/Generate",
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
    var pDate = getBdToDbFormat($("#pDate").val());
    var pFromDate = getBdToDbFormat($("#pFromDate").val());
    var pToDate = getBdToDbFormat($("#pToDate").val());
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pShiftId = $("#pShiftId").select2('data');

    var fromDate = $("#pFromDate").val().split("-");
    var toDate = $("#pToDate").val().split("-");
    var monthFrom = fromDate[1];
    var monthTo = toDate[1];

    //pDate, pFromDate, pToDate, pEmployeeId, pDepartmentId, pSectionId, pShiftId

    if (!isEmpty(pDate))
    {
        if (!isEmpty(pFromDate))
        {
            if (!isEmpty(pToDate))
            {
                if (pDepartmentId.id != "0")
                {
                    if (pSectionId.id != "0")
                    {
                        if (pEmployeeId.id != "0")
                        {
                            if (pShiftId.id != "0")
                            {
                                if (pFromDate <= pToDate)
                                {
                                    if (Math.ceil((Date.parse(pToDate) - Date.parse(pFromDate)) / (1000 * 60 * 60 * 24)) <= 6)
                                    {
                                        if (monthFrom == monthTo)
                                        {
                                            return true;
                                        }
                                        else {
                                            warningNotify("From Date & To Date must be within Same Month");
                                            return false;
                                        }
                                    }
                                    else {
                                        warningNotify("From Date & To Date must be within 7 Days");
                                        return false;
                                    }
                                }
                                else {
                                    warningNotify("From Date can't be gether then To Date");
                                    return false;
                                }
                            }
                            else {
                                warningNotify("Please Choose Shift");
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
                warningNotify("Please Choose To Date");
                return false;
            }
        }
        else {
            warningNotify("Please Choose From Date");
            return false;
        }
    }
    else {
        warningNotify("Please Choose Entry Date");
        return false;
    }
}
function init() {
    clear();
    getShiftData();
}
function clear() {
    $("#pDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#pFromDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#pToDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());

    getDepartment();

    $("#btnGenerateLoad").hide();
    //$("#btnGenerate").prop('disabled', true);
}
function getDepartment() {
    var url = "/Payroll/DutyRoster/getDepartment";
    //console.log("getDepartment: " + url);
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
    var url = "/Payroll/DutyRoster/getSection?pDepartmentId=" + pDepartmentId;
    //console.log("getSection: " + url);
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
    var url = "/Payroll/DutyRoster/getEmployee?pDepartmentId=" + pDepartmentId + "&pSectionId=" + pSectionId;
    //console.log("getEmployee: " + url);
    $("#pEmployeeId").select2('data', { id: '0', text: '==Choose a Section==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Section==</option>'
        if (pDepartmentId != "%") {
            item += '<option value="' + '%' + '">All</option>'
        }
        $("#pEmployeeId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pEmployeeId").html(item);
    });
}

function checkDutyRoster()
{
    var pFromDate = getBdToDbFormat($("#pFromDate").val());
    var pToDate = getBdToDbFormat($("#pToDate").val());
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');

    //pFromDate,pToDate,pEmployeeId,pDepartmentId,pSectionId

    var url = "/Payroll/DutyRoster/checkDutyRoster?pFromDate=" + pFromDate + "&pToDate=" + pToDate +
        "&pEmployeeId=" + pEmployeeId.id + "&pDepartmentId=" + pDepartmentId.id + "&pSectionId=" + pSectionId.id;
    //console.log(url);
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

function getShiftData() {
    $('#pShiftId').select2('data', { id: 0, text: '==Choose a Shift==' });
    var url = "/Payroll/DutyRoster/getShiftData";
    $.getJSON(url, function (data) {
        var item = "";
        $('#pShiftId').empty();
        item += '<option value="' + 0 + '">==Choose a Shift==</option>'
        $('#pShiftId').html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $('#pShiftId').html(item);
    });

}


