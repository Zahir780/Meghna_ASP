var dataTable;

$(document).ready(function () {
    loadDataTable();
});


function loadDataTable() {
    dataTable = $('#tbShift').DataTable({
        "ajax": {
            "url": "/Payroll/ShiftInfo/GetAll"
        },
        "columns": [
            { "data": "vShiftName", "width": "20%" },
            { "data": "vShiftType", "width": "10%" },
            {
                'data': 'dShiftStart', "width": "15%", "class": "text-center",
                "render": function (data) {
                    var date = new Date(0);
                    date.setSeconds(data.totalSeconds);
                    var timeString = date.toISOString().substr(11, 8);

                    var H = +timeString.substr(0, 2);
                    var h = (H % 12) || 12;
                    h = (h < 10) ? ("0" + h) : h;  // leading 0 at the left for 1 digit hours
                    var ampm = H < 12 ? " AM" : " PM";
                    timeString = h + timeString.substr(2, 3) + ampm;

                    return timeString;
                }
            },
            {
                "data": "dShiftEnd", "width": "15%", className: "text-center",
                "render": function (data) {
                    var date = new Date(0);
                    date.setSeconds(data.totalSeconds);
                    var timeString = date.toISOString().substr(11, 8);

                    var H = +timeString.substr(0, 2);
                    var h = (H % 12) || 12;
                    h = (h < 10) ? ("0" + h) : h;  // leading 0 at the left for 1 digit hours
                    var ampm = H < 12 ? " AM" : " PM";
                    timeString = h + timeString.substr(2, 3) + ampm;

                    return timeString;
                }
            },
            {
                'data': 'dLateInLimit', "width": "15%", "class": "text-center",
                "render": function (data) {
                    var date = new Date(0);
                    date.setSeconds(data.totalSeconds);
                    var timeString = date.toISOString().substr(11, 8);

                    var H = +timeString.substr(0, 2);
                    var h = (H % 12) || 12;
                    h = (h < 10) ? ("0" + h) : h;  // leading 0 at the left for 1 digit hours
                    var ampm = H < 12 ? " AM" : " PM";
                    timeString = h + timeString.substr(2, 3) + ampm;

                    return timeString;
                }
            },
            {
                "data": "dEarlyOutLimit", "width": "15%", className: "text-center",
                "render": function (data) {
                    var date = new Date(0);
                    date.setSeconds(data.totalSeconds);
                    var timeString = date.toISOString().substr(11, 8);

                    var H = +timeString.substr(0, 2);
                    var h = (H % 12) || 12;
                    h = (h < 10) ? ("0" + h) : h;  // leading 0 at the left for 1 digit hours
                    var ampm = H < 12 ? " AM" : " PM";
                    timeString = h + timeString.substr(2, 3) + ampm;

                    return timeString;
                }
            },
            {
                "data": "iActive", "width": "10%", "class": "text-center",
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