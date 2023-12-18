var dataTable;
var table = "#tbDateBetweenAttendanceManuallyMachine";
var commonUrl = "https://localhost:44359/";
$(document).ready(function ()
{
    init();
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');

    var url = "/Payroll/DateBetweenAttendanceManuallyMachine/GetAttendance?pEmployeeId=" + pEmployeeId.id +
        "&pDepartmentId=" + pDepartmentId.id + "&pSectionId=" + pSectionId.id ;
    loadDataTable(url);
    btnAction();

});

function btnAction() {
    $("#pDepartmentId").click(function () {
        var pDepartmentId = $("#pDepartmentId").select2('data');
        if (pDepartmentId.id != "0") {
            getSection(pDepartmentId.id);
        }
    });
    $("#pSectionId").click(function () {
        var pDepartmentId = $("#pDepartmentId").select2('data');
        var pSectionId = $("#pSectionId").select2('data');
        if (pDepartmentId.id != "0" && pSectionId.id != "0") {
            getEmployee(pDepartmentId.id, pSectionId.id);
        }
    });

    $("#pEmployeeId").change(function () {
        var pEmployeeId = $("#pEmployeeId").select2('data');
        var pDepartmentId = $("#pDepartmentId").select2('data');
        var pSectionId = $("#pSectionId").select2('data');

        if (pEmployeeId.id != "0" && pDepartmentId.id != "0" && pSectionId.id != "0")
        {
            var url = "/Payroll/DateBetweenAttendanceManuallyMachine/GetAttendance?pEmployeeId=" + pEmployeeId.id +
                "&pDepartmentId=" + pDepartmentId.id + "&pSectionId=" + pSectionId.id;
            console.log("Url: " + commonUrl + url);
            dataTable.ajax.url(url).load();
        }
    });
    $("#btnSave").click(function () {
        if (checkValidation()) {
            saveWork();
        }
    });
    $("#btnRefresh").click(function () {
        clear();
        tableClear();
    });
}

function saveWork() {
    var formdata = new FormData();

    var pEmployeeId = $("#pEmployeeId").select2('data');

    var dFromDate = getBdToDbFormat($("#dFromDate").val());
    var vHHFrom = $("#vHHFrom").select2('data');
    var vMMFrom = $("#vMMFrom").select2('data');
    var vAmPmFrom = $("#vAmPmFrom").select2('data');

    var dToDate = getBdToDbFormat($("#dToDate").val());
    var vHHTo = $("#vHHTo").select2('data');
    var vMMTo = $("#vMMTo").select2('data');
    var vAmPmTo = $("#vAmPmTo").select2('data');

    var vEmployeeId = document.querySelectorAll(table + " tr td .vEmployeeId");
    var vReason = document.querySelectorAll(table + " tr td .vReason");

    var inTime = "";
    var outTime = "";


    if (vAmPmFrom.id == "PM") {
        inTime = parseInt(vHHFrom.id) + 12 + ":" + vMMFrom.id + ":00";
    }
    else {
        inTime = vHHFrom.id + ":" + vMMFrom.id + ":00";
    }

    if (vAmPmTo.id=="PM") {
        outTime = parseInt(vHHTo.id) + 12 + ":" + vMMTo.id + ":00";
    }
    else {
        outTime = vHHTo.id + ":" + vMMTo.id + ":00";
    }


    /*
     vEmployeeId,vReason
    */

    var objData = {
        pEmployeeId: pEmployeeId.id,
        dFromDate: dFromDate,
        inTime: dFromDate + ' ' + inTime,

        dToDate: dToDate,
        outTime: dFromDate + ' ' + outTime,
        objList: []
    }
    //console.log(objData);

    for (var i = 0; i < vEmployeeId.length; i++) {
        if ($("#" + vReason[i].id).val() !='') {
            objData.objList.push(
                {
                    "vEmployeeId": $("#" + vEmployeeId[i].id).text(),
                    "vReason": $("#" + vReason[i].id).val()
                }
            );

        }
    }
    console.log(objData);
    save(objData);
}
function save(objData)
{
    $("#btnSave").hide();
    $("#btnSaveLoad").show();
    $.ajax({
        url: "/Payroll/DateBetweenAttendanceManuallyMachine/generateAttendance",
        data: objData,
        type: 'POST',
        async: false,
        success: function (res)
        {
            console.log(res);
            if (res.success) {
                console.log(res);
                console.log(JSON.stringify(objData));
                $("#btnSave").show();
                $("#btnSaveLoad").hide();
                successNotify(res.message);
                clear();
                tableClear();
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}

function loadDataTable(url) {
    //console.log(url);
    dataTable = $('#tbDateBetweenAttendanceManuallyMachine').DataTable({
        "ajax": {
            "url": url
        },
        "language":
        {
            "decimal": "",
            "infoPostFix": "",
            "thousands": ",",
            "loadingRecords": "Loading...",
            "processing": "Processing...",
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            }
        },
        /*"columnDefs": [
            { 'sortable': true, 'searchable': false, 'visible': false, 'type': 'text', 'targets': [0] }
        ],*/
        "pageLength": 25,
        "processing": true,
        "scrollX": true,
        "ordering": false,
        "scrollCollapse": true,
        "order": [[0, "asc"]],
        "fixedColumns": {
            "leftColumns": 2
        },
        "columns": [
            {
                "data": "vEmployeeId",
                "render": function (data) {
                    var id = Math.floor((Math.random() * 100) + 1);
                    return `
                        <div>
                            <label class="vEmployeeId" id="vEmployeeId${id}">${data}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vEmployeeCode: "vEmployeeCode", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label class="text-right" id="vEmployeeCode${data.vEmployeeId}" style="width:50px;">${data.vEmployeeCode.replaceAll(".0000", "")}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vEmployeeName: "vEmployeeName", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label style="width:200px;">${data.vEmployeeName}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vDepartmentName: "vDepartmentName", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label style="width:150px;">${data.vDepartmentName}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vSectionName: "vSectionName", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label style="width:150px;">${data.vSectionName}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vReason: "vReason", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:150px;" value="${data.vReason}" 
                                id="vReason${data.vEmployeeId}" class="form-control input-sm vReason"/>
                            </div>
                           `;
                }
            }
        ]
    });
}






function checkValidation()
{
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var dFromDate = getBdToDbFormat($("#dFromDate").val());
    var dToDate = getBdToDbFormat($("#dToDate").val());
    var pEmployeeId = $("#pEmployeeId").select2('data');

    var emp = document.querySelectorAll(table + " tr td .vEmployeeId");
    var isEmployee = false;
    for (var i = 0; i < emp.length; i++) {
        if ($("#" + emp[i].id).text() != "") {
            isEmployee = true;
        }
    }
    if (dFromDate != "0" && dFromDate != "")
    {
        if (dToDate != "0" && dToDate != "")
        {
            if (pDepartmentId.id != "0" && pDepartmentId.id != "") {
                if (pSectionId.id != "0" && pSectionId.id != "") {
                    if (pEmployeeId.id != "0" && pEmployeeId.id != "") {
                        if (isEmployee) {
                            return true;
                        }
                        else {
                            warningNotify("No Data Found!");
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
    else
    {
        warningNotify("Please Choose From Date");
        return false;
    }
}
function init() {
    clear();
}
function clear() {
    getDepartment();
    $('#pDepartmentId').select2('data', { id: "0", text: "==Choose a Department==" });
    $('#pSectionId').select2('data', { id: "0", text: "==Choose a Section==" });
    $('#pEmployeeId').select2('data', { id: "0", text: "==Choose a Employee==" });

    $("#dFromDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#dToDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#btnSaveLoad").hide();
}
function tableClear() {
    var url = "/Payroll/DateBetweenAttendanceManuallyMachine/GetAttendance?pSalaryDate=" + 0 + "&pEmployeeId=" + 0 +
        "&pDepartmentId=" + 0 + "&pSectionId=" + 0;
    dataTable.ajax.url(url).load();
}
function getDepartment() {
    var url = "/Payroll/DateBetweenAttendanceManuallyMachine/getDepartment";
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
    var url = "/Payroll/DateBetweenAttendanceManuallyMachine/getSection?pDepartmentId=" + pDepartmentId;
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
    var url = "/Payroll/DateBetweenAttendanceManuallyMachine/getEmployee?pDepartmentId=" + pDepartmentId + "&pSectionId=" + pSectionId;
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


