var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tbDepartment').DataTable({
        "ajax": {
            "url": "/Payroll/DepartmentInfo/GetAll"
        },
        "columns": [
            { "data": "vDepartmentCode", "width": "3%", className: "text-center" },
            { "data": "vDepartmentName", "width": "40%" },
            {
                "data": "iActive", "width": "10%",
                "render": function (data) {
                    if (data) {
                        return '<div style="color:#239347" ><b> Active </b></div>';
                    } else {
                        return '<div style="color:#c71f1f"><b> Inactive </b></div>';
                    }
                }
            },
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