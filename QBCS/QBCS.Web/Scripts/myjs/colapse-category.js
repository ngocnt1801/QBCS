$(function () {
    categoryView = {
        init: function () {
            this.categoryItem = $('.list-group-item');
            this.categoryItem.on('click', function () {
                $('.fa', this)
                    .toggleClass('fa-plus-circle')
                    .toggleClass('fa-minus-circle');

                $('.list-group-item').removeClass('active');
                this.className += ' active';
                categoryOctopus.loadQuestion(this.attributes["data-link"].value);

            });
            this.questionListContainter = $('#list-question');
            
        },
        setOnClickDisableBtn: function () {
            this.toggleDisableBtn = $(".toggleDisable");
            this.toggleDisableBtn.off('click');
            this.toggleDisableBtn.on('click', function () {
                categoryOctopus.toggleDisable(this.attributes["data-url"].value, $('#' + this.attributes["data-id"].value));
            })
        }
    };

    categoryOctopus = {
        init: function () {
            categoryView.init();
            this.loadQuestion("/QBCS.Web/Question/GetQuestions?courseId=" + categoryView.questionListContainter[0].attributes["data-id"].value)
        },
        loadQuestion: function (url) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (response) {
                    categoryView.questionListContainter.html(response);
                    $("#dataTable").dataTable({
                        "ordering": false,
                        "columnDefs": [
                            { "targets": 0, "width": "5%"},
                            { "targets": 1, "width": "80%"},
                            { "targets": 2, "width": "15%", 'className': 'text-center'}
                        ]
                    });
                    categoryView.setOnClickDisableBtn();
                }
            });
        },
        toggleDisable: function (url, item) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function () {
                    item.remove();
                }
            });
        }
    }

    categoryOctopus.init();

});