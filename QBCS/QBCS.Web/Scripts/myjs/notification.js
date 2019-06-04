$(function () {
    notificationModel = {
        count: 0,
        listNotification: []
    };

    notificationView = {
        init: function () {
            this.notificationSpan = $("#count_notification");
            this.notificationContainter = $("#list_notification");
        },

        render: function () {
            this.renderCountNotification();
            this.renderListNotification();
        },

        getTemplateNotification: function () {
            var template = "<a class='dropdown-item d-flex align-items-center' href='{{noti.link}}'><div class='mr-3'>" +
                "<div class='icon-circle bg-primary'>" +
                "<i class='fas {{noti.icon}} text-white'></i>" +
                "</div>" +
                "</div>" +
                "<div>" +
                "<div class='small text-gray-500'>{{noti.date}}</div>" +
                "<span class='font-weight-bold'>{{noti.message}}</span>" +
                "</div>" +
                "</a>";

            return template;
                
        },

        renderCountNotification: function(){
            var count = notificationOctopus.getCount();
            if (count > 0) {
                this.notificationSpan.html(count);
            } else {
                this.notificationSpan.html('');
            }
        },

        renderListNotification: function(){
            var listNotification = notificationOctopus.getListNotification();
            this.notificationContainter.empty();
            listNotification.forEach(element => {
                this.notificationContainter.append(notificationView.renderNotification(element.Message));
            });
        },
        renderNotification: function(message){
            var template = notificationView.getTemplateNotification();
            template = template.replace("{{noti.link}}", "#")
                                .replace("{{noti.icon}}", "fa-file-alt")
                                .replace("{{noti.date}}", "Today")
                                .replace("{{noti.message}}", message);

            return template;
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
        getListNotification: function(){
            return notificationModel.listNotification;
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
                    response.forEach(element => {
                        notificationModel.listNotification.push(element);
                    });
                    
                    notificationModel.count = response.length;
                    notificationView.render();
                }
            });
        }
    };

    notificationOctopus.init();
});