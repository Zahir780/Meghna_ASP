$(document).ready(function () { 
    $("#vSectionCode").change(function () {
        if (codeCheck($(this).val())) {
            $("#vSectionCode").val("");
            $("#vSectionCode").focus();
            warningNotify("Section Code already taken! try new one.");
        }
    });
    $("#vSectionName").change(function () {
        if (nameCheck($(this).val())) {
            $("#vSectionName").val("");
            $("#vSectionName").focus();
            warningNotify("Section name already taken! try new one.");
        }
    });
    $("#vSectionNameBangl").change(function () {
        if (nameBanglaCheck($(this).val())) {
            $("#vSectionNameBangl").val("");
            $("#vSectionNameBangl").focus();
            warningNotify("Bangla Section name already taken! try new one.");
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
    var url = "/Payroll/SectionInfo/GetMaxSection";
    var result = "";
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            console.log(res);
            result = res.data;
        }
    });
    return result;
}

function codeCheck(name) {
    var url = "/Payroll/SectionInfo/codeCheck?name=" + name;
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
function nameCheck(name) {
    var url = "/Payroll/SectionInfo/nameCheck?name=" + name;
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
function nameBanglaCheck(name) {
    var url = "/Payroll/SectionInfo/nameBanglaCheck?name=" + name;
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
    var maxCode = maxId();
    $("#vSectionID").val(maxCode);
}

$(document).ready(function () {
    $("#btnAdd").click(function () {
        init();     
        $("#SectionInfo").modal('show');       
    });
    $('#tbSection').on('click', '.tbEdit', function () {
        setEnableDisable(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbSection").dataTable().fnGetData(tr);
        $("#btnLoadEdit").hide();
        if (isData(data.iAutoId)) {
            findWork(data.iAutoId);
            $("#SectionInfo").modal('show');
        }
        else {
            errorNotify("Data not found");
        }
    });
    $("#btnSave").click(function () {
        if (checkValiAutoIdation()) {
            saveWork(false,false);
        }
    });
    $("#btnEdit").click(function () {
        if (checkValiAutoIdation()) {
            saveWork(false,true);
        }
    });
    $("#btnSaveNew").click(function () {
        if (checkValiAutoIdation()) {
            saveWork(true,false);
        }
    });
});
function findWork(iAutoId) {
    if (iAutoId != "0" && iAutoId != "") {
        $.ajax({
            url: "/Payroll/SectionInfo/findData",
            data: { iAutoId: iAutoId },
            async: false,
            success: function (res) {
                console.log("find:" + JSON.stringify(res.data));
                if (res.isFind) {
                    if (!isEmpty(res.data))
                    {
                        $("#iAutoId").val(res.data.iAutoId);
                        $("#vSectionID").val(res.data.vSectionID);
                        $("#vSectionCode").val(res.data.vSectionCode);
                        $("#vSectionName").val(res.data.vSectionName);
                        $("#vSectionNameBangla").val(res.data.vSectionNameBangla);
                        if (res.data.iActive == "1") {
                            $('#iActive').select2('data', { id: res.data.iActive, text: "Active" });
                        }
                        else {
                            $('#iActive').select2('data', { id: res.data.iActive, text: "Inactive" });
                        }
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
        url: "/Payroll/SectionInfo/findData",
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
    var iAutoId = $("#iAutoId").val();
    var vSectionID = $("#vSectionID").val();
    var vSectionCode = $("#vSectionCode").val();
    var vSectionName = $("#vSectionName").val();
    var vSectionNameBangla = $("#vSectionNameBangla").val();
    var iActive = $("#iActive").select2('data');
    var status = true;
    if (iActive.id == 0) { status = false; }
   
    var jsonData =
    {
        iAutoId: iAutoId,
        vSectionID: vSectionID,
        vSectionCode: vSectionCode,
        vSectionName: vSectionName,
        vSectionNameBangla: vSectionNameBangla,
        iActive: status
    }

    $.ajax({
        type:"POST",
        url: "/Payroll/SectionInfo/SectionSave",
        data: jsonData,
        async: false,        
        success: function (res) {
            if (res.success) {
                clear();
                if (!isNew) {
                    $("#SectionInfo").modal('hide');
                }
                showButton(isEdit);
                $("#vSectionName").focus();
                $('#tbSection').DataTable().ajax.reload();
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
    $("#vSectionID").val("");
    $("#vSectionCode").val("");
    $("#vSectionName").val("");
    $("#vSectionNameBangla").val("");
    $('#iActive').select2('data', { id: '1', text: 'Active' });
}
function checkValiAutoIdation()
{
    if ($("#vSectionName").val() != "") {
        return true;
    }
    else {
        warningNotify("Please Provide Section Name!");
        return false;
    }
}

