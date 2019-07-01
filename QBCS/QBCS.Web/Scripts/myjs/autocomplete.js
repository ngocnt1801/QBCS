$(function () {
    //$('#myautocomplete').change({

    //});
    $('#myautocomplete').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "http://localhost/QBCS.Web/User/GetLecturer",
                type: "GET",
                dataType: "json",
                data: request,
                error: function (data) {

                },
                success: function (data) {
                    return response(data);
                }
            });
        }
    });
});