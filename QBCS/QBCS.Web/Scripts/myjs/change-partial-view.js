function insert_number_modal() {
    var notInsert;
    var checkAgain;
    var editable = parseInt($("#total-editable").text());
    var invalid = parseInt($("#total-invalid").text());
    var deleteQ = parseInt($("#total-delete").text());
    var success = parseInt($("#total-success").text());
    var totalQues = parseInt($("#total-question").text());

    notInsert = editable + invalid + deleteQ;
    checkAgain = totalQues - (editable + invalid + deleteQ + success);

    $("#txtNotInsert").val(notInsert);
    $("#txtCheckAgain").val(checkAgain);
}

//$("#btnSaveQuestion").click(function () {

//});
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
    $('#section-editable').on('click', function () {
        startLoading();
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
                    render: function (data, type, row, meta) {
                        var questionObj = {};
                        var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                        var code = 'Question Code: ' + row.Code + '</p>';
                        var options = '';
                        for (var i = 0; i < row.Options.length; i++) {
                            options = options + '<div id="Option' + i + '" class="container-fluid"></div>';
                        }
                        var images = row.Images;
                        var image = "";
                        if (images != null) {
                            for (var im = 0; im < images.length; im++) {
                                image = image + '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + images[im].Source + '" /></p>';
                            }
                        } else {
                            image = "";
                        }
                        var questionContent = '<div id="q_' + row.Id + '"><div id="Question"></div>' + image + options + '</div>';
                        var result = category + code + questionContent;
                        return result;
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
            fnDrawCallback: function (data) {
                //original question
                var question = data.json.data;
                var q = 0;
                for (q = 0; q < question.length; q++) {
                    var jq = '#q_' + question[q].Id + ' #Question';
                    var changeContent = question[q]["QuestionContent"];
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
                        changeContent = changeContent.split("&lt;b&gt;").join("");
                        changeContent = changeContent.split("&lt;/b&gt;").join("");
                        changeContent = changeContent.split("&lt;span&gt;").join("");
                        changeContent = changeContent.split("&lt;/span&gt;").join("");
                        changeContent = changeContent.split("&lt;u&gt;").join("");
                        changeContent = changeContent.split("&lt;/u&gt;").join("");
                        changeContent = changeContent.split("&lt;i&gt;").join("");
                        changeContent = changeContent.split("&lt;/i&gt;").join("");
                        changeContent = changeContent.split("&lt;sub&gt;").join("<sub>");
                        changeContent = changeContent.split("&lt;/sub&gt;").join("</sub>");
                        changeContent = changeContent.split("&lt;sup&gt;").join("<sup>");
                        changeContent = changeContent.split("&lt;/sup&gt;").join("</sup>");


                        changeContent = changeContent.split("[html]").join("");
                        breakContent = changeContent.split("&lt;cbr&gt;").join("·")
                            .split("<cbr>").join("·")
                            .split("&lt;br&gt;").join("·")
                            .split("<br>").join("·")
                            .split("<br />").join("·")
                            .split("<br/>").join("·")
                            .split("&lt;br /&gt;").join("·")
                            .split("&lt;br/&gt;").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(changeContent);
                    }
                    for (var w = 0; w < breakContent.length; w++) {
                        $(jq).append('<p id="qcontent_' + w + '"></p>');
                        var jqw = '#q_' + question[q].Id + ' #Question #qcontent_' + w;
                        $(jqw).text(breakContent[w]);
                    }
                    breakContent = [];
                    var o = 0;
                    for (o = 0; o < question[q]["Options"].length; o++) {
                        var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        var jo = '#q_' + question[q].Id + ' #Option' + o;
                        var optionContent = question[q]["Options"][o]["OptionContent"];
                        var optionCorrect = question[q]["Options"][o]["IsCorrect"];
                        var optionImages = question[q]["Options"][o]["Images"];
                        if (isHtml) {
                            optionContent = optionContent.split("&lt;p&gt;").join("");
                            optionContent = optionContent.split("&lt;/p&gt;").join("");
                            optionContent = optionContent.split("&lt;span&gt;").join("");
                            optionContent = optionContent.split("&lt;/span&gt;").join("");
                            optionContent = optionContent.split("&lt;b&gt;").join("");
                            optionContent = optionContent.split("&lt;/b&gt;").join("");
                            optionContent = optionContent.split("&lt;span&gt;").join("");
                            optionContent = optionContent.split("&lt;/span&gt;").join("");
                            optionContent = optionContent.split("&lt;u&gt;").join("");
                            optionContent = optionContent.split("&lt;/u&gt;").join("");
                            optionContent = optionContent.split("&lt;i&gt;").join("");
                            optionContent = optionContent.split("&lt;/i&gt;").join("");
                            optionContent = optionContent.split("&lt;sub&gt;").join("<sub>");
                            optionContent = optionContent.split("&lt;/sub&gt;").join("</sub>");
                            optionContent = optionContent.split("&lt;sup&gt;").join("<sup>");
                            optionContent = optionContent.split("&lt;/sup&gt;").join("</sup>");


                            optionContent = optionContent.split("[html]").join("");
                            breakContent = optionContent.split("&lt;cbr&gt;").join("·")
                                .split("<cbr>").join("·")
                                .split("&lt;br&gt;").join("·")
                                .split("<br>").join("·")
                                .split("<br />").join("·")
                                .split("<br/>").join("·")
                                .split("&lt;br /&gt;").join("·")
                                .split("&lt;br/&gt;").join("·");
                            breakContent = breakContent.split("·");
                        } else {
                            breakContent.push(optionContent);
                        }
                        for (var b = 0; b < breakContent.length; b++) {
                            $(jo).append('<p id="ocontent_' + b + '"></p>');
                            var ch = $(jo).length;
                            var jow = '#q_' + question[q].Id + ' #Option' + o + ' #ocontent_' + b;
                            if (b == 0) {
                                $(jow).text(letters[o] + '. ' + breakContent[b]);
                            } else {
                                $(jow).text(breakContent[b]);
                            }
                        }
                        if (optionImages != null) {
                            for (var i = 0; i < optionImages.length; i++) {
                                $(jo).append('<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + optionImages[i].Source + '" /></p>');
                            }
                        }
                        if (optionCorrect) {
                            $(jo).addClass('text-right-answer');
                        }
                    }

                    $("#tableSuccess .delete-question-dt").off('click');
                    $("#tableSuccess .delete-question-dt").on('click', function () {
                        minusTotal($("#total-success"));
                        plusTotal($("#total-delete"));

                        sendAjax($(this).attr('data-url'));
                        $('#section-success').trigger('click');
                    })
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
                    render: function (data, type, row, meta) {
                        var questionObj = {};
                        var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                        var code = 'Question Code: ' + row.Code + '</p>';
                        var options = '';
                        for (var i = 0; i < row.Options.length; i++) {
                            options = options + '<div id="Option' + i + '" class="container-fluid"></div>';
                        }
                        var images = row.Images;
                        var image = "";
                        if (images != null) {
                            for (var im = 0; im < images.length; im++) {
                                image = image + '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + images[im].Source + '" /></p>';
                            }
                        } else {
                            image = "";
                        }
                        var questionContent = '<div id="q_' + row.Id + '"><div id="Question"></div>' + image + options + '</div>';
                        var result = category + code + questionContent;
                        return result;
                    }
                },
                {
                    data: "Message"
                },
                {
                    data: "Id",
                    render: function (data) {
                        var importId = $('#importId').val();
                        var edit = '<a href="/Import/GetQuestionTemp?tempId=' + data + '" class="btn btn-primary mb-2 col-md-12">Edit</a>';
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
            fnDrawCallback: function (data) {
                //original question
                var question = data.json.data;
                var q = 0;
                for (q = 0; q < question.length; q++) {
                    var jq = '#q_' + question[q].Id + ' #Question';
                    var changeContent = question[q]["QuestionContent"];
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
                        changeContent = changeContent.split("&lt;b&gt;").join("");
                        changeContent = changeContent.split("&lt;/b&gt;").join("");
                        changeContent = changeContent.split("&lt;span&gt;").join("");
                        changeContent = changeContent.split("&lt;/span&gt;").join("");
                        changeContent = changeContent.split("&lt;u&gt;").join("");
                        changeContent = changeContent.split("&lt;/u&gt;").join("");
                        changeContent = changeContent.split("&lt;i&gt;").join("");
                        changeContent = changeContent.split("&lt;/i&gt;").join("");
                        changeContent = changeContent.split("&lt;sub&gt;").join("<sub>");
                        changeContent = changeContent.split("&lt;/sub&gt;").join("</sub>");
                        changeContent = changeContent.split("&lt;sup&gt;").join("<sup>");
                        changeContent = changeContent.split("&lt;/sup&gt;").join("</sup>");


                        changeContent = changeContent.split("[html]").join("");
                        breakContent = changeContent.split("&lt;cbr&gt;").join("·")
                            .split("<cbr>").join("·")
                            .split("&lt;br&gt;").join("·")
                            .split("<br>").join("·")
                            .split("<br />").join("·")
                            .split("<br/>").join("·")
                            .split("&lt;br /&gt;").join("·")
                            .split("&lt;br/&gt;").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(changeContent);
                    }
                    for (var w = 0; w < breakContent.length; w++) {
                        $(jq).append('<p id="qcontent_' + w + '"></p>');
                        var jqw = '#q_' + question[q].Id + ' #Question #qcontent_' + w;
                        $(jqw).text(breakContent[w]);
                    }
                    breakContent = [];
                    var o = 0;
                    for (o = 0; o < question[q]["Options"].length; o++) {
                        var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        var jo = '#q_' + question[q].Id + ' #Option' + o;
                        var optionContent = question[q]["Options"][o]["OptionContent"];
                        var optionCorrect = question[q]["Options"][o]["IsCorrect"];
                        var optionImages = question[q]["Options"][o]["Images"];
                        if (optionContent != null) {
                            if (isHtml) {
                                optionContent = optionContent.split("&lt;p&gt;").join("");
                                optionContent = optionContent.split("&lt;/p&gt;").join("");
                                optionContent = optionContent.split("&lt;span&gt;").join("");
                                optionContent = optionContent.split("&lt;/span&gt;").join("");
                                optionContent = optionContent.split("&lt;b&gt;").join("");
                                optionContent = optionContent.split("&lt;/b&gt;").join("");
                                optionContent = optionContent.split("&lt;span&gt;").join("");
                                optionContent = optionContent.split("&lt;/span&gt;").join("");
                                optionContent = optionContent.split("&lt;u&gt;").join("");
                                optionContent = optionContent.split("&lt;/u&gt;").join("");
                                optionContent = optionContent.split("&lt;i&gt;").join("");
                                optionContent = optionContent.split("&lt;/i&gt;").join("");
                                optionContent = optionContent.split("&lt;sub&gt;").join("<sub>");
                                optionContent = optionContent.split("&lt;/sub&gt;").join("</sub>");
                                optionContent = optionContent.split("&lt;sup&gt;").join("<sup>");
                                optionContent = optionContent.split("&lt;/sup&gt;").join("</sup>");


                                optionContent = optionContent.split("[html]").join("");
                                breakContent = optionContent.split("&lt;cbr&gt;").join("·")
                                    .split("<cbr>").join("·")
                                    .split("&lt;br&gt;").join("·")
                                    .split("<br>").join("·")
                                    .split("<br />").join("·")
                                    .split("<br/>").join("·")
                                    .split("&lt;br /&gt;").join("·")
                                    .split("&lt;br/&gt;").join("·");
                                breakContent = breakContent.split("·");
                            } else {
                                breakContent.push(optionContent);
                            }
                            for (var b = 0; b < breakContent.length; b++) {
                                $(jo).append('<p id="ocontent_' + b + '"></p>');
                                var ch = $(jo).length;
                                var jow = '#q_' + question[q].Id + ' #Option' + o + ' #ocontent_' + b;
                                if (b == 0) {
                                    $(jow).text(letters[o] + '. ' + breakContent[b]);
                                } else {
                                    $(jow).text(breakContent[b]);
                                }
                            }
                        }
                        if (optionImages != null) {
                            for (var i = 0; i < optionImages.length; i++) {
                                $(jo).append('<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + optionImages[i].Source + '" /></p>');
                            }
                        }
                        if (optionCorrect) {
                            $(jo).addClass('text-right-answer');
                        }
                    }

                    $("#tableInvalid .delete-question-dt").off('click');
                    $("#tableInvalid .delete-question-dt").on('click', function () {
                        minusTotal($("#total-invalid"));
                        plusTotal($("#total-delete"));

                        sendAjax($(this).attr('data-url'));
                        $('#section-invalid').trigger('click');
                    })
                }
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
                    render: function (data, type, row, meta) {
                        var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                        var code = 'Question Code: ' + row.Code + '</p>';
                        var options = '';
                        for (var i = 0; i < row.Options.length; i++) {
                            options = options + '<div id="Option' + i + '" class="container-fluid"></div>';
                        }
                        var images = row.Images;
                        var image = "";
                        if (images != null) {
                            for (var im = 0; im < images.length; im++) {
                                image = image + '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + images[im].Source + '" /></p>';
                            }
                        } else {
                            image = "";
                        }
                        var questionContent = '<div id="q_' + row.Id + '"><div id="Question"></div>' + image + options + '</div>';
                        var result = category + code + questionContent;
                        return result;
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
            fnDrawCallback: function (data) {
                var question = data.json.data;
                var q = 0;
                for (q = 0; q < question.length; q++) {
                    var jq = '#q_' + question[q].Id + ' #Question';
                    var changeContent = question[q]["QuestionContent"];
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
                        changeContent = changeContent.split("&lt;b&gt;").join("");
                        changeContent = changeContent.split("&lt;/b&gt;").join("");
                        changeContent = changeContent.split("&lt;span&gt;").join("");
                        changeContent = changeContent.split("&lt;/span&gt;").join("");
                        changeContent = changeContent.split("&lt;u&gt;").join("");
                        changeContent = changeContent.split("&lt;/u&gt;").join("");
                        changeContent = changeContent.split("&lt;i&gt;").join("");
                        changeContent = changeContent.split("&lt;/i&gt;").join("");
                        changeContent = changeContent.split("&lt;sub&gt;").join("<sub>");
                        changeContent = changeContent.split("&lt;/sub&gt;").join("</sub>");
                        changeContent = changeContent.split("&lt;sup&gt;").join("<sup>");
                        changeContent = changeContent.split("&lt;/sup&gt;").join("</sup>");


                        changeContent = changeContent.split("[html]").join("");
                        breakContent = changeContent.split("&lt;cbr&gt;").join("·")
                            .split("<cbr>").join("·")
                            .split("&lt;br&gt;").join("·")
                            .split("<br>").join("·")
                            .split("<br />").join("·")
                            .split("<br/>").join("·")
                            .split("&lt;br /&gt;").join("·")
                            .split("&lt;br/&gt;").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(changeContent);
                    }
                    for (var w = 0; w < breakContent.length; w++) {
                        $(jq).append('<p id="qcontent_' + w + '"></p>');
                        var jqw = '#q_' + question[q].Id + ' #Question #qcontent_' + w;
                        $(jqw).text(breakContent[w]);
                    }
                    breakContent = [];
                    var o = 0;
                    for (o = 0; o < question[q]["Options"].length; o++) {
                        var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                        var jo = '#q_' + question[q].Id + ' #Option' + o;
                        var optionContent = question[q]["Options"][o]["OptionContent"];
                        var optionCorrect = question[q]["Options"][o]["IsCorrect"];
                        var optionImages = question[q]["Options"][o]["Images"];
                        if (isHtml) {
                            optionContent = optionContent.split("&lt;p&gt;").join("");
                            optionContent = optionContent.split("&lt;/p&gt;").join("");
                            optionContent = optionContent.split("&lt;span&gt;").join("");
                            optionContent = optionContent.split("&lt;/span&gt;").join("");
                            optionContent = optionContent.split("&lt;b&gt;").join("");
                            optionContent = optionContent.split("&lt;/b&gt;").join("");
                            optionContent = optionContent.split("&lt;span&gt;").join("");
                            optionContent = optionContent.split("&lt;/span&gt;").join("");
                            optionContent = optionContent.split("&lt;u&gt;").join("");
                            optionContent = optionContent.split("&lt;/u&gt;").join("");
                            optionContent = optionContent.split("&lt;i&gt;").join("");
                            optionContent = optionContent.split("&lt;/i&gt;").join("");
                            optionContent = optionContent.split("&lt;sub&gt;").join("<sub>");
                            optionContent = optionContent.split("&lt;/sub&gt;").join("</sub>");
                            optionContent = optionContent.split("&lt;sup&gt;").join("<sup>");
                            optionContent = optionContent.split("&lt;/sup&gt;").join("</sup>");


                            optionContent = optionContent.split("[html]").join("");
                            breakContent = optionContent.split("&lt;cbr&gt;").join("·")
                                .split("<cbr>").join("·")
                                .split("&lt;br&gt;").join("·")
                                .split("<br>").join("·")
                                .split("<br />").join("·")
                                .split("<br/>").join("·")
                                .split("&lt;br /&gt;").join("·")
                                .split("&lt;br/&gt;").join("·");
                            breakContent = breakContent.split("·");
                        } else {
                            breakContent.push(optionContent);
                        }
                        for (var b = 0; b < breakContent.length; b++) {
                            $(jo).append('<p id="ocontent_' + b + '"></p>');
                            var ch = $(jo).length;
                            var jow = '#q_' + question[q].Id + ' #Option' + o + ' #ocontent_' + b;
                            if (b == 0) {
                                $(jow).text(letters[o] + '. ' + breakContent[b]);
                            } else {
                                $(jow).text(breakContent[b]);
                            }
                        }
                        if (optionImages != null) {
                            for (var i = 0; i < optionImages.length; i++) {
                                $(jo).append('<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + optionImages[i].Source + '" /></p>');
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

    $('#section-bank-editable').on('click', function () {
        startLoading();
        var templateEditable = '<table class="table table-bordered table-hover text-custom" id="tableBankEditable" width="100%" cellspacing="0">' +
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
        $('#tableBankEditable').DataTable().destroy();
        $('#tableBankEditable').remove();
        $('#editable #importTable').append(templateEditable);
        $('#editable').show();
        this.table1 = initTableBankEditable();

        this.table1.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });

        stopLoading();
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
    if (value <= 0) {
        totalSpan.text(0);
    } else {
        totalSpan.text(value - 1);
    }
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
    initTableBankEditable();
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
                render: function (data, type, row, meta) {
                    if (row != null) {

                        //notify if not image
                        var result = "";
                        if (row.IsNotImage) {
                            result = "<p class='text-danger'>This question may be has an image</p>";
                        }

                        var questionObj = {};
                        var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                        var code = '<p>Question Code: ' + row.Code + '</p>';
                        var options = '';
                        for (var i = 0; i < row.Options.length; i++) {
                            options = options + '<div id="Option' + i + '" class="container-fluid"></div>';
                        }
                        var images = row.Images;
                        var image = "";
                        if (images != null) {
                            for (var im = 0; im < images.length; im++) {
                                image = image + '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + images[im].Source + '" /></p>';
                            }
                        } else {
                            image = "";
                        }
                        var questionContent = '<div id="q_' + row.Id + '"><div id="Question"></div>' + image + options + '</div>';
                        var editButton = '<a href="/Import/GetQuestionTemp?tempId=' + row.Id + '" class="btn btn-primary ml-1 float-right">Edit</a>';
                        var acceptButton = '<button class="btn btn-success ml-1 accept-question-dt float-right" data-url="/Import/Skip?questionId=' + row.Id + '&url=' + window.location.href + '">Accept</button>';
                        var deleteButton = '<button class="btn btn-danger delete-question-dt float-right" data-url="/Import/Delete?questionId=' + row.Id + '&url=' + window.location.href + '">Delete</button>';

                        result = result + category + code + questionContent;

                        if (row.DuplicatedQuestion != null || row.Message.length == 0) {
                            result = result + '<div class="row mt-5"><div class="col-md-12 bottom-right-cell">' + editButton + acceptButton + deleteButton + '</div></div>';
                        }
                        return result;
                    }
                }
            },
            {
                render: function (data, type, row) {
                    if (row != null) {
                        if (row.DuplicatedQuestion != null) {

                            //notify if not image
                            var result = "";
                            if (row.IsNotImage) {
                                result = "<p class='text-danger'>This question may be has an image</p>";
                            }

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
                            var options = '';
                            for (var i = 0; i < row.Options.length; i++) {
                                options = options + '<div id="Option' + i + '" class="container-fluid"></div>';
                            }
                            var images = row.DuplicatedQuestion.Images;
                            var image = "";
                            if (images != null) {
                                for (var im = 0; im < images.length; im++) {
                                    image = image + '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + images[im].Source + '" /></p>';
                                }
                            } else {
                                image = "";
                            }
                            var questionContent = '<div id="d_' + row.Id + '"><div id="Question"></div>' + image + options + '</div>';
                            var editButton = '<a href="/Import/GetQuestionTemp?tempId=' + row.DuplicatedQuestion.Id + '" class="btn btn-primary float-right ml-1">Edit</a>';
                            var acceptButton = '<button class="btn btn-success ml-1 accept-question-dt float-right" data-url="/Import/Skip?questionId=' + row.DuplicatedQuestion.Id + '&url=' + window.location.href + '">Accept</button>';
                            var deleteButton = '<button class="btn btn-danger float-right delete-question-dt"  data-url="/Import/Delete?questionId=' + row.DuplicatedQuestion.Id + '&url=' + window.location.href + '">Delete</button>';

                            result = result + status + category + code + questionContent;
                            if (!row.DuplicatedQuestion.IsBank && !row.DuplicatedQuestion.IsAnotherImport && row.DuplicatedQuestion.Status == 2) {
                                result = result + '<div class="row"><div class=" col-md-12 bottom-right-cell">' + editButton + acceptButton + deleteButton + '</div></div>';
                            }
                            return result;

                        }
                        else {
                            //notify if not image
                            var result = "";
                            if (row.IsNotImage) {
                                result = "<p>There is no duplicate</p>";
                            } else {
                                result = row.Message + '<br/> <a href="/Import/GetDuplicatedDetail/' + row.Id + '" class="text-info btn-link font-weight-bold" > See more</a >';
                            }
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

        fnDrawCallback: function (data) {
            //original question
            var question = data.json.data;
            var q = 0;
            for (q = 0; q < question.length; q++) {
                var jq = '#q_' + question[q].Id + ' #Question';
                var changeContent = question[q]["QuestionContent"];
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
                    changeContent = changeContent.split("&lt;b&gt;").join("");
                    changeContent = changeContent.split("&lt;/b&gt;").join("");
                    changeContent = changeContent.split("&lt;span&gt;").join("");
                    changeContent = changeContent.split("&lt;/span&gt;").join("");
                    changeContent = changeContent.split("&lt;u&gt;").join("");
                    changeContent = changeContent.split("&lt;/u&gt;").join("");
                    changeContent = changeContent.split("&lt;i&gt;").join("");
                    changeContent = changeContent.split("&lt;/i&gt;").join("");
                    changeContent = changeContent.split("&lt;sub&gt;").join("<sub>");
                    changeContent = changeContent.split("&lt;/sub&gt;").join("</sub>");
                    changeContent = changeContent.split("&lt;sup&gt;").join("<sup>");
                    changeContent = changeContent.split("&lt;/sup&gt;").join("</sup>");


                    changeContent = changeContent.split("[html]").join("");
                    breakContent = changeContent.split("&lt;cbr&gt;").join("·")
                        .split("<cbr>").join("·")
                        .split("&lt;br&gt;").join("·")
                        .split("<br>").join("·")
                        .split("<br />").join("·")
                        .split("<br/>").join("·")
                        .split("&lt;br /&gt;").join("·")
                        .split("&lt;br/&gt;").join("·");
                    breakContent = breakContent.split("·");
                } else {
                    breakContent.push(changeContent);
                }
                for (var w = 0; w < breakContent.length; w++) {
                    $(jq).append('<p id="qcontent_' + w + '"></p>');
                    var jqw = '#q_' + question[q].Id + ' #Question #qcontent_' + w;
                    $(jqw).text(breakContent[w]);
                }
                breakContent = [];
                var o = 0;
                for (o = 0; o < question[q]["Options"].length; o++) {
                    var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    var jo = '#q_' + question[q].Id + ' #Option' + o;
                    var optionContent = question[q]["Options"][o]["OptionContent"];
                    var optionCorrect = question[q]["Options"][o]["IsCorrect"];
                    var optionImages = question[q]["Options"][o]["Images"];
                    if (isHtml) {
                        optionContent = optionContent.split("&lt;p&gt;").join("");
                        optionContent = optionContent.split("&lt;/p&gt;").join("");
                        optionContent = optionContent.split("&lt;span&gt;").join("");
                        optionContent = optionContent.split("&lt;/span&gt;").join("");
                        optionContent = optionContent.split("&lt;b&gt;").join("");
                        optionContent = optionContent.split("&lt;/b&gt;").join("");
                        optionContent = optionContent.split("&lt;span&gt;").join("");
                        optionContent = optionContent.split("&lt;/span&gt;").join("");
                        optionContent = optionContent.split("&lt;u&gt;").join("");
                        optionContent = optionContent.split("&lt;/u&gt;").join("");
                        optionContent = optionContent.split("&lt;i&gt;").join("");
                        optionContent = optionContent.split("&lt;/i&gt;").join("");
                        optionContent = optionContent.split("&lt;sub&gt;").join("<sub>");
                        optionContent = optionContent.split("&lt;/sub&gt;").join("</sub>");
                        optionContent = optionContent.split("&lt;sup&gt;").join("<sup>");
                        optionContent = optionContent.split("&lt;/sup&gt;").join("</sup>");


                        optionContent = optionContent.split("[html]").join("");
                        breakContent = optionContent.split("&lt;cbr&gt;").join("·")
                            .split("<cbr>").join("·")
                            .split("&lt;br&gt;").join("·")
                            .split("<br>").join("·")
                            .split("<br />").join("·")
                            .split("<br/>").join("·")
                            .split("&lt;br /&gt;").join("·")
                            .split("&lt;br/&gt;").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(optionContent);
                    }
                    for (var b = 0; b < breakContent.length; b++) {
                        $(jo).append('<p id="ocontent_' + b + '"></p>');
                        var ch = $(jo).length;
                        var jow = '#q_' + question[q].Id + ' #Option' + o + ' #ocontent_' + b;
                        if (b == 0) {
                            $(jow).text(letters[o] + '. ' + breakContent[b]);
                        } else {
                            $(jow).text(breakContent[b]);
                        }
                    }
                    if (optionImages != null) {
                        for (var i = 0; i < optionImages.length; i++) {
                            $(jo).append('<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + optionImages[i].Source + '" /></p>');
                        }
                    }
                    if (optionCorrect) {
                        $(jo).addClass('text-right-answer');
                    }
                }

                //duplicate question
                var duplicate = question[q].DuplicatedQuestion;
                var jd = '#d_' + question[q].Id + ' #Question';
                if (duplicate != null) {
                    var changeduplicateContent = duplicate.QuestionContent;
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
                        changeduplicateContent = changeduplicateContent.split("&lt;b&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/b&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;span&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/span&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;u&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/u&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;i&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/i&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;sub&gt;").join("<sub>");
                        changeduplicateContent = changeduplicateContent.split("&lt;/sub&gt;").join("</sub>");
                        changeduplicateContent = changeduplicateContent.split("&lt;sup&gt;").join("<sup>");
                        changeduplicateContent = changeduplicateContent.split("&lt;/sup&gt;").join("</sup>");


                        changeduplicateContent = changeduplicateContent.split("[html]").join("");
                        breakContent = changeduplicateContent.split("&lt;cbr&gt;").join("·")
                            .split("<cbr>").join("·")
                            .split("&lt;br&gt;").join("·")
                            .split("<br>").join("·")
                            .split("<br />").join("·")
                            .split("<br/>").join("·")
                            .split("&lt;br /&gt;").join("·")
                            .split("&lt;br/&gt;").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(changeduplicateContent);
                    }
                    for (var f = 0; f < breakContent.length; f++) {
                        $(jd).append('<p id="dcontent_' + f + '"></p>');
                        var jdf = '#d_' + question[q].Id + ' #Question #dcontent_' + f;
                        $(jdf).text(breakContent[f]);
                    }
                    breakContent = [];
                    for (o = 0; o < duplicate["Options"].length; o++) {
                        var jod = '#d_' + question[q].Id + ' #Option' + o;
                        var dupOptionContent = duplicate["Options"][o]["OptionContent"];
                        var dupOptionCorrect = duplicate["Options"][o]["IsCorrect"];
                        var dupOptionImages = duplicate["Options"][o]["Images"];
                        if (isHtml) {
                            dupOptionContent = dupOptionContent.split("&lt;p&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/p&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;b&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/b&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;u&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/u&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;i&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/i&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;sub&gt;").join("<sub>");
                            dupOptionContent = dupOptionContent.split("&lt;/sub&gt;").join("</sub>");
                            dupOptionContent = dupOptionContent.split("&lt;sup&gt;").join("<sup>");
                            dupOptionContent = dupOptionContent.split("&lt;/sup&gt;").join("</sup>");


                            dupOptionContent = dupOptionContent.split("[html]").join("");
                            breakContent = dupOptionContent.split("&lt;cbr&gt;").join("·")
                                .split("<cbr>").join("·")
                                .split("&lt;br&gt;").join("·")
                                .split("<br>").join("·")
                                .split("<br />").join("·")
                                .split("<br/>").join("·")
                                .split("&lt;br /&gt;").join("·")
                                .split("&lt;br/&gt;").join("·");
                            breakContent = breakContent.split("·");
                        } else {
                            breakContent.push(dupOptionContent);
                        }
                        for (var c = 0; c < breakContent.length; c++) {
                            $(jod).append('<p id="ocontent_' + c + '"></p>');
                            var jodw = '#d_' + question[q].Id + ' #Option' + o + ' #ocontent_' + c;
                            if (c == 0) {
                                $(jodw).text(letters[o] + '. ' + breakContent[c]);
                            } else {
                                $(jodw).text(breakContent[c]);
                            }
                        }
                        if (dupOptionImages != null) {
                            for (var i = 0; i < dupOptionImages.length; i++) {
                                $(jod).append('<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + optionImages[i].Source + '" /></p>');
                            }
                        }
                        if (dupOptionCorrect) {
                            $(jod).addClass('text-right-answer');
                        }
                    }
                }

            }

            $("#tableEditable .delete-question-dt").off('click');
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

function initTableBankEditable() {

    var table1 = $('#tableBankEditable').DataTable({
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
            url: "/Question/GetQuestionByCourseIdAndType",
            type: "GET",
            data: {
                courseId: $('#courseId').val(),
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
                render: function (data, type, row, meta) {
                    if (row != null) {

                        //notify if not image
                        var result = "";
                        if (row.IsNotImage) {
                            result = "<p class='text-danger'>This question may be has an image</p>";
                        }

                        var questionObj = {};
                        var category = '<p class="text-custom">Category: ' + row.Category + '<br/>';
                        var code = '<p>Question Code: ' + row.Code + '</p>';

                        var status = "";
                        var statusClass = "";
                        var statusName = "";
                        switch (row.Status) {
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
                        status = '<span class="badge ml-2 ' + statusClass + '">' + statusName + '</span>';

                        var options = '';
                        for (var i = 0; i < row.Options.length; i++) {
                            options = options + '<div id="Option' + i + '" class="container-fluid"></div>';
                        }
                        var images = row.Images;
                        var image = "";
                        if (images != null) {
                            for (var im = 0; im < images.length; im++) {
                                image = image + '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + images[im].Source + '" /></p>';
                            }
                        } else {
                            image = "";
                        }
                        var questionContent = '<div id="q_' + row.Id + '"><div id="Question"></div>' + image + options + '</div>';
                        var editButton = '<a href="/Question/GetQuestionDetail?id=' + row.Id + '" class="btn btn-primary ml-1 float-right">Edit</a>';
                        var acceptButton = '<button class="btn btn-success ml-1 accept-question-dt float-right" data-url="/Question/Skip?questionId=' + row.Id + '&url=' + window.location.href + '">Accept</button>';
                        var deleteButton = '<button class="btn btn-danger delete-question-dt float-right" data-url="/Question/Delete?questionId=' + row.Id + '&url=' + window.location.href + '">Delete</button>';

                        result = result + status + category + code + questionContent;

                        if (row.DuplicatedQuestion != null || row.Message.length == 0) {
                            result = result + '<div class="row mt-5"><div class="col-md-12 bottom-right-cell">' + editButton + acceptButton + deleteButton + '</div></div>';
                        }
                        return result;
                    }
                }
            },
            {
                render: function (data, type, row) {
                    if (row != null) {
                        if (row.DuplicatedQuestion != null) {

                            //notify if not image
                            var result = "";
                            if (row.IsNotImage) {
                                result = "<p class='text-danger'>This question may be has an image</p>";
                            }

                            var questionObj = {};
                            var category = '<p class="text-custom">Category: ' + row.DuplicatedQuestion.CourseName + '<br/>';
                            var code = ' <p class="text-custom">Question Code: ' + row.DuplicatedQuestion.Code + '</p>';

                            var status = "";
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
                            status = '<span class="badge ml-2 ' + statusClass + '">' + statusName + '</span>';

                            var options = '';
                            for (var i = 0; i < row.Options.length; i++) {
                                options = options + '<div id="Option' + i + '" class="container-fluid"></div>';
                            }
                            var images = row.DuplicatedQuestion.Images;
                            var image = "";
                            if (images != null) {
                                for (var im = 0; im < images.length; im++) {
                                    image = image + '<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + images[im].Source + '" /></p>';
                                }
                            } else {
                                image = "";
                            }
                            var questionContent = '<div id="d_' + row.Id + '"><div id="Question"></div>' + image + options + '</div>';
                            var editButton = '<a href="/Question/GetQuestionDetail?id=' + row.DuplicatedQuestion.Id + '" class="btn btn-primary float-right ml-1">Edit</a>';
                            var acceptButton = '<button class="btn btn-success ml-1 accept-question-dt float-right" data-url="/Question/Skip?questionId=' + row.DuplicatedQuestion.Id + '&url=' + window.location.href + '">Accept</button>';
                            var deleteButton = '<button class="btn btn-danger float-right delete-question-dt"  data-url="/Question/Delete?questionId=' + row.DuplicatedQuestion.Id + '&url=' + window.location.href + '">Delete</button>';

                            result = result + status + category + code + questionContent;
                            result = result + '<div class="row"><div class=" col-md-12 bottom-right-cell">' + editButton + acceptButton + deleteButton + '</div></div>';
                            return result;

                        }
                        else {
                            //notify if not image
                            var result = "";
                            if (row.IsNotImage) {
                                result = "<p>There is no duplicate</p>";
                            } else {
                                result = row.Message + '<br/> <a href="/Question/GetDuplicatedDetail/' + row.Id + '" class="text-info btn-link font-weight-bold" > See more</a >';
                            }
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

        fnDrawCallback: function (data) {
            //original question
            var question = data.json.data;
            var q = 0;
            for (q = 0; q < question.length; q++) {
                var jq = '#q_' + question[q].Id + ' #Question';
                var changeContent = question[q]["QuestionContent"];
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
                    changeContent = changeContent.split("&lt;b&gt;").join("");
                    changeContent = changeContent.split("&lt;/b&gt;").join("");
                    changeContent = changeContent.split("&lt;span&gt;").join("");
                    changeContent = changeContent.split("&lt;/span&gt;").join("");
                    changeContent = changeContent.split("&lt;u&gt;").join("");
                    changeContent = changeContent.split("&lt;/u&gt;").join("");
                    changeContent = changeContent.split("&lt;i&gt;").join("");
                    changeContent = changeContent.split("&lt;/i&gt;").join("");
                    changeContent = changeContent.split("&lt;sub&gt;").join("<sub>");
                    changeContent = changeContent.split("&lt;/sub&gt;").join("</sub>");
                    changeContent = changeContent.split("&lt;sup&gt;").join("<sup>");
                    changeContent = changeContent.split("&lt;/sup&gt;").join("</sup>");


                    changeContent = changeContent.split("[html]").join("");
                    breakContent = changeContent.split("&lt;cbr&gt;").join("·")
                        .split("<cbr>").join("·")
                        .split("&lt;br&gt;").join("·")
                        .split("<br>").join("·")
                        .split("<br />").join("·")
                        .split("<br/>").join("·")
                        .split("&lt;br /&gt;").join("·")
                        .split("&lt;br/&gt;").join("·");
                    breakContent = breakContent.split("·");
                } else {
                    breakContent.push(changeContent);
                }
                for (var w = 0; w < breakContent.length; w++) {
                    $(jq).append('<p id="qcontent_' + w + '"></p>');
                    var jqw = '#q_' + question[q].Id + ' #Question #qcontent_' + w;
                    $(jqw).text(breakContent[w]);
                }
                breakContent = [];
                var o = 0;
                for (o = 0; o < question[q]["Options"].length; o++) {
                    var letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    var jo = '#q_' + question[q].Id + ' #Option' + o;
                    var optionContent = question[q]["Options"][o]["OptionContent"];
                    var optionCorrect = question[q]["Options"][o]["IsCorrect"];
                    var optionImages = question[q]["Options"][o]["Images"];
                    if (isHtml) {
                        optionContent = optionContent.split("&lt;p&gt;").join("");
                        optionContent = optionContent.split("&lt;/p&gt;").join("");
                        optionContent = optionContent.split("&lt;span&gt;").join("");
                        optionContent = optionContent.split("&lt;/span&gt;").join("");
                        optionContent = optionContent.split("&lt;b&gt;").join("");
                        optionContent = optionContent.split("&lt;/b&gt;").join("");
                        optionContent = optionContent.split("&lt;span&gt;").join("");
                        optionContent = optionContent.split("&lt;/span&gt;").join("");
                        optionContent = optionContent.split("&lt;u&gt;").join("");
                        optionContent = optionContent.split("&lt;/u&gt;").join("");
                        optionContent = optionContent.split("&lt;i&gt;").join("");
                        optionContent = optionContent.split("&lt;/i&gt;").join("");
                        optionContent = optionContent.split("&lt;sub&gt;").join("<sub>");
                        optionContent = optionContent.split("&lt;/sub&gt;").join("</sub>");
                        optionContent = optionContent.split("&lt;sup&gt;").join("<sup>");
                        optionContent = optionContent.split("&lt;/sup&gt;").join("</sup>");


                        optionContent = optionContent.split("[html]").join("");
                        breakContent = optionContent.split("&lt;cbr&gt;").join("·")
                            .split("<cbr>").join("·")
                            .split("&lt;br&gt;").join("·")
                            .split("<br>").join("·")
                            .split("<br />").join("·")
                            .split("<br/>").join("·")
                            .split("&lt;br /&gt;").join("·")
                            .split("&lt;br/&gt;").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(optionContent);
                    }
                    for (var b = 0; b < breakContent.length; b++) {
                        $(jo).append('<p id="ocontent_' + b + '"></p>');
                        var ch = $(jo).length;
                        var jow = '#q_' + question[q].Id + ' #Option' + o + ' #ocontent_' + b;
                        if (b == 0) {
                            $(jow).text(letters[o] + '. ' + breakContent[b]);
                        } else {
                            $(jow).text(breakContent[b]);
                        }
                    }
                    if (optionImages != null) {
                        for (var i = 0; i < optionImages.length; i++) {
                            $(jo).append('<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + optionImages[i].Source + '" /></p>');
                        }
                    }
                    if (optionCorrect) {
                        $(jo).addClass('text-right-answer');
                    }
                }

                //duplicate question
                var duplicate = question[q].DuplicatedQuestion;
                var jd = '#d_' + question[q].Id + ' #Question';
                if (duplicate != null) {
                    var changeduplicateContent = duplicate.QuestionContent;
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
                        changeduplicateContent = changeduplicateContent.split("&lt;b&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/b&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;span&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/span&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;u&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/u&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;i&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;/i&gt;").join("");
                        changeduplicateContent = changeduplicateContent.split("&lt;sub&gt;").join("<sub>");
                        changeduplicateContent = changeduplicateContent.split("&lt;/sub&gt;").join("</sub>");
                        changeduplicateContent = changeduplicateContent.split("&lt;sup&gt;").join("<sup>");
                        changeduplicateContent = changeduplicateContent.split("&lt;/sup&gt;").join("</sup>");


                        changeduplicateContent = changeduplicateContent.split("[html]").join("");
                        breakContent = changeduplicateContent.split("&lt;cbr&gt;").join("·")
                            .split("<cbr>").join("·")
                            .split("&lt;br&gt;").join("·")
                            .split("<br>").join("·")
                            .split("<br />").join("·")
                            .split("<br/>").join("·")
                            .split("&lt;br /&gt;").join("·")
                            .split("&lt;br/&gt;").join("·");
                        breakContent = breakContent.split("·");
                    } else {
                        breakContent.push(changeduplicateContent);
                    }
                    for (var f = 0; f < breakContent.length; f++) {
                        $(jd).append('<p id="dcontent_' + f + '"></p>');
                        var jdf = '#d_' + question[q].Id + ' #Question #dcontent_' + f;
                        $(jdf).text(breakContent[f]);
                    }
                    breakContent = [];
                    for (o = 0; o < duplicate["Options"].length; o++) {
                        var jod = '#d_' + question[q].Id + ' #Option' + o;
                        var dupOptionContent = duplicate["Options"][o]["OptionContent"];
                        var dupOptionCorrect = duplicate["Options"][o]["IsCorrect"];
                        var dupOptionImages = duplicate["Options"][o]["Images"];
                        if (isHtml) {
                            dupOptionContent = dupOptionContent.split("&lt;p&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/p&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;b&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/b&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/span&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;u&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/u&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;i&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;/i&gt;").join("");
                            dupOptionContent = dupOptionContent.split("&lt;sub&gt;").join("<sub>");
                            dupOptionContent = dupOptionContent.split("&lt;/sub&gt;").join("</sub>");
                            dupOptionContent = dupOptionContent.split("&lt;sup&gt;").join("<sup>");
                            dupOptionContent = dupOptionContent.split("&lt;/sup&gt;").join("</sup>");


                            dupOptionContent = dupOptionContent.split("[html]").join("");
                            breakContent = dupOptionContent.split("&lt;cbr&gt;").join("·")
                                .split("<cbr>").join("·")
                                .split("&lt;br&gt;").join("·")
                                .split("<br>").join("·")
                                .split("<br />").join("·")
                                .split("<br/>").join("·")
                                .split("&lt;br /&gt;").join("·")
                                .split("&lt;br/&gt;").join("·");
                            breakContent = breakContent.split("·");
                        } else {
                            breakContent.push(dupOptionContent);
                        }
                        for (var c = 0; c < breakContent.length; c++) {
                            $(jod).append('<p id="ocontent_' + c + '"></p>');
                            var jodw = '#d_' + question[q].Id + ' #Option' + o + ' #ocontent_' + c;
                            if (c == 0) {
                                $(jodw).text(letters[o] + '. ' + breakContent[c]);
                            } else {
                                $(jodw).text(breakContent[c]);
                            }
                        }
                        if (dupOptionImages != null) {
                            for (var i = 0; i < dupOptionImages.length; i++) {
                                $(jod).append('<p><img class="exam-image" onclick="img_zoom(this)" src="data:image/png;base64, ' + optionImages[i].Source + '" /></p>');
                            }
                        }
                        if (dupOptionCorrect) {
                            $(jod).addClass('text-right-answer');
                        }
                    }
                }

            }

            $("#tableEditable .delete-question-dt").off('click');
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