$(document).ready(function () {
    var questionModel = {
        activeList: [],
        save: function () {
            localStorage.activeList = this.activeList.join(",");
        },
        get: function () {
            if (localStorage.activeList != undefined && localStorage.activeList.length > 0) {
                this.activeList = localStorage.activeList.split(',');
            }
            return this.activeList;
        },
        remove: function (value) {
            this.activeList.splice($.inArray(value, this.activeList), 1);
            this.save();
        },
        add: function (value) {
            this.activeList.push(value);
            this.save();
        },
        empty: function () {
            this.activeList = [];
            this.save();
        }
    };

    var questionView = {
        init: function () {
            this.initCheckboxToggle();
            
        },
        initCheckboxToggle: function () {
            $('.toggle-question').on('click', function () {
                if ($(this).siblings("input").get(0).checked) {
                    //uncheck 
                    //hidden question
                    $($(this).attr("toggle-for")).addClass("hidden");
                    questionOctopus.saveActiveList();
                    questionOctopus.removeCheckbox($(this).attr('id'));
                } else {
                    //check
                    //show question
                    $($(this).attr("toggle-for")).removeClass("hidden");
                    questionOctopus.saveActiveList();
                    questionOctopus.addCheckbox($(this).attr('id'));
                }
            });
        },
        showListCardActive: function () {
            $.each($("td"), function () {
                $(this).addClass("hidden");
            });

            $.each($("input[type='checkbox']"), function () {
                $(this).prop('checked', false)
            });

            var checkedList = questionOctopus.getActiveList();
            $.each(checkedList, function (index, item) {
                $('#'+item).trigger('click');
            })
        }

    };

    var questionOctopus = {
        init: function () {
            questionView.init();
            if (this.getActiveList().length > 0) {
                questionView.showListCardActive();
            }
        },
        saveActiveList: function () {
            questionModel.empty();
            $.each($('input[type="checkbox"]:checked'), function () {
                questionModel.add($('label[for=' + $(this).attr('id') + ']').attr('id'));
            })
        },
        addCheckbox: function (id) {
            questionModel.add(id);
        },
        removeCheckbox: function (id) {
            questionModel.remove(id);
        },
        getActiveList: function () {
            return questionModel.get();
        },
        isHasActiveList: function () {
            return
            localStorage.activeList != undefined
            && localStorage.activeList.length > 0;
        }
    };

    questionOctopus.init();

});

