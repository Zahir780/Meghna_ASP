var dataTable;
var table = "#tbSalary";

$(document).ready(function () {
    init();
    var pSalaryDate = $("#pSalaryDate").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');

    var url = "/Payroll/EditMonthlySalary/GetSalary?pSalaryDate=" + pSalaryDate.id + "&pEmployeeId=" + pEmployeeId.id +
        "&pDepartmentId=" + pDepartmentId.id + "&pSectionId=" + pSectionId.id;
    console.log(url);
    loadDataTable(url);
    btnAction();

});

function btnAction() {
    $("#pSalaryDate").click(function () {
        var pSalaryDate = $("#pSalaryDate").select2('data');
        if (!isEmpty(pSalaryDate)) {
            getDepartment(pSalaryDate.id);
        }
    });
    $("#pDepartmentId").click(function () {
        var pSalaryDate = $("#pSalaryDate").select2('data');
        var pDepartmentId = $("#pDepartmentId").select2('data');
        if (pSalaryDate.id != "0" && pDepartmentId.id != "0") {
            getSection(pSalaryDate.id, pDepartmentId.id);
        }
    });
    $("#pSectionId").click(function () {
        var pSalaryDate = $("#pSalaryDate").select2('data');
        var pDepartmentId = $("#pDepartmentId").select2('data');
        var pSectionId = $("#pSectionId").select2('data');
        if (pSalaryDate.id != "0" && pDepartmentId.id != "0" && pSectionId.id != "0") {
            getEmployee(pSalaryDate.id, pDepartmentId.id, pSectionId.id);
        }
    });

    $("#pEmployeeId").change(function () {
        var pSalaryDate = $("#pSalaryDate").select2('data');
        var pEmployeeId = $("#pEmployeeId").select2('data');
        var pDepartmentId = $("#pDepartmentId").select2('data');
        var pSectionId = $("#pSectionId").select2('data');

        if (pSalaryDate.id != "0" && pEmployeeId.id != "0" && pDepartmentId.id != "0" && pSectionId.id != "0")
        {
            var url = "/Payroll/EditMonthlySalary/GetSalary?pSalaryDate=" + pSalaryDate.id + "&pEmployeeId=" + pEmployeeId.id +
                "&pDepartmentId=" + pDepartmentId.id + "&pSectionId=" + pSectionId.id;
            //console.log("Url: "+url);
            dataTable.ajax.url(url).load();
        }
    });
    $("#btnSave").click(function () {
        if (checkValidation()) {
            updateWork();
        }
    });
    $("#btnRefresh").click(function () {
        clear();
        tableClear();
    });
}
function checkValidation()
{
    var pSalaryDate = $("#pSalaryDate").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');

    var emp = document.querySelectorAll(table + " tr td .vEmployeeId");
    var isEmployee = false;
    for (var i = 0; i < emp.length; i++) {
        if ($("#" + emp[i].id).text() != "") {
            isEmployee = true;
        }
    }
    if (pSalaryDate.id != "0" && pSalaryDate.id != "")
    {
        if (pDepartmentId.id != "0" && pDepartmentId.id != "")
        {
            if (pSectionId.id != "0" && pSectionId.id != "")
            {
                if (pEmployeeId.id != "0" && pEmployeeId.id != "")
                {
                    if (isEmployee)
                    {
                        return true;
                    }
                    else
                    {
                        warningNotify("No Data Found!");
                        return false;
                    }
                }
                else
                {
                    warningNotify("Please Choose Employee");
                    return false;
                }
            }
            else
            {
                warningNotify("Please Choose Section");
                return false;
            }
        }
        else
        {
            warningNotify("Please Choose Department");
            return false;
        }
    }
    else
    {
        warningNotify("Please Choose Salary Month");
        return false;
    }
}

function updateWork() {
 
    var pSalaryDate = $("#pSalaryDate").val();
    var pEmployeeId = $("#pEmployeeId").val();
    var pDepartmentId = $("#pDepartmentId").val();
    var pSectionId = $("#pSectionId").val();

    var details = [];
    $("#tbSalary TBODY TR").each(function () {
        var row = $(this);

        var vEmployeeId = row.find("TD").eq(0).find("label").html().trim(); 
        var iTotalDay = parseFloat(replaceAll(row.find("TD").eq(6).find("input").val().trim(), ",", ""));
        var iHoliday = parseFloat(replaceAll(row.find("TD").eq(7).find("input").val().trim(), ",", ""));
        var iLeaveDay = parseFloat(replaceAll(row.find("TD").eq(9).find("input").val().trim(), ",", ""));
        var iPresentDay = parseFloat(replaceAll(row.find("TD").eq(10).find("input").val().trim(), ",", ""));
        var iNetPayableDays = parseFloat(replaceAll(row.find("TD").eq(11).find("input").val().trim(), ",", ""));
        var mGrossSalary = parseFloat(replaceAll(row.find("TD").eq(15).find("input").val().trim(), ",", ""));
        var mNetPayableSalary = parseFloat(replaceAll(row.find("TD").eq(16).find("input").val().trim(), ",", ""));
        var iTotalOTHour = parseFloat(replaceAll(row.find("TD").eq(17).find("input").val().trim(), ",", ""));
        var mOtRate = parseFloat(replaceAll(row.find("TD").eq(18).find("input").val().trim(), ",", ""));
        var mFridayRate = parseFloat(replaceAll(row.find("TD").eq(19).find("input").val().trim(), ",", ""));
        var iHolidayPresentCount = parseFloat(replaceAll(row.find("TD").eq(20).find("input").val().trim(), ",", ""));
        var mOtAmount = parseFloat(replaceAll(row.find("TD").eq(21).find("input").val().trim(), ",", ""));
        var mOtherEarning = parseFloat(replaceAll(row.find("TD").eq(22).find("input").val().trim(), ",", ""));
        var mGrossPayable = parseFloat(replaceAll(row.find("TD").eq(23).find("input").val().trim(), ",", ""));
        var mIncomeTax = parseFloat(replaceAll(row.find("TD").eq(24).find("input").val().trim(), ",", ""));
        var mOtherDeduction = row.find("TD").eq(25).find("input").val().trim();
        var mRevenueStamp = row.find("TD").eq(26).find("input").val().trim();
        var mAdvanceSalary = row.find("TD").eq(27).find("input").val().trim(); 
        var mTotalDeduction = row.find("TD").eq(28).find("input").val().trim(); 
        var mPayableSalary = row.find("TD").eq(29).find("input").val().trim(); 

        if (vEmployeeId != "" && vEmployeeId != "0") {
            var myDataList = {
                vEmployeeId: vEmployeeId,
                iTotalDay: iTotalDay,
                iHoliday: iHoliday,
                iLeaveDay: iLeaveDay,
                iPresentDay: iPresentDay,
                iNetPayableDays: iNetPayableDays,
                mGrossSalary: mGrossSalary,
                mNetPayableSalary: mNetPayableSalary,
                iTotalOTHour: iTotalOTHour,
                mOtRate: mOtRate,
                mFridayRate: mFridayRate,
                iHolidayPresentCount: iHolidayPresentCount,
                mOtAmount: mOtAmount,
                mOtherEarning: mOtherEarning,
                mGrossPayable: mGrossPayable,
                mIncomeTax: mIncomeTax,
                mOtherDeduction: mOtherDeduction,
                mRevenueStamp: mRevenueStamp,
                mAdvanceSalary: mAdvanceSalary,
                mTotalDeduction: mTotalDeduction,
                mPayableSalary: mPayableSalary,
            };
            details.push(myDataList);

            console.log("myDataList: ", myDataList);
           
        }
    });


 var objData = {
        pSalaryDate: pSalaryDate,
        pEmployeeId: pEmployeeId,
        pDepartmentId: pDepartmentId,
        pSectionId: pSectionId,
        objList: details
    };

    console.log("objData: ", objData);
    update(objData);
}

function update(objData) {
    $("#btnSave").hide();
    $("#btnSaveLoad").show();
    $.ajax({
        url: "/Payroll/EditMonthlySalary/UpdateSalary",
        data: objData,
        type: 'POST',
        async: false,
        success: function (res) {
            //console.log(res);
            if (res.success) {
                console.log(objData);
                $("#btnSave").show();
                $("#btnSaveLoad").hide();
                successNotify(res.message);
                $('#pEmployeeId').val(0).trigger('change');
                tableClear();
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}

function loadDataTable(url) {
    console.log(url);

    dataTable = $('#tbSalary').DataTable({
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
        "scrollY": 500,
        "scrollX": true,
        "scrollCollapse": true,
        "paging": false,
        "processing": true,
        "scrollX": true,
        "ordering": false,
        "fixedColumns": {
            "leftColumns": 2,
            "rightColumns": 1
        },

        "bPaginate": false,
        "bFilter": false,
        
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
                "data": { vServiceType: "vServiceType", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label style="width:150px;" id="vServiceType${data.vEmployeeId}">${data.vServiceType}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { iTotalDay: "iTotalDay", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:35px;" value="${data.iTotalDay}" id="iTotalDay${data.vEmployeeId}" 
                            class="form-control input-sm amount iTotalDay" readonly/>
                        </div>
                        `;

                }
            },
            {
                "data": { iHoliday: "iHoliday", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:35px;" value="${data.iHoliday}" id="iHoliday${data.vEmployeeId}" 
                            class="form-control input-sm amount iHoliday" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { iWorkingDay: "iWorkingDay", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label class="text-center" style="width:25px;">${data.iWorkingDay}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { iLeaveDay: "iLeaveDay", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:35px;" value="${data.iLeaveDay}" id="iLeaveDay${data.vEmployeeId}" 
                                onkeyup="amountCalculate('${data.vEmployeeId}')" class="form-control input-sm amount iLeaveDay"/>
                            </div>
                           `;
                }
            },
            {
                "data": { iPresentDay: "iPresentDay", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:35px;" value="${data.iPresentDay}" id="iPresentDay${data.vEmployeeId}" 
                            class="form-control input-sm amount iPresentDay" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { iNetPayableDays: "iNetPayableDays", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:35px;" value="${data.iNetPayableDays}" id="iNetPayableDays${data.vEmployeeId}" 
                            class="form-control input-sm amount iNetPayableDays" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { mBasic: "mBasic", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label class="text-right" style="width:70px;">${data.mBasic.replaceAll(".0000", "") }</label>
                        </div>
                        `;
                }
            },
            {
                "data": { mHouseRent: "mHouseRent", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label class="text-right" style="width:70px;">${data.mHouseRent.replaceAll(".0000", "")}</label>
                        </div>
                        `;
                }
            },
            {
                "data": { mMedicalAllowance: "mMedicalAllowance", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div>
                            <label class="text-right" style="width:70px;">${data.mMedicalAllowance.replaceAll(".0000", "") }</label>
                        </div>
                        `;
                }
            },
            {
                "data": { mGrossSalary: "mGrossSalary", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:60px;" value="${data.mGrossSalary.replaceAll(".0000", "") }" id="mGrossSalary${data.vEmployeeId}"
                            class="form-control input-sm amount mGrossSalary" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { mNetPayableSalary: "mNetPayableSalary", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:80px;" value="${data.mNetPayableSalary.replaceAll(".0000", "") }" id="mNetPayableSalary${data.vEmployeeId}"
                            class="form-control input-sm amount mNetPayableSalary" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { iTotalOTHour: "iTotalOTHour", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:70px;" value="${data.iTotalOTHour.replaceAll(".0000", "") }" id="iTotalOTHour${data.vEmployeeId}"
                                onkeyup="amountCalculate('${data.vEmployeeId}')" class="form-control input-sm amount iTotalOTHour"/>
                            </div>
                           `;
                }
            },
            {
                "data": { mOtRate: "mOtRate", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:70px;" value="${data.mOtRate.replaceAll(".0000", "") }" id="mOtRate${data.vEmployeeId}" 
                                onkeyup="amountCalculate('${data.vEmployeeId}')" class="form-control input-sm amount mOtRate"/>
                            </div>
                           `;
                }
            },
            {
                "data": { mFridayRate: "mFridayRate", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:70px;" value="${data.mFridayRate.replaceAll(".0000", "") }" id="mFridayRate${data.vEmployeeId}" 
                                onkeyup="amountCalculate('${data.vEmployeeId}')" class="form-control input-sm amount mFridayRate"/>
                            </div>
                           `;
                }
            },
            {
                "data": { iHolidayPresentCount: "iHolidayPresentCount", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:35px;" value="${data.iHolidayPresentCount.replaceAll(".0000", "") }" id="iHolidayPresentCount${data.vEmployeeId}"
                                class="form-control input-sm amount iHolidayPresentCount"/>
                            </div>
                           `;
                }
            },
            {
                "data": { mOtAmount: "mOtAmount", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:70px;" value="${data.mOtAmount.replaceAll(".0000", "") }" id="mOtAmount${data.vEmployeeId}" 
                                onkeyup="amountCalculate('${data.vEmployeeId}')" class="form-control input-sm amount mOtAmount"/>
                            </div>
                           `;
                }
            },
            {
                "data": { mOtherEarning: "mOtherEarning", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:70px;" value="${data.mOtherEarning.replaceAll(".0000", "") }" id="mOtherEarning${data.vEmployeeId}" 
                                onkeyup="amountCalculate('${data.vEmployeeId}')" class="form-control input-sm amount mOtherEarning"/>
                            </div>
                           `;
                }
            },
            {
                "data": { mGrossPayable: "mGrossPayable", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:80px;" value="${data.mGrossPayable.replaceAll(".0000", "") }" id="mGrossPayable${data.vEmployeeId}" 
                            class="form-control input-sm amount mGrossPayable" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { mIncomeTax: "mIncomeTax", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:60px;" value="${data.mIncomeTax.replaceAll(".0000", "") }" id="mIncomeTax${data.vEmployeeId}" 
                            class="form-control input-sm amount mIncomeTax" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { mOtherDeduction: "mOtherDeduction", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:70px;" value="${data.mOtherDeduction.replaceAll(".0000", "") }" id="mOtherDeduction${data.vEmployeeId}" 
                                onkeyup="amountCalculate('${data.vEmployeeId}')" 
                                class="form-control input-sm amount mOtherDeduction"/>
                            </div>
                           `;
                }
            },
            {
                "data": { mRevenueStamp: "mRevenueStamp", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:35px;" value="${data.mRevenueStamp.replaceAll(".0000", "") }" id="mRevenueStamp${data.vEmployeeId}" 
                            class="form-control input-sm amount mRevenueStamp" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { mAdvanceSalary: "mAdvanceSalary", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <input type="text" style="width:70px;" value="${data.mAdvanceSalary.replaceAll(".0000", "") }" id="mAdvanceSalary${data.vEmployeeId}" 
                                onkeyup="amountCalculate('${data.vEmployeeId}')" 
                                class="form-control input-sm amount mAdvanceSalary"/>
                            </div>
                           `;
                }
            },
            {
                "data": { mTotalDeduction: "mTotalDeduction", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:80px;" value="${data.mTotalDeduction.replaceAll(".0000", "") }" id="mTotalDeduction${data.vEmployeeId}" 
                            class="form-control input-sm amount mTotalDeduction" readonly/>
                        </div>
                        `;
                }
            },
            {
                "data": { mPayableSalary: "mPayableSalary", vEmployeeId: "vEmployeeId" },
                "render": function (data) {
                    return `
                        <div class="text-center">
                            <input type="text" style="width:80px;" value="${data.mPayableSalary.replaceAll(".0000", "") }" id="mPayableSalary${data.vEmployeeId}"
                            class="form-control input-sm amount mPayableSalary" readonly/>
                        </div>
                        `;
                }
            }
        ]
    });
}


function amountCalculate(id)
{
    var iTotalDay = parseFloat($("#iTotalDay" + id).val()) || 0;
    //console.log("iTotalDay : " + iTotalDay);

    var iHoliday = parseFloat($("#iHoliday" + id).val()) || 0;
    //console.log("iHoliday : " + iHoliday);

    var iLeaveDay = parseFloat($("#iLeaveDay" + id).val()) || 0;

    //console.log("iLeaveDay : " + iLeaveDay);

    var iPresentDay = parseFloat($("#iPresentDay" + id).val()) || 0;
    //console.log("iPresentDay : " + iPresentDay);


    //if (iLeaveDay > iPresentDay) {
    //    errorNotify("LeaveDay can't be greater than PresentDay");
    //    $('#iLeaveDay' + id).css('background-color', 'red');
    //} else {
    //    $('#iLeaveDay' + id).css('background-color', ''); 
    //}

    var iNetPayableDays = parseFloat(iHoliday + iLeaveDay + iPresentDay) || 0;
    //console.log("iNetPayableDays : " + iNetPayableDays);

   if (iNetPayableDays > iTotalDay) {
        errorNotify("NetPayableDays can't be greater than TotalDay");
        $('#iNetPayableDays' + id).css('background-color', 'red');
    } else {
        $('#iNetPayableDays' + id).css('background-color', ''); 
    }

    var mGrossSalary = parseFloat($("#mGrossSalary" + id).val()) || 0;
    //console.log("mGrossSalary : " + mGrossSalary);

    var mNetPayableSalary = Math.round((mGrossSalary / iTotalDay) * iNetPayableDays); 
    //console.log("mNetPayableSalary : " + mNetPayableSalary);

    var vServiceType = $("#vServiceType" + id).text();
    //console.log("vServiceType : " + vServiceType);

    var iTotalOTHour = parseFloat($("#iTotalOTHour" + id).val()) || 0;
    //console.log("iTotalOTHour : " + iTotalOTHour);

    var mOtRate = parseFloat($("#mOtRate" + id).val()) || 0;
    //console.log("mOtRate : " + mOtRate);

    var iHolidayPresentCount = parseFloat($("#iHolidayPresentCount" + id).val()) || 0;
    //console.log("iHolidayPresentCount : " + iHolidayPresentCount);

    var mFridayRate = parseFloat($("#mFridayRate" + id).val()) || 0;
    //console.log("mFridayRate : " + mFridayRate);

    var mOtAmount = 0.00;

    if (vServiceType == "Factory Worker") {
        mOtAmount = Math.round(parseFloat(iTotalOTHour * mOtRate)) || 0;
    }

    if (vServiceType == "Security") {
        mOtAmount = Math.round(parseFloat(iHolidayPresentCount * mFridayRate)) || 0;
    }

    console.log("vServiceType: " + vServiceType);
    console.log("mOtAmount : " + mOtAmount);

    var mOtherEarning = parseFloat($("#mOtherEarning" + id).val()) || 0;
    //console.log("mOtherEarning : " + mOtherEarning);

    var mGrossPayable = parseFloat(mNetPayableSalary + mOtherEarning + mOtAmount) || 0;
    //console.log("mOtAmount : " + mOtAmount);

    var mIncomeTax = parseFloat($("#mIncomeTax" + id).val()) || 0;
    console.log("mIncomeTax : " + mIncomeTax);

    var mOtherDeduction = parseFloat($("#mOtherDeduction" + id).val()) || 0;
    //console.log("mOtherDeduction : " + mOtherDeduction);

    var mRevenueStamp = parseFloat($("#mRevenueStamp" + id).val()) || 0;
    //console.log("mRevenueStamp : " + mRevenueStamp);

    var mAdvanceSalary = parseFloat($("#mAdvanceSalary" + id).val()) || 0;
    //console.log("mAdvanceSalary : " + mAdvanceSalary);

    var mTotalDeduction = parseFloat(mIncomeTax + mOtherDeduction + mRevenueStamp + mAdvanceSalary) || 0;
    //console.log("mTotalDeduction : " + mTotalDeduction);

    var mPayableSalary = Math.round(mGrossPayable - mTotalDeduction);
    //console.log("mPayableSalary : " + mPayableSalary);

    $('#iNetPayableDays' + id).val(iNetPayableDays);
    $('#mNetPayableSalary' + id).val(mNetPayableSalary);
    $('#mOtAmount' + id).val(mOtAmount);

    $('#mGrossPayable' + id).val(mGrossPayable);
    $('#mTotalDeduction' + id).val(mTotalDeduction);
    $('#mPayableSalary' + id).val(mPayableSalary);
}

function deleteSalary() {
    var pSalaryDate = $("#pSalaryDate").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');

    var jsonData =
    {
        pSalaryDate: pSalaryDate.id,
        pEmployeeId: pEmployeeId.id,
        pDepartmentId: pDepartmentId.id,
        pSectionId: pSectionId.id
    }
    $("#btnDelete").hide();
    $("#btnDeleteLoad").show();
    $.ajax({
        type: "POST",
        url: "/Payroll/DeleteMonthlySalary/deleteSalary",
        data: jsonData,
        async: false,
        success: function (res) {
            //console.log(JSON.stringify(res.data));
            if (res.success) {
                $("#btnDelete").show();
                $("#btnDeleteLoad").hide();
                clear();
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}

function checkValidation() {
    var pSalaryDate = $("#pSalaryDate").val();
    var pDepartmentId = $("#pDepartmentId").select2('data');
    var pSectionId = $("#pSectionId").select2('data');
    var pEmployeeId = $("#pEmployeeId").select2('data');

    if (!isEmpty(pSalaryDate)) {
        if (pDepartmentId.id != "0") {
            if (pSectionId.id != "0") {
                if (pEmployeeId.id != "0") {
                    return true;
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
        warningNotify("Please Choose Salary Date");
        return false;
    }
}
function init() {
    clear();
}
function clear() {
    getSalaryDate();
    $('#pDepartmentId').select2('data', { id: "0", text: "==Choose a Department==" });
    $('#pSectionId').select2('data', { id: "0", text: "==Choose a Section==" });
    $('#pEmployeeId').select2('data', { id: "0", text: "==Choose a Employee==" });
    $("#btnSaveLoad").hide();
}
function tableClear() {
    var url = "/Payroll/EditMonthlySalary/GetSalary?pSalaryDate=" + 0 + "&pEmployeeId=" + 0 +
        "&pDepartmentId=" + 0 + "&pSectionId=" + 0;
    dataTable.ajax.url(url).load();
}
function getSalaryDate() {
    var url = "/Payroll/EditMonthlySalary/getSalaryDate";
    console.log("getSalaryDate: " + url);
    $("#pSalaryDate").select2('data', { id: '0', text: '==Choose a Date==' });
    $.getJSON(url, function (data) {
        var item = "";
        item += '<option value="' + '0' + '">==Choose a Date==</option>'
        $("#pSalaryDate").html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $("#pSalaryDate").html(item);
    });
}
function getDepartment(pSalaryDate) {
    var url = "/Payroll/EditMonthlySalary/getDepartment?pSalaryDate=" + pSalaryDate;
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
function getSection(pSalaryDate, pDepartmentId) {
    var url = "/Payroll/EditMonthlySalary/getSection?pSalaryDate=" + pSalaryDate + "&pDepartmentId=" + pDepartmentId;
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
function getEmployee(pSalaryDate, pDepartmentId, pSectionId) {
    var url = "/Payroll/EditMonthlySalary/getEmployee?pSalaryDate=" + pSalaryDate + "&pDepartmentId=" + pDepartmentId + "&pSectionId=" + pSectionId;
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


