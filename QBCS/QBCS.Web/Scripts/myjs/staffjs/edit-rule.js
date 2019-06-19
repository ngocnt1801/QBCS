

    
$(document).ready(function () {
        $('#save').on('click', function () {
            var check = true;
            var qlMin = 0;
            var qlMax = 0;
            var nOpMin = 0;
            var nOpMax = 0;
            var olMin = 0;
            var olMax = 0;
            var cOlMin = 0;
            var cOlMax = 0;
            var iOlMin = 0;
            var iOlMax = 0;
            var testList = [];
            var defaultActivateDate = $("#default_datetime").val();
            if (defaultActivateDate != "") {
                $('#table_Question tbody tr').each(function () {
                    var Obj = {};
                    Obj['KeyId'] = $(this).find('#id').text();
                    var KeyId = parseInt(Obj['KeyId']);
                    switch (KeyId) {
                        case 1:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                qlMin = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 2:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                qlMax = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 3:
                            var select3 = $('#select_3').val();
                            for (var i = 0; i < select3.length; i++) {
                                var Obj2 = {};
                                Obj2['KeyId'] = Obj['KeyId'];
                                if ($(this).find('input[type="checkbox"][id="case_sensitive"]').is(':checked') == true) {
                                    Obj2['Value'] = "·case_sensitive·" + select3[i];
                                } else {
                                    Obj2['Value'] = select3[i];
                                }
                                Obj2['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                Obj2['ActivateDate'] = $(this).find('#date').val();
                                if (Obj2['ActivateDate'] == "") {
                                    Obj2['ActivateDate'] = defaultActivateDate;
                                }
                                testList.push(Obj2);
                            }
                            break;
                        case 4:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                nOpMin = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 5:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                nOpMax = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                    }
                    //if (Obj['KeyId'] == 3) {
                    //    var select3 = $('#select_3').val();
                    //    for (var i = 0; i < select3.length; i++) {
                    //        var Obj2 = {};
                    //        Obj2['KeyId'] = Obj['KeyId'];
                    //        Obj2['Value'] = select3[i];
                    //        Obj2['ActivateDate'] = $(this).find('#date').val();
                    //        if (Obj2['ActivateDate'] == "") {
                    //            Obj2['ActivateDate'] = defaultActivateDate;
                    //        }
                    //        testList.push(Obj2);
                    //    }

                    //} else {
                    //    var value = $(this).find('#value').val();
                    //    var parseValue = parseInt(value);
                    //    if (Number.isInteger(parseValue)) {
                    //        Obj['Value'] = value;
                    //        Obj['ActivateDate'] = $(this).find('#date').val();
                    //        if (Obj['ActivateDate'] == "") {
                    //            Obj['ActivateDate'] = defaultActivateDate;
                    //        }
                    //        testList.push(Obj);
                    //    } else {
                    //        swal('Min max must be integer!!!');
                    //        $(this).find('#value').focus();
                    //        check = false;
                    //    }
                    //}
                });

                $('#table_Option tbody tr').each(function () {
                    var Obj = {};
                    Obj['KeyId'] = $(this).find('#id').text();
                    var KeyId = parseInt(Obj['KeyId']);
                    switch (KeyId) {
                        case 6:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                olMin = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 7:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                olMax = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 9:
                            var select9 = $('#select_9').val();
                            for (var i = 0; i < select9.length; i++) {
                                var Obj2 = {};
                                Obj2['KeyId'] = Obj['KeyId'];
                                if ($(this).find('input[type="checkbox"]').is(':checked') == true) {
                                    Obj2['Value'] = "·case_sensitive·" + select9[i];
                                } else {
                                    Obj2['Value'] = select9[i];
                                }
                                Obj2['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                Obj2['ActivateDate'] = $(this).find('#date').val();
                                if (Obj2['ActivateDate'] == "") {
                                    Obj2['ActivateDate'] = defaultActivateDate;
                                }
                                testList.push(Obj2);
                            }
                            break;
                        case 8:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                    }
                    //if (Obj['KeyId'] == 9) {
                    //    var select3 = $('#select_9').val();
                    //    for (var i = 0; i < select3.length; i++) {
                    //        var Obj2 = {};
                    //        Obj2['KeyId'] = Obj['KeyId'];
                    //        Obj2['Value'] = select3[i];
                    //        Obj2['ActivateDate'] = $(this).find('#date').val();
                    //        if (Obj2['ActivateDate'] == "") {
                    //            Obj2['ActivateDate'] = defaultActivateDate;
                    //        }
                    //        testList.push(Obj2);
                    //    }

                    //} else {
                    //    var value = $(this).find('#value').val();
                    //    var parseValue = parseInt(value);
                    //    if (Number.isInteger(parseValue)) {
                    //        Obj['Value'] = value;
                    //        Obj['ActivateDate'] = $(this).find('#date').val();
                    //        if (Obj['ActivateDate'] == "") {
                    //            Obj['ActivateDate'] = defaultActivateDate;
                    //        }
                    //        testList.push(Obj);
                    //    } else {
                    //        swal('Min max must be integer!!!');
                    //        check = false;
                    //    }
                    //}
                });

                $('#table_Correct_Option tbody tr').each(function () {
                    var Obj = {};
                    Obj['KeyId'] = $(this).find('#id').text();
                    var KeyId = parseInt(Obj['KeyId']);
                    switch (KeyId) {
                        case 10:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                cOlMin = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 11:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                cOlMax = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 12:
                            var select12 = $('#select_12').val();
                            for (var i = 0; i < select12.length; i++) {
                                var Obj2 = {};
                                Obj2['KeyId'] = Obj['KeyId'];
                                if ($(this).find('input[type="checkbox"]').is(':checked') == true) {
                                    Obj2['Value'] = "·case_sensitive·" + select12[i];
                                } else {
                                    Obj2['Value'] = select12[i];
                                }
                                Obj2['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                Obj2['ActivateDate'] = $(this).find('#date').val();
                                if (Obj2['ActivateDate'] == "") {
                                    Obj2['ActivateDate'] = defaultActivateDate;
                                }
                                testList.push(Obj2);
                            }
                            break;
                    }
                });

                $('#table_Incorrect_Option tbody tr').each(function () {
                    var Obj = {};
                    Obj['KeyId'] = $(this).find('#id').text();
                    var KeyId = parseInt(Obj['KeyId']);
                    switch (KeyId) {
                        case 13:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                iOlMin = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 14:
                            var value = $(this).find('#value').val();
                            var parseValue = parseInt(value);
                            if (Number.isInteger(parseValue)) {
                                Obj['Value'] = value;
                                Obj['ActivateDate'] = $(this).find('#date').val();
                                if (Obj['ActivateDate'] == "") {
                                    Obj['ActivateDate'] = defaultActivateDate;
                                }
                                Obj['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                testList.push(Obj);
                                iOlMax = value;
                            } else {
                                swal('Min max must be integer!!!');
                                $(this).find('#value').focus();
                                check = false;
                            }
                            break;
                        case 15:
                            var select15 = $('#select_15').val();
                            for (var i = 0; i < select15.length; i++) {
                                var Obj2 = {};
                                Obj2['KeyId'] = Obj['KeyId'];
                                if ($(this).find('input[type="checkbox"]').is(':checked') == true) {
                                    Obj2['Value'] = "·case_sensitive·" + select15[i];
                                } else {
                                    Obj2['Value'] = select15[i];
                                }
                                Obj2['IsUse'] = $(this).find('input[type="checkbox"][id="is_use"]').is(':checked');
                                Obj2['ActivateDate'] = $(this).find('#date').val();
                                if (Obj2['ActivateDate'] == "") {
                                    Obj2['ActivateDate'] = defaultActivateDate;
                                }
                                testList.push(Obj2);
                            }
                            break;
                    }
                });

                if (qlMin > qlMax || nOpMin > nOpMax || olMin > olMax || cOlMin > cOlMax || iOlMin > iOlMax) {
                    check = false;
                    swal('Min must be smaller than Max');
                }
                if (check) {
                    $.ajax({
                        type: "POST",
                        url: 'UpdateAllRule',
                        data: JSON.stringify(testList),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response != null) {
                                window.location.replace('Index');
                            }
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        },
                        error: function (response) {
                            alert(response.responseText);
                        }
                    });
                }
            } else {
                swal('Default Activation Date must be set !!!');
            }
        });
    });