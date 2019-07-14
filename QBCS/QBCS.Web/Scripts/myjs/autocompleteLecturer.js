$(function () {
    //$('#myautocomplete').change({

    //});
    $('#myautocomplete').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/User/GetLecturer",
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
                    for (var i = 0; i < data.length; i++) {
                        var Obj = {};
                        Obj['label'] = data[i].Fullname + " (" + data[i].Code + ")";
                        Obj['value'] = data[i].Id;
                        result.push(Obj);
                    }
                    return response(result);
                }
            });
        },
        select: function (event, ui) {
            event.preventDefault();
            var lec = ui.item.label.split(" (")[0];
            $('#myautocomplete').val(lec);
            $("#autocompleteLecturerId").val(ui.item.value);
        }
    });
});