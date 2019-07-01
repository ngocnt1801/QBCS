$(function () {
    questionView = {
        init: function () {
            this.setEventRemoveOption();
            this.optionListContainer = $("#option-list-container");

            $(".add-option").on("click", function () {
                questionView.addNewOption();
            });
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
