var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tbEmployee').DataTable({
        "ajax": {
            "url": "/Payroll/EmployeeInfo/GetAll"
        },
        "columns": [
            { "data": "vEmployeeCode", "width": "10%", className: "text-center" },
            { "data": "vEmployeeName", "width": "20%" },
            { "data": "vDesignationName", "width": "20%" },
            { "data": "vDepartmentName", "width": "20%" },
            { "data": "vServiceType", "width": "20%" },
            {
                "data": "iAutoId",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a class="btn btn-sm btn-primary text-white tbEdit">
                                    <i class="fa fa-edit"></i> 
                                </a>
                            </div>
                           `;
                }, "width": "10%"
            }
        ]
    });
}