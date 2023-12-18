$(document).ready(function () {
	var userid = $('#user-me').val();
	//var usertype = $('#user-type').text();
	//console.info("start sync for " + userid);
	//console.info("start sync for " + usertype);

	var source = new EventSource('/api/dashboardpush?id=' + userid);
	source.onmessage = function (event) {
		
		var myJSON = JSON.parse(event.data)
		if (myJSON != null) {
			//console.info(myJSON);
			setValue(myJSON);
		}
	};

	$(document).ajaxStart(function () {
		// Show image container
		$(".dashboardloader").show();
	});
	$(document).ajaxComplete(function () {
		// Hide image container
		setTimeout(function () { $(".dashboardloader").hide(); }, 1000);
	});
});

function DailySummary(v) {
	var dateElement = document.getElementById("currentDate");
	const todate = dateElement.dataset.date // "2020-02-02"
	var date = new Date(todate);
	//console.info(date);
	if (v == 'next') {
		date = new Date(date.setDate(date.getDate() + 1));
	} else {
		date = new Date(date.setDate(date.getDate() - 1));
	}
	//console.info(date);
	dateElement.setAttribute("data-date", date);
	dateElement.innerHTML = (moment(date).format('DD-MM-YYYY'));

	getData(date);
}

function refresh() {
	var date = new Date();
	var currentDate = document.getElementById("currentDate");
	currentDate.setAttribute("data-date", date);
	currentDate.innerHTML = (moment(date).format('DD-MM-YYYY'));
	getData(date);
}

function getData(date) {
	date = (moment(date).format('YYYY-MM-DD HH:mm:ss'));
	console.log(date);
	$.ajax({
		type: "GET",
		url: "/Admin/Home/InitDashboardData?date=" + encodeURI(date),
		success: function (response) {
			//console.log(response)
			var myJSON = JSON.parse(response)
			if (myJSON != null) {
				//console.info(myJSON);
				setValue(myJSON);
			}
		},
	});
}

function setValue(myJSON) {

	$('#deiselSalesAmountToday').html(myJSON.todayDieselSalesAmount.toLocaleString() +' Tk.');
	$('#deiselSalesQtyToday').html('(' + myJSON.todayDieselSalesQty.toLocaleString() + ' Ltr.)');
	$('#dieseldaysalesAmountToday').html(myJSON.todayDaySalesAmount.toLocaleString() + ' Tk.');
	$('#dieselnightsalesAmountToday').html(myJSON.todayNightSalesAmount.toLocaleString() + ' Tk.');

	$('#notCollectionAmount').html(myJSON.notCollectionAmount.toLocaleString() + ' Tk.');
	$('#notCollectionLtr').html('(' + myJSON.notCollectionLtr.toLocaleString() + ' Ltr.)');
	$('#notCollectionMemo').html('Invoice qty (' + myJSON.notCollectionMemo.toLocaleString() + ')');

	$('#deiselSalesAmountCurrentMonth').html(myJSON.currentMonthDieselSalesAmount.toLocaleString() + ' Tk.');
	$('#deiselSalesQtyCurrentMonth').html('(' + myJSON.currentMonthDieselSalesQty.toLocaleString() + ' Ltr.)');

	$('#deiselSalesAmountPreviousMonth').html(myJSON.previousMonthDieselSalesAmount.toLocaleString() + ' Tk.');
	$('#deiselSalesQtyPreviousMonth').html('(' + myJSON.previousMonthDieselSalesQty.toLocaleString() + ' Ltr.)');

	$('#deiselSalesAmountCurrentYear').html(myJSON.currentYearDieselSalesAmount.toLocaleString() + ' Tk.');
	$('#deiselSalesQtyCurrentYear').html('(' + myJSON.currentYearDieselSalesQty.toLocaleString() + ' Ltr.)');

	$('#deiselSalesAmountPreviousYear').html(myJSON.previousYearDieselSalesAmount.toLocaleString() + ' Tk.');
	$('#deiselSalesQtyPreviousYear').html('(' + myJSON.previousYearDieselSalesQty.toLocaleString() + ' Ltr.)');

	$('#salesAmountToday').html(myJSON.todaySalesAmount.toLocaleString() + ' Tk.');
	$('#salesQtyToday').html('(MR. Qty ' + myJSON.todaySalesMemoQty.toLocaleString() + ')');
	$('#daysalesAmountToday').html(myJSON.todayDaySalesAmount.toLocaleString() + ' Tk.');
	$('#nightsalesAmountToday').html(myJSON.todayNightSalesAmount.toLocaleString() + ' Tk.');

	$('#salesCollectionAmountToday').html(myJSON.todaySalesCollection.toLocaleString() + ' Tk.');
	$('#salesCollectionQtyToday').html('(MR. Qty ' + myJSON.todaySalesCollectionQty.toLocaleString() + ')');

	$('#dueAmountToday').html(myJSON.todaySalesDue.toLocaleString() + ' Tk.');
	$('#dueQtyToday').html('(MR. Qty ' + myJSON.todaySalesDueQty.toLocaleString() + ')');

	$('#PurchaseAmountToday').html(myJSON.todayPurchase.toLocaleString() + ' Tk.');

	$('#expanseAmountToday').html(myJSON.todayExpance.toLocaleString() + ' Tk.');

	$('#todayCashBalance').html(myJSON.todayCashBalance.toLocaleString() + ' Tk.');
	$('#todayBankBalance').html(myJSON.todayBankBalance.toLocaleString() + ' Tk.');

	$('#income').html(myJSON.totalIncome.toLocaleString() + ' Tk.');
	$('#expanse').html(myJSON.totalExpanse.toLocaleString() + ' Tk.');
	$('#profit').html(myJSON.totalProfit.toLocaleString() + ' Tk.');
	$('#dues').html(myJSON.totalDues.toLocaleString() + ' Tk.');
	$('#cash').html(myJSON.totalCash.toLocaleString() + ' Tk.');
	$('#bank').html(myJSON.totalBank.toLocaleString() + ' Tk.');

	$('#gun1').html(myJSON.Gun1.toLocaleString());
	$('#gun2').html(myJSON.Gun2.toLocaleString());
	$('#gun3').html(myJSON.Gun3.toLocaleString());
	$('#gun4').html(myJSON.Gun4.toLocaleString());
	$('#gun5').html(myJSON.Gun5.toLocaleString());
	$('#gun6').html(myJSON.Gun6.toLocaleString());
	$('#gun7').html(myJSON.Gun7.toLocaleString());
	$('#gun8').html(myJSON.Gun8.toLocaleString());
	$('#gun9').html(myJSON.Gun9.toLocaleString());
	$('#gun10').html(myJSON.Gun10.toLocaleString());
}



