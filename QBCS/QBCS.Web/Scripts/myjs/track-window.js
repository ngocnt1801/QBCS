

$(document).ready(function () {

    $(function () {

        // Bind the event.
        $(window).hashchange(function () {
            $('#spinner').css("display", "block");
            $('#spinner').css("z-index", "1060");
            $('#pleaseWaitDialog').modal();

            setTimeout(function () {
                $('#spinner').css("display", "none");
                $('#pleaseWaitDialog').modal('hide');
            }, 500);
        })

        // Trigger the event (useful on page load).
        $(window).hashchange();

    });
});