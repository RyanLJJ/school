// JavaScript source code
var today = new Date();
var dd = today.getDate() + 1;
var mm = today.getMonth() + 1; //January is 0
var yyyy = today.getFullYear();

if (dd < 10) {
    dd = '0' + dd
}

if (mm < 10) {
    mm = '0' + mm
}

tomorrow = yyyy + '-' + mm + '-' + dd;

var nextYear = (yyyy + 1) + '-' + mm + '-' + dd
document.getElementById('startDate').setAttribute("min", tomorrow);
document.getElementById('startDate').setAttribute("max", nextYear);


var typePrices = new Array();
typePrices['Deluxe'] = 400;
typePrices['Premier'] = 450;

var viewPrices = new Array();
viewPrices["Lower Floor"] = 0;
viewPrices["City"] = 0;
viewPrices["Garden"] = 30;
viewPrices["Harbour"] = 30;

var bedPrices = new Array();
bedPrices["Single"] = 30;
bedPrices["Twin"] = 55;
bedPrices["Double"] = 55;
bedPrices["Queen"] = 70;
bedPrices["King"] = 85;

function getDetails() {
    var roomType = document.getElementById("roomType").value;
    var roomView = document.getElementById("roomView").value;
    var roomBed = document.getElementById("roomBed").value;
    var nights = document.getElementById("nights").value;
    var price = 0;

    price += typePrices[roomType];
    price += viewPrices[roomView];
    price += bedPrices[roomBed];
    price *= nights;

    document.getElementById("noBooking").innerHTML = null;
    document.getElementById("typeDetails").innerHTML = "Room Type: " + roomType;
    document.getElementById("viewDetails").innerHTML = "Room View: " + roomView;
    document.getElementById("priceDetails").innerHTML = "Price: $" + price;

}



function submitForm() {
    var nights = document.getElementById("nights").value;
    var bookDate = new Date(document.getElementById("startDate").value);
    var bookDay = bookDate.getDate();
    var endDay = bookDate.getDate() + parseInt(nights);
    var bookMonth = bookDate.getMonth() + 1;
    var bookYear = bookDate.getFullYear();

    daysInMonth = new Date(bookYear, bookMonth, 0).getDate(); /*Gets last day of month, then gets day value of it*/

    if (bookDay < 10) {
        bookDay = '0' + bookDay;
    }

    if (endDay < 10) {
        endDay = '0' + endDay;
    }

    if (bookMonth < 10) {
        bookMonth = '0' + bookMonth;
    }

    confirmBookDate = bookYear + '-' + bookMonth + '-' + bookDay;
    endBookDate = bookYear + '-' + bookMonth + '-' + endDay;


    if (endDay > daysInMonth) {
        endBookDate = bookYear + '-' + (bookMonth+1) + '-' + (endDay-daysInMonth);
    }
    
    var confirmMsg = confirm("Do you want to confirm your booking?");
    if (confirmMsg == true) {

        document.getElementById("dateDetails").innerHTML = "Your stay will be from " + confirmBookDate + " to " + endBookDate + ".";

        var custName = document.getElementById("name").value;
        document.getElementById("customerDetails").innerHTML = "Booking successful! Thank you for your booking, " + custName + ".";
        document.getElementById("roomField").disabled = true;
        document.getElementById("customerField").disabled = true;
        document.getElementById("submitButton").disabled = true;
    }
}

function resetForm() {
    document.getElementById("roomField").disabled = false;
    document.getElementById("customerField").disabled = false;
    document.getElementById("submitButton").disabled = false;
    document.getElementById("typeDetails").innerHTML = "No bookings have been made yet.";
    document.getElementById("viewDetails").innerHTML = null;
    document.getElementById("priceDetails").innerHTML = null;
    document.getElementById("dateDetails").innerHTML = null;
    document.getElementById("customerDetails").innerHTML = null;
}



