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
                    if (data == null || data == "") {
                        var result = ["Not found"];
                        return response(result);
                    }
                    for (var i = 0; i < data.length; i++) {
                        var Obj = {};
                        Obj['label'] = data[i].Name + " (" + data[i].Code + ")";
                        Obj['value'] = data[i].Id;
                        result.push(Obj);
                    }
                    return response(data);
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