$(document).ready(function () {
    var importId = $('#importId').val();
    var targetId = $('#targetId').val();
    var userId = $('#userId').val();
    var tableActivity = $('#tableActivity').DataTable({
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
            url: "/Activity/GetActivityDatatable",
            type: "GET",
            data: {
                importId: importId != undefined ? importId : null,
                targetId: targetId != undefined ? targetId : null,
                userId: userId != undefined ? userId : null,
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
                data: "LogDate",
                render: function (data) {
                    var date = new Date(+data.replace(/\D/g, ''));
                    var day = date.getDate();
                    var month = date.getMonth()+1;
                    var year = date.getFullYear();
                    return day + '/' + month + '/' + year;
                }
            },
            {
                render: function (data, type, row) {
                    var result = "";
                    var action = row.Action;
                    if (action == 'Cancel' || action == 'Save') {
                        result = action + '<text> Import File </text>' + row.TargetId + '<text>st</text>';
                    } else if (action == 'Import') {
                        result = action + '<text> File </text>' + row.TargetId + '<text>st</text>';
                    } else if (action == 'Move') {
                        result = action + '<text> Question </text>' + row.TargetId + '<text>st</text>';
                    } else if (action == 'Update') {
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
                    var action = row.Action;
                    if (action == 'Import') {
                        result =
                            '<a href="@Url.Action("GetListTargetByID", "Activity", new { id = ' + row.Id + ', targetId = ' + row.TargetId + ' })" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    if (action == "Update" && row.TargetName == "Question") {
                        result =
                            '<a href="@Url.Action("GetUpdateActivityById", "Activity", new { id = ' + row.Id + ' })" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    if (action == "Move" && row.TargetName == "Question") {
                        result =
                            '<a href="@Url.Action("GetMoveActivityById", "Activity", new { id = ' + row.Id + ' })" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    if (action == "Update" && row.TargetName == "Rule") {
                        result =
                            '<a href="@Url.Action("Index", "Rule")" class="btn btn-primary mb-2 col-md-12 spinner-loading">Detail</a>';
                    }
                    return result;
                }
            }
        ],
        columnDefs: [
            { targets: 0, width: "2%" },
            { targets: 1, width: "25%" },
            { targets: 2, width: "15%" },
            { targets: 3, width: "43%" },
            { targets: 4, width: "15%" }
        ]
    });
    tableActivity.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
});