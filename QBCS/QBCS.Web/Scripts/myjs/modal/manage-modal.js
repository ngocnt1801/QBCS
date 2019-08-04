function turnon_modal(response) {
    $('#btnImport').click(function () {
        $('#errorMessageDiv').hide();
        content = $('#inFile').val();
        if (content != "") {
            $('#success-modal').modal();
        }
       
    });
    
    $('#btnSuccessOk').click(function () {
        $('#inFile').val("");
    });
    if (response == "Error") {
        displayError();
    }
}

function displayError() {
    $('#errorMessageDiv').show();
}

$(document).ready(function () {
   
    turnon_modal();
});
