function customs_text() {
    $('#btnUpdate').click(function (e) {
        var content;
        content = $('#customs-display').html();
        //var input;
        //input = $('#hidden-question-content').html();
        $('#hidden-question-content').val("[html]" + content);
    });
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
    newElem.html(text);
}




$(document).ready(function () {
    highlight($("#new"), $("#old"));
    customs_text();
    
});
