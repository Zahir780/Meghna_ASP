var x = 0;
var dropInfo, dropData;

var table = "gridHoliday";

$(document).ready(function () {
    init();
    dropInfo = null;
    dropData = null;
    getholidayData();

    /*x = 0;
    tableClear(1);
    var dFindDate = '2022-07-01';
    findWork(dFindDate);

    $("#btnAddRow").click(function () {
        x = x + 1;
        addRow(x);
    });*/

});

function findWork(dFindDate) {
    x = 0;
    if (dFindDate != "0" && dFindDate != "") {
        $.ajax({
            url: "/Payroll/HolidayDeclaration/FindData",
            data: { dFindDate: dFindDate },
            async: false,
            success: function (res) {
                var d = res.data;
                //console.log(res);
                if (res.isFind) {
                    for (var i = 0; i < d.length; i++) {
                        if (i == 0) {
                            //console.log("date:" + d[i].dDate + " & month:" + d[i].vMonth + " -" + d[i].vMonthName);
                            $("#date").val(d[i].dDate);
                            $("#year").select2('data', { id: d[i].iYear, text: d[i].iYear });
                            $("#month").select2('data', { id: d[i].vMonth, text: d[i].vMonthName });
                        }
                        /*x = x + 1;
                        addRow(x);
                        $("#dDate" + x).text(d[i].dDate);
                        $("#vOccasion" + x).val(d[i].vOccasion);*/

                        /*
                        $("#iAutoId" + x).text(d[i].iAutoId);           --for Label
                        $("#vOccasion" + x).val(amountShowWithComma(d[i].mBasic));      --for Others
                        */
                    }
                }
            }
        });
    }
}

function getholidayData()
{
    /* initialize the calendar */

    var Calendar = FullCalendar.Calendar;
    var calendarEl = document.getElementById('calendar');

    calendarEl.innerHTML = "";

    var holidayData = [];

    /*var url = "/Payroll/HolidayDeclaration/getAll";
    $.getJSON(url, function (response) {
        console.log(response.data);
        $.each(response.data, function (i, value) {
            //console.log(value);
            holidayData.push({
                title: 'title',
                description: "description",
                start: value.dDate,
                data: value,
                allDay: false
            });

        });
    });*/

    $.ajax({
        url: "/Payroll/HolidayDeclaration/getAll",
        async: false,
        success: function (res) {
            var d = res.data;
            console.log(d);
            for (var i = 0; i < d.length; i++) {
                console.log(d[i].vOccasion);
                console.log(d[i].dDate);

                if (d[i].vOccasion == "Holiday") {
                    holidayData.push({
                        title: d[i].vOccasion,
                        start: d[i].dDate,
                        dDate: d[i].dDate
                    });
                }
                else {
                    holidayData.push({
                        title: d[i].vOccasion,
                        start: d[i].dDate,
                        dDate: d[i].dDate,
                        backgroundColor: '#00a65a'
                    });
                }
            }
        }
    });

    var calendar = new Calendar(calendarEl,
    {
        plugins: ['bootstrap', 'interaction', 'dayGrid', 'timeGrid'],
        header: {left: 'prev',center: 'title',right: 'next'},
        'themeSystem': 'bootstrap',

        //navLinks: true, // can click day/week names to navigate views
        events: holidayData,
        editable: true,
        droppable: false,
        selectable: true,
        selectMirror: true,
        selectOverlap: false,
        //disableDragging: false,
        select: function (arg) {
            var title = prompt('Occasion:');
            if (title)
            {
                if (title == "Holiday") {
                    calendar.addEvent({
                        title: title,
                        start: arg.start,
                        end: arg.end,
                        allDay: arg.allDay
                    })
                }
                else {
                    calendar.addEvent({
                        title: title,
                        start: arg.start,
                        end: arg.end,
                        allDay: arg.allDay,
                        backgroundColor: '#00a65a'
                    })
                }
                saveWork(moment(arg.start).format("YYYY-MM-DD"), title)
            }
            /*console.log(arg);
            x = x + 1;
            addRow(x);
            $("#dDate" + x).text(moment(arg.start).format("YYYY-MM-DD"));
            $("#vOccasion" + x).val(title);*/

            calendar.unselect()
        },
        eventClick: function (arg) {
            if (confirm('Are you sure you want to delete this event?')) {
                deleteWork(moment(arg.event.extendedProps.dDate).format("YYYY-MM-DD"))
                arg.event.remove()
            }
        },
        allDaySlot: false,
        selectHelper: true,
        displayEventTime: false

    });
    calendar.render();
}

function saveWork(dDate, vOccasion)
{
    var jsonData =
    {
        dDate: dDate,
        vOccasion: vOccasion
    }
    $.ajax({
        type: "POST",
        url: "/Payroll/HolidayDeclaration/HolidaySave",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                successNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}
function deleteWork(dDate)
{
    var jsonData =
    {
        dDate: dDate
    }
    $.ajax({
        type: "POST",
        url: "/Payroll/HolidayDeclaration/HolidayDelete",
        data: jsonData,
        async: false,
        success: function (res) {
            if (res.success) {
                warningNotify(res.message);
            }
            else {
                errorNotify(res.message);
            }
        }
    });
}

function init()
{
    clear();
}
function clear() {
    /*tableClear(1);
    x = 0;
    addRow(x);*/
}


/*
function tableClear(start) {
    var tb = document.getElementById(table);
    var rowCount = tb.rows.length;
    for (var i = start; i < rowCount; i++) {
        tb.deleteRow(start);
    }
}
function addRow(x)
{
    var tableArray = new Array();
    tableArray = ['Date', 'Occasion', ''];
    var gridHoliday = document.getElementById('gridHoliday');
    //gridHoliday.style.backgroundColor = "green";
    var rowCount = gridHoliday.rows.length;
    var tr = gridHoliday.insertRow(rowCount);
    tr = gridHoliday.insertRow(rowCount);

    for (var c = 0; c < tableArray.length; c++) {
        var td = document.createElement('td');

        td = tr.insertCell(0);
        if (parseInt(x) % 2 == 0) {
            td.setAttribute('style', 'background-color:#fff;color:#000');
        }
        else {
            td.setAttribute('style', 'background-color:#eee;color:#000');
        }
        if (c == 2) {
            var ele = document.createElement('label');
            ele.setAttribute('class', 'dDate');
            ele.setAttribute('style', 'text-align:center; width: 100%;');
            ele.setAttribute('id', 'dDate' + x);
            td.appendChild(ele);
        }
        if (c == 1) {
            var ele = document.createElement('input');
            ele.setAttribute('class', 'form-control input-sm  vOccasion');
            ele.setAttribute('type', 'text');
            ele.setAttribute('id', 'vOccasion' + x);
            td.appendChild(ele);
        }
        if (c == 0) {
            var button = document.createElement('a');
            button.innerHTML = '<i class="fa fa-trash-o" aria-hidden="true"></i>';
            button.setAttribute('class', 'btn btn-default btn-sm');
            button.setAttribute('style', 'color:#fc0313');
            button.setAttribute('onclick', 'removeRow(this)');
            td.appendChild(button);
        }
        $("#dDate" + x).focus();
    }
}
function removeRow(oButton) {
    var tableId = "gridHoliday";
    var empTab = document.getElementById(tableId);
    var elements = document.querySelectorAll("#" + tableId + " tr td .dDate");
    if (elements.length > 1) {
        empTab.deleteRow(oButton.parentNode.parentNode.rowIndex);
    }
    else {
        $("#dDate" + oButton.parentNode.parentNode.rowIndex).text("");
        $("#vOccasion" + oButton.parentNode.parentNode.rowIndex).val("");
    }
}
*/