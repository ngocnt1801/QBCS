﻿$(function () {
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
                    return response(data);
                }
            });
        },
        select: function (event, ui) {
            $('#myautocomplete').val(i.item[0].innerText.split(" (")[0]);
        }
    });
});