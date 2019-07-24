$(function () {
    questionView = {
        init: function () {
            this.setEventRemoveOption();
            this.optionListContainer = $("#option-list-container");

            $(".add-option").on("click", function () {
                questionView.addNewOption();
            });
            questionView.uploadImg();
        },

        getOptionTemplate: function () {
            return (
                '<div class="form-inline mb-2">' +
                '<div class="customs-display col-md-10 customs-display-input" contenteditable="true" data-for="">' +
                '</div>'+
                '<input type="hidden" class="form-control col-lg-11 text-custom mt-2 option-content" value="" name="" id="">' +
                '<div class="col-md-1">' +
                '<label class="switch float-left">' +
                '<input type="checkbox" class="success option-correct" name="" value="true">' +
                '<span class="slider round"></span>' +
                "</label>" +
                "</div>" +
                '<div class="col-md-1">' +
                '<i class="fa fa-times-circle text-danger delete-option"></i>' +
                "</div>" +
                "</div>"
            );
        },

        setEventRemoveOption: function () {
            $(".delete-option").off("click");
            $(".delete-option").on("click", function () {
                $(this)
                    .parent()
                    .parent()
                    .empty();
                questionView.resetIndexParamName();
            });
        },

        uploadImg: function () {

            var tempate = ` <label>Image in Question</label>
                    <p>
                        <img id="ques-img" class="exam-image mt-2" onclick="img_zoom(this)" src="data:image/png;base64, {{imageSrc}}" />
                    </p>
                    <input type="hidden" name="Image" value="{{imageSrc}}" id="hidden-img" />`;

            $("#uploadImage").change(function (e) {

                for (var i = 0; i < e.originalEvent.srcElement.files.length; i++) {

                    var file = e.originalEvent.srcElement.files[i];

                    var img = $("#ques-img");
                    var reader = new FileReader();
                    reader.onloadend = function () {
                        if (img != undefined && img != null && img.length > 0) {
                            img[0].src = reader.result;
                            var realData = img[0].src.split(",")[1];
                            $('#hidden-img').val(realData);
                        } else {
                            var realData = reader.result.split(",")[1];
                            tempate = tempate.replace(new RegExp("{{imageSrc}}", 'g'), realData);
                            $("#question-image-container").append(tempate);
                        }
                        
                    }
                    reader.readAsDataURL(file);
                }
            });
        },

        resetIndexParamName: function () {
            $.each($(".option-content"), function (index, item) {
                this.attributes["name"].value = "Options[" + index + "].OptionContent";
                this.attributes["id"].value = "o-" + index;
            });

            $.each($(".customs-display-input"), function (index, item) {
                this.attributes["data-for"].value = "#o-" + index;
            });

            $.each($(".option-correct"), function (index, item) {
                this.attributes["name"].value = "Options[" + index + "].IsCorrect";
            });
        },

        addNewOption: function () {
            var template = this.getOptionTemplate();
            this.optionListContainer.append(template);
            this.resetIndexParamName();
            this.setEventRemoveOption();
        }
    };

    questionOctopus = {
        init: function () {
            questionView.init();
        }
    };

    questionOctopus.init();
});
