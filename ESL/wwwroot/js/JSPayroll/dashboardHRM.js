var colorOverTime = "#C76644";
var colorSalary = "#6FACBE";
var colorNight = "#8774B0";
var colorElectricity = "#0AA35C";
var colorSalaryLoan = "#E86065";
var colorTypeWiseSalary = "#E86065";
var colors = ["#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
    "#80DEEA", "#7986CB", "#F06292", "#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
    "#80DEEA", "#7986CB", "#F06292", "#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
    "#80DEEA", "#7986CB", "#F06292", "#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
    "#80DEEA", "#7986CB", "#F06292",];

$(document).ready(function () {
    init();
    $("#year").change(function () {
        salaryDataShow($(this).val());
    });

    /*
    
    $("#monthPfFund").change(function () {
        pfFundDataShow($(this).val());
    });
    $("#monthPfLoan").change(function () {
        pfLoanDataShow($(this).val());
        pfLoanInterestDataShow($(this).val());
    });
    $("#monthPfRecovery").change(function () {
        pfRecoveryDataShow($(this).val());
        pfRecoveryInterestDataShow($(this).val());
    });
    $("#btnRefresh").click(function (event) {
        event.preventDefault();
        init();
    });
    
    */

});
function init() {
    yearLoad("#year", getCYear());
    $("#dDateDailyManPowerStatus").val(getCDay() + '-' + getCMonth() + '-' + getCYear());

    employeeDataShow();
    typeWiseSalaryDataShow(getCMonth());
    salaryDataShow(getCYear());
    overTimeDataShow();
    mealAllowDataShow();

    /*
    
    monthLoad("#monthPfFund", "/Admin/Home/GetMonth", getCMonth());
    monthLoad("#monthPfLoan", "/Admin/Home/GetMonth", getCMonth());
    monthLoad("#monthPfRecovery", "/Admin/Home/GetMonth?type=recoveryTable", getCMonth());

    */

    /*
    
    electricityDataShow();
    pfFundDataShow(getCMonth());
    pfLoanDataShow(getCMonth());
    pfLoanInterestDataShow(getCMonth());
    pfRecoveryDataShow(getCMonth());
    pfRecoveryInterestDataShow(getCMonth());
    
    */

}
function yearLoad(id, setValue) {
    $(id).select2('data', { id: 0, text: "Choose Year" });
    var url = "/Admin/Home/GetYear";
    $.getJSON(url, function (data) {
        var item = "";
        $(id).empty();
        item += '<option value="' + 0 + '">Choose Year</option>'
        $(id).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(id).html(item);
        if (setValue != undefined) {
            $(id).select2('data', { id: setValue, text: setValue });
        }
    });
}
function monthLoad(id, url, setValue) {
    $(id).select2('data', { id: 0, text: chooseMonth });
    $.getJSON(url, function (data) {
        var item = "";
        $(id).empty();
        item += '<option value="' + 0 + '">' + chooseMonth + '</option>'
        $(id).html(item);
        $.each(data, function (i, opt) {
            item += '<option value="' + opt.value + '">' + opt.text + '</option>'
        });
        $(id).html(item);
        if (setValue != undefined) {
            $(id).select2('data', { id: setValue, text: getMonthName(parseInt(setValue)) });
        }
    });
}
function salaryDataShow(year) {
    var colorFlotSalary = [
        "#2baab1", '#E36159', "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292", "#FDD835", "#4527A0", "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292", "#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292", "#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292"
    ];
    var xData = [];
    var yData = [];
    var chart = verticalBarChartShow("#flotSalary", "", 328);
    $.ajax({
        url: '/Admin/Home/GetMonthAndSalary',
        data: { year: year },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                xData.push(getMonthName(parseInt(d[i].iMonth)));
                yData.push(parseFloat(d[i].mNetSalary).toFixed(0));
            }
            chart.updateOptions({
                colors: colorFlotSalary,
                series: [{
                    data: yData
                }],
                xaxis: {
                    categories: xData
                },
                plotOptions: {
                    bar: {
                        distributed: true,
                    }
                },
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'light',
                        type: "horizontal",
                        shadeIntensity: 0.25,
                        gradientToColors: undefined,
                        inverseColors: true,
                        opacityFrom: 0.85,
                        opacityTo: 0.85,
                        stops: [50, 0, 100]
                    }
                },
            })
        }
    });
}
function employeeDataShow()
{
    var pieData = [];
    $.ajax({
        url: '/Admin/Home/GetEmployee',
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                pieData.push({
                    label: d[i].vEmpType,
                    data: [[1, d[i].vEmployee]],
                    color: colors[i+8]
                });
            }
            //console.log(pieData);
            pieChartShow(pieData, "#flotPie");
        }
    });
}

function overTimeDataShow() {
    var colorFlotSalary = [
        "#2baab1", '#E36159', "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292", "#FDD835", "#4527A0", "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292", "#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292", "#0088cc", "#2baab1", "#734ba9", '#E36159', "#FDD835", "#4527A0", "#CDDC39",
        "#80DEEA", "#7986CB", "#F06292"];
    var xData = [];
    var yData = [];
    var chart = verticalBarChartShow("#flotOverTime", "Over Time (2022) ", 320, 80);
    $.ajax({
        url: '/Admin/Home/GetOverTimeChart',
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                xData.push(getMonthName(parseInt(d[i].iMonth)))
                yData.push(parseFloat(d[i].mNetAmount).toFixed(0));
            }
            chart.updateOptions({
                series: [{
                    data: yData
                }],
                xaxis: {
                    categories: xData
                },
                plotOptions: {
                    bar: {
                        distributed: true,
                    }
                },
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'light',
                        type: "horizontal",
                        shadeIntensity: 0.25,
                        gradientToColors: undefined,
                        inverseColors: true,
                        opacityFrom: 0.85,
                        opacityTo: 0.85,
                        stops: [50, 0, 100]
                    }
                }
            })
        }
    });
}

function mealAllowDataShow() {
    var xData = [];
    var yData = [];
    var chart = verticalBarChartShow("#flotMeal", "Meal Info (2022) ", 320, 80);
    $.ajax({
        url: '/Admin/Home/getMealAllowChart',
        async: false,
        success: function (res) {
            var d = res.data;
            console.log(d);
            for (var i = 0; i < d.length; i++) {
                xData.push(getMonthName(parseInt(d[i].iMonth)))
                yData.push(parseFloat(d[i].mNetAmount).toFixed(0));
            }
            chart.updateOptions({
                series: [{
                    data: yData
                }],
                xaxis: {
                    categories: xData
                },
                plotOptions: {
                    bar: {
                        distributed: true,
                    },
                },
                colors: colors,
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'light',
                        type: "horizontal",
                        shadeIntensity: 0.25,
                        gradientToColors: undefined,
                        inverseColors: true,
                        opacityFrom: 0.85,
                        opacityTo: 0.85,
                        stops: [50, 0, 100]
                    }
                },

            })


        }
    });
}

function electricityDataShow() {
    var xData = [];
    var yData = [];
    var chart = verticalBarChartShow("#flotElectricity", "we`y¨r wej", 220);
    $.ajax({
        url: '/Admin/Home/GetElectricityChart',
        async: false,
        success: function (res) {
            var d = res.data;
            console.log(d);
            for (var i = 0; i < d.length; i++) {
                xData.push(getMonthName(parseInt(d[i].iMonth)))
                yData.push(parseFloat(d[i].mNetAmount).toFixed(0));
            }
            chart.updateOptions({
                series: [{
                    data: yData
                }],
                xaxis: {
                    categories: xData
                },
                fill: {
                    colors: ["#05A259"],
                    type: 'gradient',
                    gradient: {
                        shade: 'light',
                        type: "horizontal",
                        shadeIntensity: 0.25,
                        gradientToColors: undefined,
                        inverseColors: true,
                        opacityFrom: 0.85,
                        opacityTo: 0.85,
                        stops: [50, 0, 100]
                    }
                },
            })
        }
    });
}
function typeWiseSalaryDataShow(month) {
    //var caption = "Salary ( " + getMonthName(month) + " )";
    var caption = "Salary ( December-2022 )";
    var xData = [];
    var yData = [];
    var chart = verticalBarChartShow("#flotSalary2", caption, 377, 80);
    $.ajax({
        url: '/Admin/Home/GetTypeWiseSalary',
        data: { month: month },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                xData.push(d[i].vEmpType)
                yData.push(parseFloat(d[i].mNetSalary).toFixed(0));
            }
            chart.updateOptions({
                series: [{
                    data: yData
                }],
                xaxis: {
                    categories: xData
                },
                plotOptions: {
                    bar: {
                        distributed: true,
                    },
                },
                colors: colors,
                fill: {
                    type: 'gradient',
                    gradient: {
                        shade: 'light',
                        type: "horizontal",
                        shadeIntensity: 0.25,
                        gradientToColors: undefined,
                        inverseColors: true,
                        opacityFrom: 0.85,
                        opacityTo: 0.85,
                        stops: [50, 0, 100]
                    }
                },
            })
        }
    });
}
function pfFundDataShow(month) {
    var xData = [];
    var yData = [];
    var chart = horizontalBarChartShow("#flotPfFund", "wcGd Rgv", 180);
    $.ajax({
        url: '/Admin/Home/GetPfFund',
        data: { month: month },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                yData.push(d[i].vEmpType);
                xData.push(d[i].mNetAmount);
            }
            chart.updateOptions({
                series: [{
                    data: xData
                }],
                xaxis: {
                    categories: yData
                }
            })

        }
    });
}
function pfLoanDataShow(month) {
    var xData = [];
    var yData = [];
    var chart = horizontalBarChartShow("#flotPfLoan", "wcGd ‡jvb my`gy³", 180);
    $.ajax({
        url: '/Admin/Home/GetPfLoan',
        data: { month: month },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                yData.push(d[i].vEmpType);
                xData.push(d[i].mNetAmount);
            }
            chart.updateOptions({
                series: [{
                    data: xData
                }],
                xaxis: {
                    categories: yData
                }
            })
        }
    });
}
function pfLoanInterestDataShow(month) {
    var xData = [];
    var yData = [];
    var chart = horizontalBarChartShow("#flotPfLoanInterest", "wcGd ‡jvb my`hy³", 180);
    $.ajax({
        url: '/Admin/Home/GetPfLoanWithInterest',
        data: { month: month },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                yData.push(d[i].vEmpType);
                xData.push(d[i].mNetAmount);
            }
            chart.updateOptions({
                series: [{
                    data: xData
                }],
                xaxis: {
                    categories: yData
                }
            })
        }
    });
}
function pfRecoveryDataShow(month) {
    var xData = [];
    var yData = [];
    var chart = horizontalBarChartShow("#flotPfRecovery", "wcGd Av`vq my`gy³", 180);
    $.ajax({
        url: '/Admin/Home/GetPfLoanRecovery',
        data: { month: month },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                yData.push(d[i].vEmpType);
                xData.push(d[i].mNetAmount);
            }
            chart.updateOptions({
                series: [{
                    data: xData
                }],
                xaxis: {
                    categories: yData
                }
            })
        }
    });
}
function pfRecoveryInterestDataShow(month) {
    var xData = [];
    var yData = [];
    var chart = horizontalBarChartShow("#flotPfRecoveryInterest", "wcGd Av`vq my`hy³", 180);
    $.ajax({
        url: '/Admin/Home/GetPfLoanRecoveryWithInterest',
        data: { month: month },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                yData.push(d[i].vEmpType);
                xData.push(d[i].mNetAmount);
            }
            chart.updateOptions({
                series: [{
                    data: xData
                }],
                xaxis: {
                    categories: yData
                }
            })
        }
    });
}
function pfLoanInterestDataShow(month) {
    var xData = [];
    var yData = [];
    var chart = horizontalBarChartShow("#flotPfLoanInterest", "wcGd ‡jvb my`hy³", 180);
    $.ajax({
        url: '/Admin/Home/GetPfLoanWithInterest',
        data: { month: month },
        async: false,
        success: function (res) {
            var d = res.data;
            for (var i = 0; i < d.length; i++) {
                yData.push(d[i].vEmpType);
                xData.push(d[i].mNetAmount);
            }
            chart.updateOptions({
                series: [{
                    data: xData
                }],
                xaxis: {
                    categories: yData
                }
            })
        }
    });
}

function pieChartShow(data, divId) {
    (function () {
        var plot = $.plot(divId, data, {
            series: {
                pie: {
                    show: true,
                    radius: 1,
                    label: {
                        show: true,
                        radius: 2 / 3,
                        formatter: function (label, series) {
                            var percentage = Math.round(series.percent) + "%";
                            return '<div style="font-size:12pt;text-align:center;padding:0px;color:white;">' + label + '-' + series.data[0][1] + '<br/><strong>' + percentage + '</strong> </div>';
                        },
                        threshold: 0.1
                    }
                }
            },
            legend: {
                show: false
            },
            grid: {
                hoverable: true,
                clickable: true
            }
        });
    })();
}
function verticalBarChartShow(divId, txtTitle, chartHeight, xAxisHeight) {
    var options = {
        series: [{
            name: '',
            data: []
        }],
        xaxis: {
            categories: []
        },
        chart: {
            type: 'bar',
            height: chartHeight,
            stacked: true,
        },
        plotOptions: {
            bar: {
                dataLabels: {
                    orientation: 'vertical',
                    position: 'bottom',
                    hideOverflowingLabels: true,
                },
                horizontal: false,
            },
        },
        legend: {
            show: false
        },
        stroke: {
            width: 1,
            colors: ['gray']
        },
        title: {
            text: (txtTitle == undefined ? "" : txtTitle)
        },
        xaxis: {
            labels: {
                show: true,
                rotate: -45,
                rotateAlways: true,
                maxHeight: xAxisHeight == undefined ? 75 : xAxisHeight
            }
        },
        yaxis: {
            title: {
                text: undefined
            },
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val
                }
            }
        },
        fill: {
            opacity: 1
        },
        dataLabels: {
            enabled: true,
            textAnchor: 'start',
            style: {
                colors: ['black'],
                fontSize: '12px',
                fontWeight:'normal'
            },
            offsetX: 0,
            dropShadow: {
                enabled: false
            }
        },
        noData: {
            text: 'Data Not Found!',
            align: 'center',
            verticalAlign: 'middle',
            offsetX: 0,
            offsetY: 0,
            style: {
                color: 'green',
                fontSize: '15px',
                fontFamily: undefined
            }
        }
    };
    var chart = new ApexCharts(document.querySelector(divId), options);
    chart.render();
    return chart;
}
function barChartShow(id, data, c) {
    (function () {
        var plot = $.plot(id, [data], {
            colors: [c],
            series: {
                bars: {
                    show: true,
                    barWidth: 0.8,
                    align: 'center'
                }
            },
            xaxis: {
                mode: 'categories',
                tickLength: 0
            },
            grid: {
                hoverable: true,
                clickable: true,
                borderColor: 'rgba(0,0,0,0.1)',
                borderWidth: 1,
                labelMargin: 15,
                backgroundColor: 'transparent'
            },
            tooltip: true,
            tooltipOpts: {
                content: '%y',
                shifts: {
                    x: -10,
                    y: 20
                },
                defaultTheme: false
            }
        });
    })();
}
function horizontalBarChartShow(divId, txtTitle, chartHeight) {
    var options = {
        series: [{
            name: '',
            data: []
        }],
        xaxis: {
            categories: []
        },
        chart: {
            type: 'bar',
            height: chartHeight,
            stacked: true,
        },
        plotOptions: {
            bar: {
                horizontal: true,
            },
        },
        stroke: {
            width: 1,
            colors: ['#fff']
        },
        title: {

            text: (txtTitle == undefined ? "" : txtTitle)
        },
        xaxis: {
            labels: {
                formatter: function (val) {
                    return val
                }
            }
        },
        yaxis: {
            title: {
                text: undefined
            },
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return val
                }
            }
        },
        dataLabels: {
            enabled: true,
            textAnchor: 'start',
            style: {
                colors: ['#fff'],
                fontSize: '16px'
            },
            offsetX: 0,
            dropShadow: {
                enabled: true
            }
        },
        fill: {
            opacity: 1
        },
        noData: {
            text: 'cªwµqvKiY n‡”Q...'
        }
    };

    var chart = new ApexCharts(document.querySelector(divId), options);
    chart.render();
    return chart;
}
function getMonthShortName(month) {
    switch (month) {
        case 1: return "Jan"; break;
        case 2: return "Feb"; break;
        case 3: return "Mar"; break;
        case 4: return "Apr"; break;
        case 5: return "May"; break;
        case 6: return "Jun"; break;
        case 7: return "Jul"; break;
        case 8: return "Aug"; break;
        case 9: return "Sep"; break;
        case 10: return "Oct"; break;
        case 11: return "Nov"; break;
        case 12: return "Dec"; break;
    }
}
function getMonthName(m) {
    var month = "";
    switch (parseInt(m)) {
        case 1: month = "January"; break;
        case 2: month = "February"; break;
        case 3: month = "March"; break;
        case 4: month = "April"; break;
        case 5: month = "May"; break;
        case 6: month = "June"; break;
        case 7: month = "July"; break;
        case 8: month = "August"; break;
        case 9: month = "September"; break;
        case 10: month = "October"; break;
        case 11: month = "November"; break;
        case 12: month = "December"; break;
    }
    return month;
}