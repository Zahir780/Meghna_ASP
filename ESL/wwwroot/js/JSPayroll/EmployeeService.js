
var xEducation = 0;
var tableEducation = "gridEducation";

var xExperience = 0;
var tableExperience = "gridExperience";

$(document).ready(function ()
{
    $("#btnAdd").click(function () {
        init();
        $("#EmployeeInfo").modal('show');
    });

    $('#tbEmployee').on('click', '.tbEdit', function () {
        setEnableDisable(true);
        var $this = $(this),
            tr = $(this).closest('tr').get(0);
        var data = $("#tbEmployee").dataTable().fnGetData(tr);
        $("#btnLoadEdit").hide();

        xEducation = 0;
        tableClearEducation(1);

        xExperience = 0;
        tableClearExperience(1);

        if (isData(data.iAutoId)) {
            findWork(data.iAutoId);
            attachmentLoad();

            var uploadId = $("#upload").val();
            if (uploadId == null || uploadId == "") {
                $("#attachmentUP").val('');
                $("#fileName").text('');
                $(".attachmentlink").attr('href', '');
                $(".attachmentshow").attr('src', '/images/default-image.png');
            }

            $("#EmployeeInfo").modal('show');
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

    $("#vEmployeeCode").change(function () {
        var empId = ($(this).val());
        if (employeeCodeCheck($(this).val())) {
            $("#vEmployeeCode").val(maxEmployeeCode());
            warningNotify("Employee ID: " + empId + " taken! try new one.");
        }
    });

    $("#vMaritalStatus").change(function () {
        var vMaritalStatus = $("#vMaritalStatus").val();
        maritalStatusOptionWork(vMaritalStatus);
    });
    $("#vEmployeeStatus").change(function () {
        var vEmployeeStatus = $("#vEmployeeStatus").val();
        employeeStatusOptionWork(vEmployeeStatus);
    });

    $('input[type=radio][name=vMoneyTransferType]').change(function () {
        vMoneyTransferType = $("input[name='vMoneyTransferType']:checked").val();
        if (vMoneyTransferType != "")
        {
            paymentOptionWork(vMoneyTransferType);
        }
    });

    /*$("#vNationality").select2({
        templateResult: function (idioma) {
            var $span = $("<span><img src='/assets/flags_common/" + idioma.text.replace(/'/g, "") + ".png'/> " + idioma.text + "</span>");
            return $span;
        },
        templateSelection: function (idioma) {
            var $span = $("<span><img src='/assets/flags_common/" + idioma.text.replace(/'/g, "") + ".png'/> " + idioma.text + "</span>");
            return $span;
        }
    });*/


    /*Education Start*/
    $("#btnAddRowEducation").click(function () {
        xEducation = xEducation + 1;
        addRowEducation(xEducation);
    });
    /*Education End*/

    /*Experience Start*/
    $("#btnAddRowExperience").click(function () {
        xExperience = xExperience + 1;
        addRowExperience(xExperience);
    });
    /*Experience End*/

});

function init() {
    showButton(false);
    setEnableDisable(false);
    clear();
    $("#vEmployeeID").val(maxEmployeeId());
    $("#vEmployeeCode").val(maxEmployeeCode());
}

function paymentOptionWork(vMoneyTransferType) {
    document.getElementById('bankOptionBankId').style.display = "none";
    document.getElementById('bankOptionBankBranchId').style.display = "none";
    document.getElementById('bankOptionAccountNo').style.display = "none";
    document.getElementById('bftnOptionRoutingNo').style.display = "none";
    document.getElementById('mobileBankinAccountMobileNo').style.display = "none";

    bankDataLoad("#vBankId");
    bankBranchDataLoad("#vBankBranchId");
    $("#vAccountNo").val("");
    $("#vRoutingNo").val("");
    $("#vAccountMobileNo").val("");

    if (vMoneyTransferType == "Bank") {
        document.getElementById('bankOptionBankId').style.display = "block";
        document.getElementById('bankOptionBankBranchId').style.display = "block";
        document.getElementById('bankOptionAccountNo').style.display = "block";
    }
    else if (vMoneyTransferType == "BFTN") {
        document.getElementById('bankOptionBankId').style.display = "block";
        document.getElementById('bankOptionBankBranchId').style.display = "block";
        document.getElementById('bankOptionAccountNo').style.display = "block";
        document.getElementById('bftnOptionRoutingNo').style.display = "block";
    }
    else if (vMoneyTransferType == "Mobile") {
        document.getElementById('mobileBankinAccountMobileNo').style.display = "block";
    }
}
function maritalStatusOptionWork(vMaritalStatus) {
    if (vMaritalStatus == "Unmarried") {
        document.getElementById('maritalStatus').style.display = "none";
        $("#dMarriageDate").val('01-01-1900');
        $("#vSpouseName").val("");
        $("#vSpouseOccupation").val("");
        $("#iNumberOfChild").val("0");
    }
    else {
        document.getElementById('maritalStatus').style.display = "block";
        $("#dMarriageDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    }
}
function employeeStatusOptionWork(vEmployeeStatus) {
    if (vEmployeeStatus == "Continue") {
        document.getElementById('employeeStatus').style.display = "none";
        $("#dStatusDate").val('01-01-1900');
    }
    else {
        document.getElementById('employeeStatus').style.display = "block";
        $("#dStatusDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    }
}

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

function maxEmployeeId() {
    var url = "/Payroll/EmployeeInfo/GetMaxEmployee";
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

function maxEmployeeCode() {
    var url = "/Payroll/EmployeeInfo/getMaxEmployeeCode";
    var result = "";
    $.ajax({
        url: url,
        async: false,
        success: function (res) {
            result = res.data;
        }
    });
    return result;
}
function employeeCodeCheck(vEmployeeCode) {
    var url = "/Payroll/EmployeeInfo/employeeCodeExist?data=" + vEmployeeCode;
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

function designationDataLoad(id, setId, setName) {
    $(id).select2('data', { id: 0, text: '==Choose a Designation==' });
    var url = "/Payroll/EmployeeInfo/getDesignation";
    $.getJSON(url, function (data) {
        var item = "";
        $(id).empty();
        item += '<option value="' + 0 + '">==Choose a Designation==</option>'
        $(id).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(id).html(item);
        if (setId != undefined && !isEmpty(setId)) {
            $(id).select2('data', { id: setId, text: setName });
        }
    });
}
function departmentDataLoad(id, setId, setName) {
    $(id).select2('data', { id: 0, text: '==Choose a Department==' });
    var url = "/Payroll/EmployeeInfo/getDepartment";
    $.getJSON(url, function (data) {
        var item = "";
        $(id).empty();
        item += '<option value="' + 0 + '">==Choose a Department==</option>'
        $(id).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(id).html(item);
        if (setId != undefined && !isEmpty(setId)) {
            $(id).select2('data', { id: setId, text: setName });
        }
    });
}
function sectionDataLoad(id, setId, setName) {
    $(id).select2('data', { id: 0, text: '==Choose a Section==' });
    var url = "/Payroll/EmployeeInfo/getSection";
    $.getJSON(url, function (data) {
        var item = "";
        $(id).empty();
        item += '<option value="' + 0 + '">==Choose a Section==</option>'
        $(id).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(id).html(item);

        if (setId != undefined && !isEmpty(setId)) {
            $(id).select2('data', { id: setId, text: setName });
        }
    });
}
function bankDataLoad(id, setId, setName) {
    $(id).select2('data', { id: 0, text: '==Choose a Bank==' });
    var url = "/Payroll/EmployeeInfo/getBank";
    $.getJSON(url, function (data) {
        var item = "";
        $(id).empty();
        item += '<option value="' + 0 + '">==Choose a Bank==</option>'
        $(id).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(id).html(item);
        if (setId != undefined && !isEmpty(setId)) {
            $(id).select2('data', { id: setId, text: setName });
        }
    });
}
function bankBranchDataLoad(id, setId, setName) {
    $(id).select2('data', { id: 0, text: '==Choose a Bank Branch==' });
    var url = "/Payroll/EmployeeInfo/getBankBranch";
    $.getJSON(url, function (data) {
        var item = "";
        $(id).empty();
        item += '<option value="' + 0 + '">==Choose a Bank Branch==</option>'
        $(id).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(id).html(item);
        if (setId != undefined && !isEmpty(setId)) {
            $(id).select2('data', { id: setId, text: setName });
        }
    });
}

function findWork(iAutoId) {
    if (iAutoId != "0" && iAutoId != "") {
        setEmployeeInfo(iAutoId);
    }
}
function setEmployeeInfo(iAutoId)
{
    var vEmployeeId = "";
    $.ajax({
        url: "/Payroll/EmployeeInfo/findData",
        data: { iAutoId: iAutoId },
        async: false,
        success: function (res) {
            //console.log("find:" + JSON.stringify(res.data));
            if (res.isFind) {
                if (!isEmpty(res.data))
                {
                    $("#upload").val(res.data.attachment);
                    $("#iAutoId").val(res.data.iAutoId);
                    vEmployeeId = res.data.vEmployeeId;

                    $("#vEmployeeId").val(vEmployeeId);

                    $("#vEmployeeCode").val(res.data.vEmployeeCode);
                    $("#vFingerId").val(res.data.vFingerId);
                    $("#vEmployeeName").val(res.data.vEmployeeName);
                    $("#vEmployeeNameBangla").val(res.data.vEmployeeNameBangla);
                    designationDataLoad('#vDesignationId', res.data.vDesignationId, res.data.vDesignationName);
                    departmentDataLoad('#vDepartmentId', res.data.vDepartmentId, res.data.vDepartmentName);
                    sectionDataLoad('#vSectionId', res.data.vSectionId, res.data.vSectionName);
                    $('#vReligion').select2('data', { id: res.data.vReligion, text: res.data.vReligion });
                    $("#vContactNo").val(res.data.vContactNo);
                    $("#vEmailAddress").val(res.data.vEmailAddress);
                    $('#vGender').select2('data', { id: res.data.vGender, text: res.data.vGender });
                    $("#dDateOfBirth").val(res.data.dDateOfBirth);
                    $("#vNationality").val(res.data.vNationality);
                    $("#vNationalIdNo").val(res.data.vNationalIdNo);
                    $('#vEmployeeType').select2('data', { id: res.data.vEmployeeType, text: res.data.vEmployeeType });
                    $('#vServiceType').select2('data', { id: res.data.vServiceType, text: res.data.vServiceType });
                    $("#dApplicationDate").val(res.data.dApplicationDate);
                    $("#dInterviewDate").val(res.data.dInterviewDate);
                    $("#dJoiningDate").val(res.data.dJoiningDate);
                    $("#dConfirmationDate").val(res.data.dConfirmationDate);
                    $('#vProbationPeriod').select2('data', { id: res.data.vProbationPeriod, text: res.data.vProbationPeriod });

                    $('#vEmployeeStatus').select2('data', { id: res.data.vEmployeeStatus, text: res.data.vEmployeeStatus });
                    $("#dStatusDate").val(res.data.dStatusDate);
                    if (res.data.vEmployeeStatus == "Continue") {
                        document.getElementById('employeeStatus').style.display = "none";
                    }
                    else {
                        document.getElementById('employeeStatus').style.display = "block";
                    }

                    $("#vFatherName").val(res.data.vFatherName);
                    $("#vMotherName").val(res.data.vMotherName);
                    $("#vPresentAddress").val(res.data.vPresentAddress);
                    $("#vPermanentAddress").val(res.data.vPermanentAddress);

                    if (res.data.vBloodGroup != undefined && !isEmpty(res.data.vBloodGroup)) {
                        $('#vBloodGroup').select2('data', { id: res.data.vBloodGroup, text: res.data.vBloodGroup });
                    }
                    else {
                        $('#vBloodGroup').select2('data', { id: "0", text: "==Choose a Group==" });
                    }

                    $('#vMaritalStatus').select2('data', { id: res.data.vMaritalStatus, text: res.data.vMaritalStatus });
                    maritalStatusOptionWork(res.data.vMaritalStatus);
                    /*if (res.data.vMaritalStatus == "Unmarried") {
                        document.getElementById('maritalStatus').style.display = "none";
                    }
                    else {
                        document.getElementById('maritalStatus').style.display = "block";
                    }*/

                    $("#dMarriageDate").val(res.data.dMarriageDate);
                    $("#vSpouseName").val(res.data.vSpouseName);
                    $("#vSpouseOccupation").val(res.data.vSpouseOccupation);
                    $("#iNumberOfChild").val(res.data.iNumberOfChild);

                    $("#mBasic").val(res.data.mBasic); amountWithComma('#mBasic');
                    $("#mHouseRent").val(res.data.mHouseRent);
                    $("#mMedicalAllowance").val(res.data.mMedicalAllowance);
                    $("#mGross").val(res.data.mGross);
                    $("#mIncomeTax").val(res.data.mIncomeTax);
                    $("#mProvidentFund").val(res.data.mProvidentFund);

                    $("input[name=vMoneyTransferType][value=" + res.data.vMoneyTransferType + "]").prop('checked', true);
                    bankDataLoad('#vBankId', res.data.vBankId, res.data.vBankName);
                    bankBranchDataLoad('#vBankBranchId', res.data.vBankBranchId, res.data.vBankBranchName);
                    $("#vAccountNo").val(res.data.vAccountNo);
                    $("#vRoutingNo").val(res.data.vRoutingNo);
                    $("#vAccountMobileNo").val(res.data.vAccountMobileNo);


                    document.getElementById('bankOptionBankId').style.display = "none";
                    document.getElementById('bankOptionBankBranchId').style.display = "none";
                    document.getElementById('bankOptionAccountNo').style.display = "none";
                    document.getElementById('bftnOptionRoutingNo').style.display = "none";
                    document.getElementById('mobileBankinAccountMobileNo').style.display = "none";

                    //console.log("res.data.vMoneyTransferType: " + res.data.vMoneyTransferType);
                    if (res.data.vMoneyTransferType == "Bank") {
                        document.getElementById('bankOptionBankId').style.display = "block";
                        document.getElementById('bankOptionBankBranchId').style.display = "block";
                        document.getElementById('bankOptionAccountNo').style.display = "block";
                    }
                    else if (res.data.vMoneyTransferType == "BFTN") {
                        document.getElementById('bankOptionBankId').style.display = "block";
                        document.getElementById('bankOptionBankBranchId').style.display = "block";
                        document.getElementById('bankOptionAccountNo').style.display = "block";
                        document.getElementById('bftnOptionRoutingNo').style.display = "block";
                    }
                    else if (res.data.vMoneyTransferType == "Mobile") {
                        document.getElementById('mobileBankinAccountMobileNo').style.display = "block";
                    }
                    /*
                    iAutoId,vEmployeeId,vEmployeeCode,vFingerId,vEmployeeName,vEmployeeNameBangla,vDesignationId,vDesignationName,
                    vDepartmentId,vDepartmentName,vSectionId,vSectionName,vReligion,vContactNo,vEmailAddress,vGender,dDateOfBirth,vNationality,
                    vNationalIdNo,vEmployeeType,vServiceType,dApplicationDate,dInterviewDate,dJoiningDate,dConfirmationDate,vProbationPeriod,
                    vEmployeeStatus,iStatus,dStatusDate,vEmployeePhoto,vFatherName,vMotherName,vPresentAddress,vPermanentAddress,vBloodGroup,
                    vMaritalStatus,dMarriageDate,vSpouseName,vSpouseOccupation,iNumberOfChild,vBankId,vBankName,vBankBranchId,vBankBranchName,
                    vAccountNo,vMoneyTransferType,vRoutingNo,vAccountMobileNo,
                    mBasic,mHouseRent,mMedicalAllowance,mGross,mIncomeTax,mProvidentFund 
                    */
                }
            }
            else {
                errorNotify("Data not found!");
            }
        }
    });
    setEducationInfo(vEmployeeId);
    setExperienceInfo(vEmployeeId);
}


function setEducationInfo(vEmployeeId)
{
    $.ajax({
        url: "/Payroll/EmployeeInfo/findEducationData",
        data: { vEmployeeId: vEmployeeId },
        async: false,
        success: function (res) {
            var d = res.data;
            if (res.isFind)
            {
                for (var i = 0; i < d.length; i++)
                {
                    xEducation = xEducation + 1;
                    addRowEducation(xEducation);

                    $("#vExamp" + xEducation).val(d[i].vExamp);
                    $("#vGroup" + xEducation).val(d[i].vGroup);
                    $("#vInstitute" + xEducation).val(d[i].vInstitute);
                    $("#vBoard" + xEducation).val(d[i].vBoard);
                    $("#vDivOrClass" + xEducation).val(d[i].vDivOrClass);
                    $("#iYear" + xEducation).val(d[i].iYear);
                }
            }
        }
    });
}
function setExperienceInfo(vEmployeeId)
{
    $.ajax({
        url: "/Payroll/EmployeeInfo/findExperienceData",
        data: { vEmployeeId: vEmployeeId },
        async: false,
        success: function (res) {
            var d = res.data;
            if (res.isFind) {
                for (var i = 0; i < d.length; i++) {
                    xExperience = xExperience + 1;
                    addRowExperience(xExperience);

                    $("#vPostOrDesignation" + xExperience).val(d[i].vPostOrDesignation);
                    $("#vCompanyName" + xExperience).val(d[i].vCompanyName);
                    $("#dFrom" + xExperience).val(d[i].dFrom);
                    $("#dTo" + xExperience).val(d[i].dTo);
                    $("#vMajorResponsibility" + xExperience).val(d[i].vMajorResponsibility);
                }
            }
        }
    });
}




/*Education Start*/
function tableClearEducation(start) {
    var tableEducation = document.getElementById('gridEducation');
    var rowCount = tableEducation.rows.length;
    for (var i = start; i < rowCount; i++) {
        tableEducation.deleteRow(start);
    }
}
function addRowEducation(xEducation) {
    var tableEducationArray = new Array();
    tableEducationArray = ['vExamp', 'vGroup', 'vInstitute', 'vBoard', 'vDivOrClass','iYear', ''];
    var gridEducation = document.getElementById(tableEducation);
    var rowCount = gridEducation.rows.length;
    var tr = gridEducation.insertRow(rowCount);
    tr = gridEducation.insertRow(rowCount);
    for (var c = 0; c < tableEducationArray.length; c++) {
        var td = document.createElement('td');

        td = tr.insertCell(0);
        if (parseInt(xEducation) % 2 == 0) {
            td.setAttribute('style', 'background-color:#fff;color:#000');
        }
        else {
            td.setAttribute('style', 'background-color:#eee;color:#000');
        }
        if (c == 6) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm vExamp');
            ele.setAttribute('id', 'vExamp' + xEducation);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 5) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm vGroup');
            ele.setAttribute('id', 'vGroup' + xEducation);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 4) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm vInstitute');
            ele.setAttribute('id', 'vInstitute' + xEducation);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 3) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm vBoard');
            ele.setAttribute('id', 'vBoard' + xEducation);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 2) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm form-control input-sm vDivOrClass');
            ele.setAttribute('id', 'vDivOrClass' + xEducation);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 1) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm form-control input-sm iYear');
            ele.setAttribute('id', 'iYear' + xEducation);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 0) {
            var button = document.createElement('a');
            button.innerHTML = '<i class="fa fa-trash-o" aria-hidden="true"></i>';
            button.setAttribute('class', 'btn btn-default btn-sm');
            button.setAttribute('style', 'color:#fc0313');
            button.setAttribute('onclick', 'removeRowEducation(this)');
            td.appendChild(button);
        }
        $("#vExamp" + xEducation).focus();
    }
}
function removeRowEducation(oButton) {
    var tableEducationId = tableEducation;
    var tab = document.getElementById(tableEducationId);
    var elements = document.querySelectorAll("#" + tableEducationId + " tr td .vExamp");
    if (elements.length > 0) {
        tab.deleteRow(oButton.parentNode.parentNode.rowIndex);
    }
}
/*Education End*/

/*Experience Start*/
function tableClearExperience(start) {
    var tableExperience = document.getElementById('gridExperience');
    var rowCount = tableExperience.rows.length;
    for (var i = start; i < rowCount; i++) {
        tableExperience.deleteRow(start);
    }
}

function addRowExperience(xExperience) {
    var tableExperienceArray = new Array();
    tableExperienceArray = ['vPostOrDesignation', 'vCompanyName', 'dFrom', 'dTo', 'vMajorResponsibility', ''];
    var gridExperience = document.getElementById(tableExperience);
    var rowCount = gridExperience.rows.length;
    var tr = gridExperience.insertRow(rowCount);
    tr = gridExperience.insertRow(rowCount);
    for (var c = 0; c < tableExperienceArray.length; c++) {
        var td = document.createElement('td');

        td = tr.insertCell(0);
        if (parseInt(xExperience) % 2 == 0) {
            td.setAttribute('style', 'background-color:#fff;color:#000');
        }
        else {
            td.setAttribute('style', 'background-color:#eee;color:#000');
        }
        if (c == 5) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm vPostOrDesignation');
            ele.setAttribute('id', 'vPostOrDesignation' + xExperience);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 4) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm vCompanyName');
            ele.setAttribute('id', 'vCompanyName' + xExperience);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 3) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm dFrom');
            ele.setAttribute('id', 'dFrom' + xExperience);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 2) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm dTo');
            ele.setAttribute('id', 'dTo' + xExperience);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }
        if (c == 1) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm form-control input-sm vMajorResponsibility');
            ele.setAttribute('id', 'vMajorResponsibility' + xExperience);
            ele.setAttribute('type', 'text');
            td.appendChild(ele);
        }

        
        if (c == 0) {
       
            var button = document.createElement('a');
        
            button.innerHTML = '<i class="fa fa-trash-o", aria-hidden="true"></i>';
            button.setAttribute('class', 'btn btn-default btn-sm');
            button.setAttribute('style', 'color:red');      
            button.setAttribute('onclick', 'RowExperiences(this)');
            td.appendChild(button);
          
        }
        $("#vPostOrDesignation" + xExperience).focus();
    }
}
function RowExperiences(oButton) {
    var tableExperienceId = tableExperience;
    var tab = document.getElementById(tableExperienceId);
    var elements = document.querySelectorAll("#" + tableExperienceId + " tr td .vPostOrDesignation");
    if (elements.length > 0) {

  
        tab.deleteRow(oButton.parentNode.parentNode.rowIndex);
     
    }
}





/*Experience End*/





function isData(iAutoId) {
    var ret = false;
    $.ajax({
        url: "/Payroll/EmployeeInfo/findData",
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
    var vEmployeeId = maxEmployeeId();
    var vEmployeeCode = maxEmployeeCode();

    if (isEdit) {
        iAutoId = $("#iAutoId").val();
        vEmployeeId = $("#vEmployeeId").val();
        vEmployeeCode = $("#vEmployeeCode").val();
    }

    var vFingerId = $("#vFingerId").val();
    var vEmployeeName = $("#vEmployeeName").val();
    var vEmployeeNameBangla = $("#vEmployeeNameBangla").val();

    var vDesignationId = $("#vDesignationId").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vReligion = $("#vReligion").select2('data');

    var vContactNo = $("#vContactNo").val();
    var vEmailAddress = $("#vEmailAddress").val();
    var vGender = $("#vGender").select2('data');

    var dDateOfBirth = getBdToDbFormat($("#dDateOfBirth").val());
    var vNationality = $("#vNationality").val();
    var vNationalIdNo = $("#vNationalIdNo").val();
    var vEmployeeType = $("#vEmployeeType").select2('data');
    var vServiceType = $("#vServiceType").select2('data');

    var dApplicationDate = getBdToDbFormat($("#dApplicationDate").val());
    var dInterviewDate = getBdToDbFormat($("#dInterviewDate").val());
    var dJoiningDate = getBdToDbFormat($("#dJoiningDate").val());
    var dConfirmationDate = getBdToDbFormat($("#dConfirmationDate").val());

    var vProbationPeriod = $("#vProbationPeriod").select2('data');
    var vEmployeeStatus = $("#vEmployeeStatus").select2('data');
    var iStatus = "0";
    var dStatusDate = getBdToDbFormat($("#dStatusDate").val());

    var vEmployeePhoto = 0;

    var vFatherName = $("#vFatherName").val();
    var vMotherName = $("#vMotherName").val();
    var vPresentAddress = $("#vPresentAddress").val();
    var vPermanentAddress = $("#vPermanentAddress").val();
    var vBloodGroup = $("#vBloodGroup").select2('data');
    var vMaritalStatus = $("#vMaritalStatus").select2('data');
    var dMarriageDate = getBdToDbFormat($("#dMarriageDate").val());
    var vSpouseName = $("#vSpouseName").val();
    var vSpouseOccupation = $("#vSpouseOccupation").val();
    var iNumberOfChild = $("#iNumberOfChild").val();

    var mBasic = replaceAll($("#mBasic").val(), ',', '');
    var mHouseRent = replaceAll($("#mHouseRent").val(), ',', '');
    var mMedicalAllowance = replaceAll($("#mMedicalAllowance").val(), ',', '');
    var mGross = replaceAll($("#mGross").val(), ',', '');
    var mIncomeTax = replaceAll($("#mIncomeTax").val(), ',', '');
    var mProvidentFund = replaceAll($("#mProvidentFund").val(), ',', '');

    var vMoneyTransferType = $("input[name='vMoneyTransferType']:checked").val();
    var vBankId = $("#vBankId").select2('data');
    var vBankBranchId = $("#vBankBranchId").select2('data');
    var vAccountNo = $("#vAccountNo").val();
    var vRoutingNo = $("#vRoutingNo").val();
    var vAccountMobileNo = $("#vAccountMobileNo").val();



    /*var objEmployeeInfo = {
        iAutoId: iAutoId, vEmployeeId: vEmployeeId, vEmployeeCode: vEmployeeCode, vFingerId: vFingerId, vEmployeeName: vEmployeeName,
        vEmployeeNameBangla: vEmployeeNameBangla,
        vDesignationId: vDesignationId.id, vDesignationName: vDesignationId.text,
        vDepartmentId: vDepartmentId.id, vDepartmentName: vDepartmentId.text, vSectionId: vSectionId.id, vSectionName: vSectionId.text, vReligion: vReligion.id,
        vContactNo: vContactNo, vEmailAddress: vEmailAddress, vGender: vGender.id, dDateOfBirth: dDateOfBirth, vNationality: vNationality,
        vNationalIdNo: vNationalIdNo, vEmployeeType: vEmployeeType.id, vServiceType: vServiceType.id,
        dApplicationDate: dApplicationDate, dInterviewDate: dInterviewDate, dJoiningDate: dJoiningDate, dConfirmationDate: dConfirmationDate,
        vProbationPeriod: vProbationPeriod.id, vEmployeeStatus: vEmployeeStatus.id, iStatus: iStatus, dStatusDate: dStatusDate,
        vEmployeePhoto: vEmployeePhoto,
        vFatherName: vFatherName, vMotherName: vMotherName, vPresentAddress: vPresentAddress, vPermanentAddress: vPermanentAddress, vBloodGroup: vBloodGroup.id,
        vMaritalStatus: vMaritalStatus.id, dMarriageDate: dMarriageDate, vSpouseName: vSpouseName, vSpouseOccupation: vSpouseOccupation,
        iNumberOfChild: iNumberOfChild, vBankId: vBankId.id, vBankName: vBankId.text, vBankBranchId: vBankBranchId.id, vBankBranchName: vBankBranchId.text,
        vAccountNo: vAccountNo, vMoneyTransferType: vMoneyTransferType, vRoutingNo: vRoutingNo, vAccountMobileNo: vAccountMobileNo,
        mBasic: mBasic, mHouseRent: mHouseRent, mMedicalAllowance: mMedicalAllowance, mGross: mGross, mIncomeTax: mIncomeTax, mProvidentFund: mProvidentFund
    }*/

    //console.log("objEmployeeInfo: ");
    //console.log(objEmployeeInfo);


    var vExamp = document.querySelectorAll("#" + tableEducation + " tr td .vExamp");
    var vGroup = document.querySelectorAll("#" + tableEducation + " tr td .vGroup");
    var vInstitute = document.querySelectorAll("#" + tableEducation + " tr td .vInstitute");
    var vBoard = document.querySelectorAll("#" + tableEducation + " tr td .vBoard");
    var vDivOrClass = document.querySelectorAll("#" + tableEducation + " tr td .vDivOrClass");
    var iYear = document.querySelectorAll("#" + tableEducation + " tr td .iYear");

    var vPostOrDesignation = document.querySelectorAll("#" + tableExperience + " tr td .vPostOrDesignation");
    var vCompanyName = document.querySelectorAll("#" + tableExperience + " tr td .vCompanyName");
    var dFrom = document.querySelectorAll("#" + tableExperience + " tr td .dFrom");
    var dTo = document.querySelectorAll("#" + tableExperience + " tr td .dTo");
    var vMajorResponsibility = document.querySelectorAll("#" + tableExperience + " tr td .vMajorResponsibility");

    var uploadFile = $("#upload").val();
    var fileUpload = $("#attachmentUP").get(0);
    var files = fileUpload.files;
    // Create FormData object  
    var formData = new FormData();
    // Looping over all files and add it to FormData object  
    for (var i = 0; i < files.length; i++) {
        formData.append(files[i].name, files[i]);
    }

    var objData = {
        iAutoId: iAutoId, vEmployeeId: vEmployeeId, vEmployeeCode: vEmployeeCode, vFingerId: vFingerId, vEmployeeName: vEmployeeName,
        vEmployeeNameBangla: vEmployeeNameBangla,
        vDesignationId: vDesignationId.id, vDesignationName: vDesignationId.text,
        vDepartmentId: vDepartmentId.id, vDepartmentName: vDepartmentId.text, vSectionId: vSectionId.id, vSectionName: vSectionId.text, vReligion: vReligion.id,
        vContactNo: vContactNo, vEmailAddress: vEmailAddress, vGender: vGender.id, dDateOfBirth: dDateOfBirth, vNationality: vNationality,
        vNationalIdNo: vNationalIdNo, vEmployeeType: vEmployeeType.id, vServiceType: vServiceType.id,
        dApplicationDate: dApplicationDate, dInterviewDate: dInterviewDate, dJoiningDate: dJoiningDate, dConfirmationDate: dConfirmationDate,
        vProbationPeriod: vProbationPeriod.id, vEmployeeStatus: vEmployeeStatus.id, iStatus: iStatus, dStatusDate: dStatusDate,
        vEmployeePhoto: vEmployeePhoto,
        vFatherName: vFatherName, vMotherName: vMotherName, vPresentAddress: vPresentAddress, vPermanentAddress: vPermanentAddress, vBloodGroup: vBloodGroup.id,
        vMaritalStatus: vMaritalStatus.id, dMarriageDate: dMarriageDate, vSpouseName: vSpouseName, vSpouseOccupation: vSpouseOccupation,
        iNumberOfChild: iNumberOfChild, vBankId: vBankId.id, vBankName: vBankId.text, vBankBranchId: vBankBranchId.id, vBankBranchName: vBankBranchId.text,
        vAccountNo: vAccountNo, vMoneyTransferType: vMoneyTransferType, vRoutingNo: vRoutingNo, vAccountMobileNo: vAccountMobileNo,
        mBasic: mBasic, mHouseRent: mHouseRent, mMedicalAllowance: mMedicalAllowance, mGross: mGross, mIncomeTax: mIncomeTax, mProvidentFund: mProvidentFund,
        objEducation: [],
        objExperience: [],
        addUpdate: isEdit,
        attachment: uploadFile
    }

    for (var i = 0; i < iYear.length; i++) {
        if ($("#" + iYear[i].id).val() !="")
        {
            objData.objEducation.push(
                {
                    "vExamp": $("#" + vExamp[i].id).val(),
                    "vGroup": $("#" + vGroup[i].id).val(),
                    "vInstitute": $("#" + vInstitute[i].id).val(),
                    "vBoard": $("#" + vBoard[i].id).val(),
                    "vDivOrClass": $("#" + vDivOrClass[i].id).val(),
                    "iYear": $("#" + iYear[i].id).val()
                }
            );
        }
    }

    for (var i = 0; i < vMajorResponsibility.length; i++) {
        if ($("#" + vMajorResponsibility[i].id).val() !="")
        {
            objData.objExperience.push(
                {
                    "vPostOrDesignation": $("#" + vPostOrDesignation[i].id).val(),
                    "vCompanyName": $("#" + vCompanyName[i].id).val(),
                    "dFrom": $("#" + dFrom[i].id).val(),
                    "dTo": $("#" + dTo[i].id).val(),
                    "vMajorResponsibility": $("#" + vMajorResponsibility[i].id).val()
                }
            );
        }
    }

    formData.append("attachment", objData.attachment);                      formData.append("addUpdate", objData.addUpdate);
    formData.append("iAutoId", objData.iAutoId);                            formData.append("vEmployeeId", objData.vEmployeeId);
    formData.append("vEmployeeCode", objData.vEmployeeCode);                formData.append("vFingerId", objData.vFingerId);
    formData.append("vEmployeeName", objData.vEmployeeName);                formData.append("vEmployeeNameBangla", objData.vEmployeeNameBangla);
    formData.append("vDesignationId", objData.vDesignationId);              formData.append("vDesignationName", objData.vDesignationName);
    formData.append("vDepartmentId", objData.vDepartmentId);                formData.append("vDepartmentName", objData.vDepartmentName);
    formData.append("vSectionId", objData.vSectionId);                      formData.append("vSectionName", objData.vSectionName);
    formData.append("vReligion", objData.vReligion);                        formData.append("vContactNo", objData.vContactNo);
    formData.append("vEmailAddress", objData.vEmailAddress);                formData.append("vGender", objData.vGender);
    formData.append("dDateOfBirth", objData.dDateOfBirth);                  formData.append("vNationality", objData.vNationality);
    formData.append("vNationalIdNo", objData.vNationalIdNo);                formData.append("vEmployeeType", objData.vEmployeeType);
    formData.append("vServiceType", objData.vServiceType);                  formData.append("dApplicationDate", objData.dApplicationDate);
    formData.append("dInterviewDate", objData.dInterviewDate);              formData.append("dJoiningDate", objData.dJoiningDate);
    formData.append("dConfirmationDate", objData.dConfirmationDate);        formData.append("vProbationPeriod", objData.vProbationPeriod);
    formData.append("vEmployeeStatus", objData.vEmployeeStatus);            formData.append("iStatus", objData.iStatus);
    formData.append("dStatusDate", objData.dStatusDate);                    formData.append("vEmployeePhoto", objData.vEmployeePhoto);
    formData.append("vFatherName", objData.vFatherName);                    formData.append("vMotherName", objData.vMotherName);
    formData.append("vPresentAddress", objData.vPresentAddress);            formData.append("vPermanentAddress", objData.vPermanentAddress);
    formData.append("vBloodGroup", objData.vBloodGroup);                    formData.append("vMaritalStatus", objData.vMaritalStatus);
    formData.append("dMarriageDate", objData.dMarriageDate);                formData.append("vSpouseName", objData.vSpouseName);
    formData.append("vSpouseOccupation", objData.vSpouseOccupation);        formData.append("iNumberOfChild", objData.iNumberOfChild);
    formData.append("vBankId", objData.vBankId);                            formData.append("vBankName", objData.vBankName);
    formData.append("vBankBranchId", objData.vBankBranchId);                formData.append("vBankBranchName", objData.vBankBranchName);
    formData.append("vAccountNo", objData.vAccountNo);                      formData.append("vMoneyTransferType", objData.vMoneyTransferType);
    formData.append("vRoutingNo", objData.vRoutingNo);                      formData.append("vAccountMobileNo", objData.vAccountMobileNo);
    formData.append("mBasic", objData.mBasic);                              formData.append("mHouseRent", objData.mHouseRent);
    formData.append("mMedicalAllowance", objData.mMedicalAllowance);        formData.append("mGross", objData.mGross);
    formData.append("mIncomeTax", objData.mIncomeTax);                      formData.append("mProvidentFund", objData.mProvidentFund);

    for (var i = 0; i < objData.objEducation.length; i++) {
        formData.append("objEducation[" + i + "].vExamp", objData.objEducation[i].vExamp);
        formData.append("objEducation[" + i + "].vGroup", objData.objEducation[i].vGroup);
        formData.append("objEducation[" + i + "].vInstitute", objData.objEducation[i].vInstitute);
        formData.append("objEducation[" + i + "].vBoard", objData.objEducation[i].vBoard);
        formData.append("objEducation[" + i + "].vDivOrClass", objData.objEducation[i].vDivOrClass);
        formData.append("objEducation[" + i + "].iYear", objData.objEducation[i].iYear);
    }
    for (var i = 0; i < objData.objExperience.length; i++) {
        formData.append("objExperience[" + i + "].vPostOrDesignation", objData.objExperience[i].vPostOrDesignation);
        formData.append("objExperience[" + i + "].vCompanyName", objData.objExperience[i].vCompanyName);
        formData.append("objExperience[" + i + "].dFrom", objData.objExperience[i].dFrom);
        formData.append("objExperience[" + i + "].dTo", objData.objExperience[i].dTo);
        formData.append("objExperience[" + i + "].vMajorResponsibility", objData.objExperience[i].vMajorResponsibility);
    }

    //console.log("objData: ");
    //console.log(objData);


    /*console.log("formData: ");
    console.log(formData);
    console.log("isNew" + isNew);
    console.log("isEdit" + isEdit);
    console.log("addUpdate" + objData.addUpdate);*/

    /*for (var value of formData.values()) {
        console.log(value);
    }*/

    $.ajax({
        url: "/Payroll/EmployeeInfo/employeeSave",
        data: formData,
        type: 'POST',
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        success: function (res) {
            if (res.success)
            {
                clear();
                if (!isNew) {
                    $("#EmployeeInfo").modal('hide');
                }
                showButton(isEdit);
                $("#vEmployeeCode").focus();
                $('#tbEmployee').DataTable().ajax.reload();
                successNotify(res.message);

            } else {
                errorNotify(res.message);
            }
        }
    });
}

function setGrossAmount() {
    var mBasic = document.getElementById('mBasic').value.replace(",", "") || 0;
    var tempHouse = Math.round(parseFloat(mBasic) / 2);
    document.getElementById('mHouseRent').value = tempHouse;
    var mHouseRent = document.getElementById('mHouseRent').value.replace(",", "") || 0;
    var mMedicalAllowance = document.getElementById('mMedicalAllowance').value.replace(",", "") || 0;
    var mGross = parseFloat(mBasic) + parseFloat(mHouseRent) + parseFloat(mMedicalAllowance);
    document.getElementById('mGross').value = mGross;
    amountWithComma('#mGross');
}

function clear() {
    clearAttachment();
    $("#iAutoId").val(0);
    $("#upload").val("");
    $("#vEmployeeID").val(maxEmployeeId());
    $("#vEmployeeCode").val(maxEmployeeCode());
    $("#vFingerId").val("");
    $("#vEmployeeName").val("");
    $("#vEmployeeNameBangla").val("");

    designationDataLoad("#vDesignationId");
    departmentDataLoad("#vDepartmentId");
    sectionDataLoad("#vSectionId");
    $("#dDateOfBirth").val(getCDay() + '-' + getCMonth() + '-' + getCYear());

    $("#vContactNo").val("");
    $("#vEmailAddress").val("user@gmail.com");
    $("#vNationality").val("Bangladeshi");
    $("#vNationalIdNo").val("");
    $("#dApplicationDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#dInterviewDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#dJoiningDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#dConfirmationDate").val(getCDay() + '-' + getCMonth() + '-' + getCYear());
    $("#dStatusDate").val('01-01-1900');
    $("#vFatherName").val("");
    $("#vMotherName").val("");
    $("#vPresentAddress").val("");
    $("#vPermanentAddress").val("");
    $("#dMarriageDate").val('01-01-1900');
    $("#vSpouseName").val("");
    $("#vSpouseOccupation").val("");
    $("#iNumberOfChild").val("0");
    bankDataLoad("#vBankId");
    bankBranchDataLoad("#vBankBranchId");
    $("#vAccountNo").val("");
    $("#vRoutingNo").val("");
    $("#vAccountMobileNo").val("");
    $("#mBasic").val("0");
    $("#mHouseRent").val("0");
    $("#mMedicalAllowance").val("0");
    $("#mGross").val("0");
    $("#mIncomeTax").val("0");
    $("#mProvidentFund").val("0");

    /*
        iStatus,vEmployeePhoto
    */
    $('#vReligion').select2('data', { id: "Islam", text: "Islam" });
    $('#vGender').select2('data', { id: "Male", text: "Male" });
    $('#vEmployeeType').select2('data', { id: "Permanent", text: "Permanent" });
    $('#vServiceType').select2('data', { id: "Staff", text: "Staff" });
    $('#vProbationPeriod').select2('data', { id: "1", text: "1" });
    $('#vEmployeeStatus').select2('data', { id: "Continue", text: "Continue" });
    $('#vBloodGroup').select2('data', { id: "0", text: "==Choose a Group==" });
    $('#vMaritalStatus').select2('data', { id: "Unmarried", text: "Unmarried" });
    $("input[name=vMoneyTransferType][value='Bank']").prop("checked", true);
    employeeStatusOptionWork("Continue");
    paymentOptionWork("Bank");
    maritalStatusOptionWork("Unmarried");

    tableClearEducation(1);
    tableClearExperience(1);
}
function checkValiAutoIdation()
{
    var dDateOfBirth = $("#dDateOfBirth").val();
    var dApplicationDate = $("#dApplicationDate").val();
    var dInterviewDate = $("#dInterviewDate").val();
    var dJoiningDate = $("#dJoiningDate").val();
    var dConfirmationDate = $("#dConfirmationDate").val();
    var vDesignationId = $("#vDesignationId").select2('data');
    var vDepartmentId = $("#vDepartmentId").select2('data');
    var vSectionId = $("#vSectionId").select2('data');
    var vEmployeeStatus = $("#vEmployeeStatus").select2('data');
    var dStatusDate = getBdToDbFormat($("#dStatusDate").val());
    var vMaritalStatus = $("#vMaritalStatus").select2('data');
    var dMarriageDate = getBdToDbFormat($("#dMarriageDate").val());
    var vSpouseName = $("#vSpouseName").val();
    var vSpouseOccupation = $("#vSpouseOccupation").val();
    var iNumberOfChild = $("#iNumberOfChild").val();
    var vMoneyTransferType = $("input[name='vMoneyTransferType']:checked").val();
    var vBankId = $("#vBankId").select2('data');
    var vBankBranchId = $("#vBankBranchId").select2('data');
    var vAccountNo = $("#vAccountNo").val();
    var vRoutingNo = $("#vRoutingNo").val();
    var vAccountMobileNo = $("#vAccountMobileNo").val();
    var iStatus = false;
    var iMaritalStatus = false;
    var iBankStatus = false;
    var iBftnStatus = false;
    var iAccountMobileNo = false;

    if ($("#vEmployeeCode").val() != "") {
        if ($("#vFingerId").val() != "") {
            if ($("#vEmployeeName").val() != "") {
                if ($("#vContactNo").val() != "") {
                    if (dDateOfBirth != "") {
                        if (dApplicationDate != "") {
                            if (dInterviewDate != "") {
                                if (dJoiningDate != "") {
                                    if (dConfirmationDate != "") {
                                        if (vDesignationId.id != "0") {
                                            if (vDepartmentId.id != "0") {
                                                if (vSectionId.id != "0") {
                                                    if (vEmployeeStatus.id == "Continue") {
                                                        iStatus = true;
                                                    }
                                                    else {
                                                        if (dStatusDate != "1900-01-01") {
                                                            iStatus = true;
                                                        }
                                                    }
                                                    if (iStatus) {
                                                        if ($("#vFatherName").val() != "") {
                                                            if ($("#vMotherName").val() != "") {
                                                                if (vMaritalStatus.id == "Unmarried") {
                                                                    iMaritalStatus = true;
                                                                }
                                                                else {
                                                                    if (dMarriageDate != "1900-01-01") {
                                                                        if (vSpouseName != "") {
                                                                            if (vSpouseOccupation != "") {
                                                                                if (iNumberOfChild != "") {
                                                                                    iMaritalStatus = true;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                if (iMaritalStatus) {
                                                                    if ($("#mBasic").val() != "") {
                                                                        if ($("#mHouseRent").val() != "") {
                                                                            if ($("#mMedicalAllowance").val() != "") {
                                                                                if ($("#mIncomeTax").val() != "") {
                                                                                    if ($("#mProvidentFund").val() != "") {
                                                                                        if (vMoneyTransferType != "") {
                                                                                            if (vMoneyTransferType == "Bank") {
                                                                                                if (vBankId.id != "0") {
                                                                                                    if (vBankBranchId.id != "0") {
                                                                                                        if (vAccountNo != "") {
                                                                                                            iBankStatus = true;
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                if (iBankStatus) {
                                                                                                    return true;
                                                                                                }
                                                                                                else {
                                                                                                    warningNotify("Please Provide Bank, Branch & Account No in Salary Info Tab!"); $("#vAccountNo").focus(); return false;
                                                                                                }
                                                                                            }
                                                                                            else if (vMoneyTransferType == "BFTN") {
                                                                                                if (vBankId.id != "0") {
                                                                                                    if (vBankBranchId.id != "0") {
                                                                                                        if (vAccountNo != "") {
                                                                                                            if (vRoutingNo != "") {
                                                                                                                iBftnStatus = true;
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                                if (iBftnStatus) {
                                                                                                    return true;
                                                                                                }
                                                                                                else {
                                                                                                    warningNotify("Please Provide Bank, Branch, Account No & Routing No in Salary Info Tab!"); $("#vRoutingNo").focus(); return false;
                                                                                                }
                                                                                            }
                                                                                            else if (vMoneyTransferType == "Mobile") {
                                                                                                if (vAccountMobileNo != "") {
                                                                                                    iAccountMobileNo = true;
                                                                                                }
                                                                                                if (iAccountMobileNo) {
                                                                                                    return true;
                                                                                                }
                                                                                                else {
                                                                                                    warningNotify("Please Provide Mobile No in Salary Info Tab!"); $("#vAccountMobileNo").focus(); return false;
                                                                                                }
                                                                                            }
                                                                                            else if (vMoneyTransferType == "Cash") {
                                                                                                return true;
                                                                                            }

                                                                                        }
                                                                                        else {
                                                                                            warningNotify("Please Provide Money Transfer Type in Salary Info Tab!"); $("#mProvidentFund").focus(); return false;
                                                                                        }
                                                                                    }
                                                                                    else {
                                                                                        warningNotify("Please Provide Provident Fund in Salary Info Tab!"); $("#mProvidentFund").focus(); return false;
                                                                                    }
                                                                                }
                                                                                else {
                                                                                    warningNotify("Please Provide Income Tax in Salary Info Tab!"); $("#mIncomeTax").focus(); return false;
                                                                                }
                                                                            }
                                                                            else {
                                                                                warningNotify("Please Provide Medical Allowance in Salary Info Tab!"); $("#mMedicalAllowance").focus(); return false;
                                                                            }
                                                                        }
                                                                        else {
                                                                            warningNotify("Please Provide House Rent in Salary Info Tab!"); $("#mHouseRent").focus(); return false;
                                                                        }
                                                                    }
                                                                    else {
                                                                        warningNotify("Please Provide Basic in Salary Info Tab!"); $("#mBasic").focus(); return false;
                                                                    }
                                                                }
                                                                else {
                                                                    warningNotify("Please Provide Employee Marital Status, Date, Spouse Name, Spouse Occupation And Number Of Child in Personal Info Tab!"); $("#vMaritalStatus").focus(); return false;
                                                                }
                                                            }
                                                            else {
                                                                warningNotify("Please Provide Mother Name in Personal Info Tab!"); $("#vMotherName").focus(); return false;
                                                            }
                                                        }
                                                        else {
                                                            warningNotify("Please Provide Father Name in Personal Info Tab!"); $("#vFatherName").focus(); return false;
                                                        }
                                                    }
                                                    else {
                                                        warningNotify("Please Provide Employee Status And Date in Official Info Tab!"); $("#vEmployeeStatus").focus(); return false;
                                                    }
                                                }
                                                else {
                                                    warningNotify("Please Provide Section in Official Info Tab!"); $("#vSectionId").focus(); return false;
                                                }
                                            }
                                            else {
                                                warningNotify("Please Provide Department in Official Info Tab!"); $("#vDepartmentId").focus(); return false;
                                            } 
                                        }
                                        else {
                                            warningNotify("Please Provide Designation in Official Info Tab!"); $("#vDesignationId").focus(); return false;
                                        }
                                    }
                                    else {
                                        warningNotify("Please Provide Confirmation Date in Official Info Tab!"); $("#dConfirmationDate").focus(); return false;
                                    }
                                }
                                else {
                                    warningNotify("Please Provide Joining Date in Official Info Tab!"); $("#dJoiningDate").focus(); return false;
                                }
                            }
                            else {
                                warningNotify("Please Provide Interview Date in Official Info Tab!"); $("#dInterviewDate").focus(); return false;
                            }
                        }
                        else {
                            warningNotify("Please Provide Application Date in Official Info Tab!"); $("#dApplicationDate").focus(); return false;
                        }
                    }
                    else {
                        warningNotify("Please Provide Date Of Birth in Official Info Tab!"); $("#dDateOfBirth").focus(); return false;
                    }
                }
                else {
                    warningNotify("Please Provide Employee Contact No in Official Info Tab!"); $("#vContactNo").focus(); return false;
                }
            }
            else {
                warningNotify("Please Provide Employee Name in Official Info Tab!"); $("#vEmployeeName").focus(); return false;
            }
        }
        else {
            warningNotify("Please Provide Finger Id in Official Info Tab!"); $("#vFingerId").focus(); return false;
        }
    }
    else {
        warningNotify("Please Provide Employee Code in Official Info Tab!"); $("#vEmployeeCode").focus(); return false;
    }
}

