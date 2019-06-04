$(function () {
    notificationModel = {
        count: 0
    };

    notificationView = {
        init: function () {
            this.notificationSpan = $("#new_notification");
        },
        render: function () {
            var count = notificationOctopus.getCount();
            if (count > 0) {
                this.notificationSpan.html(count);
            } else {
                this.notificationSpan.html('');
            }

        }
    };


    notificationOctopus = {
        init: function () {
            notificationView.init();
            this.connectHub();
        },
        getCount: function () {
            return notificationModel.count;
        },
        connectHub: function () {
            //declare a proxy to reference the hub
            var notifications = $.connection.importResultHub;

            //function that hub can call to broadcast message
            notifications.client.updateNotification = function () {
                notificationOctopus.getAmountNewNotification();
            };

            //start the connection
            $.connection.hub.start().done(function () {
                notificationOctopus.getAmountNewNotification();
            });
        },
        getAmountNewNotification: function () {
            $.ajax({
                url: '/QBCS.Web/Notification/GetNotification',
                type: 'GET',
                success: function (response) {
                    notificationModel.count = response;
                    notificationView.render();
                }
            });
        }
    };

    notificationOctopus.init();
});