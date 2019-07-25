$(document).ready(function () {
    var categoryModel = {
        currentCategory: {
            Id: 0,
            Name: "",
        },
        currentLOC: {
            Id: 0,
            Name: "",
        }
    };

    var categoryView = {
        init: function () {
            this.nameField = $("#categoryName");
            this.idField = $("#categoryId");

            this.updateModal = $("#update-category-modal");

            $(".edit-category").on('click', function () {
                categoryOctopus.setCurrentCategory($(this).attr("data-name"), $(this).attr("data-id"));
                categoryView.showUpdateModal();
            })

            this.locNameField = $("#locName");
            this.locIdField = $("#locId");

            this.updateLocModal = $("#update-loc-modal");

            $(".edit-loc").on('click', function () {
                categoryOctopus.setCurrentLOC($(this).attr("data-name"), $(this).attr("data-id"));
                categoryView.showUpdateLocModal();
            })

        },
        showUpdateModal: function () {
            var currentCategory = categoryOctopus.getCurrentCategory();
            this.nameField.val(currentCategory.Name);
            this.idField.val(currentCategory.Id);

            this.updateModal.modal("show");
        },
        showUpdateLocModal: function () {
            var currentLOC = categoryOctopus.getCurrentLOC();
            this.locNameField.val(currentLOC.Name);
            this.locIdField.val(currentLOC.Id);

            this.updateLocModal.modal("show");
        }
    };

    var categoryOctopus = {
        init: function () {
            categoryView.init();
        },
        setCurrentCategory: function (name, id) {
            categoryModel.currentCategory.Name = name;
            categoryModel.currentCategory.Id = id;
        },
        getCurrentCategory: function () {
            return categoryModel.currentCategory;
        },
        setCurrentLOC: function (name, id) {
            categoryModel.currentLOC.Name = name;
            categoryModel.currentLOC.Id = id;
        },
        getCurrentLOC: function () {
            return categoryModel.currentLOC;
        }
    };

    categoryOctopus.init();

});

