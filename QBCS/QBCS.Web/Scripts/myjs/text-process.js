﻿//function customs_text() {
//    $('#btnUpdate').click(function (e) {
//        var content;
//        content = $('#customs-display').html();
//        //var input;
//        //input = $('#hidden-question-content').html();
//        $('#hidden-question-content').val("[html]" + content);

//        option_custom();
//    });
//}
function option_custom() {
    var op;
    $.each($(".option-customs"), function () {
        op = $(this).html();
        op = op.split("<cbr>").join("");
        op = op.split("&lt;cbr&gt;").join("<br/>");
        op = op.split("&lt;br&gt;").join("<br/>");
        op = op.split("[html]").join("");
        $(this).html(op);
    });
}
function question_custom() {
    var op;
    $.each($(".question-custom"), function () {
        op = $(this).html();
        op = op.split("<cbr>").join("");
        op = op.split("&lt;cbr&gt;").join("<br/>");
        op = op.split("&lt;br&gt;").join("<br/>");
        op = op.split("[html]").join("");
        $(this).html(op);
    });
}
function customs_display() {
    var content;
    content = $('#customs-display').html();
    if (content.indexOf("[html]") >= 0) {
        content = content.split("<cbr>").join("&lt;br&gt;");
        content = content.split("&lt;cbr&gt;").join("<br/>");
        content = content.split("&lt;br&gt;").join("<br/>");
        content = content.split("<br>").join("<br/>");
        content = content.split("&lt;br&gt;").join("&lt;br&gt;");
        content = content.split("&lt;p&gt;").join("");
        content = content.split("&lt;/p&gt;").join("");
        content = content.split("&lt;b&gt;").join("");
        content = content.split("&lt;/b&gt;").join("");
        content = content.split("&lt;span&gt;").join("");
        content = content.split("&lt;/span&gt;").join("");
        content = content.split("&lt;/span&gt;").join("");
        content = content.split("[html]").join("");
    } else {
        content = content.split("<cbr>").join("<br/>");
        content = content.split("&lt;cbr&gt;").join("<br/>");
    }
    $('#customs-display').html(content);
}

function highlight(newElem, oldElem) {
    var oldText = oldElem.text(),
        text = '';
    var newText = newElem.text();
    var temp;
    var temp2;
    //if (oldText.indexOf("\t")) {
    //    oldText = oldText.split("\t");
    //}
    // oldText.html(oldText);
    oldText = oldText.split("<cbr>").join("");
    oldText = oldText.split("&lt;cbr&gt;").join("");
    oldText = oldText.split("&lt;br&gt;").join("");
    oldText = oldText.split("<br>").join("");
    oldText = oldText.split("&lt;br&gt;").join("");
    oldText = oldText.split("\t").join("");
    temp = oldText;
    temp2 = newText.split("\t").join("");
    temp2 = temp2.replace(/(\r\n|\n|\r|\t)/gm, "");
    temp2 = temp2.replace(/\s+/g, " ");
    temp2 = temp2.split("");
    temp2.forEach(function (val, i) {

        i--;
        if (val != temp.charAt(i)) {
            text += "<span class='highlight'>" + val + "</span>";
            i++;
        }
        else {
            text += val;
        }

    });
    newElem.val(text);
}

function customs_text() {
    $('#btnUpdate').click(function (e) {
        var content;
        content = $('.question-custom').html();
        //var input;
        //input = $('#hidden-question-content').html();
        $('#hidden-question-content').val("[html]" + content);

        $.each($(".customs-display"), function () {
            var content = $(this).html();
            $(this.attributes["data-for"].value).val("[html]" + content);
        })
       // $('#success-modal').modal('show');
    });
}


$(document).ready(function () {
    highlight($("#new"), $("#old"));
    customs_text();
   // customs_display();
    option_custom();
    question_custom();
});