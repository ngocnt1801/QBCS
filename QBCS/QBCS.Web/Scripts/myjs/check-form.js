
function isNotEmpty() {
    var check = true;
    var question = $('.question-custom').html();
    if (question == "") {
        swal('Question is null');
        check = false;
    }
    //if (check) {
    //    $('.form-group').submit();
    //}
}

$(document).ready(function () {
    //var $form = $('.form-group');
    //var initialState = $form.serialize();

    //$form.submit(function (e) {
    //    if (initialState === $form.serialize()) {
    //        $("#error-text").val("Question is not updated");
    //        $("#error-text").show();
    //        alert('Form is unchanged!');
    //    } else {
    //        $("#error-text").val("Question is not updated");
    //        $("#error-text").show();
    //        alert('Form has changed!');
    //    }
    //    e.preventDefault();
    //});
    //$('#btnUpdate').on('click', function () {
    //    var check = true;
    //    var question = $('.question-custom').val();
    //    if (question == "") {
    //        swal('Question is null');
    //        check = false;
    //    }
    //    if (check) {
    //        $('.form-group').submit();
    //    }
    //});
    //isNotEmpty();
});