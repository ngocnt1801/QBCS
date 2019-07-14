function turnon_modal() {
   
    $('#btnImport').click(function () {
        content = $('#inFile').val();
        if (content != "") {
            $('#success-modal').modal();
        }
       
    });
}

$(document).ready(function () {
    turnon_modal();
});
