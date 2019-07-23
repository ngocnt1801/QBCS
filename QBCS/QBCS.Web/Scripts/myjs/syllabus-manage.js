$(document).ready(function () {
    var syllabusModel = {
        currentSyllabus: {
            Id: 0,
            Name: "",
            Amount: 0
        }
    };

    var syllabusView = {
        init: function () {
            this.nameField = $("#sylname");
            this.amountField = $("#sylamount");
            this.idField = $("#sylid");

            this.updateModal = $("#update-syllabus-modal");
            this.locContainer = $("#loc-container");

            $(".edit-syllabus").on('click', function () {
                syllabusOctopus.setCurrentSyllabus($(this).attr("data-name"), $(this).attr("data-amount"), $(this).attr("data-id"));
                syllabusView.showUpdateModal();
            })

            $(".show-loc").on('click', function () {
                syllabusOctopus.loadLOC($(this).attr("data-url"));
            });

            $(document).on('click', '.add-loc-btn', function () {
                syllabusOctopus.addLOC($(this).attr("data-url"), $("#selectedList").val(), $(this).attr("data-syl"));
            })

            $(document).on('click', '.remove-loc', function () {
                syllabusOctopus.loadLOC($(this).attr("data-url"));
            })
        },
        showUpdateModal: function () {
            var currentSyllabus = syllabusOctopus.getCurrentSyllabus();
            this.nameField.val(currentSyllabus.Name);
            this.amountField.val(currentSyllabus.Amount);
            this.idField.val(currentSyllabus.Id);

            this.updateModal.modal("show");
        }
    };

    var syllabusOctopus = {
        init: function () {
            syllabusView.init();
        },
        setCurrentSyllabus: function (name, amount, id) {
            syllabusModel.currentSyllabus.Name = name;
            syllabusModel.currentSyllabus.Id = id;
            syllabusModel.currentSyllabus.Amount = amount;
        },
        getCurrentSyllabus: function () {
            return syllabusModel.currentSyllabus;
        },
        loadLOC: function (url) {
            $.ajax({
                url: url,
                type: 'GET',
                success: function (response) {
                    syllabusView.locContainer.html(response);
                }
            });
        },
        addLOC: function (url, locId, sylId) {
            $.ajax({
                url: url,
                data: {
                    locId: locId,
                    syllabusId: sylId
                },
                type: 'GET',
                success: function (response) {
                    syllabusView.locContainer.html(response);
                }
            });
        }
    };

    syllabusOctopus.init();

});

