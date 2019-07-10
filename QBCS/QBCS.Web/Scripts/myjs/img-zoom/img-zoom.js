function img_zoom(image) {
    // Get the modal
    var modal = document.getElementById("myModal");

    // Get the image and insert it inside the modal - use its "alt" text as a caption
    var modalImg = document.getElementById("img01");
    var captionText = document.getElementById("caption");
    modal.style.display = "block";
    modalImg.src = $(image).attr("src");
    captionText.innerHTML = $(image).attr("alt");


    // Get the <span> element that closes the modal
    var span = document.getElementsByClassName("image-close")[0];

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";
    }
}
