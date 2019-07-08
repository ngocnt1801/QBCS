

$(document).ready(function () {
    var datatableView = {
        init: function () {
            this.initImportHistoryTable();
        },

        initImportHistoryTable: function () {
            $('#import-history-table').DataTable();
        }
    };

    var datatableOctopus = {
        init: function () {
            datatableView.init();
        }
    };

    datatableOctopus.init();

});

