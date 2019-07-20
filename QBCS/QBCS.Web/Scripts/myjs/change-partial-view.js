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
        startLoading();

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
        this.table1 = initTableEditable();

        this.table1.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });

        stopLoading();
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
            rowId: 'Id',
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
                        return '<button data-url="/Import/Delete?questionId=' + data + '&url=' + window.location.href + '" class="btn btn-danger float-md-center delete-question-dt" data-id="#' + data + '" data-from="tableSuccess" data-to="tableDelete">Delete</button>'
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

                
                $("#tableSuccess .delete-question-dt").on('click', function () {
                    minusTotal($("#total-success"));
                    plusTotal($("#total-delete"));

                    sendAjax($(this).attr('data-url'));
                    $('#section-success').trigger('click');
                })
            }
        });
        table3.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });

    $('#section-invalid').on('click', function () {
        startLoading();
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
            rowId: 'Id',
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
                        var deleteQ = '<button data-url="/Import/Delete?questionId=' + data + '&url=' + window.location.href + '" class="btn btn-danger col-md-12 delete-question-dt" data-from="tableInvalid" data-to="tableDelete" data-id="#' + data + '">Delete</button>';
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

                $("#tableInvalid .delete-question-dt").on('click', function () {
                    minusTotal($("#total-invalid"));
                    plusTotal($("#total-delete"));

                    sendAjax($(this).attr('data-url'));
                    $('#section-invalid').trigger('click');
                })
            }
        });
        table4.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
        
        stopLoading();
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
            rowId: 'Id',
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
                        return '<a href="/Import/Recovery?tempId=' + data + '&url=' + window.location.href + '" class="btn btn-success float-md-center data-id="' + data + '">Restore</a>'
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

function minusTotal(totalSpan) {
    var value = parseInt(totalSpan.text());
    totalSpan.text(value - 1);
}

function plusTotal(totalSpan) {
    var value = parseInt(totalSpan.text());
    totalSpan.text(value + 1);
    return value + 1;
}

function getQuestionObject(deleteRow) {
    var question = { Content: "", Duplicate: "", Message: "", Id: 0 };

    question.Content = deleteRow.find('div[data-question="content"]')[0].innerHTML;
    if (deleteRow.find('div[data-question="duplicate"]').length > 0) {
        question.Duplicate = deleteRow.find('div[data-question="duplicate"]')[0].innerHTML;
    }
    if (deleteRow.find('div[data-question="message"]').length > 0) {
        question.Message = deleteRow.find('div[data-question="message"]')[0].innerHTML;
    }
    question.Id = parseInt(deleteRow.attr('data-id'));
    return question;
}

function sendAjax(url) {
    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {

        }
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
    startLoading();
    $('#editable').show();
    initTableEditable();
    stopLoading();
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
function reloadTable(url, container) {
    startLoading();
    $.ajax({
        url: url,
        type: 'GET',
        success: function (response) {
            $(container).html(response);
            var table1 = $('#tableEditable').DataTable({
                columns: [
                    null,
                    {
                        "render": function (data, type, row) {
                            if (data.indexOf("[html]") >= 0) {
                                data = data.split("&lt;cbr&gt;").join("<br/>");
                                data = data.split("&lt;br&gt;").join("<br/>");
                                data = data.split("&lt;p&gt;").join("");
                                data = data.split("&lt;br/&gt;").join("<br/>");
                                data = data.split("&lt;/p&gt;").join("");
                                data = data.split("&lt;b&gt;").join("");
                                data = data.split("&lt;/b&gt;").join("");
                                data = data.split("&lt;span&gt;").join("");
                                data = data.split("&lt;/span&gt;").join("");
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
                                data = data.split("&lt;/p&gt;").join("");
                                data = data.split("&lt;b&gt;").join("");
                                data = data.split("&lt;/b&gt;").join("");
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
                ]
            });
            stopLoading();
        }
    });
}

function initTableEditable() {

    content = [];
    duplicate = [];
    countTable = 0;
    countDuplicate = 0;

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
        rowId: 'Id',
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
                        var editButton = '<a href="/Import/GetQuestionTemp?tempId=' + row.Id + '" class="btn btn-primary ml-1 float-right">Edit</a>';
                        var acceptButton = '<button class="btn btn-success ml-1 accept-question-dt float-right" data-url="/Import/Skip?questionId=' + row.Id + '&url=' + window.location.href + '">Accept</button>';
                        var deleteButton = '<button class="btn btn-danger delete-question-dt float-right" data-url="/Import/Delete?questionId=' + row.Id + '&url=' + window.location.href + '">Delete</button>';
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
                        if (row.DuplicatedQuestion != null) {
                            result = result + '<div class="row"><div class="col-md-12 bottom-right-cell">' + editButton + acceptButton + deleteButton + '</div></div>';
                        }
                        return result;
                    }
                }
            },
            {
                render: function (data, type, row) {
                    if (row != null) {
                        if (row.DuplicatedQuestion != null) {
                            var questionObj = {};
                            var category = '<p> </p>';
                            var code = ' <p class="text-custom">' + row.DuplicatedQuestion.CourseName + row.DuplicatedQuestion.Code + '</p>';
                            var status = "";
                            if (!row.DuplicatedQuestion.IsBank) {
                                var statusClass = "";
                                var statusName = "";

                                switch (row.DuplicatedQuestion.Status) {
                                    case 2:
                                        statusClass = "badge-warning";
                                        statusName = "Editable";
                                        break;
                                    case 3:
                                        statusClass = "badge-danger";
                                        statusName = "Deleted";
                                        break;
                                    case 4:
                                        statusClass = "badge-success";
                                        statusName = "Success";
                                        break;
                                }

                                status = '<span class="badge ml-2 ' + statusClass + '">' + statusName + '</span>'
                            }
                            var questionContent = '<p id="dcontent_' + countDuplicate + '"></p>';
                            var editButton = '<a href="/Import/GetQuestionTemp?tempId=' + row.DuplicatedQuestion.Id + '" class="btn btn-primary float-right ml-1">Edit</a>';
                            var acceptButton = '<button class="btn btn-success float-right ml-1 accept-question-dt">Accept</button>';
                            var deleteButton = '<button class="btn btn-danger float-right delete-question-dt"  data-url="/Import/Delete?questionId=' + row.DuplicatedQuestion.Id + '&url=' + window.location.href + '">Delete</button>';
                            questionObj['QuestionContent'] = row.DuplicatedQuestion.QuestionContent;
                            var image = row.DuplicatedQuestion.Image;
                            if (image != null && image != "") {
                                image = '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + image + '" /></p>';
                            } else {
                                image = "";
                            }
                            var options = [];
                            var i = 0;
                            for (i = 0; i < row.DuplicatedQuestion.Options.length; i++) {
                                var option = {};
                                option["content"] = changeHtml(row.DuplicatedQuestion.Options[i].OptionContent);
                                option["correct"] = row.DuplicatedQuestion.Options[i].IsCorrect;
                                options.push(option);
                            }
                            questionObj["Options"] = options;
                            duplicate.push(questionObj);
                            var result = status + category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                result = result + '<div id="docontent_' + countDuplicate + '_' + i + '" class="container-fluid"></div>';
                            }
                            countDuplicate++;
                            if (!row.DuplicatedQuestion.IsBank && !row.DuplicatedQuestion.IsAnotherImport && row.DuplicatedQuestion.Status == 2) {
                                result = result + '<div class="row"><div class=" col-md-12 bottom-right-cell">' + editButton + acceptButton + deleteButton + '</div></div>';
                            }
                            return result;

                        }
                        else {
                            var result = row.Message + '<br/> <a href="/Import/GetDuplicatedDetail/' + row.Id + '" class="text-info btn-link font-weight-bold" > See more</a >';
                            return result;
                        }

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

            $("#tableEditable .delete-question-dt").on('click', function () {
                minusTotal($("#total-editable"));
                plusTotal($("#total-delete"));

                sendAjax($(this).attr('data-url'));
                $('#section-editable').trigger('click');
            })

            $("#tableEditable .accept-question-dt").on('click', function () {
                minusTotal($("#total-editable"));
                plusTotal($("#total-success"));

                sendAjax($(this).attr('data-url'));
                $('#section-editable').trigger('click');
            })
        }
    });
    return table1;
}

function startLoading() {
    $('#spinner').css("display", "block");
    $('#spinner').css("z-index", "1060");
    $('#pleaseWaitDialog').modal();
}

function stopLoading() {
    $('#spinner').css("display", "none");
    $('#pleaseWaitDialog').modal('hide');
}