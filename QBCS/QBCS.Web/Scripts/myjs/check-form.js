
function isNotEmpty() {
    var question = $('.question-custom').html();
    if (question == "") {
        alert("Question is null");
    }
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
    isNotEmpty();
});