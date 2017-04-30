$(function () {
    $("#imageInput").change(UpdateFileName);

    $('#postTags').tagsinput({
        confirmKeys: [32],
        trimValue: true,
        maxChars: 20
    });

    $('#postTags').on('itemAdded', function (event) {
        $(".bootstrap-tagsinput").append('<input id="' + event.item + '" name="Tags" type="hidden" value="' + event.item + '"/>');
    });

    $('#postTags').on('itemRemoved', function (event) {
        $("[id = '" + event.item + "']").remove();
    });
});

function UpdateFileName() {
    var imageName = $("#imageInput").get(0).files[0].name;

    $("#imageNameField").val(imageName);

    $("#imagePreview").fadeOut(700);
}