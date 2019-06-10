$(document).ready(function () {

    // Toolbar extra buttons
    var btnFinish = $('<button></button>').text('Finish')
                                     .addClass('btn btn-info')
                                     .attr('disabled', 'disabled')
                                     .attr('id', 'btnFinish')
                                     .on('click', function () {
                                         var link = 'http://localhost/QBCS.Web/Question/ViewGeneratedExamination';
                                         $.ajax({
                                             type: "GET",
                                             url: link,
                                             success: function (data) {
                                                 window.location.href = data.redirecturl;
                                             },
                                             error: function (httpRequest, textStatus, errorThrown) {
                                                 alert("Error: " + textStatus + " " + errorThrown + " " + httpRequest);
                                             }
                                         });
                                     });
    var btnCancel = $('<button></button>').text('Cancel')
                                     .addClass('btn btn-danger')
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
        if (stepNumber === 1) {
            $('#btnFinish').removeAttr('disabled');
        }
        if (stepNumber === 0 && stepDirection === 'forward') {
            var courseId = $("input[name=course]:checked").val();
            var string = "<div class='col-lg-8'><table class='table table-striped'><tbody><tr class='d-flex'><td class='col-1'><div class='el-checkbox el-checkbox-green text-center'><span class='margin-l'></span><input type='checkbox' id='selectAllCheckbox' /><label class='el-checkbox-style' for='selectAllCheckbox'></label></div></td><td lass='col-11'><h5>Select All</h5></td></tr>";
            $.ajax({
                type: "GET",
                url: "http://localhost/QBCS.Web/api/TopicAPI/topics",
                data: {
                    CourseId : courseId
                },
                cache: false,
                dataType: "xml",
                success: function (xml) {
                    $(xml).find('TopicViewModel').each(function () {
                        var name = $(this).find("Name").text();
                        var id = $(this).find("Id").text();
                        string = string + "<tr class='d-flex'><td class='col-1'><div class='el-checkbox el-checkbox-green text-center'><span class='margin-l'></span><input type='checkbox' id='checkbox" + id + "' /><label class='el-checkbox-style' for='checkbox" + id + "'></label></div></td><td class='col-11'><h5 style='word-wrap:break-word;'>" + name + "</h5></td></tr>"
                    });
                    string = string + "</tbody></table></div>";
                    $("#step-2").html(string);
                }
            });
        }
    });

    $("#searchCourse").submit(function (event) {
        event.preventDefault();
        var searchCourseValue = $("#searchCourseValue").val();
        var appendValue = "";
        $.ajax({
            type: "GET",
            url: "http://localhost/QBCS.Web/api/CourseAPI/searchCourse",
            data: {
                courseCode : searchCourseValue
            },
            dataType: "xml",
            success: function (xml) {
                $(xml).find('CourseViewModel').each(function () {
                    var id = $(this).find("Id").text();
                    var code = $(this).find("Code").text();
                    appendValue = appendValue + "<tr><td><div class='el-radio'><span class='margin-r'></span><input type='radio' name='course' id='1_" + id + "' value='" + id + "'><label class='el-radio-style' for='1_" + id + "'></label></div></td><td><h5>" + code + "</h5></td></tr>";
                });
                $("#listCourse").replaceWith(appendValue);
            }
        });
    });
    
});

