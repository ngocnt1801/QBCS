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
function bt_group_active(){
    $(".btn-group > .btn").click(function(){
        $(".btn-group > .btn").removeClass("active");
        $(this).addClass("active");
    });
}
function nav_bar_active() {
    $('.navbar-nav a').click(function() {
        $('.navbar-nav').find('li.active').removeClass('active');
        $(this).parent('li').addClass('active');
    });
}
