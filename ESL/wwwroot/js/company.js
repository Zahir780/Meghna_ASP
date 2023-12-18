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
                "url": "/Admin/Company/GetAll"
            },

            "columns": [
                { "data": "name", "width": "35%" },
                { "data": "streetAddress", "width": "45%" },
                { "data": "phoneNumber", "width": "10%" },

                {
                    "data": "id",
                    "render": function (data) {
                        return `
                            <div class="text-center">
                                
                                <a href="/Admin/Company/Upsert/${data}" class="on-default edit-row"><i class="fa fa-edit"></i></a>
								<!--<a onclick=Delete("/Admin/Company/Delete/${data}") class="on-default remove-row"><i class="fa fa-trash-o"></i></a> -->
                            </div>
                           `;
                    }, "width": "10%"
                }
            ]
        });
    } else {
        dataTable = $('#tblData').DataTable({
            "ajax": {
                "url": "/Admin/Company/GetAll"
            },

            "columns": [
                { "data": "name", "width": "45%" },
                { "data": "streetAddress", "width": "45%" },
                { "data": "phoneNumber", "width": "10%" },

                
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