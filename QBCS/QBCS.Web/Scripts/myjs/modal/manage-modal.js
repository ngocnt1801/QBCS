function turnon_modal(response) {
    $('#btnImport').click(function () {
        $('#errorMessageDiv').hide();
        content = $('#inFile').val();
    });
    
    $('#btnSuccessOk').click(function () {
        $('#inFile').val("");
    });
    if (response == "Error") {
        displayError();
    } else {
        content = $('#inFile').val();
        if (content != "") {
            $("#process-time").text(parsetTime(response));
            $('#success-modal').modal();
        }
    }
}

function displayError() {
    $('#errorMessageDiv').show();
}

function parsetTime(t) {
    var days = Math.floor(t / (3600 * 24));
    t -= days * 3600 * 24;
    var hrs = Math.floor(t / 3600);
    t -= hrs * 3600;
    var mnts = Math.floor(t / 60);
    t -= mnts * 60;
    var result =
        days > 0 ? (days + " days ") : "" +
        hrs > 0 ? (hrs + " Hrs ") : "" +
        mnts > 0 ? (mnts + " Minutes ") : "" +
        t > 0 ? (t + " Seconds") : "";
    return result;
}

$(document).ready(function () {
   
    turnon_modal();
});
