$(document).ready(function () {  
    $("#iRank").change(function () {
        if (nameCheck($(this).val())) {
            $("#iRank").val("");
            $("#iRank").focus();
            warningNotify("DesignationRank name already taken! try new one.");
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
    var url = "/Payroll/DesignationRankInfo/GetMaxDesignationRank";
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

function nameCheck(name) {
    var url = "/Payroll/DesignationRankInfo/nameCheck?name=" + name;
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
    $("#vRankID").val(maxCode);
}

$(document).ready(function () {
    $("#btnAdd").click(function () {
        init();     
        $("#DesignationRankInfo").modal('show');
       
    });
    $('#tbDesignationRank').on('click', '.tbEdit', function () {
        setEnableDisable(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbDesignationRank").dataTable().fnGetData(tr);
        $("#btnLoadEdit").hide();
        if (isData(data.iAutoId)) {
            findWork(data.iAutoId);
            $("#DesignationRankInfo").modal('show');
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
            url: "/Payroll/DesignationRankInfo/findData",
            data: { iAutoId: iAutoId },
            async: false,
            success: function (res) {
                console.log("find:" + JSON.stringify(res.data));
                if (res.isFind) {
                    if (!isEmpty(res.data))
                    {
                        $("#iAutoId").val(res.data.iAutoId);
                        $("#vRankID").val(res.data.vRankID);
                        $("#iRank").val(res.data.iRank);
                        $("#vRemarks").val(res.data.vRemarks);
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
        url: "/Payroll/DesignationRankInfo/findData",
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
    var vRankID = $("#vRankID").val();
    var iRank = $("#iRank").val();
    var vRemarks = $("#vRemarks").val();
    var iActive = $("#iActive").select2('data');
    var status = true;
    if (iActive.id == 0) { status = false; }
   
    var jsonData =
    {
        iAutoId: iAutoId,
        vRankID: vRankID,
        iRank: iRank,
        vRemarks: vRemarks,
        iActive: status
    }

    $.ajax({
        type:"POST",
        url: "/Payroll/DesignationRankInfo/DesignationRankSave",
        data: jsonData,
        async: false,        
        success: function (res) {
            if (res.success) {
                clear();
                if (!isNew) {
                    $("#DesignationRankInfo").modal('hide');
                }
                showButton(isEdit);
                $("#iRank").focus();
                $('#tbDesignationRank').DataTable().ajax.reload();
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
    $("#vRankID").val("");
    $("#iRank").val("");
    $("#vRemarks").val("");
    $('#iActive').select2('data', { id: '1', text: 'Active' });
}
function checkValiAutoIdation()
{
    if ($("#iRank").val() != "") {
        return true;
    }
    else {
        warningNotify("Please Provide DesignationRank Name!");
        return false;
    }
}

