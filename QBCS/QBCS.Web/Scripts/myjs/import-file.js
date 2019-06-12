$(function() {
  questionTempView = {
    init: function() {
      this.editable = $(".editableTemp");
      $.each(this.editable, function(index, element) {
        element.onclick = function() {
          importFileOctopus.redirectToDetail(
            element.attributes["data-id"].value
          );
        };
      });
    }
  };

  resultView = {
    init: function() {
      $("#import-btn").unbind("click");
      this.importBtn = $("#import-btn");
      this.importBtn.on("click", function() {
        importFileOctopus.importQuestionToBank(
          this.attributes["data-id"].value
        );
      });

      $("#cancel-import-btn").unbind("click");
      this.cancelImportBtn = $("#cancel-import-btn");
      this.cancelImportBtn.on("click", function() {
        importFileOctopus.cancelImport(this.attributes["data-id"].value);
      });
    }
  };

  importFileOctopus = {
    init: function() {
      questionTempView.init();
      resultView.init();
      this.bs_input_file();
    },

    bs_input_file: function() {
      $(".input-file").before(function() {
        if (
          !$(this)
            .prev()
            .hasClass("input-ghost")
        ) {
          var element = $(
            "<input type='file' class='input-ghost' style='visibility:hidden; height:0' accept='.txt,.xml'>"
          );
          element.attr("name", $(this).attr("name"));
          element.change(function() {
            element
              .next(element)
              .find("input")
              .val(
                element
                  .val()
                  .split("\\")
                  .pop()
              );
          });
          $(this)
            .find("button.btn-choose")
            .click(function() {
              element.click();
            });
          $(this)
            .find("button.btn-reset")
            .click(function() {
              element.val(null);
              $(this)
                .parents(".input-file")
                .find("input")
                .val("");
            });
          $(this)
            .find("input")
            .css("cursor", "pointer");
          $(this)
            .find("input")
            .mousedown(function() {
              $(this)
                .parents(".input-file")
                .prev()
                .click();
              return false;
            });
          return element;
        }
      });
    },

    redirectToDetail: function(id) {
      document.location.href = "/QBCS.Web/Import/GetQuestionTemp?tempId=" + id;
    },
    importQuestionToBank: function(importId) {
    //   console.log("import to bank");
    //   $.ajax({
    //     url: "/QBCS.Web/Import/AddToBank",
    //     type: "GET",
    //     data: { importId: importId },
    //     success: function(response) {
    //       document.location.href = "/QBCS.Web/";
    //     }
    //   });

    document.location.href = "/QBCS.Web/Import/AddToBank?importId=" + importId;
    },
    cancelImport: function(importId) {
      //   console.log("import to bank");
      //   $.ajax({
      //     url: "/QBCS.Web/Import/Cancel",
      //     type: "GET",
      //     data: { importId: importId },
      //     success: function(response) {
      //       document.location.href = "/QBCS.Web/";
      //     }
      //   });
      document.location.href = "/QBCS.Web/Import/Cancel?importId=" + importId;
    }
  };

  importFileOctopus.init();
});
