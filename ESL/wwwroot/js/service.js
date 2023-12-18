function isEmpty(obj) {
    for (var prop in obj) {
        if (obj.hasOwnProperty(prop)) {
            return false;
        }
    }
    return true;
}
// Amount/Number Field Start
function number(e) {
    e = e || window.event;
    var charCode = e.which ? e.which : e.keyCode;
    return /\d/.test(String.fromCharCode(charCode));
}
function amount(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode
    if (charCode > 31 && (charCode != 46 && (charCode < 48 || charCode > 57)))
        return false;
    return true;
}
function amountWithComma(debitAmount) {
    var val = $(debitAmount).val();
    val = val.replace(/[^0-9\.]/g, '');

    if (val != "") {
        valArr = val.split('.');
        valArr[0] = (parseInt(valArr[0], 10)).toLocaleString();
        val = valArr.join('.');
    }
    $(debitAmount).val(val);
}

// Replace All comma Start
function replaceAll(str, find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
}
    // Replace All comma End

function amountShowWithComma(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
// Amount/Number Field End
//Date Format Change Start
function getBdToDbFormat(date) {
    var x = date.split("-");
    var day = x[0];
    var month = x[1];
    var year = x[2];
    return year + '-' + month + '-' + day;
}
function getDbToBdFormat(date) {
    var x = date.split("-");
    var day = x[2];
    var month = x[1];
    var year = x[0];
    return day + '-' + month + '-' + year;
}
//Date Format Change End
//Get Full Age Start
function getAgeFull(birthDate, toDate) {
    if (!isNaN(birthDate) || !isNaN(toDate)) {
        return "";
    }
    var birthdate = birthDate;
    var senddate = toDate;
    var x = birthdate.split("-");
    var y = senddate.split("-");
    var bdays = x[0];
    var bmonths = x[1];
    var byear = x[2];
    var sdays = y[0];
    var smonths = y[1];
    var syear = y[2];
    if (sdays < bdays) {
        sdays = parseInt(sdays) + 30;
        smonths = parseInt(smonths) - 1;
        var fdays = sdays - bdays;
    }
    else {
        var fdays = sdays - bdays;
    }
    if (smonths < bmonths) {
        smonths = parseInt(smonths) + 12;
        syear = syear - 1;
        var fmonths = smonths - bmonths;
    }
    else {
        var fmonths = smonths - bmonths;
    }

    var fyear = syear - byear;
    console.log(fyear + '>' + fmonths + '>' + fdays);
    return fyear + 'Y. ' + fmonths + 'M. ' + fdays + 'D';
}
//Get Full Age End
//For New Date Start
function getCDayLast() {
    var today = new Date();
    var today = new Date(today.getFullYear(), today.getMonth() + 1, 0);
    var dd = String(today.getDate()).padStart(2, '0');
    return dd;
}
function getCDayFirst() {
    return "01";
}
function getCDay() {
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    return dd;
}
function getCMonth() {
    var today = new Date();
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    return mm;
}
function getCYear() {
    var dateobj = new Date();
    var year = dateobj.getFullYear();
    return year;
}
//For New Date End
document.onkeydown = function (e) {
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'S'.charCodeAt(0)) {
        $('#btnSave').click();
    }
}
document.onkeydown = function (e) {
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'E'.charCodeAt(0)) {
        $('#btnEdit').click();
    }
}
// Focus enter Start
$(document).on('keypress', 'form input,select', function (event) {
    event.stopImmediatePropagation();
    if (event.which == 13) {
        event.preventDefault();
        var $input = $('form input,select');
        if ($(this).is($input.last())) {
            $('#btnSave').focus();
        }
        else {
            $input.eq($input.index(this) + 1).focus();
        }
    }
})
// Focus Enter End

function focusScript(event, ele) {
    //See notes about 'which' and 'key'
    if (event.which == 13 || event.keyCode == 13) {
        console.log(ele)
        $(ele).focus();
        return false;
    }
    return true;
}

/*To Update Background of specific ID*/
function backgroundColor(event) {
    if (isEmpty(document.getElementById(event.target.id).value)) {
        document.getElementById(event.target.id).style.backgroundColor = "white";
    } else {
        document.getElementById(event.target.id).style.backgroundColor = "beige";
    }
}

function inputMask(event, mask) {
    with (event) {
        stopPropagation()
        preventDefault()
        if (!charCode) return
        var c = String.fromCharCode(charCode)
        if (c.match(/\D/)) return
        with (target) {
            var val = value.substring(0, selectionStart) + c + value.substr(selectionEnd)
            var pos = selectionStart + 1
        }
    }
    var nan = count(val, /\D/, pos);
    val = val.replace(/\D/g, '');
    var mask = mask.match(/^(\D*)(.+0)(\D*)$/)
    if (!mask) return
    if (val.length > count(mask[2], /0/)) return
    for (var txt = '', im = 0, iv = 0; im < mask[2].length && iv < val.length; im += 1) {
        var c = mask[2].charAt(im)
        txt += c.match(/\D/) ? c : val.charAt(iv++)
    }
    with (event.target) {
        value = mask[1] + txt + mask[3]
        selectionStart = selectionEnd = pos + (pos == 1 ? mask[1].length : count(value, /\D/, pos) - nan)
    }
    function count(str, c, e) {
        e = e || str.length
        for (var n = 0, i = 0; i < e; i += 1) if (str.charAt(i).match(c)) n += 1
        return n
    }
}

// Focus Enter End
$(document).ready(function () {
    $('.datepick').datepicker({
        format: "dd-mm-yyyy",
        autoclose: true,
        disableTouchKeyboard: true,
        todayHighlight: true
    });
    $('.datetimepick').date

});
$.fn.dataTable.render.moment = function (from, to, locale) {
    // Argument shifting
    if (arguments.length === 1) {
        locale = 'en';
        to = from;
        from = 'YYYY-MM-DD';
    }
    else if (arguments.length === 2) {
        locale = 'en';
    }

    return function (d, type, row) {
        if (!d) {
            return type === 'sort' || type === 'type' ? 0 : d;
        }

        var m = window.moment(d, from, locale, true);

        // Order and type get a number value from Moment, everything else
        // sees the rendered value
        return m.format(type === 'sort' || type === 'type' ? 'x' : to);
    };
};

function autocomplete(inp, arr) {
    /*the autocomplete function takes two arguments,
    the text field element and an array of possible autocompleted values:*/
    var currentFocus;
    /*execute a function when someone writes in the text field:*/
    inp.addEventListener("input", function (e) {
        var a, b, i, val = this.value;
        /*close any already open lists of autocompleted values*/
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        /*create a DIV element that will contain the items (values):*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        /*append the DIV element as a child of the autocomplete container:*/
        this.parentNode.appendChild(a);
        /*for each item in the array...*/
        for (i = 0; i < arr.length; i++) {
            /*check if the item starts with the same letters as the text field value:*/
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                /*create a DIV element for each matching element:*/
                b = document.createElement("DIV");
                /*make the matching letters bold:*/
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                /*insert a input field that will hold the current array item's value:*/
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                /*execute a function when someone clicks on the item value (DIV element):*/
                b.addEventListener("click", function (e) {
                    /*insert the value for the autocomplete text field:*/
                    inp.value = this.getElementsByTagName("input")[0].value;
                    /*close the list of autocompleted values,
                    (or any other open lists of autocompleted values:*/
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }
    });
    /*execute a function presses a key on the keyboard:*/
    inp.addEventListener("keydown", function (e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            /*If the arrow DOWN key is pressed,
            increase the currentFocus variable:*/
            currentFocus++;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 38) { //up
            /*If the arrow UP key is pressed,
            decrease the currentFocus variable:*/
            currentFocus--;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 13) {
            /*If the ENTER key is pressed, prevent the form from being submitted,*/
            e.preventDefault();
            if (currentFocus > -1) {
                /*and simulate a click on the "active" item:*/
                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {
        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}
