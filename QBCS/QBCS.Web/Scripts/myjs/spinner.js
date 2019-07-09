$(document).ready(function () {
    $('.btn').on('click', function () {
        $('#spinner-grow').css("display", "block");

        setTimeout(function () {
            $this.html($this.data('original-text'));
        }, 2000);
    });
});

