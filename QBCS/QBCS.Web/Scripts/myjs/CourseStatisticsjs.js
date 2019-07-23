$(document).ready(function () {
    var checkCourse = [];
    var checkLO = [];
    $('#listCourseStat a').each(function () {
        var Obj = {};
        Obj['Id'] = $(this).attr('id');
        Obj['Value'] = $(this).text().trim();
        if (Obj['Id'].includes("course")) {
            checkCourse.push(Obj);
        }

    });
    $('input').keyup(function (e) {
        var input = this.value.toLowerCase();
        for (var i = 0; i < checkCourse.length; i++) {
            var value = checkCourse[i]['Value'].toLowerCase();
            var jqCourse = "#" + checkCourse[i]['Id'];
            //var jqLO = jqCourse.replace("course_", "c-");
            if (!value.includes(input)) {
                $(jqCourse).hide();
                //$(jqLO).removeClass("show");
            } else {
                $(jqCourse).show();
            }
        }
    });
});