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
            url: "/LogAction/GetLogAction",
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
                data: "TimeAgo"
            },
            {
                data: "Message"
            },
            {
                render: function (data, type, row) {
                    var result = row.Fullname + " (" + row.UserCode + ")";
                    return result;
                }
            },
            {
                data: "Method"
            },
            {
                data: "Route"
            },
            {
                data: "Ip"
            },
            {
                data: "LogDate",
                render: function (data) {
                    var date = new Date(+data.replace(/\D/g, ''));
                    var day = date.getDate();
                    var month = date.getMonth() + 1;
                    var year = date.getFullYear();
                    var hour = date.getHours();
                    var minute = date.getMinutes();
                    var sec = date.getSeconds();
                    if (month < 10) {
                        return year + '-0' + month + '-' + day + ' ' + hour + ':' + minute + ':' + sec;
                    }
                    return year + '-' + month + '-' + day + ' ' + hour + ':' + minute + ':' + sec;
                }
            }
        ]
        //columnDefs: [
        //    { targets: 0, width: "2%" },
        //    { targets: 1, width: "15%" },
        //    { targets: 2, width: "15%" },
        //    { targets: 3, width: "53%" },
        //    { targets: 4, width: "15%" }
        //]
    });
    tableLog.on('page.dt', function () {
        $('html, body').animate({
            scrollTop: $(".dataTables_wrapper").offset().top
        }, 'slow');
    });
});