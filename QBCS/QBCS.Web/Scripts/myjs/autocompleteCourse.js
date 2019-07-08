$(function () {
    //$('#myautocomplete').change({

    //});
    $('#autocompleteCourse').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/LearningOutcome/LoadCourseLive",
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
                        var Obj = {};
                        Obj['label'] = data[i].Name + " (" + data[i].Code + ")";
                        Obj['value'] = data[i].Id;
                        result.push(Obj);
                    }
                    return response(result);
                }
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            var course = ui.item.label.split(" (")[0];
            $("#autocompleteCourse").val(course);
            $("#autocompleteCourseId").val(ui.item.value);
        }
    });
});