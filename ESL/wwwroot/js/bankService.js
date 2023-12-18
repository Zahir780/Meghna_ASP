$(document).ready(function () {
  
    $("#nameId").change(function () {
        if (nameCheck($(this).val())) {
            $("#nameId").val("");
            $("#nameId").focus();
            warningNotify("Bank name already taken! try new one.");
        }
        else {
        }
    });
});
function maxId(id) {
    var url = "/Admin/BankInfo/getMax?id=" + id;
    var result = "";
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    console.log("Result : " + result);
    return result;
}
function nameCheck(name) {
    var url = "/Admin/BankInfo/nameCheck?name=" + name;
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
function maxPCode(id, isCode) {
    var url = "/Admin/BankInfo/getMaxPrimaryCode";
    console.log("Console , Url : " + url);

    var result = "";
    var data = { group: id, code: isCode };
    $.ajax({
        url: url,
        data: data,
        async: false,
        success: function (res) {
            result = res.maxData;
        }
    });
    return result;
}

$(document).ready(function () {
    $("#btnAdd").click(function () {
        clear();
        document.getElementById('add').style.display = "block";
        document.getElementById('edit').style.display = "none";
        $("#primaryGroup").modal('show');
    });
    $('#tbBank').on('click', '.tbEdit', function () {
        document.getElementById('add').style.display = "none";
        document.getElementById('edit').style.display = "block";
        
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbBank").dataTable().fnGetData(tr);
        
        if (isData(data.id)) {
            findWork(data.id);
            $("#primaryGroup").modal('show');
        }
        else {
            errorNotify("Data not found");
        }
    });
    $("#btnSave").click(function () {
        if (checkValidation()) {
            saveWork(false,false);
        }
    });
    $("#btnEdit").click(function () {
        if (checkValidation()) {
            saveWork(false,true);
        }
    });
    $("#btnSaveNew").click(function () {
        if (checkValidation()) {
            saveWork(true,false);
        }
    });
});
function findWork(id) {
    if (id != "0" && id != "") {
        $.ajax({
            url: "/Admin/BankInfo/findData",
            data: { id: id },
            async: false,
            success: function (res) {
                if (res.isFind) {
                    if (!isEmpty(res.data)) {
                        $("#autoId").val(res.data.id);                       
                        $("#nameId").val(res.data.name);                      
                        $("#primaryGroupId").val(res.data.primaryGroupId);
                      
                    }
                }
                else {
                    errorNotify("Data not found!");
                }
            }
        });
    }
}
function isData(id) {
    var ret = false;
    $.ajax({
        url: "/Admin/BankInfo/findData",
        data: { id: id },
        async: false,
        success: function (res) {
            ret = res.isFind;
        }
    });
    return ret;
}
function saveWork(isNew,isEdit) {
    var id = $("#autoId").val();   
    var name = $("#nameId").val();  
    var primaryGroupId = $("#primaryGroupId").val();
  
    if (!isEdit) {
        primaryGroupId = maxId(type.substring(0, 1));
        
    }

    var jsonData = {
        Id: id, Type: type, Code: code, Name: name,
        GroupType: groupType, NoteNo: noteNo, PrimaryGroupId: primaryGroupId
    }
    $.ajax({
        type:"POST",
        url: "/Admin/BankInfo/primarySave",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                clear();
                if (!isNew) {
                    $("#primaryGroup").modal('hide');
                }
                $("#nameId").focus();
                $('#tbBank').DataTable().ajax.reload();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }

    });
}
function clear() {
    $("#autoId").val(0);   
    $("#nameId").val("");  
    $("#primaryGroupId").val("");  
}
function checkValidation() {
    if ($("#primaryGroupId").val()!="") {
        
            if ($("#nameId").val() != "") {
                                
                return true;
            }
            else {
                warningNotify("Please give name ");
                return false;
            }      
    }
    else {
        warningNotify("Please primary group id ");
        return false;
    }
}