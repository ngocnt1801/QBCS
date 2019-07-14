//$(document).ready(function () {
//    function compareHash(current, previous) {
//        for (var i = 0, len = Math.min(current.length, previous.length); i < len; i++) {
//            if (current.charAt(0) != previous.charAt(0)) break;
//        }
//        current = current.substr(i);
//        previous = previous.substr(i);
//        for (var i = 0, len = Math.min(current.length, previous.length); i < len; i++) {
//            if (current.substr(-1) != previous.substr(-1)) break;
//        }

//        //Array: Current = New hash, previous = old hash
//        return [current, previous];
//    }
//    var lastHash = location.hash;
//    $(window).bind('hashchange', function () {
//        var newHash = location.hash;
//        // Do something
//        //var diff = compareHash(newHash, lastHash);
//        //alert("Difference between old and new hash:\n" + diff[0] + "\n\n" + dif[1]);

//        $('#spinner').css("display", "block");
//        $('#spinner').css("z-index", "1060");
//        $('#pleaseWaitDialog').modal();

//        setTimeout(function () {
//            $('#spinner').css("display", "none");
//            $('#pleaseWaitDialog').modal('hide');
//        }, 500);

//        //At the end of the func:
//        lastHash = newHash;
//    });


//});

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
    ////attaching the event listener
    //$(window).on('hashchange', function () {
    //    $('#spinner').css("display", "block");
    //    $('#spinner').css("z-index", "1060");
    //    $('#pleaseWaitDialog').modal();

    //    setTimeout(function () {
    //        $('#spinner').css("display", "none");
    //        $('#pleaseWaitDialog').modal('hide');
    //    }, 500);
    //});

    ////manually tiggering it if we have hash part in URL
    //if (window.location.hash) {
    //    $(window).trigger('hashchange')
    //}
});