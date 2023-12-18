var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "pageLength": 50,
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today) {
                        //user is currently locked
                        return `
                            <div class="text-center">
                                <button onclick=LockUnlock('${data.id}','1') class="btn btn-danger">
                                    <i class="fa fa-lock-open"></i>  Unlock
                                </button>

                                <a href="/Admin/User/Upsert/${data.id}" class="btn btn-primary on-default edit-row">
                                     <i class="fa fa-edit" ></i>  Access
                                </a>
                            </div>
                           `;
                    }
                    else {
                        return `
                            <div class="text-center">
                                <button onclick=LockUnlock('${data.id}','0') class="btn btn-success" >
                                    <i class="fa fa-lock"></i>  Lock
                                </button>

                               <a href="/Admin/User/Upsert/${data.id}" class="btn btn-primary on-default edit-row">
                                     <i class="fa fa-edit" ></i>  Access
                                </a>
                            </div>
                           `;
                    }
                    
                }, "width": "25%"
            }
        ]
    });
}

function LockUnlock(id, status) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: { id: id, status: status },
        success: function (data) {
            if (data.success) {
                dataTable.ajax.reload();
                successNotify(data.message);
            }
            else {
                errorNotify(data.message);
            }
        }
    });

}

