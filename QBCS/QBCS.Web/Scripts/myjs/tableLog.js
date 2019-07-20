$(document).ready(function () {
    var tableLog = $('#tableLog').DataTable({
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
            url: "Log",
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
                data: "Fullname"
            },
            {
                data: "LogDate"
            },
            {
                render: function (data, type, row) {
                    var result = "";
                    if (row.Action.equals('Cancel') || row.Action.equals('Save')) {
                        result = row.Action + '<text> Import File </text>' + row.TargetId + '<text>st</text>';
                    } else if (row.Action.equals('Import')) {
                        result = row.Action + '<text> File </text>' + row.TargetId + '<text>st</text>';
                    } else if (row.Action.equals('Move')) {
                        result = row.Action + '<text> Question </text>' + row.TargetId + '<text>st</text>';
                    } else if (row.Action.equals('Update')) {
                        result = row.Message;
                    } else {
                        if (row.TargetId != null && row.TargetId != 0) {
                            result = row.Message + " " + row.TargetId + "st";
                        }
                    }
                    return result;
                }
            },
            {
                render: function (data, type, row) {
                    var result = '<p class="text-custom">N/A</p>';
                    if (row.Action.equals('Import')) {
                        result =
                            '<a href="@Url.Action("GetListTargetByID", "Activity", new { id = ' + row.Id + ', targetId = ' + row.TargetId + ' })" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    if (row.TargetName.equals("Question") && row.Action.equals("Update")) {
                        result =
                            '<a href="@Url.Action("GetUpdateActivityById", "Activity", new { id = ' + row.Id + ' })" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    if (row.TargetName.equals("Question") && row.Action.equals("Move")) {
                        result =
                            '<a href="@Url.Action("GetMoveActivityById", "Activity", new { id = ' + row.Id + ' })" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    if (row.TargetName.equals("Rule") && row.Action.equals("Update")) {
                        result =
                            '<a href="@Url.Action("Index", "Rule")" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    return result;
                }
            }
        ],
        columnDefs: [
            { targets: 0, width: "2%" },
            { targets: 1, width: "15%" },
            { targets: 2, width: "15%" },
            { targets: 3, width: "53%" },
            { targets: 4, width: "15%" }
        ]
    });
    tableLog.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
});