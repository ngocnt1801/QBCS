$(function () {

    categoryViewExam = {
        init: function () {
            this.categoryItem = $(".show-category-exam .list-group-item");
            this.categoryItem.on("click", function () {
                $(".show-category-exam .fa", this)
                    .toggleClass("fa-plus-circle")
                    .toggleClass("fa-minus-circle");

                $(".show-category-exam .list-group-item").removeClass("active");
                this.className += " active";
                $("spinner").show();
                categoryOctopusExam.loadQuestion(this.attributes["data-link"].value);                
            });
            this.questionListContainter = $("#list-question-exam");
        }
        
    };

    categoryOctopusExam = {
        init: function () {
            categoryViewExam.init();
            if (categoryViewExam.questionListContainter[0] != null) {
                this.loadQuestion(
                    "/Examination/GetQuestions?courseId=" +
                    categoryViewExam.questionListContainter[0].attributes["data-id"].value
                );
            }
        },        
        loadQuestion: function (url) {
            $('#spinner').css("display", "block");
            $('#spinner').css("z-index", "1060");
            $('#pleaseWaitDialog').modal();
            $.ajax({
                url: url,
                type: "GET",
                success: function (response) {
                    categoryViewExam.questionListContainter.html(response);

                    var tableExam = $('#listQuestionGenrate').DataTable({
                        columns: [
                            null,
                            null,
                            {
                                "render": function (data, type, row) {
                                    if (data.indexOf("[html]") >= 0) {
                                        data = data.split("&lt;cbr&gt;").join("<br/>");
                                        data = data.split("&lt;br&gt;").join("<br/>");
                                        data = data.split("&lt;p&gt;").join("");
                                        data = data.split("&lt;/p&gt;").join("");
                                        data = data.split("&lt;b&gt;").join("");
                                        data = data.split("&lt;/b&gt;").join("");
                                        data = data.split("&lt;span&gt;").join("");
                                        data = data.split("&lt;/span&gt;").join("");
                                        data = data.split("&lt;/span&gt;").join("");
                                        data = data.split("[html]").join("");
                                    }
                                    return data
                                }
                            },
                            null
                        ],
                        columnDefs: [
                            {
                                type: 'formatted-num',
                                targets: 2
                            },
                            {
                                type: 'formatted-level',
                                targets: 3
                            },
                            {
                                'targets': [0, 1, 4],
                                'orderable': false,
                            }
                        ]
                    });                   
                    setTimeout(function () {
                        $('#spinner').css("display", "none");
                        $('#pleaseWaitDialog').modal('hide');
                    }, 500);

                }
            });
        }
    };

    categoryOctopusExam.init();
});
