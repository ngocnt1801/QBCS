function bt_change_partial(url, btn) {

        $.get(url, {}, function (response) {
            $("#question-import").html(response);
            $('#dataTable').DataTable();
        });
        $(".btn-group > .btn").removeClass("active");
        $(btn).addClass("active");
}

function nav_bar_active() {
    $('.navbar-nav a').click(function (e) {
        $('.navbar-nav').find('li.active').removeClass('active');
        $(this).parent('li').addClass('active');
    });
}

$(document).ready(function () {
    nav_bar_active();
});
