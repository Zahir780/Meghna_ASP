$(document).ready(function () {  
    $("#vDepartmentCode").change(function () {
        if (codeCheck($(this).val())) {
            $("#vDepartmentCode").val("");
            $("#vDepartmentCode").focus();
            warningNotify("Department Code already taken! try new one.");
        }
        else {
        }
    });
    $("#vDepartmentName").change(function () {
        if (nameCheck($(this).val())) {
            $("#vDepartmentName").val("");
            $("#vDepartmentName").focus();
            warningNotify("Department name already taken! try new one.");
        }
        else {
        }
    });
    $("#vDepartmentNameBangla").change(function () {
        if (nameBanglaCheck($(this).val())) {
            $("#vDepartmentNameBangla").val("");
            $("#vDepartmentNameBangla").focus();
            warningNotify("Bangla Department name already taken! try new one.");
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
    var url = "/Payroll/DepartmentInfo/GetMaxDepartment";
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
    var url = "/Payroll/DepartmentInfo/codeCheck?name=" + name;
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
    var url = "/Payroll/DepartmentInfo/nameCheck?name=" + name;
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
    var url = "/Payroll/DepartmentInfo/nameBanglaCheck?name=" + name;
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
    $("#vDepartmentID").val(maxCode);
}

$(document).ready(function () {
    $("#btnAdd").click(function () {
        init();
     
        $("#DepartmentInfo").modal('show');
       
    });
    $('#tbDepartment').on('click', '.tbEdit', function () {
        setEnableDisable(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbDepartment").dataTable().fnGetData(tr);
        $("#btnLoadEdit").hide();
        if (isData(data.iAutoId)) {
            findWork(data.iAutoId);
            $("#DepartmentInfo").modal('show');
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
            url: "/Payroll/DepartmentInfo/findData",
            data: { iAutoId: iAutoId },
            async: false,
            success: function (res) {
                console.log("find:" + JSON.stringify(res.data));
                if (res.isFind) {
                    if (!isEmpty(res.data))
                    {
                        $("#iAutoId").val(res.data.iAutoId);
                        $("#vDepartmentID").val(res.data.vDepartmentID);
                        $("#vDepartmentCode").val(res.data.vDepartmentCode);
                        $("#vDepartmentName").val(res.data.vDepartmentName);
                        $("#vDepartmentNameBangla").val(res.data.vDepartmentNameBangla);
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
        url: "/Payroll/DepartmentInfo/findData",
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
    var vDepartmentID = $("#vDepartmentID").val();
    var vDepartmentCode = $("#vDepartmentCode").val();
    var vDepartmentName = $("#vDepartmentName").val();
    var vDepartmentNameBangla = $("#vDepartmentNameBangla").val();
    var iActive = $("#iActive").select2('data');
    var status = true;
    if (iActive.id == 0) { status = false; }
   
    var jsonData =
    {
        iAutoId: iAutoId,
        vDepartmentID: vDepartmentID,
        vDepartmentCode: vDepartmentCode,
        vDepartmentName: vDepartmentName,
        vDepartmentNameBangla: vDepartmentNameBangla,
        iActive: status
    }
    console.log(jsonData);
    $.ajax({
        type:"POST",
        url: "/Payroll/DepartmentInfo/DepartmentSave",
        data: jsonData,
        async: false,        
        success: function (res) {
            if (res.success) {
                clear();
                if (!isNew) {
                    $("#DepartmentInfo").modal('hide');
                }
                showButton(isEdit);
                $("#vDepartmentName").focus();
                $('#tbDepartment').DataTable().ajax.reload();
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
    $("#vDepartmentID").val("");
    $("#vDepartmentCode").val("");
    $("#vDepartmentName").val("");
    $("#vDepartmentNameBangla").val("");
    $('#iActive').select2('data', { id: '1', text: 'Active' });
}
function checkValiAutoIdation()
{
    if ($("#vDepartmentName").val() != "") {
        return true;
    }
    else {
        warningNotify("Please Provide Department Name!");
        return false;
    }
}

