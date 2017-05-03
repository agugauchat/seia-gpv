$(function () {
    // Inicializa el componente para el resaltado de sintaxis de codigo.
    hljs.initHighlightingOnLoad();

    // Botón 'Denunciar Publicación'
    $("#denunciatePost").on("click", function () {
        $("#postComplaintModal").modal('show');
    })

    $('#postComplaintForm').submit(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: '/Complaint/PostComplaint',
            data: $("#postComplaintForm").serialize(),
            success: function (response) {
                if (response != null && response.success) {
                    UpdatePostComplaintModalWithSuccess(response);
                }
                else {
                    UpdatePostComplaintModalWithError();
                }
            },
            error: function (response) {
                UpdatePostComplaintModalWithError
            }
        });
    });

    // Botón 'Denunciar Comentario'
    $("[id = denunciateCommentary]").on("click", function () {
        ClearCommentaryComplaintModal();

        // Se obtiene el Id del comentario que se está queriendo denunciar.
        var commentaryId = $(this).attr("commentary-id");

        // Se completa el hidden del formulario con ese Id obtenido.
        $("#commentaryIdFormField").val(commentaryId);

        $("#commentaryComplaintModal").modal('show');
    })

    $('#commentaryComplaintForm').submit(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: '/Complaint/CommentaryComplaint',
            data: $("#commentaryComplaintForm").serialize(),
            success: function (response) {
                if (response != null && response.success) {
                    UpdateCommentaryComplaintModalWithSuccess(response);
                }
                else {
                    UpdateCommentaryComplaintModalWithError();
                }
            },
            error: function (response) {
                UpdateCommentaryComplaintModalWithError();
            }
        });
    });
})

// Posts Region

function UpdatePostComplaintModalWithSuccess(response) {
    // Se quita todo el formulario y se muestra el mensaje de exito.
    $("#postComplaintResultText").text(response.Message);
    $("#postComplaintResultText").addClass("text-success");
    $("#postComplaintCommentary").remove();
    $("#postComplaintSubmit").remove();
    $("#postComplaintResult").fadeIn(300);
    $("#postGoBackBtn").show();

    // Se elimnina el botón para denunciar el post recientemente denunciado.
    $("#denunciatePost").remove();
}

function UpdatePostComplaintModalWithError() {
    // Se quita todo el formulario y se muestra el mensaje de error.
    $("#postComplaintResultText").text("Ha ocurrido un error al procesar la solicitud.");
    $("#postComplaintResultText").addClass("text-danger");
    $("#postComplaintCommentary").remove();
    $("#postComplaintSubmit").remove();
    $("#postComplaintResult").fadeIn(300);
    $("#postGoBackBtn").show();

    // Se elimnina el botón para denunciar el post recientemente denunciado.
    $("#denunciatePost").remove();
}

// End Posts Region

// Commentary Region

function UpdateCommentaryComplaintModalWithSuccess(response) {
    // Se quita todo el formulario y se muestra el mensaje de exito.
    $("#commentaryComplaintResultText").text(response.Message);
    $("#commentaryComplaintResultText").addClass("text-success");
    $("#commentaryComplaintComment").hide();
    $("#commentaryComplaintSubmit").hide();
    $("#commentaryComplaintResult").fadeIn(300);
    $("#commentaryGoBackBtn").show();

    // Se elimnina el botón para denunciar el comentario recientemente denunciado.
    var commentaryId = $("#commentaryIdFormField").val();
    $("[commentary-id = " + commentaryId + "]").remove();
}

function UpdateCommentaryComplaintModalWithError() {
    // Se quita todo el formulario y se muestra el mensaje de error.
    $("#commentaryComplaintResultText").text("Ha ocurrido un error al procesar la solicitud.");
    $("#commentaryComplaintResultText").addClass("text-danger");
    $("#commentaryComplaintComment").hide();
    $("#commentaryComplaintSubmit").hide();
    $("#commentaryComplaintResult").fadeIn(300);
    $("#commentaryGoBackBtn").show();

    // Se elimnina el botón para denunciar el comentario recientemente denunciado.
    var commentaryId = $("#commentaryIdFormField").val();
    $("[commentary-id = " + commentaryId + "]").remove();
}

function ClearCommentaryComplaintModal() {
    //  Se quita todo el contenido del formulario.
    $("#commentaryComplaintTextArea").val("");
    $("#commentaryComplaintComment").show();
    $("#commentaryComplaintSubmit").show();
    $("#commentaryComplaintResult").hide();
    $("#commentaryGoBackBtn").hide();
}

// End Commentary Region