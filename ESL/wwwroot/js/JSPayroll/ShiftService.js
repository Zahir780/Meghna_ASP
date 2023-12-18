$(document).ready(function () {
    $("#btnAdd").click(function () {
        init();
        $("#ShiftInfo").modal('show');

    });
    $('#tbShift').on('click', '.tbEdit', function () {
        setEnableDisable(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbShift").dataTable().fnGetData(tr);
        $("#btnLoadEdit").hide();
        if (isData(data.iAutoId)) {
            findWork(data.iAutoId);
            $("#ShiftInfo").modal('show');
        }
        else {
            errorNotify("Data not found");
        }
    });
    $("#btnSave").click(function () {
        if (checkValiAutoIdation()) {
            saveWork(false, false);
        }
    });
    $("#btnEdit").click(function () {
        if (checkValiAutoIdation()) {
            saveWork(false, true);
        }
    });
    $("#btnSaveNew").click(function () {
        if (checkValiAutoIdation()) {
            saveWork(true, false);
        }
    });
});
$(document).ready(function () {
    $("#vShiftName").change(function () {
        if (nameCheck($(this).val())) {
            $("#vShiftName").val("");
            $("#vShiftName").focus();
            warningNotify("Shift name already taken! try new one.");
        }
        else {
        }
    });
});
function hideButton(isEdit) {
    if (isEdit) {
        $("#btnEdit").hide();
        $("#btnDelete").hide();
        $("#btnLoadEdit").show();
    } else {
        $("#btnSave").hide();
        $("#btnSaveNew").hide();
        $("#btnLoad").show();
    }
}
function showButton(isEdit) {
    if (isEdit) {
        $("#btnEdit").show();
        $("#btnDelete").show();
        $("#btnLoadEdit").hide();
    } else {
        $("#btnSave").show();
        $("#btnSaveNew").show();
        $("#btnLoad").hide();
    }
}
function maxId() {
    var url = "/Payroll/ShiftInfo/GetMaxShift";
    var result = "";
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            //console.log(res);
            result = res.data;
        }
    });
    return result;
}

function nameCheck(name) {
    var url = "/Payroll/ShiftInfo/nameCheck?name=" + name;
    var result = false;
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.isFind;
        }
    });
    return result;
}

function setEnableDisable(edit) {
    document.getElementById('add').style.display = "none";
    document.getElementById('edit').style.display = "none";
    if (edit) {
        document.getElementById('add').style.display = "none";
        document.getElementById('edit').style.display = "block";
    } else {
        document.getElementById('add').style.display = "block";
        document.getElementById('edit').style.display = "none";
    }
}
function init() {
    showButton(false);
    setEnableDisable(false);
    clear();
    $("#vShiftId").val(maxId());
}
function findWork(iAutoId) {
    if (iAutoId != "0" && iAutoId != "") {
        $.ajax({
            url: "/Payroll/ShiftInfo/findData",
            data: { iAutoId: iAutoId },
            async: false,
            success: function (res) {
                //console.log("find:" + JSON.stringify(res.data));
                if (res.isFind) {
                    if (!isEmpty(res.data)) {
                        $("#iAutoId").val(res.data.iAutoId);
                        $("#vShiftId").val(res.data.vShiftId);
                        $("#vShiftName").val(res.data.vShiftName);
                        $('#vShiftType').select2('data', { id: res.data.vShiftType, text: res.data.vShiftType });

                        $('#').select2('data', { id: res.data.vHHShiftStart, text: res.data.vHHShiftStart });

                        var tValue = "";

                        if (res.data.vAMPMShiftStart == "PM") {
                            tValue = getTimeValue(res.data.vHHShiftStart - 12);
                            $('#vHHShiftStart').select2('data', { id: tValue, text: tValue });
                        }
                        else {
                            $('#vHHShiftStart').select2('data', { id: res.data.vHHShiftStart, text: res.data.vHHShiftStart });
                        }
                        $('#vMMShiftStart').select2('data', { id: res.data.vMMShiftStart, text: res.data.vMMShiftStart });
                        $('#vAMPMShiftStart').select2('data', { id: res.data.vAMPMShiftStart, text: res.data.vAMPMShiftStart });

                        if (res.data.vAMPMLateInLimit == "PM") {
                            tValue = getTimeValue(res.data.vHHLateInLimit - 12);
                            $('#vHHLateInLimit').select2('data', { id: tValue, text: tValue });
                        }
                        else {
                            $('#vHHLateInLimit').select2('data', { id: res.data.vHHLateInLimit, text: res.data.vHHLateInLimit });
                        }


                        if (res.data.vAMPMShiftEnd == "PM") {
                            tValue = getTimeValue(res.data.vHHShiftEnd - 12);
                            $('#vHHShiftEnd').select2('data', { id: tValue, text: tValue });
                        }
                        else {
                            $('#vHHShiftEnd').select2('data', { id: res.data.vHHShiftEnd, text: res.data.vHHShiftEnd });
                        }
                        $('#vMMShiftEnd').select2('data', { id: res.data.vMMShiftEnd, text: res.data.vMMShiftEnd });
                        $('#vAMPMShiftEnd').select2('data', { id: res.data.vAMPMShiftEnd, text: res.data.vAMPMShiftEnd });

                        if (res.data.vAMPMLateInLimit == "PM") {
                            tValue = getTimeValue(res.data.vHHLateInLimit - 12);
                            $('#vHHLateInLimit').select2('data', { id: tValue, text: tValue });
                        }
                        else {
                            $('#vHHLateInLimit').select2('data', { id: res.data.vHHLateInLimit, text: res.data.vHHLateInLimit });
                        }
                        //$('#vHHLateInLimit').select2('data', { id: res.data.vHHLateInLimit, text: res.data.vHHLateInLimit });
                        $('#vMMLateInLimit').select2('data', { id: res.data.vMMLateInLimit, text: res.data.vMMLateInLimit });
                        $('#vAMPMLateInLimit').select2('data', { id: res.data.vAMPMLateInLimit, text: res.data.vAMPMLateInLimit });

                        if (res.data.vAMPMEarlyOutLimit == "PM") {
                            tValue = getTimeValue(res.data.vHHEarlyOutLimit - 12);
                            $('#vHHEarlyOutLimit').select2('data', { id: tValue, text: tValue });
                        }
                        else {
                            $('#vHHEarlyOutLimit').select2('data', { id: res.data.vHHEarlyOutLimit, text: res.data.vHHEarlyOutLimit });
                        }
                        //$('#vHHEarlyOutLimit').select2('data', { id: res.data.vHHEarlyOutLimit, text: res.data.vHHEarlyOutLimit });
                        $('#vMMEarlyOutLimit').select2('data', { id: res.data.vMMEarlyOutLimit, text: res.data.vMMEarlyOutLimit });
                        $('#vAMPMEarlyOutLimit').select2('data', { id: res.data.vAMPMEarlyOutLimit, text: res.data.vAMPMEarlyOutLimit });

                        /*vHHShiftStart,vMMShiftStart,vAMPMShiftStart,
                        vHHShiftEnd,vMMShiftEnd,vAMPMShiftEnd,
                        vHHLateInLimit,vMMLateInLimit,vAMPMLateInLimit,
                        vHHEarlyOutLimit,vMMEarlyOutLimit,vAMPMEarlyOutLimit
                        iHHDuration,iMMDuration
                        */

                        $("#iHHDuration").val(res.data.iHHDuration);
                        $("#iMMDuration").val(res.data.iMMDuration);

                        if (res.data.iActive == "True") {
                            $('#iActive').select2('data', { id: "1", text: "Active" });
                        }
                        else {
                            $('#iActive').select2('data', { id: "0", text: "Inactive" });
                        }

                        $("#vCompanyId").val(res.data.vCompanyId);
                        $("#UserId").val(res.data.UserId);
                        $("#UserIp").val(res.data.UserIp);
                        $("#EntryTime").val(res.data.EntryTime);
                    }
                }
                else {
                    errorNotify("Data not found!");
                }
            }
        });
    }
}
function isData(iAutoId) {
    var ret = false;
    $.ajax({
        url: "/Payroll/ShiftInfo/findData",
        data: { iAutoId: iAutoId },
        async: false,
        success: function (res) {
            ret = res.isFind;
        }
    });
    return ret;
}
function saveWork(isNew, isEdit) {
    hideButton(isEdit);
    var iAutoId = 0;
    var vShiftId = maxId();

    if (isEdit) {
        iAutoId = $("#iAutoId").val();
        vShiftId = $("#vShiftId").val();
    }
    var vShiftName = $("#vShiftName").val();
    var vShiftType = $("#vShiftType").select2('data');

    var iActive = $("#iActive").select2('data');
    var status = true;
    if (iActive.id == 0) { status = false; }

    var vHHShiftStart = $("#vHHShiftStart").select2('data');
    var vMMShiftStart = $("#vMMShiftStart").select2('data');
    var vAMPMShiftStart = $("#vAMPMShiftStart").select2('data');

    var vHHShiftEnd = $("#vHHShiftEnd").select2('data');
    var vMMShiftEnd = $("#vMMShiftEnd").select2('data');
    var vAMPMShiftEnd = $("#vAMPMShiftEnd").select2('data');

    var vHHLateInLimit = $("#vHHLateInLimit").select2('data');
    var vMMLateInLimit = $("#vMMLateInLimit").select2('data');
    var vAMPMLateInLimit = $("#vAMPMLateInLimit").select2('data');

    var vHHEarlyOutLimit = $("#vHHEarlyOutLimit").select2('data');
    var vMMEarlyOutLimit = $("#vMMEarlyOutLimit").select2('data');
    var vAMPMEarlyOutLimit = $("#vAMPMEarlyOutLimit").select2('data');

    var dShiftStart = "";
    var dShiftEnd = "";

    var dLateInLimit = "";
    var dEarlyOutLimit = "";

    if (vAMPMShiftStart.id == "PM") {
        //console.log("vAMPMShiftStart" + vAMPMShiftStart.id);
        dShiftStart = parseInt(vHHShiftStart.id) + 12 + ":" + vMMShiftStart.id + ":00";
    }
    else {
        dShiftStart = vHHShiftStart.id + ":" + vMMShiftStart.id + ":00";
    }

    //console.log("vAMPMShiftEnd" + vAMPMShiftEnd.id);
    if (vAMPMShiftEnd.id == "PM") {
        //console.log("vAMPMShiftEnd" + vAMPMShiftEnd.id);
        dShiftEnd = parseInt(vHHShiftEnd.id) + 12 + ":" + vMMShiftEnd.id + ":00";
    }
    else {
        dShiftEnd = vHHShiftEnd.id + ":" + vMMShiftEnd.id + ":00";
    }

    if (vAMPMLateInLimit.id == "PM") {
        //console.log("vAMPMLateInLimit" + vAMPMLateInLimit.id);
        dLateInLimit = parseInt(vHHLateInLimit.id) + 12 + ":" + vMMLateInLimit.id + ":00";
    }
    else {
        dLateInLimit = vHHLateInLimit.id + ":" + vMMLateInLimit.id + ":00";
    }

    if (vAMPMEarlyOutLimit.id == "PM") {
        //console.log("vAMPMEarlyOutLimit" + vAMPMEarlyOutLimit.id);
        dEarlyOutLimit = parseInt(vHHEarlyOutLimit.id) + 12 + ":" + vMMEarlyOutLimit.id + ":00";
    }
    else {
        dEarlyOutLimit = vHHEarlyOutLimit.id + ":" + vMMEarlyOutLimit.id + ":00";
    }

    var iHHDuration = $("#iHHDuration").val();
    var iMMDuration = $("#iMMDuration").val();

    /*vHHShiftStart,vMMShiftStart,vAMPMShiftStart,
    vHHShiftEnd,vMMShiftEnd,vAMPMShiftEnd,
    vHHLateInLimit,vMMLateInLimit,vAMPMLateInLimit,
    vHHEarlyOutLimit,vMMEarlyOutLimit,vAMPMEarlyOutLimit,
    iHHDuration,iMMDuration*/

    var jsonData =
    {
        iAutoId: iAutoId,
        vShiftId: vShiftId,
        vShiftName: vShiftName,
        vShiftType: vShiftType.id,
        dShiftStart: dShiftStart,
        dShiftEnd: dShiftEnd,
        dLateInLimit: dLateInLimit,
        dEarlyOutLimit: dEarlyOutLimit,
        iHHDuration: iHHDuration,
        iMMDuration: iMMDuration,
        iActive: status,
        addUpdate: isEdit
    }
    //console.log(jsonData);
    $.ajax({
        type: "POST",
        url: "/Payroll/ShiftInfo/shiftSave",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                clear();
                if (!isNew) {
                    $("#ShiftInfo").modal('hide');
                }
                showButton(isEdit);
                $("#vShiftName").focus();
                $('#tbShift').DataTable().ajax.reload();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}

function clear() {
    $("#iAutoId").val(0);
    $("#vShiftName").val("");
    $('#vShiftType').select2('data', { id: 'Day', text: 'Day' });
    $("#vShiftId").val(maxId());
    $("#iHHDuration").val("");
    $("#iMMDuration").val("");
    $('#iActive').select2('data', { id: '1', text: 'Active' });
}
function checkValiAutoIdation() {
    if ($("#vShiftName").val() != "") {
        if ($("#iHHDuration").val() != "") {
            if ($("#iMMDuration").val() != "") {
                return true;
            }
            else {
                warningNotify("Please Provide Duration Minute!");
                return false;
            }
        }
        else {
            warningNotify("Please Provide Duration Hours!");
            return false;
        }
    }
    else {
        warningNotify("Please Provide Shift Name!");
        return false;
    }
}
function getTimeValue(timeValue) {
    //console.log(timeValue);
    if (timeValue == 0) { timeValue = '0' + timeValue; }
    else if (timeValue == 1) { timeValue = '0' + timeValue; }
    else if (timeValue == 2) { timeValue = '0' + timeValue; }
    else if (timeValue == 3) { timeValue = '0' + timeValue; }
    else if (timeValue == 4) { timeValue = '0' + timeValue; }
    else if (timeValue == 5) { timeValue = '0' + timeValue; }
    else if (timeValue == 6) { timeValue = '0' + timeValue; }
    else if (timeValue == 7) { timeValue = '0' + timeValue; }
    else if (timeValue == 8) { timeValue = '0' + timeValue; }
    else if (timeValue == 9) { timeValue = '0' + timeValue; }

    return timeValue;
}

