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
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {
        if (stepNumber === 2) {
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

    //$("#btnSearch").click(function (event) {
    //    event.preventDefault();
    //    var searchCourseValue = $("#searchCourseValue").val();
    //    var appendValue = "<tbody id='listCourse'>";
    //    $.ajax({
    //        type: "GET",
    //        url: "http://localhost/QBCS.Web/api/CourseAPI/searchCourse",
    //        data: {
    //            courseCode : searchCourseValue
    //        },
    //        dataType: "xml",
    //        success: function (xml) {
    //            $(xml).find('CourseViewModel').each(function () {
    //                var id = $(this).find("Id").text();
    //                var code = $(this).find("Code").text();
    //                appendValue = appendValue + "<tr><td><div class='el-radio'><span class='margin-r'></span><input type='radio' name='course' id='1_" + id + "' value='" + id + "'><label class='el-radio-style' for='1_" + id + "'></label></div></td><td><h5>" + code + "</h5></td></tr>";
    //            });
    //            appendValue = appendValue + "</tbody>";
    //            $("#listCourse").replaceWith(appendValue);
    //        }
    //    });
    //});

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
    $("#exportExamination").submit(function (event) {
        event.preventDefault();
        var examinationId = $("input[name='examinationId']").val();
        var fileExtension = $('#fileExtension').find(":selected").text();
        window.location = "http://localhost/QBCS.Web/api/ExaminationAPI/export?examinationId=" + examinationId + "&fileExtension=" + fileExtension;
        //    $.ajax({
        //        type: "GET",
        //        url: "http://localhost/QBCS.Web/api/ExaminationAPI/export",
        //        data: {
        //            examinationId: examinationId,
        //            fileExtension: fileExtension
        //        },
        //        success: function () {
        //        }
        //    });
    });
    $(".tab-slider--body").hide();
    $(".tab-slider--body:first").show();


    //set up datatable
    $('#dataTableExam').DataTable({
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
        ]
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


