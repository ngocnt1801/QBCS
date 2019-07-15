var fileobj;
function addClassDrop(e) {
    $('#drop_file_zone').addClass('active-drop');
    e.preventDefault();
    return false;
}
function removeClassDrop(e) {
    $('#drop_file_zone').removeClass('active-drop');
    e.preventDefault();
    return false;
}
function upload_file(e) {
    e.preventDefault();
    $('#drop_file_zone').removeClass('active-drop');
    $('#selectfile').get(0).files = e.dataTransfer.files;
    fileobj = $('#selectfile').get(0).files[0];
    var element = $(
        "<input type='file' class='input-ghost' style='visibility:hidden; height:0' accept='.txt,.xml,.doc,.docx'>"
    );
    var nameFile = fileobj.name;
    $('#inFile').val(nameFile);
    element.attr("name", nameFile);
}


//function ajax_file_upload(file_obj) {
//    if (file_obj != undefined) {
//        var form_data = new FormData();
//        form_data.append('file', file_obj);
//        $.ajax({
//            type: 'POST',
//            url: '/Import/AddToBank/importId=' + this.attributes["data-id"].value,
//            contentType: false,
//            processData: false,
//            data: form_data,
//            success: function (response) {
//                $('#selectfile').val('');
//                document.location.href = "/QBCS.Web/";
               
//            }
//        });
//    }
//}
