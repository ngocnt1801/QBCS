function turnon_modal() {
    $('#btnImport').click(function () {
       
        content = $('#inFile').val();
        if (content != "") {
            $('#success-modal').modal();
        }
       
    });
    $('#btnSuccessOk').click(function () {
        $('#inFile').val("");
    });
}

$(document).ready(function () {
    turnon_modal();
});
