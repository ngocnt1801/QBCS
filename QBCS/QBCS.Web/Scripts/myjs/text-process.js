//function customs_text() {
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
        op = op.split("&lt;u&gt;").join("");
        op = op.split("&lt;/u&gt;").join("");
        op = op.split("&lt;i&gt;").join("");
        op = op.split("&lt;/i&gt;").join("");
        op = op.split("&lt;sub&gt;").join("<sub>");
        op = op.split("&lt;/sub&gt;").join("</sub>");
        op = op.split("&lt;sup&gt;").join("<sup>");
        op = op.split("&lt;/sup&gt;").join("</sup>");
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
        op = op.split("&lt;u&gt;").join("");
        op = op.split("&lt;/u&gt;").join("");
        op = op.split("&lt;i&gt;").join("");
        op = op.split("&lt;/i&gt;").join("");
        op = op.split("&lt;sub&gt;").join("<sub>");
        op = op.split("&lt;/sub&gt;").join("</sub>");
        op = op.split("&lt;sup&gt;").join("<sup>");
        op = op.split("&lt;/sup&gt;").join("</sup>");
        op = op.split("[html]").join("");
        $(this).html(op);
    });
}
function customs_display_p() {
    $.each($(".customs-display-p"), function () {
        var content;
        content = $(this).html();
        if (content.indexOf("[html]") >= 0) {
            content = content.split("<cbr>").join("&lt;br&gt;");
            content = content.split("&lt;cbr&gt;").join("<br/>");
            content = content.split("<br>").join("<br/>");
            content = content.split("&lt;br&gt;").join("&lt;br&gt;");
            content = content.split("&lt;p&gt;").join("");
            content = content.split("&lt;").join("<");
            content = content.split("&gt;").join(">");
            content = content.split("&lt;/p&gt;").join("");
            content = content.split("&lt;b&gt;").join("");
            content = content.split("&lt;/b&gt;").join("");
            content = content.split("&lt;span&gt;").join("");
            content = content.split("&lt;/span&gt;").join("");
            content = content.split("&lt;/span&gt;").join("");
            content = content.split("&lt;u&gt;").join("");
            content = content.split("&lt;/u&gt;").join("");
            content = content.split("&lt;i&gt;").join("");
            content = content.split("&lt;/i&gt;").join("");
            content = content.split("&lt;sub&gt;").join("<sub>");
            content = content.split("&lt;/sub&gt;").join("</sub>");
            content = content.split("&lt;sup&gt;").join("<sup>");
            content = content.split("&lt;/sup&gt;").join("</sup>");
            content = content.split("[html]").join("");
        }
        $(this).html(content);
    });
}

function customs_display() {
    var content;
    content = $('.question-custom').html();
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
        content = content.split("&lt;u&gt;").join("");
        content = content.split("&lt;/u&gt;").join("");
        content = content.split("&lt;i&gt;").join("");
        content = content.split("&lt;/i&gt;").join("");
        content = content.split("&lt;sub&gt;").join("<sub>");
        content = content.split("&lt;/sub&gt;").join("</sub>");
        content = content.split("&lt;sup&gt;").join("<sup>");
        content = content.split("&lt;/sup&gt;").join("</sup>");
        content = content.split("[html]").join("");
    } 
    $('.question-custom').html(content);
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
        var option;
        content = $('.question-custom').html();
        //option = $('.option-customs').html();
        //var input;
        //input = $('#hidden-question-content').html();
        content = content.split("\t").join("");
        content = content.split("\n").join("");
        content = content.replace(/(\r\n|\n|\r|\t)/gm, "");
        content = content.replace(/\s+/g, " ");
        $('#hidden-question-content').val("[html]" + content);
        $.each($(".option-customs"), function () {
            option = $(this).html();
            option = option.split("\t").join("");
            option = option.split("\n").join("");
            option = option.replace(/(\r\n|\n|\r|\t)/gm, "");
            option = option.replace(/\s+/g, " ");
            $(this).html(option);
            $('.hidden-option').val(option);
        });

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
    customs_display_p();
    customs_display();
    option_custom();
    question_custom();
 
});
