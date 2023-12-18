var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    var element = document.getElementById("tblData");
    var isEdit = element.getAttribute('data-edit')

    if (isEdit === 'True') {
        dataTable = $('#tblData').DataTable({
            "ajax": {
                "url": "/Admin/Project/GetAll"
            },
            "columns": [
                { "data": "projectCode", "width": "15%" },
                { "data": "name", "width": "35%" },
                { "data": "locations", "width": "40%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="text-center">
                                <a href="/Admin/Project/Upsert/${data}" class="btn btn-sm btn-primary text-white">
                                    <i class="fa fa-edit"></i> 
                                </a>
                                <!--<a onclick=Delete("/Admin/Project/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fa fa-trash-alt"></i> 
                                </a>-->
                            </div>
                           `;
                    }, "width": "10%"
                }
            ]
        });
    } else {
        dataTable = $('#tblData').DataTable({
            "ajax": {
                "url": "/Admin/Project/GetAll"
            },
            "columns": [
                { "data": "projectCode", "width": "15%" },
                { "data": "name", "width": "45%" },
                { "data": "locations", "width": "40%" }
                
            ]
        });
    }
    
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}