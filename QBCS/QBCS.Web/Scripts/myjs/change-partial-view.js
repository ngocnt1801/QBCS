function bt_change_partial(url) {
    //$(document).ready(function () {
    //    $('#btn-right').click(function () {
    //        $.get('', {}, function (response) {
    //            $("#question-import").html(response);
    //        });
    //    });
       
    //});
  
        $.get(url, {}, function (response) {
            $("#question-import").html(response);
            $('#dataTable').DataTable();
        });
    
}
