$(document).ready(function () {
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

                } else {
                    //check
                    //show question
                    $($(this).attr("toggle-for")).removeClass("hidden");
                }
            });
        }
        
    };

    var questionOctopus = {
        init: function () {
            questionView.init();
        }
    };

    questionOctopus.init();

});

