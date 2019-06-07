$(function () {
    QuestionTempView = {
        init: function () {
            this.editable = $(".editableTemp");
            $.each(this.editable, function(index, element){
                element.onclick = function(){
                    ImportFileOctopus.redirectToDetail(element.attributes["data-id"].value);
                }
            })
        },
        
    };

    ImportFileOctopus = {
        init: function () {
            QuestionTempView.init();
            this.bs_input_file();

        },

        bs_input_file: function () {
            $(".input-file").before(
                function () {
                    if (!$(this).prev().hasClass('input-ghost')) {
                        var element = $("<input type='file' class='input-ghost' style='visibility:hidden; height:0' accept='.txt,.xml'>");
                        element.attr("name", $(this).attr("name"));
                        element.change(function () {
                            element.next(element).find('input').val((element.val()).split('\\').pop());
                        });
                        $(this).find("button.btn-choose").click(function () {
                            element.click();
                        });
                        $(this).find("button.btn-reset").click(function () {
                            element.val(null);
                            $(this).parents(".input-file").find('input').val('');
                        });
                        $(this).find('input').css("cursor", "pointer");
                        $(this).find('input').mousedown(function () {
                            $(this).parents('.input-file').prev().click();
                            return false;
                        });
                        return element;
                    }
                }
            );
        },

        redirectToDetail: function(id){
            document.location.href= '/QBCS.Web/Import/GetQuestionTemp?tempId='+id;
        }
    }

    ImportFileOctopus.init();
});

