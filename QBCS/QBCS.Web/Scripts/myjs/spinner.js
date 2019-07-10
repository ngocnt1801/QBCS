$(document).ready(function () {
    $('.btn-spinner').on('click', function () {
        $('#spinner').css("display", "block");
        $('#spinner').css("z-index", "1060");
        $("#nhiModal").modal();
        //setTimeout(function () {
        //    $this.html($this.data('original-text'));
        //}, 100000);
    });
});

