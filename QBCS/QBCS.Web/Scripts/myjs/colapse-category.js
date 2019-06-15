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
            this.moveBtn = $("#move-btn");
            this.moveBtn.on('click', function () {
                categoryView.addCheckbox();
            })

            this.moveBtnGroup = $('#move-btn-group');
        },
        setOnClickDisableBtn: function () {
            this.toggleDisableBtn = $(".toggleDisable");
            this.toggleDisableBtn.off('click');
            this.toggleDisableBtn.on('click', function () {
                categoryOctopus.toggleDisable(this.attributes["data-url"].value, $('#' + this.attributes["data-id"].value));
            })
        },
        addCheckbox: function () {
            this.listCheckbox = $(".checkbox");
            this.listCheckbox.removeClass('hidden');
            this.addMoveButtonGroup();
        },
        addMoveButtonGroup: function(){
            this.moveBtnGroup.empty();
            this.moveBtnGroup.append('<div class="col mb-2">'
            + '<button class="btn btn-success float-right" id="move-btn-submit">Move</button> '
            + '<button class="btn btn-danger float-right mr-1" id="cancel-move">Cancel</button>' 
            + '</div>')

            this.cancelMove = $('#cancel-move');
            this.cancelMove.on('click', function(){
                categoryView.removeButtonGroup();
            })
        },
        removeButtonGroup: function(){
            this.moveBtnGroup.empty();
            this.listCheckbox.addClass('hidden');
            this.moveBtnGroup.append('<div class="col mb-2"> <button class="btn btn-info float-right" id="move-btn">Move Questions</button> </div>');
            this.moveBtn = $("#move-btn");
            this.moveBtn.on('click', function () {
                categoryView.addCheckbox();
            })
        },
        toggleAll: function(){
            var selectAllChk = $('#select-all')[0];
            $.each($('.checkbox'), function(index, item){
                item.checked = selectAllChk.checked;
            });
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
                            { "targets": 1, "width": "75%"},
                            { "targets": 2, "width": "15%", 'className': 'text-center'},
                            { "targets": 3, "width": "5%", 'className': 'text-center'},
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