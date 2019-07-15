$(document).ready(function () {    
    // Toolbar extra buttons
    var btnFinish = $('<button></button>').text('Finish')
        .addClass('btn btn-info')
        .attr('disabled', 'disabled')
        .attr('type', 'submit')
        .attr('id', 'btnFinish')
        .hide();
    //.on('click', function () {

    //    var link = "/QBCS.Web/Question/ViewGeneratedExamination";
    //    $.ajax({
    //        type: "GET",
    //        url: link,
    //        success: function (data) {
    //            window.location.href = data.redirecturl;
    //        },
    //        error: function (httpRequest, textStatus, errorThrown) {
    //            alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
    //        }
    //    });
    //});
    var btnCancel = $('<button></button>').text('Cancel')
        .addClass('btn btn-info')
        .on('click', function () { $('#smartwizard').smartWizard("reset"); });

    // Smart Wizard
    $('#smartwizard').smartWizard({
        selected: 0,
        theme: 'arrows',
        transitionEffect: 'fade',
        toolbarSettings: {
            toolbarPosition: 'bottom',
            toolbarExtraButtons: [btnFinish, btnCancel]
        }
    });
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection) {
        if (stepNumber === 3) {
            $('#btnFinish').removeAttr('disabled');
            $('#btnFinish').show();
        }
    });

    $('#check-all').on('change', function (event) {
        event.preventDefault();
        if (this.checked) {
            $('.checkbox').each(function () {
                this.checked = true;
            });
        } else {
            $('.checkbox').each(function () {
                this.checked = false;
            });
        }
    });

    $('.checkbox').on('change', function () {
        if ($('.checkbox:checked').length === $('.checkbox').length) {
            $('#check-all').prop('checked', true);
        } else {
            $('#check-all').prop('checked', false);
        }
    });
    $("#easy").focusout(function () {
        var easyPercent = $("#easy").val();
        var hardAndNormal = 100 - easyPercent;
        var normalPercent = (60 * hardAndNormal) / 100;
        var hardPercent = 100 - easyPercent - normalPercent;
        $("#hard").val(hardPercent);
        $("#normal").val(normalPercent);
    });
    $("#normal").focusout(function () {
        var easyPercent = $("#easy").val();
        var normalPercent = $("#normal").val();
        var hardPercent = 100 - easyPercent - normalPercent;
        $("#hard").val(hardPercent);
    });
    $('.btnExport').on('click', function (e) {
        var counter = $(this).data("value");
        var examinationId = $("input[name='examinationId-" + counter + "']").val();
        var examGroup = $("input[name='examGroup-" + counter + "']").val();
        var fileExtension = $("#fileExtension-" + counter).find(":selected").text();
        var exportAll = $("#exportAll-" + counter).prop('checked');
        if (exportAll === true) {
            window.location = "/ExaminationAPI/exportAll?examGroup=" + examGroup + "&fileExtension=" + fileExtension;
        } else {
            window.location = "/ExaminationAPI/export?examinationId=" + examinationId + "&fileExtension=" + fileExtension;
        }

    });
    $('#exportDetailExamination').on('submit',function(e){
        e.preventDefault();
        var examinationId = $("input[name='examinationId']").val();
        var fileExtension = $("#fileExtension").find(":selected").text();
        window.location = "/ExaminationAPI/export?examinationId=" + examinationId + "&fileExtension=" + fileExtension;

    });    
    $('.delete-question').on('click', function (e) {
        e.preventDefault();
        var questionId = $(this).data('question-id');
        bootbox.dialog({
            message: "Are you sure you want to Delete Question and replace by another question?",
            title: "<i class='fas fa-trash-alt'></i> Delete Question!",
            buttons: {
                success: {
                    label: "No",
                    className: "btn-info",
                    callback: function () {
                        $('.bootbox').modal('hide');
                    }
                },
                danger: {
                    label: "Delete",
                    className: "btn-primary",
                    callback: function () {
                        window.location = "/Examination/DeleteQuestionInExam?questionId=" + questionId;
                        //$.ajax({
                        //    type: 'POST',
                        //    url: '/Examination/DeleteQuestionInExam',
                        //    data: 'questionId=' + questionId

                        //})
                    }
                }
            }
        });
    });

    $(".tab-slider--body").hide();
    $(".tab-slider--body:first").show();

    //extend sort by number in string
    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "formatted-num-pre": function (a) {
            a = (a === "-" || a === "") ? 0 : a.replace(/[^\d\-\.]/g, "");
            return parseFloat(a);
        },

        "formatted-num-asc": function (a, b) {
            return a - b;
        },

        "formatted-num-desc": function (a, b) {
            return b - a;
        }
    });
    //extend sort by level
    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "formatted-level-pre": function (a) {
            if (a.toLowerCase() === "easy") {
                a = 1;
            } else if (a.toLowerCase() === "medium") {
                a = 2;
            } else if (a.toLowerCase() === "hard") {
                a = 3;
            }
            //a = (a === "-" || a === "") ? 0 : a.replace(/[^\d\-\.]/g, "");
            return parseFloat(a);
        },

        "formatted-level-asc": function (a, b) {
            return a - b;
        },

        "formatted-level-desc": function (a, b) {
            return b - a;
        }
    });
    //set up datatable
    var tableExam = $('#dataTableExam').DataTable({
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
            null,
            null
        ],
        columnDefs: [
            {
                type: 'formatted-num',
                targets: 4
            },
            {
                type: 'formatted-level',
                targets: 3
            },
            {
                'targets': [0, 1, 2],
                'orderable': false,
            }
        ]
    });
    var tableExam1 = $('#dataTableExam-1').DataTable({
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
            null,
            null
        ],
        columnDefs: [
            {
                type: 'formatted-num',
                targets: 4
            },
            {
                type: 'formatted-level',
                targets: 3
            },
            {
                'targets': [0, 1, 2, 5],
                'orderable': false,
            }
        ]
    });
    var tableExam2 = $('#dataTableExam-2').DataTable({
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
            null,
            null
        ],
        columnDefs: [
            {
                type: 'formatted-num',
                targets: 4
            },
            {
                type: 'formatted-level',
                targets: 3
            },
            {
                'targets': [0, 1, 2, 5],
                'orderable': false,
            }
        ]
    });
    var tableExam3 = $('#dataTableExam-3').DataTable({
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
            null,
            null
        ],
        columnDefs: [
            {
                type: 'formatted-num',
                targets: 4
            },
            {
                type: 'formatted-level',
                targets: 3
            },
            {
                'targets': [0, 1, 2, 5],
                'orderable': false,
            }
        ]
    });
    var tableExam4 = $('#dataTableExam-4').DataTable({
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
            null,
            null
        ],
        columnDefs: [
            {
                type: 'formatted-num',
                targets: 4
            },
            {
                type: 'formatted-level',
                targets: 3
            },
            {
                'targets': [0, 1, 2, 5],
                'orderable': false,
            }
        ]
    });
    var tableExam5 = $('#dataTableExam-5').DataTable({
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
            null,
            null
        ],
        columnDefs: [
            {
                type: 'formatted-num',
                targets: 4
            },
            {
                type: 'formatted-level',
                targets: 3
            },
            {
                'targets': [0, 1, 2, 5],
                'orderable': false,
            }
        ]
    });
    var tableHistoryExam = $("#datatable-history-exam").DataTable({
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
            null,
            null,
            null,
            null,
            null
        ],
        columnDefs: [
            {
                'targets': [0, 1, 2, 7],
                'orderable': false,
            },
            {
                'targets': [0, 3, 4, 5, 6],
                'width': "2%"
            },
            {
                'targets': [1, 7],
                'width': "11%"
            },
        ]
    });

    tableExam.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
    tableExam1.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
    tableExam2.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
    tableExam3.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
    tableExam4.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
    tableExam5.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
    $('#btnSaveManually').attr('disabled', 'disabled');
    var tableQuestionExamManually = $('#listQuestionExam').DataTable({
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
            null,
            null,
            null
        ],
        columnDefs: [
            {
                type: 'formatted-num',
                targets: 3
            },
            {
                type: 'formatted-level',
                targets: 4
            },
            {
                'targets': [0, 1, 2, 5],
                'orderable': false,
            }
        ]
    });
});
$(".tab-slider--nav li").click(function () {
    $(".tab-slider--body").hide();
    var activeTab = $(this).attr("rel");
    $("#" + activeTab).fadeIn();
    if ($(this).attr("rel") === "tab2") {
        $('.tab-slider--tabs').addClass('slide');
        $('#flagPercent').attr('value', 'grade');
    } else {
        $('.tab-slider--tabs').removeClass('slide');
        $('#flagPercent').attr('value', 'percent');
    }
    $(".tab-slider--nav li").removeClass("active");
    $(this).addClass("active");
});
$(document).on('click', '.add-question', function (e) {
    e.preventDefault();
    var tableExamManually = $('#listQuestionGenrate').dataTable();
    var indexRow = $(this).data('row-id');
    var data = tableExamManually.api().row(indexRow).data();
    var questionCode = data[0];
    var questionContent = data[1];
    var questionLO = data[2];
    var questionLevel = data[3];
    var tableQuestionAdded = $('#listQuestionExam').dataTable();
    if (!checkQuestionCode(questionCode)) {
        tableQuestionAdded.fnAddData([
            null,
            questionCode,
            questionContent,
            questionLO,
            questionLevel,
            '<button class="delete-question btn-danger btn delete-question-exam"><i class="fas fa-trash-alt"></i></button>'
        ]);
        $(".form-group-exam").append("<input type='hidden' class='input-question-code' name='questionCode' value='" + questionCode + "'/>");
        $('#btnSaveManually').removeAttr('disabled');
        countNumberOfQuestion();
        toastr.options.timeOut = 500;
        toastr.success("Question " + questionCode + " has been added to exam!");
    } else {
        toastr.options.timeOut = 500;
        toastr.warning("Question " + questionCode + " already exists in the exam");
    }
});
$(document).on('click', '.delete-question-exam', function (e) {
    var dtRow = $(this).parents('tr');
    var table = $('#listQuestionExam').DataTable();
    var data = table.row(dtRow[0].rowIndex - 1).data();
    var questionCode = data[1];
    $(".form-group-exam input[value=" + questionCode + "]").remove();
    table.row(dtRow[0].rowIndex - 1).remove().draw(false);
    var dataTable = $('#listQuestionExam').dataTable().api().rows().data();
    if (dataTable.length === 0) {
        $('#btnSaveManually').attr('disabled', 'disabled');
    }
    countNumberOfQuestion();
});
function checkQuestionCode(questionCode) {
    var tableQuestionAdded = $('#listQuestionExam').dataTable();
    var data = tableQuestionAdded.api().rows().data();
    for (var i = 0; i < data.length; i++) {
        if (data[i][1] === questionCode) {
            return true;
        }
    }
    return false;
};
function countNumberOfQuestion() {
    var questionEasy = 0;
    var questionMedium = 0;
    var questionHard = 0;
    var tableQuestionAdded = $('#listQuestionExam').dataTable();
    var data = tableQuestionAdded.api().rows().data();
    for (var i = 0; i < data.length; i++) {
        if (data[i][4] === "Easy") {
            questionEasy++;
        } else if (data[i][4] === "Medium") {
            questionMedium++;
        } else if (data[i][4] === "Hard") {
            questionHard++;
        }
    }
    $('#numberEasyQuestion').html("Number of easy question : " + questionEasy);
    $('#numberMediumQuestion').html("Number of medium question : " + questionMedium);
    $('#numberHardQuestion').html("Number of hard question : " + questionHard);
};




