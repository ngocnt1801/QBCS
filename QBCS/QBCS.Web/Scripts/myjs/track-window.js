
$(document).ready(function () {

    $(function () {
        $(document).on('click', '.spinner-loading',function () {
            $(`<div class=loadingDiv>
                    <div class="lds-ring"><div></div><div></div><div></div><div></div></div>
                </div>`).prependTo(document.body);
        })

        // Bind the event.
        //$(window).hashchange(function () {
        //    $('#spinner').css("display", "block");
        //    $('#spinner').css("z-index", "1060");
        //    $('#pleaseWaitDialog').modal();

        //    setTimeout(function () {
        //        $('#spinner').css("display", "none");
        //        $('#pleaseWaitDialog').modal('hide');
        //    }, 500);
        //})


    });
    

});