function bt_change_partial(url, btn) {

    $.get(url, {}, function (response) {
        $("#question-import").html(response);
        $('#dataTable').DataTable();
    });
    $(".btn-group > .btn").removeClass("active");
    $(btn).addClass("active");
}

function nav_bar_active() {
    $('.navbar-nav a').click(function (e) {
        $('.navbar-nav').find('li.active').removeClass('active');
        $(this).parent('li').addClass('active');
    });
}

function customs_display() {
    $.each($(".customs-display"), function () {
        var content;
        content = $(this).html();
        if (content.indexOf("[html]") >= 0) {
            content = content.split("<cbr>").join("&lt;br&gt;");
            content = content.split("&lt;cbr&gt;").join("<br/>");
            content = content.split("<br>").join("<br/>");
            content = content.split("&lt;br&gt;").join("&lt;br&gt;");
            content = content.split("&lt;br/&gt;").join("<br/>");
            content = content.split("&lt;p&gt;").join("");

            content = content.split("&lt;/p&gt;").join("");
            content = content.split("&lt;b&gt;").join("");
            content = content.split("&lt;/b&gt;").join("");
            content = content.split("&lt;span&gt;").join("");
            content = content.split("&lt;/span&gt;").join("");
            content = content.split("&lt;/span&gt;").join("");
            content = content.split("&lt;u&gt;").join("");
            content = content.split("&lt;/u&gt;").join("");
            content = content.split("&lt;i&gt;").join("");
            content = content.split("&lt;/i&gt;").join("");
            content = content.split("&lt;sub&gt;").join("<sub>");
            content = content.split("&lt;/sub&gt;").join("</sub>");
            content = content.split("&lt;sup&gt;").join("<sup>");
            content = content.split("&lt;/sup&gt;").join("</sup>");
            content = content.split("[html]").join("");
        }
        $(this).html(content);
    });
}
function customs_display_duplicate() {
    var content;
    content = $('#customs-display-duplicate').html();
    if (content.indexOf("[html]") >= 0) {
        content = content.split("&lt;cbr&gt;").join("<br/>");
        content = content.split("&lt;br&gt;").join("<br/>");
        content = content.split("&lt;p&gt;").join("");
        content = content.split("&lt;br/&gt;").join("<br/>");
        content = content.split("&lt;/p&gt;").join("");
        content = content.split("&lt;b&gt;").join("");
        content = content.split("&lt;/b&gt;").join("");
        content = content.split("&lt;span&gt;").join("");
        content = content.split("&lt;/span&gt;").join("");
        content = content.split("&lt;/span&gt;").join("");
        content = content.split("&lt;u&gt;").join("");
        content = content.split("&lt;/u&gt;").join("");
        content = content.split("&lt;i&gt;").join("");
        content = content.split("&lt;/i&gt;").join("");
        content = content.split("&lt;sub&gt;").join("<sub>");
        content = content.split("&lt;/sub&gt;").join("</sub>");
        content = content.split("&lt;sup&gt;").join("<sup>");
        content = content.split("&lt;/sup&gt;").join("</sup>");
        content = content.split("[html]").join("");
    }
    $('#customs-display-duplicate').html(content);
}

function clickSection() {
    var content = [];
    var duplicate = [];
    var countTable = 0;
    var countDuplicate = 0;
    $('#section-editable').on('click', function () {
        content = [];
        duplicate = [];
        countTable = 0;
        countDuplicate = 0;
        var templateEditable = '<table class="table table-bordered table-hover text-custom" id="tableEditable" width="100%" cellspacing="0">' +
            '<thead>' +
            '<tr>' +
            '<th>No.</th>' +
            '<th>Question</th>' +
            '<th>Duplicated With</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody>' +
            '</tbody>' +
            '</table>';
        $('#tableEditable').DataTable().destroy();
        $('#tableEditable').remove();
        $('#tableSuccess').DataTable().destroy();
        $('#tableSuccess').remove();
        $('#tableInvalid').DataTable().destroy();
        $('#tableInvalid').remove();
        $('#editable #importTable').append(templateEditable);
        $('#editable').show();
        var table1 = $('#tableEditable').DataTable({
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
                url: "/Question/GetQuestionByImportIdAndType",
                type: "GET",
                data: {
                    importId: $('#importId').val(),
                    type: "editable"
                },
                dataType: "json"
            },

            columns: [
                {
                    data: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    data: "QuestionTempViewModel",
                    render: function (data, type, row, meta) {
                        if (row != null) {
                            var questionObj = {};
                            var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                            var code = 'Question Code: ' + row.Code + '</p>';
                            var questionContent = '<p class="text-custom" id="qcontent_' + countTable + '"></p>';
                            questionObj['QuestionContent'] = row.QuestionContent;
                            var editButton = '<a href="" class="btn btn-primary float-md-right ml-1">Edit</a>';
                            var acceptButton = '<button class="btn btn-success float-md-right ml-1 reload-partial">Accept</button>';
                            var deleteButton = '<button class="btn btn-danger float-md-right reload-partial">Delete</button>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                result = result + '<div class="text-custom" id="ocontent_' + countTable + '_' + i + '" class="container-fluid"></div>';
                            }
                            countTable++;
                            result = result + editButton + acceptButton + deleteButton;
                            return result;
                        }
                    }
                },
                {
                    data: "DuplicatedQuestion",
                    render: function (data, type, row) {
                        if (data != null) {
                            var questionObj = {};
                            var category = '<p> </p>';
                            var code = ' <p class="text-custom">' + data.CourseName + data.Code + '</p>';
                            var status = "";
                            //if (!data.IsBank) {
                            status = '<span class="badge ml-2">' + data.Status + '</span>'
                            //}
                            var questionContent = '<p id="dcontent_' + countDuplicate + '"></p>';
                            var editButton = '<a href="" class="btn btn-primary float-md-right ml-1">Edit</a>';
                            var acceptButton = '<button class="btn btn-success float-md-right ml-1 reload-partial">Accept</button>';
                            var deleteButton = '<button class="btn btn-danger float-md-right reload-partial">Delete</button>';
                            questionObj['QuestionContent'] = row.QuestionContent;
                            var image = data.Image;
                            if (image != null && image != "") {
                                image = '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + image + '" /></p>';
                            } else {
                                image = "";
                            }
                            var options = [];
                            var i = 0;
                            for (i = 0; i < data.Options.length; i++) {
                                var option = {};
                                option["content"] = changeHtml(data.Options[i].OptionContent);
                                option["correct"] = data.Options[i].IsCorrect;
                                options.push(option);
                            }
                            questionObj["Options"] = options;
                            duplicate.push(questionObj);
                            var result = status + category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                result = result + '<div id="docontent_' + countDuplicate + '_' + i + '" class="container-fluid"></div>';
                            }
                            countDuplicate++;
                            result = result + editButton + acceptButton + deleteButton;
                            return result;
                        }
                    }
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "49%" },
                { targets: 2, width: "49%" }
            ],

            fnDrawCallback: function () {
                //original question
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

                //duplicate question
                countDuplicate = 0;
                for (var d = 0; d < duplicate.length; d++) {
                    var jd = "#dcontent_" + d;
                    var changeduplicateContent = duplicate[d]["QuestionContent"];
                    breakContent = [];
                    isHtml = false;
                    if (changeduplicateContent.indexOf("[html]") >= 0) {
                        isHtml = true;
                    }
                    if (isHtml) {
                        changeduplicateContent = changeduplicateContent.split("&lt;p&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/p&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;span&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/span&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("[html]").join("");
                        breakContent = changeduplicateContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(changeduplicateContent);
                    }
                    for (var f = 0; f < breakContent.length; f++) {
                        $(jd).append('<p id="dcontent_' + d + '_' + f + '"></p>');
                        var jdf = "#dcontent_" + d + '_' + f;
                        $(jdf).text(breakContent[f]);
                    }
                    breakContent = [];
                    for (var dO = 0; dO < duplicate[d]["Options"].length; dO++) {
                        //letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        var jdO = "#docontent_" + d + "_" + dO;
                        var optionDContent = duplicate[d]["Options"][dO]["content"];
                        var optionDCorrect = duplicate[d]["Options"][dO]["correct"];
                        if (isHtml) {
                            optionDContent = optionDContent.split("&lt;p&gt;").join("");
                            optionDContent = optionDContent.split("&lt;/p&gt;").join("");
                            optionDContent = optionDContent.split("&lt;span&gt;").join("");
                            optionDContent = optionDContent.split("&lt;/span&gt;").join("");
                            optionDContent = optionDContent.split("[html]").join("");
                            breakContent = optionDContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
                            breakContent = breakContent.split("·");
                        } else {
                            breakContent.push(optionDContent);
                        }
                        for (b = 0; b < breakContent.length; b++) {
                            $(jdO).append('<p id="docontent_' + d + '_' + dO + '_' + b + '"></p>');
                            var jdOw = "#docontent_" + d + '_' + dO + '_' + b;
                            if (b == 0) {
                                $(jdOw).text(letters[dO] + '. ' + breakContent[b]);
                            } else {
                                $(jdOw).text(breakContent[b]);
                            }
                        }
                        if (optionDCorrect) {
                            $(jdO).addClass('text-right-answer');
                        }
                    }
                }
            }
        });

        table1.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });

    $('#section-success').on('click', function () {
        content = [];
        countTable = 0;
        var templateSuccess = '<table class="table table-bordered table-hover text-custom" id="tableSuccess" width="100%" cellspacing="0">' +
            '<thead>' +
            '<tr>' +
            '<th>No.</th>' +
            '<th>Question</th>' +
            '<th>Action</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody>' +
            '</tbody>' +
            '</table>';
        $('#tableEditable').DataTable().destroy();
        $('#tableEditable').remove();
        $('#tableSuccess').DataTable().destroy();
        $('#tableSuccess').remove();
        $('#tableInvalid').DataTable().destroy();
        $('#tableInvalid').remove();
        $('#success #importTable').append(templateSuccess);
        $('#success').show();
        var table3 = $('#tableSuccess').DataTable({
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
                url: "/Question/GetQuestionByImportIdAndType",
                type: "GET",
                data: {
                    importId: $('#importId').val(),
                    type: "success"
                },
                dataType: "json"
            },

            columns: [
                {
                    data: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    data: "QuestionTempViewModel",
                    render: function (data, type, row, meta) {
                        if (row != null) {
                            var questionObj = {};
                            var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                            var code = 'Question Code: ' + row.Code + '</p>';
                            var questionContent = '<p class="text-custom" id="qcontent_' + countTable + '"></p>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                result = result + '<div class="text-custom" id="ocontent_' + countTable + '_' + i + '" class="container-fluid"></div>';
                            }
                            countTable++;
                            return result;
                        }
                    }
                },
                {
                    data: "Id",
                    render: function (data) {
                        return '<button class="btn btn-danger float-md-center delete-question-dt" data-id="'+data+'">Delete</button>'
                    }
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "88%" },
                { targets: 2, width: "10%"}
            ],
            fnDrawCallback: function () {
                //original question
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
        table3.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });

    $('#section-invalid').on('click', function () {
        content = [];
        countTable = 0;
        var templateInvalid = '<table class="table table-bordered table-hover text-custom" id="tableInvalid" width="100%" cellspacing="0">' +
            '<thead>' +
            '<tr>' +
            '<th>No.</th>' +
            '<th>Question</th>' +
            '<th>Message</th>' +
            '<th>Action</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody>' +
            '</tbody>' +
            '</table>';
        $('#tableEditable').DataTable().destroy();
        $('#tableEditable').remove();
        $('#tableSuccess').DataTable().destroy();
        $('#tableSuccess').remove();
        $('#tableInvalid').DataTable().destroy();
        $('#tableInvalid').remove();
        $('#invalid #importTable').append(templateInvalid);
        $('#invalid').show();
        var table4 = $('#tableInvalid').DataTable({
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
                url: "/Question/GetQuestionByImportIdAndType",
                type: "GET",
                data: {
                    importId: $('#importId').val(),
                    type: "invalid"
                },
                dataType: "json"
            },

            columns: [
                {
                    data: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    data: "QuestionTempViewModel",
                    render: function (data, type, row, meta) {
                        if (row != null) {
                            var questionObj = {};
                            var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                            var code = 'Question Code: ' + row.Code + '</p>';
                            var questionContent = '<p class="text-custom" id="qcontent_' + countTable + '"></p>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                result = result + '<div class="text-custom" id="ocontent_' + countTable + '_' + i + '" class="container-fluid"></div>';
                            }
                            countTable++;
                            return result;
                        }
                    }
                },
                {
                    data: "Message"
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
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "68%" },
                { targets: 2, width: "20%" },
                { targets: 3, width: "10%" }
            ],
            fnDrawCallback: function () {
                //original question
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
        table4.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });


    $('#section-delete').on('click', function () {
        content = [];
        countTable = 0;
        var templateDelete = '<table class="table table-bordered table-hover text-custom" id="tableDelete" width="100%" cellspacing="0">' +
            '<thead>' +
            '<tr>' +
            '<th>No.</th>' +
            '<th>Question</th>' +
            '<th>Action</th>' +
            '</tr>' +
            '</thead>' +
            '<tbody>' +
            '</tbody>' +
            '</table>';
        $('#tableEditable').DataTable().destroy();
        $('#tableEditable').remove();
        $('#tableSuccess').DataTable().destroy();
        $('#tableSuccess').remove();
        $('#tableInvalid').DataTable().destroy();
        $('#tableInvalid').remove();
        $('#tableDelete').DataTable().destroy();
        $('#tableDelete').remove();
        $('#delete #importTable').append(templateDelete);
        $('#delete').show();
        var table2 = $('#tableDelete').DataTable({
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
                url: "/Question/GetQuestionByImportIdAndType",
                type: "GET",
                data: {
                    importId: $('#importId').val(),
                    type: "delete"
                },
                dataType: "json"
            },

            columns: [
                {
                    data: function (data, type, row, meta) {
                        return meta.row + meta.settings._iDisplayStart + 1;
                    }
                },
                {
                    data: "QuestionTempViewModel",
                    render: function (data, type, row, meta) {
                        if (row != null) {
                            var questionObj = {};
                            var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                            var code = 'Question Code: ' + row.Code + '</p>';
                            var questionContent = '<p class="text-custom" id="qcontent_' + countTable + '"></p>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                result = result + '<div class="text-custom" id="ocontent_' + countTable + '_' + i + '" class="container-fluid"></div>';
                            }
                            countTable++;
                            return result;
                        }
                    }
                },
                {
                    data: "Id",
                    render: function (data) {
                        return '<button class="btn btn-danger float-md-center delete-question-dt" data-id="' + data + '">Restore</button>'
                    }
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "88%" },
                { targets: 2, width: "10%" }
            ],
            fnDrawCallback: function () {
                //original question
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
        table2.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });




    var tableCustoms = $('#table-customs').DataTable({
        columns: [
            {
                "render": function (data, type, row) {
                    if (data.indexOf("[html]") >= 0) {
                        data = data.split("&lt;cbr&gt;").join("<br/>");
                        data = data.split("&lt;br&gt;").join("<br/>");
                        data = data.split("&lt;p&gt;").join("");
                        data = data.split("&lt;br/&gt;").join("<br/>");
                        data = data.split("&lt;b&gt;").join("");
                        data = data.split("&lt;/b&gt;").join("");
                        data = data.split("&lt;/p&gt;").join("");
                        data = data.split("&lt;span&gt;").join("");
                        data = data.split("&lt;/span&gt;").join("");
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

                    return data
                }
            },
            {
                "render": function (data, type, row) {
                    if (data.indexOf("[html]") >= 0) {
                        data = data.split("&lt;cbr&gt;").join("<br/>");
                        data = data.split("&lt;br&gt;").join("<br/>");
                        data = data.split("&lt;p&gt;").join("");
                        data = data.split("&lt;br/&gt;").join("<br/>");
                        data = data.split("&lt;b&gt;").join("");
                        data = data.split("&lt;/b&gt;").join("");
                        data = data.split("&lt;/p&gt;").join("");
                        data = data.split("&lt;span&gt;").join("");
                        data = data.split("&lt;/span&gt;").join("");
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
                    return data
                }
            }
        ],
        columnDefs: [
            { targets: 0, width: "50%" },
            { targets: 1, width: "50%" },
        ]
    });
    tableCustoms.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
}

function toggleTableDuplicate() {
    $(".toggle-table").on('click', function () {
        $(".table-toggle").addClass("hidden");
        $(this.attributes["data-toggle"].value).removeClass("hidden");
    });
}

function table_on_top() {
    var table = $('#dataTable')
    table.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
}
function spinner_loading() {

}
function loadFirstTable() {
    var content = [];
    var duplicate = [];
    var countTable = 0;
    var countDuplicate = 0;
    $('#editable').show();
    var table1 = $('#tableEditable').DataTable({
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
            url: "/Question/GetQuestionByImportIdAndType",
            type: "GET",
            data: {
                importId: $('#importId').val(),
                type: "editable"
            },
            dataType: "json"
        },

        columns: [
            {
                data: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                data: "QuestionTempViewModel",
                render: function (data, type, row, meta) {
                    if (row != null) {
                        var questionObj = {};
                        var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                        var code = 'Question Code: ' + row.Code + '</p>';
                        var questionContent = '<p class="text-custom" id="qcontent_' + countTable + '"></p>';
                        questionObj['QuestionContent'] = row.QuestionContent;
                        var editButton = '<a href="" class="btn btn-primary float-md-right ml-1">Edit</a>';
                        var acceptButton = '<button class="btn btn-success float-md-right ml-1 reload-partial">Accept</button>';
                        var deleteButton = '<button class="btn btn-danger float-md-right reload-partial">Delete</button>';
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
                        var result = category + code + questionContent + image;
                        for (i = 0; i < options.length; i++) {
                            result = result + '<div class="text-custom" id="ocontent_' + countTable + '_' + i + '" class="container-fluid"></div>';
                        }
                        countTable++;
                        result = result + editButton + acceptButton + deleteButton;
                        return result;
                    }
                }
            },
            {
                data: "DuplicatedQuestion",
                render: function (data, type, row) {
                    if (data != null) {
                        var questionObj = {};
                        var status = "";
                        //if (!data.IsBank) {
                            status = '<span class="badge ml-2">' + data.Status + '</span>'
                        //}
                        var category = '<p> </p>';
                        var code = ' <p class="text-custom">' + data.CourseName + data.Code + '</p>';
                        var questionContent = '<p id="dcontent_' + countDuplicate + '"></p>';
                        questionObj['QuestionContent'] = row.QuestionContent;
                        var editButton = '<a href="" class="btn btn-primary float-md-right ml-1">Edit</a>';
                        var acceptButton = '<button class="btn btn-success float-md-right ml-1 reload-partial">Accept</button>';
                        var deleteButton = '<button class="btn btn-danger float-md-right reload-partial">Delete</button>';
                        var image = data.Image;
                        if (image != null && image != "") {
                            image = '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + image + '" /></p>';
                        } else {
                            image = "";
                        }
                        var options = [];
                        var i = 0;
                        for (i = 0; i < data.Options.length; i++) {
                            var option = {};
                            option["content"] = changeHtml(data.Options[i].OptionContent);
                            option["correct"] = data.Options[i].IsCorrect;
                            options.push(option);
                        }
                        questionObj["Options"] = options;
                        duplicate.push(questionObj);
                        var result = status + category + code + questionContent + image;
                        for (i = 0; i < options.length; i++) {
                            result = result + '<div id="docontent_' + countDuplicate + '_' + i + '" class="container-fluid"></div>';
                        }
                        countDuplicate++;
                        result = result + editButton + acceptButton + deleteButton;
                        return result;
                    }
                }
            }
        ],
        columnDefs: [
            { targets: 0, width: "2%" },
            { targets: 1, width: "49%" },
            { targets: 2, width: "49%" }
        ],

        fnDrawCallback: function () {
            //original question
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

            //duplicate question
            countDuplicate = 0;
            for (var d = 0; d < duplicate.length; d++) {
                var jd = "#dcontent_" + d;
                var changeduplicateContent = duplicate[d]["QuestionContent"];
                breakContent = [];
                isHtml = false;
                if (changeduplicateContent.indexOf("[html]") >= 0) {
                    isHtml = true;
                }
                if (isHtml) {
                    changeduplicateContent = changeduplicateContent.split("&lt;p&gt;").join("");
                    changeduplicateContent = changeduplicateContent.split("&lt;/p&gt;").join("");
                    changeduplicateContent = changeduplicateContent.split("&lt;span&gt;").join("");
                    changeduplicateContent = changeduplicateContent.split("&lt;/span&gt;").join("");
                    changeduplicateContent = changeduplicateContent.split("[html]").join("");
                    breakContent = changeduplicateContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
                    breakContent = breakContent.split("·");
                } else {
                    breakContent.push(changeduplicateContent);
                }
                for (var f = 0; f < breakContent.length; f++) {
                    $(jd).append('<p id="dcontent_' + d + '_' + f + '"></p>');
                    var jdf = "#dcontent_" + d + '_' + f;
                    $(jdf).text(breakContent[f]);
                }
                breakContent = [];
                for (var dO = 0; dO < content[d]["Options"].length; dO++) {
                    //letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    var jdO = "#docontent_" + d + "_" + dO;
                    var optionDContent = content[d]["Options"][dO]["content"];
                    var optionDCorrect = content[d]["Options"][dO]["correct"];
                    if (isHtml) {
                        optionDContent = optionDContent.split("&lt;p&gt;").join("");
                        optionDContent = optionDContent.split("&lt;/p&gt;").join("");
                        optionDContent = optionDContent.split("&lt;span&gt;").join("");
                        optionDContent = optionDContent.split("&lt;/span&gt;").join("");
                        optionDContent = optionDContent.split("[html]").join("");
                        breakContent = optionDContent.split("&lt;cbr&gt;").join("·").split("<cbr>").join("·").split("&lt;br&gt;").join("·").split("<br>").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(optionDContent);
                    }
                    for (b = 0; b < breakContent.length; b++) {
                        $(jdO).append('<p id="docontent_' + d + '_' + dO + '_' + b + '"></p>');
                        var jdOw = "#docontent_" + d + '_' + dO + '_' + b;
                        if (b == 0) {
                            $(jdOw).text(letters[dO] + '. ' + breakContent[b]);
                        } else {
                            $(jdOw).text(breakContent[b]);
                        }
                    }
                    if (optionDCorrect) {
                        $(jdO).addClass('text-right-answer');
                    }
                }
            }
        }
    });

}
$(document).ready(function () {
    nav_bar_active();
    hideAll();
    loadFirstTable();
    clickSection();
    //toggleTableDuplicate();
    customs_display();
    customs_display_duplicate();
    table_on_top();
});
function hideAll() {
    $('#editable1').hide();
    $('#editable2').hide();
    $('#success').hide();
    $('#invalid').hide();
}
function changeHtml(data) {
    if (data.indexOf("[html]") >= 0) {
        data = data.split("&lt;cbr&gt;").join("<br/>");
        data = data.split("&lt;br&gt;").join("<br/>");
        data = data.split("&lt;p&gt;").join("");
        data = data.split("&lt;br/&gt;").join("<br/>");
        data = data.split("&lt;b&gt;").join("");
        data = data.split("&lt;/b&gt;").join("");
        data = data.split("&lt;/p&gt;").join("");
        data = data.split("&lt;span&gt;").join("");
        data = data.split("&lt;/span&gt;").join("");
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