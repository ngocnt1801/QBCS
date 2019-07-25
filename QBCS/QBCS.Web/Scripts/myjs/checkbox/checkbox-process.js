$("#selectAll").click(function () {
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