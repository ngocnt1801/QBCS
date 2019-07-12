var fileobj;
function upload_file(e) {
    e.preventDefault();
    $('#drop_file_zone').addClass = 'active-drop';
    fileobj = e.dataTransfer.files[0];
    //ajax_file_upload(fileobj);
}

//function file_explorer() {
//    document.getElementById('selectfile').click();
//    document.getElementById('selectfile').onchange = function () {
//        fileobj = document.getElementById('selectfile').files[0];
//        ajax_file_upload(fileobj);
//    };
//}

function ajax_file_upload(file_obj) {
    if (file_obj != undefined) {
        var form_data = new FormData();
        form_data.append('file', file_obj);
        $.ajax({
            type: 'POST',
            url: '/Import/AddToBank/importId=' + this.attributes["data-id"].value,
            contentType: false,
            processData: false,
            data: form_data,
            success: function (response) {
                $('#selectfile').val('');
                document.location.href = "/QBCS.Web/";
               
            }
        });
    }
}
