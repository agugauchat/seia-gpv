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

    $("#deleteImage").change(function () {
        var value = $(this).is(":checked");

        if (value) {
            $("#imageInputContainer").fadeOut(700);
            $("#imagePreview").fadeOut(700);
        }
        else {
            $("#imageInputContainer").fadeIn(700);

            // Si ya se había cargado una nueva imagen durante la edición,
            // entonces no debe mostrarse el botón 'Ver imagen'
            if ($("#imageInput").val() == "") {
                $("#imagePreview").fadeIn(700);
            }
        }
    })
});

function UpdateFileName() {
    var imageName = $("#imageInput").get(0).files[0].name;

    $("#imageNameField").val(imageName);

    $("#imagePreview").fadeOut(700);

    $("#deleteImageContainer").show();
}