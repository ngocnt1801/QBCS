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
        //if (stepNumber === 0 && stepDirection === 'forward') {
        //    var courseId = $("input[name=course]:checked").val();
        //    var string = "<div class='col-lg-8'><table class='table table-striped'><tbody>";
        //    $.ajax({
        //        type: "GET",
        //        url: "http://localhost/QBCS.Web/api/TopicAPI/topics",
        //        data: {
        //            CourseId : courseId
        //        },
        //        cache: false,
        //        dataType: "xml",
        //        success: function (xml) {
        //            $(xml).find('TopicViewModel').each(function () {
        //                var name = $(this).find("Name").text();
        //                var id = $(this).find("Id").text();
        //                var idValue = $(this).find("IdValue").text();
        //                string = string + "<tr class='d-flex'><td class='col-1'><div class='el-checkbox el-checkbox-green text-center form-group'><span class='margin-l'></span><input value='" + idValue + "' type='checkbox' class='checkbox' name='topic' id='checkbox" + id + "' /><label class='el-checkbox-style' for='checkbox" + id + "'></label></div></td><td class='col-11'><h5 style='word-wrap:break-word;'>" + name + "</h5></td></tr>"
        //            });
        //            $(xml).find('LearningOutcomeViewModel').each(function () {
        //                var name = $(this).find("Name").text();
        //                var id = $(this).find("Id").text();
        //                var idValue = $(this).find("IdValue").text();
        //                string = string + "<tr class='d-flex'><td class='col-1'><div class='el-checkbox el-checkbox-green text-center form-group'><span class='margin-l'></span><input value='" + idValue + "' type='checkbox' class='checkbox' name='topic' id='checkbox" + id + "' /><label class='el-checkbox-style' for='checkbox" + id + "'></label></div></td><td class='col-11'><h5 style='word-wrap:break-word;'>" + name + "</h5></td></tr>"
        //            });
        //            string = string + "</tbody></table></div>";
        //            $("#listTopic").html(string);
        //        }
        //    });
        //}
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
    $("#exportExamination").submit(function (event) {
        event.preventDefault();
        var examinationId = $("input[name='examinationId']").val();
        var fileExtension = $('#fileExtension').find(":selected").text();
        var getCategory = $('#getCategory').prop('checked');
        window.location = "/ExaminationAPI/export?examinationId=" + examinationId + "&fileExtension=" + fileExtension + "&getCategory=" + getCategory;
    });
    $('.btnExport').on('click', function (e) {
        var counter = $(this).data("value");
        var examinationId = $("input[name='examinationId-" + counter + "']").val();
        var fileExtension = $("#fileExtension-" + counter).find(":selected").text();
        var getCategory = $("#getCategory-" + counter).prop('checked');
        window.location = "/ExaminationAPI/export?examinationId=" + examinationId + "&fileExtension=" + fileExtension + "&getCategory=" + getCategory;
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
                'targets': [1, 2],
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
                'targets': [1, 2],
                'orderable': false,
            }
        ]
    });
    var tableExam2= $('#dataTableExam-2').DataTable({
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
                'targets': [1, 2],
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
                'targets': [1, 2],
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
                'targets': [1, 2],
                'orderable': false,
            }
        ]
    });
    var tableExam5 =  $('#dataTableExam-5').DataTable({
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
                'targets': [1, 2],
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
});
$(".tab-slider--nav li").click(function () {
    $(".tab-slider--body").hide();
    var activeTab = $(this).attr("rel");
    $("#" + activeTab).fadeIn();
    if ($(this).attr("rel") == "tab2") {
        $('.tab-slider--tabs').addClass('slide');
        $('#flagPercent').attr('value', 'grade');
    } else {
        $('.tab-slider--tabs').removeClass('slide');
        $('#flagPercent').attr('value', 'percent');
    }
    $(".tab-slider--nav li").removeClass("active");
    $(this).addClass("active");
});


