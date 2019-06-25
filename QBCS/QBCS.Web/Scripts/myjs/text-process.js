function customs_text() {
    $('#btnUpdate').click(function (e) {
        var content;
        content = $('#customs-display').html();
        //var input;
        //input = $('#hidden-question-content').html();
        $('#hidden-question-content').val("[html]" + content);
    });
}
$(document).ready(function () {
    customs_text();
});
