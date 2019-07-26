$(document).ready(function () {
    $('#check-all').on('change', function (event) {
        event.preventDefault();
        if (this.checked) {
            $('.cb-course').each(function () {
                this.checked = true;
            });
        } else {
            $('.cb-course').each(function () {
                this.checked = false;
            });
        }
    });

    $('.cb-course').on('change', function () {
        if ($('.cb-course:checked').length === $('.cb-course').length) {
            $('#check-all').prop('checked', true);
        } else {
            $('#check-all').prop('checked', false);
        }
    });
});