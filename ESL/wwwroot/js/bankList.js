var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tbBank').DataTable({
        "ajax": {
            "url": "/Admin/BankInfo/GetAll"
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "bankName", "width": "65%" },
            { "data": "bankLogo", "width": "15%" },
            {
                "data": "id",
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