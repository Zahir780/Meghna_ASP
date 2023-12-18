var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tbDesignation').DataTable({
        "ajax": {
            "url": "/Payroll/DesignationInfo/GetAll"
        },
        "columns": [
            { "data": "vDesignationCode", "width": "3%", className: "text-center" },
            { "data": "vDesignationName", "width": "40%" },
            { "data": "iRank", "width": "10%" },
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