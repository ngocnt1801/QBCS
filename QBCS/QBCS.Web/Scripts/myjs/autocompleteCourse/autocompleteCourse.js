$(function () {
    //$('#myautocomplete').change({

    //});
    $('#autocompleteCourse').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "http://localhost/QBCS.Web/LearningOutcome/LoadCourse",
                type: "GET",
                dataType: "json",
                data: request,
                error: function (data) {

                },
                success: function (data) {
                    var result = [];
                    if (data == null || data == "") {
                        result = ["Not found"];
                        return response(result);
                    }
                    for (var i = 0; i < data.length; i++){
                        var string = data[i].Name + " (" + data[i].Code + ")";
                        result.push(string);
                    }
                    return response(result);
                }
            });
        }
    });
});