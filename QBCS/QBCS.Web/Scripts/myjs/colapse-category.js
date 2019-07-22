$(function () {
    categoryModel = {
        listQuestionSelected: [],
        categorySelected: {
            categoryId: 0,
            learningoutcomeId: 0,
            levelId: 0
        },
        isMoveQuestion: false
    };

    categoryView = {
        init: function () {
            this.categoryItem = $(".show-category .list-group-item");
            this.categoryItem.on("click", function () {
                //$(".show-category .fa", this)
                //    .toggleClass("fa-plus-circle")
                //    .toggleClass("fa-minus-circle");

                if ($($(this).children("i").get(0)).hasClass("fa-plus-circle")) {
                    $($(this).children("i").get(0)).removeClass("fa-plus-circle");
                    $($(this).children("i").get(0)).addClass("fa-minus-circle");
                } else if ($($(this).children("i").get(0)).hasClass("fa-minus-circle")) {
                    $($(this).children("i").get(0)).removeClass("fa-minus-circle");
                    $($(this).children("i").get(0)).addClass("fa-plus-circle");
                }

                $(".show-category .list-group-item").removeClass("active");
                this.className += " active";
                categoryOctopus.loadQuestion(this.attributes["data-link"].value);
                categoryView.removeButtonGroup();
            });

            this.modelCategoryItem = $(".modal-category .list-group-item");
            this.modelCategoryItem.on("click", function () {

                if ($($(this).children("i").get(0)).hasClass("fa-plus-circle")) {
                    $($(this).children("i").get(0)).removeClass("fa-plus-circle");
                    $($(this).children("i").get(0)).addClass("fa-minus-circle");
                } else if ($($(this).children("i").get(0)).hasClass("fa-minus-circle")) {
                    $($(this).children("i").get(0)).removeClass("fa-minus-circle");
                    $($(this).children("i").get(0)).addClass("fa-plus-circle");
                }

                $(".modal-category .list-group-item").removeClass("active");
                this.className += " active";
                categoryOctopus.selectCategory(
                    this.attributes["data-category"].value,
                    this.attributes["data-lo"].value,
                    this.attributes["data-level"].value
                );
            });

            this.questionListContainter = $("#list-question");
            this.moveBtn = $("#move-btn");
            this.moveBtn.on("click", function () {
                categoryView.addCheckbox();
                categoryOctopus.setMoveQuestion(true);

            });

            this.moveBtnGroup = $("#move-btn-group");

            this.moveQuestionSubmitBtn = $("#move-question-btn");
            this.moveQuestionSubmitBtn.on("click", function () {
                categoryOctopus.updateCategory();
            });

            this.initTable();



        },
        downCount: function (spanCount) {
            var currentCount = spanCount.attr("data-count");
            currentCount = currentCount - 1;

            spanCount.attr("data-count", currentCount);
            spanCount.html("(" + currentCount + ")");
        },
        setOnClickDisableBtn: function () {
            this.toggleDisableBtn = $(".toggleDisable");
            this.toggleDisableBtn.off("click");
            this.toggleDisableBtn.on("click", function () {
                categoryOctopus.toggleDisable(
                    this.attributes["data-url"].value,
                    //$("#" + this.attributes["data-id"].value)
                    $(this)
                );
            });
        },
        resetCount: function (disableBtn) {
            this.downCount($(disableBtn.attr("data-category")));
            this.downCount($(disableBtn.attr("data-lo")));
            this.downCount($(disableBtn.attr("data-lv")));
            disableBtn.parent().parent().remove();
        },
        addCheckbox: function () {
            this.listCheckbox = $(".checkbox");
            this.listCheckbox.removeClass("hidden");
            this.addMoveButtonGroup();
        },
        addMoveButtonGroup: function () {
            this.moveBtnGroup.empty();
            this.moveBtnGroup.append(
                '<div class="col mb-2">' +
                '<button class="btn btn-primary float-right" id="move-btn">Move</button> ' +
                '<button class="btn btn-danger float-right mr-1" id="cancel-move">Cancel</button>' +
                "</div>"
            );

            this.cancelMove = $("#cancel-move");
            this.cancelMove.on("click", function () {
                categoryView.removeButtonGroup();
                categoryOctopus.setMoveQuestion(false)
            });

            this.moveBtn = $("#move-btn");
            this.moveBtn.on("click", function () {
                categoryView.modal = $("#modal");
                categoryView.modal.modal("show");
                //categoryView.listChecked = $(".checkbox[id!='select-all']:checked");
                //categoryOctopus.addListSelectedQuestion();
                categoryView.renderListQuestionSelected();
                categoryView.modal.off("hidden.bs.modal");
                categoryView.modal.on("hidden.bs.modal", function () {
                    categoryView.resetCheckList();
                });
            });
        },
        removeButtonGroup: function () {
            this.moveBtnGroup.empty();
            this.listCheckbox = $(".checkbox");
            this.listCheckbox.addClass("hidden");
            this.moveBtnGroup.append(
                '<div class="col mb-2"> <button class="btn btn-primary float-right" id="move-btn">Move Questions</button> </div>'
            );
            this.moveBtn = $("#move-btn");
            this.moveBtn.on("click", function () {
                categoryView.addCheckbox();
                categoryOctopus.setMoveQuestion(true);
            });
        },
        toggleAll: function () {
            var selectAllChk = $("#select-all")[0];
            $(".checkbox[id!='select-all']").trigger('click');
            //$.each($(".checkbox"), function (index, item) {
            //    item.checked = selectAllChk.checked;
            //});
        },
        renderListQuestionSelected: function () {
            var questionCotainer = $("#selected-questions");
            questionCotainer.empty();
            var listQuestions = categoryOctopus.getListQuestionSelected();
            $.each(listQuestions, function (index, item) {
                questionCotainer.append(
                    "<tr>" +
                    "<td>" +
                    (index + 1) +
                    "</td>" +
                    "<td>" +
                    item.code +
                    "</td>" +
                    "<td><i class='fa fa-times-circle text-danger remove-selected' data-index='" +
                    index +
                    "'></i></td>" +
                    "</tr>"
                );
            });

            $(".remove-selected").on("click", function () {
                $(this)
                    .parent()
                    .parent()
                    .remove();
                categoryOctopus.removeQuestionSelected(
                    this.attributes["data-index"].value
                );
            });

            this.renderCountQuestionSelected();
        },
        renderCountQuestionSelected: function () {
            var listQuestions = categoryOctopus.getListQuestionSelected();
            $("#selected-count").html(listQuestions.length + " questions");
        },
        resetCheckList: function () {
            var listQuestionSelected = categoryOctopus.getListQuestionSelected();

            $.each($(".checkbox"), function (index, item) {
                item.checked = false;
            });

            $.each(listQuestionSelected, function (index, item) {
                $("#" + item.id + " .checkbox")[0].checked = true;
            });
        },
        initTable: function () {
            $("#list-course-table").dataTable({
                columns: [
                    null,
                    null,
                    null
                ]
            });
        },
        setOnClickCkb: function () {
            $(".checkbox[id!='select-all']").off('change');
            $(".checkbox[id!='select-all']").on('change', function () {

                if (this.checked) {
                    categoryOctopus.addSelectedQuestion(this.attributes["data-code"].value, this.attributes["data-id"].value);
                } else {
                    categoryOctopus.deleteSelectedQuestion(this.attributes["data-code"].value, this.attributes["data-id"].value);
                }

            });
        }
    };

    categoryOctopus = {
        init: function () {
            categoryView.init();
            if (categoryView.questionListContainter[0] != null) {
                this.loadQuestion(
                    "/Question/GetQuestionsDatatable?courseId=" +
                    categoryView.questionListContainter[0].attributes["data-id"].value
                );
            }
        },
        getListQuestionSelected: function () {
            return categoryModel.listQuestionSelected;
        },
        loadQuestion: function (url) {
            $('#spinner').css("display", "block");
            $('#spinner').css("z-index", "1060");
            $('#pleaseWaitDialog').modal();
            if (url.includes("GetCourseDetailStat")) {
                $.ajax({
                    url: url,
                    type: "GET",
                    success: function (response) {
                        categoryView.questionListContainter.html(response);
                        {
                            //var table = $("#dataTable").dataTable({
                            //      ordering: false,
                            //      columnDefs: [
                            //          { targets: 0, width: "5%" },
                            //          { targets: 1, width: "75%" },
                            //          { targets: 2, width: "15%", className: "text-center" },
                            //          { targets: 3, width: "5%", className: "text-center" }
                            //      ],
                            //      columns: [
                            //          null,
                            //          {
                            //              width: "75%",
                            //              render: function (data, type, row) {
                            //                  if (data.indexOf("[html]") >= 0) {
                            //                      data = data.split("&lt;cbr&gt;").join("<br/>");
                            //                      data = data.split("&lt;br&gt;").join("<br/>");
                            //                      data = data.split("&lt;br&gt;").join("<br/>");
                            //                      data = data.split("&lt;p&gt;").join("");
                            //                      data = data.split("&lt;/p&gt;").join("");
                            //                      data = data.split("&lt;span&gt;").join("");
                            //                      data = data.split("&lt;/span&gt;").join("");
                            //                      data = data.split("[html]").join("");
                            //                  }

                            //                  return data;
                            //              }
                            //          },
                            //          null,
                            //          null,
                            //      ]
                            //  });
                            //  $('#dataTable').on('draw.dt', function () {
                            //      if (categoryModel.isMoveQuestion) {
                            //          $.each($(".checkbox"), function (index, item) {
                            //              $(item).removeClass("hidden");
                            //          });
                            //      } else {
                            //          $.each($(".checkbox"), function (index, item) {
                            //              $(item).addClass( "hidden");
                            //          });
                            //      }
                            //      categoryView.setOnClickCkb();
                            //  });
                        }
                        categoryView.setOnClickCkb();
                        categoryView.setOnClickDisableBtn();
                        table.on('page.dt', function () {
                            $('html, body').animate({
                                scrollTop: $(".dataTables_wrapper").offset().top
                            }, 'slow');
                        });
                        setTimeout(function () {
                            $('#spinner').css("display", "none");
                            $('#pleaseWaitDialog').modal('hide');
                        }, 500);

                    }
                });
            } else {
                $('#dataTable').DataTable().destroy();
                $('#dataTable').remove();
                var setTable = '<table class="table table-bordered table-hover text-custom" id="dataTable" width="100%" cellspacing="0">' +
                    '<thead>' +
                    '<tr>' +
                    '<th>No.</th>' +
                    '<th>Question</th>' +
                    '<th>Action</th>' +
                    '<th>' +
                    '<input type="checkbox" class="checkbox hidden" id="select-all" value="true" onclick="categoryView.toggleAll()" />' +
                    '</th>' +
                    '</tr>' +
                    '</thead>' +
                    '<tbody>' +
                    '</tbody>' +
                    '</table>';
                $('#divTable').append(setTable);



                var content = [];
                var countTable = 0;

                var dataTable = $('#dataTable').DataTable({
                    paging: true,
                    ordering: false,
                    filter: true,
                    destroy: true,
                    searching: true,
                    serverSide: true,
                    lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
                    Processing: true,
                    ajax:
                    {
                        async: false,
                        url: url,
                        type: "GET",
                        dataType: "json",
                        contentType: "application/json; charset=utf-8"
                    },
                    columns: [
                        {
                            data: function (data, type, row, meta) {
                                return meta.row + meta.settings._iDisplayStart + 1;
                            }
                        },
                        {
                            data: "QuestionViewModel",
                            render: function (data, type, row, meta) {
                                var questionObj = {};
                                var code = '<p>Question Code: ' + row.Code + '</p>';
                                var questionContent = '<p id="qcontent_' + countTable + '"></p>';
                                questionObj['QuestionContent'] = row.QuestionContent;
                                var image = row.Image;
                                if (image != null && image != "") {
                                    image = '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + image + '" /></p>';
                                } else {
                                    image = "";
                                }
                                var options = [];
                                var i = 0;
                                for (i = 0; i < row.Options.length; i++) {
                                    var option = {};
                                    option["content"] = changeHtml(row.Options[i].OptionContent);
                                    option["correct"] = row.Options[i].IsCorrect;
                                    options.push(option);
                                }
                                questionObj["Options"] = options;
                                content.push(questionObj);
                                var result = code + questionContent + image;
                                for (i = 0; i < options.length; i++) {
                                    result = result + '<div id="ocontent_' + countTable + '_' + i + '" class="container-fluid"></div>';
                                }
                                countTable++;
                                return result;
                            }
                        },
                        {
                            data: "Id",
                            render: function (data) {
                                var importId = $('#importId').val();
                                var edit = '<a href="/Import/GetQuestionTemp/' + data + '" class="btn btn-primary mb-2 col-md-12">Edit</a>';
                                var deleteQ = '<a href="/Import/Delete?questionId=' + data + '&importId=' + importId + '" class="btn btn-danger col-md-12">Delete</a>';
                                var result = edit + deleteQ;
                                return result;
                            }
                        },
                        {
                            render: function (data, type, row) {
                                var id = row.Id;
                                var code = row.Code;
                                var result = '<input type="checkbox" class="checkbox hidden" data-id="' + id + '" data-code="' + code + '" />';
                                return result;
                            }
                        }
                    ],
                    columnDefs: [
                        { targets: 0, width: "2%" },
                        { targets: 1, width: "98%" }
                    ],
                    fnDrawCallback: function () {
                        countTable = 0;
                        for (var q = 0; q < content.length; q++) {
                            var jq = "#qcontent_" + q;
                            var changeContent = content[q]["QuestionContent"];
                            var breakContent = [];
                            var isHtml = false;
                            if (changeContent.indexOf("[html]") >= 0) {
                                isHtml = true;
                            }
                            if (isHtml) {
                                changeContent = changeContent.split("&lt;p&gt;").join("");
                                changeContent = changeContent.split("&lt;/p&gt;").join("");
                                changeContent = changeContent.split("&lt;span&gt;").join("");
                                changeContent = changeContent.split("&lt;/span&gt;").join("");
                                changeContent = changeContent.split("[html]").join("");
                                breakContent = changeContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
                                breakContent = breakContent.split("·");
                            } else {
                                breakContent.push(changeContent);
                            }
                            for (var w = 0; w < breakContent.length; w++) {
                                $(jq).append('<p id="qcontent_' + q + '_' + w + '"></p>');
                                var jqw = "#qcontent_" + q + '_' + w;
                                $(jqw).text(breakContent[w]);
                            }
                            breakContent = [];
                            for (var o = 0; o < content[q]["Options"].length; o++) {
                                var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                                var jo = "#ocontent_" + q + "_" + o;
                                var optionContent = content[q]["Options"][o]["content"];
                                var optionCorrect = content[q]["Options"][o]["correct"];
                                if (isHtml) {
                                    optionContent = optionContent.split("&lt;p&gt;").join("");
                                    optionContent = optionContent.split("&lt;/p&gt;").join("");
                                    optionContent = optionContent.split("&lt;span&gt;").join("");
                                    optionContent = optionContent.split("&lt;/span&gt;").join("");
                                    optionContent = optionContent.split("[html]").join("");
                                    breakContent = optionContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
                                    breakContent = breakContent.split("·");
                                } else {
                                    breakContent.push(optionContent);
                                }
                                for (var b = 0; b < breakContent.length; b++) {
                                    $(jo).append('<p id="ocontent_' + q + '_' + o + '_' + b + '"></p>');
                                    var jow = "#ocontent_" + q + '_' + o + '_' + b;
                                    if (b == 0) {
                                        $(jow).text(letters[o] + '. ' + breakContent[b]);
                                    } else {
                                        $(jow).text(breakContent[b]);
                                    }
                                }
                                if (optionCorrect) {
                                    $(jo).addClass('text-right-answer');
                                }
                            }
                        }
                    }
                });

                $('#dataTable').on('draw.dt', function () {
                    if (categoryModel.isMoveQuestion) {
                        $.each($(".checkbox"), function (index, item) {
                            $(item).removeClass("hidden");
                        });
                    } else {
                        $.each($(".checkbox"), function (index, item) {
                            $(item).addClass("hidden");
                        });
                    }
                    categoryView.setOnClickCkb();
                });

                dataTable.on('page.dt', function () {
                    $('html, body').animate({
                        scrollTop: $(".dataTables_wrapper").offset().top
                    }, 'slow');
                });


                categoryView.setOnClickCkb();
                categoryView.setOnClickDisableBtn();

                setTimeout(function () {
                    $('#spinner').css("display", "none");
                    $('#pleaseWaitDialog').modal('hide');
                }, 500);

            }
            $.ajax({
                url: url,
                type: "GET",
                success: function (response) {
                    categoryView.questionListContainter.html(response);

                    var table = $("#dataTable").dataTable({
                        ordering: false,
                        columnDefs: [
                            { targets: 0, width: "5%" },
                            { targets: 1, width: "75%" },
                            { targets: 2, width: "15%", className: "text-center" },
                            { targets: 3, width: "5%", className: "text-center" }
                        ],
                        columns: [
                            null,
                            {
                                width: "75%",
                                render: function (data, type, row) {
                                    if (data.indexOf("[html]") >= 0) {
                                        data = data.split("&lt;cbr&gt;").join("<br/>");
                                        data = data.split("&lt;br&gt;").join("<br/>");
                                        data = data.split("&lt;br/&gt;").join("<br/>");
                                        data = data.split("&lt;p&gt;").join("");
                                        data = data.split("&lt;/p&gt;").join("");
                                        data = data.split("&lt;span&gt;").join("");
                                        data = data.split("&lt;/span&gt;").join("");

                                        
                                        data = data.split("&lt;b&gt;").join("");
                                        data = data.split("&lt;/b&gt;").join("");
                                        data = data.split("&lt;span&gt;").join("");
                                    
                                        data = data.split("&lt;u&gt;").join("");
                                        data = data.split("&lt;/u&gt;").join("");
                                        data = data.split("&lt;i&gt;").join("");
                                        data = data.split("&lt;/i&gt;").join("");
                                        data = data.split("&lt;sub&gt;").join("<sub>");
                                        data = data.split("&lt;/sub&gt;").join("</sub>");
                                        data = data.split("&lt;sup&gt;").join("<sup>");
                                        data = data.split("&lt;/sup&gt;").join("</sup>");
                                        data = data.split("[html]").join("");
                                    }

                                    return data;
                                }
                            },
                            null,
                            null,
                        ]
                    });
                    $('#dataTable').on('draw.dt', function () {
                        if (categoryModel.isMoveQuestion) {
                            $.each($(".checkbox"), function (index, item) {
                                $(item).removeClass("hidden");
                            });
                        } else {
                            $.each($(".checkbox"), function (index, item) {
                                $(item).addClass("hidden");
                            });
                        }
                        categoryView.setOnClickCkb();
                    });
                    categoryView.setOnClickCkb();
                    categoryView.setOnClickDisableBtn();
                    table.on('page.dt', function () {
                        $('html, body').animate({
                            scrollTop: $(".dataTables_wrapper").offset().top
                        }, 'slow');
                    });
                    $('#spinner').css("display", "none");
                    $('#pleaseWaitDialog').modal('hide');

                }
            });
        },
        toggleDisable: function (url, item) {
            $.ajax({
                url: url,
                type: "GET",
                success: function () {
                    categoryView.resetCount(item);
                }
            });
        },
        emptyListSelected: function () {
            categoryModel.listQuestionSelected = [];
        },
        addSelectedQuestion: function (code, id) {
            var index = categoryModel.listQuestionSelected.findIndex(l => l.id === id);
            if (index < 0) {
                categoryModel.listQuestionSelected.push({ code: code, id: id });
            }
        },
        deleteSelectedQuestion: function (code, id) {
            var index = categoryModel.listQuestionSelected.findIndex(l => l.id === id);
            removeQuestionSelected(index);
        },
        addListSelectedQuestion: function () {
            var listSelected = categoryView.listChecked;
            this.emptyListSelected();
            $.each(listSelected, function (index, item) {
                categoryOctopus.addSelectedQuestion(
                    item.attributes["data-code"].value,
                    item.attributes["data-id"].value
                );
            });

            categoryView.renderListQuestionSelected();
        },
        removeQuestionSelected: function (index) {
            categoryModel.listQuestionSelected.splice(index, 1);
            categoryView.renderCountQuestionSelected();
        },
        updateCategory: function () {
            var ids = [];
            $.each(categoryModel.listQuestionSelected, function (index, item) {
                ids.push(item.id);
            });

            $.ajax({
                url: "/Question/UpdateCategory",
                type: "POST",
                data: {
                    ids: ids,
                    categoryId: categoryModel.categorySelected.categoryId,
                    learningOutcomeId: categoryModel.categorySelected.learningoutcomeId,
                    levelId: categoryModel.categorySelected.levelId
                },
                success: function () {
                    document.location.reload();
                }
            });
        },
        selectCategory: function (categoryId, loId, levelId) {
            categoryModel.categorySelected.categoryId = categoryId;
            categoryModel.categorySelected.learningoutcomeId = loId;
            categoryModel.categorySelected.levelId = levelId;
        },
        isMoveQuestion: function () {
            return categoryModel.isMoveQuestion;
        },
        setMoveQuestion: function (value) {
            categoryModel.isMoveQuestion = value;
        }
    };

    categoryOctopus.init();
    search();
});
function search() {
    $("#datatableTest").dataTable().on('search.dt', function () {
        var dataString = $(this).attr('data');

        alert(dataString);
    });
}
//async function setDatatable(url) {

//    var content = [];
//    var countTable = 0;

//    var dataTable = $('#dataTable').DataTable({
//        paging: true,
//        ordering: false,
//        filter: true,
//        destroy: true,
//        searching: true,
//        serverSide: true,
//        lengthMenu: [[10, 20, 50, -1], [10, 20, 50, "All"]],
//        Processing: true,
//        ajax:
//        {
//            async: false,
//            url: url,
//            type: "GET",
//            dataType: "json",
//        },

//        columns: [
//            {
//                data: function (data, type, row, meta) {
//                    return meta.row + meta.settings._iDisplayStart + 1;
//                }
//            },
//            {
//                data: "QuestionViewModel",
//                render: function (data, type, row, meta) {
//                    var questionObj = {};
//                    var optionList = [];
//                    var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
//                    var code = '<p>Question Code: ' + row.Code + '</p>';
//                    var questionContent = '<p id="qcontent_' + countTable + '"></p>';
//                    questionObj['QuestionContent'] = row.QuestionContent;
//                    var image = row.Image;
//                    if (image != null && image != "") {
//                        image = '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + image + '" /></p>';
//                    } else {
//                        image = "";
//                    }
//                    var options = [];
//                    var i = 0;
//                    for (i = 0; i < row.Options.length; i++) {
//                        var option = {};
//                        option["content"] = changeHtml(row.Options[i].OptionContent);
//                        option["correct"] = row.Options[i].IsCorrect;
//                        options.push(option);
//                    }
//                    questionObj["Options"] = options;
//                    content.push(questionObj);
//                    var result = code + questionContent + image;
//                    for (i = 0; i < options.length; i++) {
//                        result = result + '<div id="ocontent_' + countTable + '_' + i + '" class="container-fluid"></div>';
//                    }
//                    countTable++;
//                    return result;
//                }
//            },
//            {
//                data: "Id",
//                render: function (data) {
//                    var importId = $('#importId').val();
//                    var edit = '<a href="/Import/GetQuestionTemp/' + data + '" class="btn btn-primary mb-2 col-md-12">Edit</a>';
//                    var deleteQ = '<a href="/Import/Delete?questionId=' + data + '&importId=' + importId + '" class="btn btn-danger col-md-12">Delete</a>';
//                    var result = edit + deleteQ;
//                    return result;
//                }
//            },
//            {
//                render: function (data, type, row) {
//                    var id = row.Id;
//                    var code = row.Code;
//                    var result = '<input type="checkbox" class="checkbox hidden" data-id="' + id + '" data-code="' + code + '" />';
//                    return result;
//                }
//            }
//        ],
//        columnDefs: [
//            { targets: 0, width: "2%" },
//            { targets: 1, width: "98%" }
//        ],
//        fnDrawCallback: function () {
//            countTable = 0;
//            for (var q = 0; q < content.length; q++) {
//                var jq = "#qcontent_" + q;
//                var changeContent = content[q]["QuestionContent"];
//                var breakContent = [];
//                var isHtml = false;
//                if (changeContent.indexOf("[html]") >= 0) {
//                    isHtml = true;
//                }
//                if (isHtml) {
//                    changeContent = changeContent.split("&lt;p&gt;").join("");
//                    changeContent = changeContent.split("&lt;/p&gt;").join("");
//                    changeContent = changeContent.split("&lt;span&gt;").join("");
//                    changeContent = changeContent.split("&lt;/span&gt;").join("");
//                    changeContent = changeContent.split("[html]").join("");
//                    breakContent = changeContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
//                    breakContent = breakContent.split("·");
//                } else {
//                    breakContent.push(changeContent);
//                }
//                for (var w = 0; w < breakContent.length; w++) {
//                    $(jq).append('<p id="qcontent_' + q + '_' + w + '"></p>');
//                    var jqw = "#qcontent_" + q + '_' + w;
//                    $(jqw).text(breakContent[w]);
//                }
//                breakContent = [];
//                for (var o = 0; o < content[q]["Options"].length; o++) {
//                    var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
//                    var jo = "#ocontent_" + q + "_" + o;
//                    var optionContent = content[q]["Options"][o]["content"];
//                    var optionCorrect = content[q]["Options"][o]["correct"];
//                    if (isHtml) {
//                        optionContent = optionContent.split("&lt;p&gt;").join("");
//                        optionContent = optionContent.split("&lt;/p&gt;").join("");
//                        optionContent = optionContent.split("&lt;span&gt;").join("");
//                        optionContent = optionContent.split("&lt;/span&gt;").join("");
//                        optionContent = optionContent.split("[html]").join("");
//                        breakContent = optionContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
//                        breakContent = breakContent.split("·");
//                    } else {
//                        breakContent.push(optionContent);
//                    }
//                    for (var b = 0; b < breakContent.length; b++) {
//                        $(jo).append('<p id="ocontent_' + q + '_' + o + '_' + b + '"></p>');
//                        var jow = "#ocontent_" + q + '_' + o + '_' + b;
//                        if (b == 0) {
//                            $(jow).text(letters[o] + '. ' + breakContent[b]);
//                        } else {
//                            $(jow).text(breakContent[b]);
//                        }
//                    }
//                    if (optionCorrect) {
//                        $(jo).addClass('text-right-answer');
//                    }
//                }
//            }
//        }
//    });

//    dataTable.on('page.dt', function () {
//        $('html, body').animate({
//            scrollTop: $(".dataTables_wrapper").offset().top
//        }, 'slow');
//    });
//}