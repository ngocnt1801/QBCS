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

function split() {
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
    var table2 = $('#tableDelete').DataTable({
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
            null
        ]
    });
    var table3 = $('#tableSuccess').DataTable({
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
            null
        ],
        columnDefs: [
            { targets: 0, width: "2%" },
            { targets: 1, width: "98%" }
        ]
    });
    var table4 = $('#tableInvalid').DataTable({
        columns: [
            null,
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
            },
            null
        ],
        columnDefs: [
            { targets: 0, width: "2%" },
            { targets: 1, width: "68%" },
            { targets: 2, width: "20%" },
            { targets: 3, width: "10%" }
        ]
    });

    table1.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });

    table2.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });

    table3.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });

    table4.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
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

    var table = {
        tableEditable: [table1, "#total-edit"],
        tableDelete: [table2, "#total-delete"],
        tableSuccess: [table3, "#total-success"],
        tableInvalid: [table4, "#total-invalid"]
    }

    $(document).on('click', '.delete-question-dt', function () {
        var fromContainer = table[$(this).attr("data-from")][0];
        var toContainer = table[$(this).attr("data-to")][0];

        minusTotal($(table[$(this).attr("data-from")][1]));
        var index = plusTotal($(table[$(this).attr("data-to")][1]));

        var question = getQuestionObject($($(this).attr('data-id')));

        //delete from
        var rows = fromContainer
            .rows($(this).attr('data-id'))
            .remove()
            .draw();
        deleteQuestion($(this).attr('data-url'));
        //add to
        var tableId = toContainer.tables().nodes().to$().attr('id');
        var dataRow = [
            index,
            '<div data-question="content">' + question.Content + '</div>'
        ];
        switch (tableId) {
            case "tableEditable":
                dataRow.push(question.Duplicate);
                break;
            case "tableDelete":
                dataRow.push(`<div data-question="action">
                                            <a href="/Import/Recovery?tempId=`+ question.Id + `&url=` + window.location.href + `" class="btn btn-success mb-2 col-md-12">Recovery</a>
                                        </div>`);
                break;
            case "tableSuccess":
                dataRow.push(`<button data-url="/Import/Delete?questionId=` + question.Id + `&url=` + window.location.href + `"
                                                    class="btn btn-danger float-md-right delete-question-dt"
                                                    data-id="#`+ question.Id + `"
                                                    data-from="tableSuccess"
                                                    data-to="tableDelete">
                                                Delete
                                            </button>`);
                break;
            case "tableInvalid":
                dataRow.push(question.Message);
                dataRow.push(`<div data-question="action">
                                            <a href="/Import/GetQuestionTemp?tmepId=`+ question.Id + `" class="btn btn-primary mb-2 col-md-12">Edit</a>
                                            <button data-url="/Import/Delete?questionId=` + question.Id + `&url=` + window.location.href + `"
                                                    class="btn btn-danger float-md-right delete-question-dt"
                                                    data-id="#`+ question.Id + `"
                                                    data-from="tableInvalid"
                                                    data-to="tableDelete">
                                                Delete
                                            </button>
                                        </div>`);
                break;
        }
        var addedrow = toContainer.row.add(dataRow).draw(false);
        addedrow.nodes().to$().attr('id', question.Id);
        addedrow.nodes().to$().attr('data-id', question.Id);
    });

    $(document).on('click', '.reload-partial', function () {
        var fromContainer = table[$(this).attr("data-from")][0];
        var toContainer = table[$(this).attr("data-to")][0];

        minusTotal($(table[$(this).attr("data-from")][1]));
        var index = plusTotal($(table[$(this).attr("data-to")][1]));

        var question = getQuestionObject($($(this).attr('data-id')));

        deleteQuestion($(this).attr('data-url'));
        reloadTable($(this).attr('data-url-reload'), $(this).attr("data-container"));

        var tableId = toContainer.tables().nodes().to$().attr('id');
        var dataRow = [
            index,
            '<div data-question="content">' + question.Content + '</div>'
        ];
        switch (tableId) {
            case "tableEditable":
                dataRow.push(question.Duplicate);
                break;
            case "tableDelete":
                dataRow.push(`<div data-question="action">
                                            <a href="/Import/Recovery?tempId=`+ question.Id + `&url=` + window.location.href + `" class="btn btn-success mb-2 col-md-12">Recovery</a>
                                        </div>`);
                break;
            case "tableSuccess":
                dataRow.push(`<button data-url="/Import/Delete?questionId=` + question.Id + `&url=` + window.location.href + `"
                                                    class="btn btn-danger float-md-right delete-question-dt"
                                                    data-id="#`+ question.Id + `"
                                                    data-from="tableSuccess"
                                                    data-to="tableDelete">
                                                Delete
                                            </button>`);
                break;
            case "tableInvalid":
                dataRow.push(question.Message);
                dataRow.push(`<div data-question="action">
                                            <a href="/Import/GetQuestionTemp?tmepId=`+ question.Id + `" class="btn btn-primary mb-2 col-md-12">Edit</a>
                                            <button data-url="/Import/Delete?questionId=` + question.Id + `&url=` + window.location.href + `"
                                                    class="btn btn-danger float-md-right delete-question-dt"
                                                    data-id="#`+ question.Id + `"
                                                    data-from="tableInvalid"
                                                    data-to="tableDelete">
                                                Delete
                                            </button>
                                        </div>`);
                break;
        }
        var addedrow = toContainer.row.add(dataRow).draw(false);
        addedrow.nodes().to$().attr('id', question.Id);
        addedrow.nodes().to$().attr('data-id', question.Id);
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

function deleteQuestion(url) {
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
function reloadTable(url, container) {
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
        }
    });
}

$(document).ready(function () {
    nav_bar_active();
    split();
    toggleTableDuplicate();
    customs_display();
    customs_display_duplicate();
    table_on_top();
});
