$(function () {
    $("#imageInput").change(UpdateFileName);
});

function UpdateFileName() {
    var imageName = $("#imageInput").get(0).files[0].name;

    $("#imageNameField").val(imageName);

    $("#imagePreview").fadeOut(700);
}