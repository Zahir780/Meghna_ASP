var dataTable;
var table = "#tbEditDailyOrMonthlyAttendance";
var commonUrl = "https://localhost:44359/";
$(document).ready(function ()
{
    init();
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var dDate = getBdToDbFormat($("#dDate").val());
    var vAttendanceBy = $("input[name='vAttendanceBy']:checked").val();

    var url = "/Payroll/EditDailyOrMonthlyAttendance/GetAttendance?vAttendanceBy=" + vAttendanceBy + "&dDate=" + dDate +"&pEmployeeId=" + pEmployeeId.id +
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
            var vAttendanceBy = $("input[name='vAttendanceBy']:checked").val();
            var dDate = "";
            if (vAttendanceBy == "Date") { dDate = getBdToDbFormat($("#dDate").val()); }
            else { dDate = $("#vMonth").select2('data').id; }

            var url = "/Payroll/EditDailyOrMonthlyAttendance/GetAttendance?vAttendanceBy=" + vAttendanceBy +"&dDate=" + dDate + "&pEmployeeId=" + pEmployeeId.id +
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
    $('input[type=radio][name=vAttendanceBy]').change(function () {
        var vAttendanceBy = $("input[name='vAttendanceBy']:checked").val();
        if (vAttendanceBy != "") {
            attendanceOptionWork(vAttendanceBy);
            var dDate = getBdToDbFormat($("#dDate").val());
            if (dDate != "" && dDate != "0") {
                getDepartment();
            }
        }
    });
    $("#vMonth").click(function () {
        var vMonth = $("#vMonth").select2('data');
        if (vMonth != "" && vMonth != "0") {
            getDepartment();
        }
    });
    $("#dDate").change(function () {
        var dDate = getBdToDbFormat($("#dDate").val());
        if (dDate != "" && dDate != "0") {
            getDepartment();
        }
    });
}

function attendanceOptionWork(vAttendanceBy) {
    document.getElementById('pDate').style.display = "none";
    document.getElementById('pMonth').style.display = "none";

    if (vAttendanceBy == "Date") {
        document.getElementById('pDate').style.display = "block";
    }
    else if (vAttendanceBy == "Month") {
        document.getElementById('pMonth').style.display = "block";
    }
}

function saveWork() {
    var pEmployeeId = $("#pEmployeeId").select2('data');

    var vEmployeeId = document.querySelectorAll(table + " tr td .vEmployeeId");

    var dAttendanceDate = document.querySelectorAll(table + " tr td .dAttendanceDate");
    var inHour = document.querySelectorAll(table + " tr td .inHour");
    var inMinute = document.querySelectorAll(table + " tr td .inMinute");

    var dOutDate = document.querySelectorAll(table + " tr td .dOutDate");
    var outHour = document.querySelectorAll(table + " tr td .outHour");
    var outMinute = document.querySelectorAll(table + " tr td .outMinute");
    var vPermittedBy = document.querySelectorAll(table + " tr td .vPermittedBy");
    var vReason = document.querySelectorAll(table + " tr td .vReason");

    //vEmployeeId, dAttendanceDate, inHour, inMinute, dOutDate, outHour, outMinute, vPermittedBy, vReason

    var inTime = "";
    var outTime = "";

    var objData = {
        pEmployeeId: pEmployeeId.id,
        objList: []
    }
    //console.log(objData);

    for (var i = 0; i < vEmployeeId.length; i++)
    {
        if ($("#" + vPermittedBy[i].id).val() != '' && $("#" + vReason[i].id).val() != '')
        {
            inTime = $("#" + dAttendanceDate[i].id).val() + " " + $("#" + inHour[i].id).val() + ":" + $("#" + inMinute[i].id).val() + ":00";
            outTime = $("#" + dOutDate[i].id).val() + " " + $("#" + outHour[i].id).val() + ":" + $("#" + outMinute[i].id).val() + ":00";
            //console.log("inTime: " + inTime + " outTime: " + outTime);
            objData.objList.push(
                {
                    "vEmployeeId": $("#" + vEmployeeId[i].id).text(),
                    "dAttendanceDate": $("#" + dAttendanceDate[i].id).val(),
                    "inTime": inTime,
                    "outTime": outTime,
                    "vPermittedBy": $("#" + vPermittedBy[i].id).val(),
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
        url: "/Payroll/EditDailyOrMonthlyAttendance/updateAttendance",
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
    var id = 0;
    //console.log(url);
    dataTable = $('#tbEditDailyOrMonthlyAttendance').DataTable({
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
        "pageLength": 25,
        "processing": true,
        "scrollX": true,
        "ordering": false,
        "scrollCollapse": true,
        "order": [[0, "asc"]],
        "fixedColumns": {
            "leftColumns": 5
        },
        "columns": [
            {
                "data": "vEmployeeId",
                "render": function (data) {
                    return `
                        <div>
                            <label class="vEmployeeId text-center" id="vEmployeeId${++id}">${data}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vEmployeeCode: "vEmployeeCode", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label class="text-center" id="vEmployeeCode${++id}" style="width:50px;">${data.vEmployeeCode.replaceAll(".0000", "")}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vEmployeeName: "vEmployeeName", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label style="width:180px;">${data.vEmployeeName}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { vDepartmentName: "vDepartmentName", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label style="width:130px;">${data.vDepartmentName}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { txtDay: "txtDay", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <label style="width:90px;">${data.txtDay}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { dAttendanceDate: "dAttendanceDate", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:85px;" value="${data.dAttendanceDate}" 
                                id="dAttendanceDate${++id}" class="form-control input-sm dAttendanceDate" readonly/>
                            </div>
                           `;
                }
            },
            {
                "data": { inHour: "inHour", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:40px;" value="${data.inHour}" 
                                id="inHour${++id}" onkeypress="backgroundColor(event)" class="form-control input-sm inHour"/>
                            </div>
                           `;
                }
            },
            {
                "data": { inMinute: "inMinute", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:40px;" value="${data.inMinute}" 
                                id="inMinute${++id}" onkeypress="backgroundColor(event)" class="form-control input-sm inMinute"/>
                            </div>
                           `;
                }
            },
            {
                "data": { dOutDate: "dOutDate", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:85px;" value="${data.dOutDate}" 
                                id="dOutDate${++id}" class="form-control input-sm dOutDate"
                                onkeypress="inputMask(event,'0000-00-00'), backgroundColor(event)" 
                                maxlength="10" />
                            </div>
                           `;
                }
            },
            {
                "data": { outHour: "outHour", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:40px;" value="${data.outHour}" 
                                id="outHour${++id}" onkeypress="backgroundColor(event)" class="form-control input-sm outHour"/>
                            </div>
                           `;
                }
            },
            {
                "data": { outMinute: "outMinute", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:40px;" value="${data.outMinute}" 
                                id="outMinute${++id}" onkeypress="backgroundColor(event)" class="form-control input-sm outMinute"/>
                            </div>
                           `;
                }
            },
            {
                "data": { vPermittedBy: "vPermittedBy", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:100px;" value="${data.vPermittedBy}" 
                                id="vPermittedBy${++id}" onkeypress="backgroundColor(event)" class="form-control input-sm vPermittedBy"/>
                            </div>
                           `;
                }
            },
            {
                "data": { vReason: "vReason", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:100px;" value="${data.vReason}" 
                                id="vReason${++id}" onkeypress="backgroundColor(event)" class="form-control input-sm vReason"/>
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

    var emp = document.querySelectorAll(table + " tr td .vEmployeeId");
    var vPermittedBy = document.querySelectorAll(table + " tr td .vPermittedBy");
    var vReason = document.querySelectorAll(table + " tr td .vReason");
    var isEmployee = false;
    for (var i = 0; i < emp.length; i++) {
        if ($("#" + emp[i].id).text() != "" && $("#" + vPermittedBy[i].id).val() != '' && $("#" + vReason[i].id).val() != '') {
            isEmployee = true;
        }
    }
    if (pDepartmentId.id != "0" && pDepartmentId.id != "") {
        if (pSectionId.id != "0" && pSectionId.id != "") {
            if (pEmployeeId.id != "0" && pEmployeeId.id != "") {
                if (isEmployee) {
                    return true;
                }
                else {
                    warningNotify("No Data Found Or check Permitted By & Reasoan!");
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
function init() {
    clear();
}
function clear() {
    $('#pDepartmentId').select2('data', { id: "0", text: "==Choose a Department==" });
    $('#pSectionId').select2('data', { id: "0", text: "==Choose a Section==" });
    $('#pEmployeeId').select2('data', { id: "0", text: "==Choose a Employee==" });

    $("#dDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    getMonth();
    getDepartment();

    $("#btnSaveLoad").hide();
}
function tableClear() {
    var url = "/Payroll/EditDailyOrMonthlyAttendance/GetAttendance?dDate=" + 0 + "&pEmployeeId=" + 0 +
        "&pDepartmentId=" + 0 + "&pSectionId=" + 0;
    dataTable.ajax.url(url).load();
}
function getMonth() {
    var url = "/Payroll/EditDailyOrMonthlyAttendance/getMonth";
    console.log("getMonth: " + commonUrl + url);
    $("#vMonth").select2('data', { id: '0', text: '==Choose a Date==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Date==</option>'
        $("#vMonth").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#vMonth").html(item);
    });
}
function getDepartment() {
    var vAttendanceBy = $("input[name='vAttendanceBy']:checked").val();
    var dDate = "";
    if (vAttendanceBy == "Date") { dDate = getBdToDbFormat($("#dDate").val()); }
    else { dDate = $("#vMonth").select2('data').id; }

    var url = "/Payroll/EditDailyOrMonthlyAttendance/getDepartment?vAttendanceBy=" + vAttendanceBy + "&dDate=" + dDate;
    console.log("getDepartment: " + commonUrl + url);
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
    var vAttendanceBy = $("input[name='vAttendanceBy']:checked").val();
    var dDate = "";
    if (vAttendanceBy == "Date") { dDate = getBdToDbFormat($("#dDate").val()); }
    else { dDate = $("#vMonth").select2('data').id; }

    var url = "/Payroll/EditDailyOrMonthlyAttendance/getSection?vAttendanceBy=" + vAttendanceBy + "&dDate=" + dDate +"&pDepartmentId=" + pDepartmentId;
    console.log("getSection: " + commonUrl + url);
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
    var vAttendanceBy = $("input[name='vAttendanceBy']:checked").val();
    var dDate = "";
    if (vAttendanceBy == "Date") { dDate = getBdToDbFormat($("#dDate").val()); }
    else { dDate = $("#vMonth").select2('data').id; }

    var url = "/Payroll/EditDailyOrMonthlyAttendance/getEmployee?vAttendanceBy=" + vAttendanceBy + "&dDate=" + dDate + "&pDepartmentId=" + pDepartmentId +
        "&pSectionId=" + pSectionId;
    console.log("getEmployee: " + commonUrl + url);
    $("#pEmployeeId").select2('data', { id: '0', text: '==Choose a Section==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Section==</option>'

        var vAttendanceBy = $("input[name='vAttendanceBy']:checked").val();
        if (vAttendanceBy == "Date") {
            item += '<option value="' + '%' + '">All</option>'
        }

        $("#pEmployeeId").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pEmployeeId").html(item);
    });
}


