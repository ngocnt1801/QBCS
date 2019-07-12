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
        $('#label-import').addClass('active');
        $('#label-bank').removeClass('active');
        $('#editable1').show();
        $('#editable2').hide();
        var table1 = $('#tableEditable1').DataTable({
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
                    type: "editable1"
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
                            var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                            var category = '<p class="text-custom">Category' + row.Category + '<br/>';
                            var code = 'Question Code: ' + row.Code + '</p>';
                            var questionContent = changeHtml(row.QuestionContent) + '<br/>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                if (options[i]['correct']) {
                                    result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                } else {
                                    result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                }
                            }
                            return result;
                        }
                    }
                },
                {
                    data: "DuplicatedQuestion",
                    render: function (data, type, row) {
                        if (data != null) {
                            var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                            var category = '<p> </p>';
                            var code = ' <p class="text-custom">' + data.CourseName + data.Code + '</p>';
                            var questionContent = changeHtml(data.QuestionContent) + '<br/>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                if (options[i]['correct']) {
                                    result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                } else {
                                    result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                }
                            }
                            return result;
                        }
                    }
                },
                {
                    data: "Id",
                    render: function (data) {
                        var importId = $('#importId').val();
                        var edit = '<a href="/Import/GetQuestionTemp/' + data + '" class="btn btn-primary mb-2 col-md-12">Edit</a>';
                        var accept = '<a href="/Import/Skip?questionId=' + data + '&importId=' + importId + '" class="btn btn-success mb-2 col-md-12">Accept</a>';
                        var deleteQ = '<a href="/Import/Delete?questionId=' + data + '&importId=' + importId + '" class="btn btn-danger col-md-12">Delete</a>';
                        var result = edit + accept + deleteQ;
                        return result;
                    }
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "44%" },
                { targets: 2, width: "44%" },
                { targets: 3, width: "10%" }
            ]
        });

        table1.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });
    
    $('#label-import').on('click', function () {
        $('#label-import').addClass('active');
        $('#label-bank').removeClass('active');
        $('#editable1').show();
        $('#editable2').hide();
        var table1 = $('#tableEditable1').DataTable({
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
                    type: "editable1"
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
                            var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                            var category = '<p class="text-custom">Category' + row.Category + '<br/>';
                            var code = 'Question Code: ' + row.Code + '</p>';
                            var questionContent = changeHtml(row.QuestionContent) + '<br/>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                if (options[i]['correct']) {
                                    result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                } else {
                                    result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                }
                            }
                            return result;
                        }
                    }
                },
                {
                    data: "DuplicatedQuestion",
                    render: function (data, type, row) {
                        if (data != null) {
                            var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                            var category = '<p> </p>';
                            var code = ' <p class="text-custom">' + data.CourseName + data.Code + '</p>';
                            var questionContent = changeHtml(data.QuestionContent) + '<br/>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                if (options[i]['correct']) {
                                    result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                } else {
                                    result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                }
                            }
                            return result;
                        }
                    }
                },
                {
                    data: "Id",
                    render: function (data) {
                        var importId = $('#importId').val();
                        var edit = '<a href="/Import/GetQuestionTemp/' + data + '" class="btn btn-primary mb-2 col-md-12">Edit</a>';
                        var accept = '<a href="/Import/Skip?questionId=' + data + '&importId=' + importId + '" class="btn btn-success mb-2 col-md-12">Accept</a>';
                        var deleteQ = '<a href="/Import/Delete?questionId=' + data + '&importId=' + importId + '" class="btn btn-danger col-md-12">Delete</a>';
                        var result = edit + accept + deleteQ;
                        return result;
                    }
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "44%" },
                { targets: 2, width: "44%" },
                { targets: 3, width: "10%" }
            ]
        });

        table1.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });

    $('#label-bank').on('click', function () {
        $('#label-bank').addClass('active');
        $('#label-import').removeClass('active');
        $('#editable2').show();
        $('#editable1').hide();
        var table2 = $('#tableEditable2').DataTable({
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
                    type: "editable2"
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
                            var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                            var category = '<p class="text-custom">Category' + row.Category + '<br/>';
                            var code = 'Question Code: ' + row.Code + '</p>';
                            var questionContent = changeHtml(row.QuestionContent) + '<br/>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                if (options[i]['correct']) {
                                    result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                } else {
                                    result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                }
                            }
                            return result;
                        }
                    }
                },
                {
                    data: "DuplicatedQuestion",
                    render: function (data, type, row) {
                        if (data != null) {
                            var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                            var category = '<p> </p>';
                            var code = ' <p class="text-custom">' + data.CourseName + data.Code + '</p>';
                            var questionContent = changeHtml(data.QuestionContent) + '<br/>';
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
                            var result = category + code + questionContent + image;
                            for (i = 0; i < options.length; i++) {
                                if (options[i]['correct']) {
                                    result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                } else {
                                    result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                                }
                            }
                            return result;
                        }
                        return null;
                    }
                },
                {
                    data: "Id",
                    render: function (data) {
                        var importId = $('#importId').val();
                        var edit = '<a href="/Import/GetQuestionTemp/' + data + '" class="btn btn-primary mb-2 col-md-12">Edit</a>';
                        var accept = '<a href="/Import/Skip?questionId=' + data + '&importId=' + importId + '" class="btn btn-success mb-2 col-md-12">Accept</a>';
                        var deleteQ = '<a href="/Import/Delete?questionId=' + data + '&importId=' + importId + '" class="btn btn-danger col-md-12">Delete</a>';
                        var result = edit + accept + deleteQ;
                        return result;
                    }
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "44%" },
                { targets: 2, width: "44%" },
                { targets: 3, width: "10%" }
            ]
        });
        table2.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });

    $('#section-success').on('click', function () {
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
                        var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                        var category = '<p class="text-custom">Category' + row.Category + '<br/>';
                        var code = 'Question Code: ' + row.Code + '</p>';
                        var questionContent = changeHtml(row.QuestionContent) + '<br/>';
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
                        var result = category + code + questionContent + image;
                        for (i = 0; i < options.length; i++) {
                            if (options[i]['correct']) {
                                result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                            } else {
                                result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                            }
                        }
                        return result;
                    }
                }
            ],
            columnDefs: [
                { targets: 0, width: "2%" },
                { targets: 1, width: "98%" }
            ]
        });
        table3.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });

    $('#section-invalid').on('click', function () {
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
                        var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                        var category = '<p class="text-custom">Category' + row.Category + '<br/>';
                        var code = 'Question Code: ' + row.Code + '</p>';
                        var questionContent = changeHtml(row.QuestionContent) + '<br/>';
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
                        var result = category + code + questionContent + image;
                        for (i = 0; i < options.length; i++) {
                            if (options[i]['correct']) {
                                result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                            } else {
                                result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                            }
                        }
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
            ]
        });
        table4.on('page.dt', function () {
            $('html, body').animate({
                scrollTop: $(".dataTables_wrapper").offset().top
            }, 'slow');
        });
    });
    {
        //var table2 = $('#tableDelete').DataTable({
    //    columns: [
    //        null,
    //        {
    //            "render": function (data, type, row) {
    //                if (data.indexOf("[html]") >= 0) {
    //                    data = data.split("&lt;cbr&gt;").join("<br/>");
    //                    data = data.split("&lt;br&gt;").join("<br/>");
    //                    data = data.split("&lt;p&gt;").join("");
    //                    data = data.split("&lt;br/&gt;").join("<br/>");
    //                    data = data.split("&lt;/p&gt;").join("");
    //                    data = data.split("&lt;b&gt;").join("");
    //                    data = data.split("&lt;/b&gt;").join("");
    //                    data = data.split("&lt;span&gt;").join("");
    //                    data = data.split("&lt;/span&gt;").join("");
    //                    data = data.split("&lt;u&gt;").join("");
    //                    data = data.split("&lt;/u&gt;").join("");
    //                    data = data.split("&lt;i&gt;").join("");
    //                    data = data.split("&lt;/i&gt;").join("");
    //                    data = data.split("&lt;sub&gt;").join("<sub>");
    //                    data = data.split("&lt;/sub&gt;").join("</sub>");
    //                    data = data.split("&lt;sup&gt;").join("<sup>");
    //                    data = data.split("&lt;/sup&gt;").join("</sup>");
    //                    data = data.split("[html]").join("");
    //                }

    //                return data
    //            }
    //        },
    //        {
    //            "render": function (data, type, row) {
    //                if (data.indexOf("[html]") >= 0) {
    //                    data = data.split("&lt;cbr&gt;").join("<br/>");
    //                    data = data.split("&lt;br&gt;").join("<br/>");
    //                    data = data.split("&lt;p&gt;").join("");
    //                    data = data.split("&lt;br/&gt;").join("<br/>");
    //                    data = data.split("&lt;/p&gt;").join("");
    //                    data = data.split("&lt;b&gt;").join("");
    //                    data = data.split("&lt;/b&gt;").join("");
    //                    data = data.split("&lt;span&gt;").join("");
    //                    data = data.split("&lt;/span&gt;").join("");
    //                    data = data.split("&lt;u&gt;").join("");
    //                    data = data.split("&lt;/u&gt;").join("");
    //                    data = data.split("&lt;i&gt;").join("");
    //                    data = data.split("&lt;/i&gt;").join("");
    //                    data = data.split("&lt;sub&gt;").join("<sub>");
    //                    data = data.split("&lt;/sub&gt;").join("</sub>");
    //                    data = data.split("&lt;sup&gt;").join("<sup>");
    //                    data = data.split("&lt;/sup&gt;").join("</sup>");
    //                    data = data.split("[html]").join("");
    //                }

    //                return data
    //            }
    //        }
    //    ]
    //});
    //var table3 = $('#tableSuccess').DataTable({
    //    columns: [
    //        null,
    //        {
    //            "render": function (data, type, row) {
    //                if (data.indexOf("[html]") >= 0) {
    //                    data = data.split("&lt;cbr&gt;").join("<br/>");
    //                    data = data.split("&lt;br&gt;").join("<br/>");
    //                    data = data.split("&lt;p&gt;").join("");
    //                    data = data.split("&lt;br/&gt;").join("<br/>");
    //                    data = data.split("&lt;/p&gt;").join("");
    //                    data = data.split("&lt;b&gt;").join("");
    //                    data = data.split("&lt;/b&gt;").join("");
    //                    data = data.split("&lt;span&gt;").join("");
    //                    data = data.split("&lt;/span&gt;").join("");
    //                    data = data.split("&lt;u&gt;").join("");
    //                    data = data.split("&lt;/u&gt;").join("");
    //                    data = data.split("&lt;i&gt;").join("");
    //                    data = data.split("&lt;/i&gt;").join("");
    //                    data = data.split("&lt;sub&gt;").join("<sub>");
    //                    data = data.split("&lt;/sub&gt;").join("</sub>");
    //                    data = data.split("&lt;sup&gt;").join("<sup>");
    //                    data = data.split("&lt;/sup&gt;").join("</sup>");
    //                    data = data.split("[html]").join("");
    //                }

    //                return data
    //            }
    //        },
    //    ],
    //    columnDefs: [
    //        { targets: 0, width: "2%" },
    //        { targets: 1, width: "98%" }
    //    ]
    //});
    //var table4 = $('#tableInvalid').DataTable({
    //    columns: [
    //        null,
    //        {
    //            "render": function (data, type, row) {
    //                if (data.indexOf("[html]") >= 0) {
    //                    data = data.split("&lt;cbr&gt;").join("<br/>");
    //                    data = data.split("&lt;br&gt;").join("<br/>");
    //                    data = data.split("&lt;p&gt;").join("");
    //                    data = data.split("&lt;br/&gt;").join("<br/>");
    //                    data = data.split("&lt;b&gt;").join("");
    //                    data = data.split("&lt;/b&gt;").join("");
    //                    data = data.split("&lt;/p&gt;").join("");
    //                    data = data.split("&lt;span&gt;").join("");
    //                    data = data.split("&lt;/span&gt;").join("");
    //                    data = data.split("&lt;u&gt;").join("");
    //                    data = data.split("&lt;/u&gt;").join("");
    //                    data = data.split("&lt;i&gt;").join("");
    //                    data = data.split("&lt;/i&gt;").join("");
    //                    data = data.split("&lt;sub&gt;").join("<sub>");
    //                    data = data.split("&lt;/sub&gt;").join("</sub>");
    //                    data = data.split("&lt;sup&gt;").join("<sup>");
    //                    data = data.split("&lt;/sup&gt;").join("</sup>");
    //                    data = data.split("[html]").join("");
    //                }

    //                return data
    //            }
    //        },
    //        {
    //            "render": function (data, type, row) {
    //                if (data.indexOf("[html]") >= 0) {
    //                    data = data.split("&lt;cbr&gt;").join("<br/>");
    //                    data = data.split("&lt;br&gt;").join("<br/>");
    //                    data = data.split("&lt;p&gt;").join("");
    //                    data = data.split("&lt;br/&gt;").join("<br/>");
    //                    data = data.split("&lt;b&gt;").join("");
    //                    data = data.split("&lt;/b&gt;").join("");
    //                    data = data.split("&lt;/p&gt;").join("");
    //                    data = data.split("&lt;span&gt;").join("");
    //                    data = data.split("&lt;/span&gt;").join("");
    //                    data = data.split("&lt;u&gt;").join("");
    //                    data = data.split("&lt;/u&gt;").join("");
    //                    data = data.split("&lt;i&gt;").join("");
    //                    data = data.split("&lt;/i&gt;").join("");
    //                    data = data.split("&lt;sub&gt;").join("<sub>");
    //                    data = data.split("&lt;/sub&gt;").join("</sub>");
    //                    data = data.split("&lt;sup&gt;").join("<sup>");
    //                    data = data.split("&lt;/sup&gt;").join("</sup>");
    //                    data = data.split("[html]").join("");
    //                }
    //                return data
    //            }
    //        },
    //        null
    //    ],
    //    columnDefs: [
    //        { targets: 0, width: "2%" },
    //        { targets: 1, width: "68%" },
    //        { targets: 2, width: "20%" },
    //        { targets: 3, width: "10%" }
    //    ]
    //});
    }
    





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
    $('#editable1').show();
    var table1 = $('#tableEditable1').DataTable({
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
                type: "editable1"
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
                    var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                    var category = '<p class="text-custom">Category' + row.Category + '<br/>';
                    var code = 'Question Code: ' + row.Code + '</p>';
                    var questionContent = changeHtml(row.QuestionContent) + '<br/>';
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
                    var result = category + code + questionContent + image;
                    for (i = 0; i < options.length; i++) {
                        if (options[i]['correct']) {
                            result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                        } else {
                            result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                        }
                    }
                    return result;
                }
            },
            {
                data: "DuplicatedQuestion",
                render: function (data) {
                    var alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
                    var category = '<p> </p>';
                    var code = ' <p class="text-custom">' + data.CourseName + data.Code + '</p>';
                    var questionContent = changeHtml(data.QuestionContent) + '<br/>';
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
                    var result = category + code + questionContent + image;
                    for (i = 0; i < options.length; i++) {
                        if (options[i]['correct']) {
                            result = result + '<div class="container-fluid text-right-answer">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                        } else {
                            result = result + '<div class="container-fluid">' + alpha[i] + '. ' + options[i]['content'] + '</div>';
                        }
                    }
                    return result;
                }
            },
            {
                data: "Id",
                render: function (data) {
                    var importId = $('#importId').val();
                    var edit = '<a href="/Import/GetQuestionTemp/' + data + '" class="btn btn-primary mb-2 col-md-12">Edit</a>';
                    var accept = '<a href="/Import/Skip?questionId=' + data + '&importId=' + importId + '" class="btn btn-success mb-2 col-md-12">Accept</a>';
                    var deleteQ = '<a href="/Import/Delete?questionId=' + data + '&importId=' + importId + '" class="btn btn-danger col-md-12">Delete</a>';
                    var result = edit + accept + deleteQ;
                    return result;
                }
            }
        ],
        columnDefs: [
            { targets: 0, width: "2%" },
            { targets: 1, width: "44%" },
            { targets: 2, width: "44%" },
            { targets: 3, width: "10%" }
        ]
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