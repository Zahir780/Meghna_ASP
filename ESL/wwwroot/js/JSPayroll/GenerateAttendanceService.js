$(document).ready(function () {
    init();
    btnAction();

});

function btnAction() {
    $("#btnGenerate").click(function () {
        if (checkValidation()) {
            generate();
        }
    });
}
function generate() {
    var pGenerateDate = getBdToDbFormat($("#pGenerateDate").val());
    var pAttendanceDate = getBdToDbFormat($("#pAttendanceDate").val());

    var jsonData =
    {
        pGenerateDate: pGenerateDate,
        pAttendanceDate: pAttendanceDate
    }
    console.log(jsonData);
    $("#btnGenerate").hide();
    $("#btnGenerateLoad").show();
    $.ajax({
        type: "POST",
        url: "/Payroll/GenerateAttendance/Generate",
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
    var pGenerateDate = $("#pGenerateDate").val();
    var pAttendanceDate = $("#pAttendanceDate").val();
    if (!isEmpty(pGenerateDate))
    {
        if (!isEmpty(pAttendanceDate))
        {
            return true;
        }
        else {
            warningNotify("Please Choose Date Of Attendance");
            return false;
        }
    }
    else {
        warningNotify("Please Choose Generate Date");
        return false;
    }
}
function init() {
    clear();
}
function clear() {
    $("#pGenerateDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#pAttendanceDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#btnGenerateLoad").hide();

    //$("#btnGenerate").prop('disabled', true);
}