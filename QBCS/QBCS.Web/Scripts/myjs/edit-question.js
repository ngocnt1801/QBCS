$(function() {
  questionView = {
    init: function() {
      this.setEventRemoveOption();
      this.optionListContainer = $("#option-list-container");

      $(".add-option").on("click", function() {
          questionView.addNewOption();
      });
    },

    getOptionTemplate: function() {
      return (
        '<div class="form-inline mb-2">' +
        '<input type="text" class="form-control col-lg-10 option-content" value="" name="">' +
        '<div class="col-lg-1">' +
        '<label class="switch float-left">' +
        '<input type="checkbox" class="success option-correct" name="" value="true">' +
        '<span class="slider round"></span>' +
        "</label>" +
        "</div>" +
        '<div class="col-1">' +
        '<i class="fa fa-times-circle text-danger delete-option"></i>' +
        "</div>" +
        "</div>"
      );
    },

    setEventRemoveOption: function() {
      $(".delete-option").off("click");
      $(".delete-option").on("click", function() {
        $(this)
          .parent()
          .parent()
          .empty();
        questionView.resetIndexParamName();        
      });
    },

    resetIndexParamName: function() {
      var count = questionOctopus.getOptionCount();
      $.each($(".option-content"), function(index, item) {
        this.attributes["name"].value = "Options[" + index + "].OptionContent";
      });

      $.each($(".option-correct"), function(index, item) {
        this.attributes["name"].value = "Options[" + index + "].IsCorrect";
      });
    },

    addNewOption: function() {
      var template = this.getOptionTemplate();
      this.optionListContainer.append(template);
      this.resetIndexParamName();
      this.setEventRemoveOption();
    }
  };

  questionOctopus = {
    init: function() {
      questionView.init();
    }
  };

  questionOctopus.init();
});
