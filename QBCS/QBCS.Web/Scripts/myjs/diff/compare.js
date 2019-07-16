function compare() {
    $("input[type=button]").click(function () {
        $("#container tr").val().prettyTextDiff();
    });
}