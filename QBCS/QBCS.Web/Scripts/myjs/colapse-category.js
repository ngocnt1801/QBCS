$(function() {
    categoryView = {
      init: function(){
        this.categoryItem = $('.list-group-item');
        this.categoryItem.on('click', function() {
          $('.fa', this)
              .toggleClass('fa-plus-circle')
                .toggleClass('fa-minus-circle');
      
            $('.list-group-item').removeClass('active');
            this.className += ' active';
            categoryOctopus.loadQuestion(this.attributes["data-link"].value);

        });

        this.questionListContainter = $('#list-question');
      }
    };

    categoryOctopus = {
      init: function(){
        categoryView.init();
        this.loadQuestion("/QBCS.Web/Question/GetQuestions?courseId=" + categoryView.questionListContainter[0].attributes["data-id"].value)
      },
      loadQuestion: function(url){
        $.ajax({
          url: url,
          type: 'GET',
          success: function(response){
            categoryView.questionListContainter.html(response);
            $("#dataTable").dataTable({
              ordering: false
            });
          }
        });
      }
    }

    categoryOctopus.init();

});