$(function () {
    // Inicializa el componente para el resaltado de sintaxis de codigo.
    hljs.initHighlightingOnLoad();

    // Botón 'Denunciar Publicación'
    $("#denunciatePost").on("click", function () {
        $("#postComplaintModal").modal('show');
    })

    $('#postComplaintForm').submit(function (e) {
        UpdatePostComplaintModalWithWaitMessage();
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
                    UpdatePostComplaintModalWithError(response);
                }
            },
            error: function (response) {
                UpdatePostComplaintModalWithError(response)
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

        return false;
    })

    $('#commentaryComplaintForm').submit(function (e) {
        UpdateCommentaryComplaintModalWithWaitMessage();
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
                    UpdateCommentaryComplaintModalWithError(response);
                }
            },
            error: function (response) {
                UpdateCommentaryComplaintModalWithError(response);
            }
        });
    });

    InitializeVoteButtonsColor();

    // Botón "Like"
    $("#likeBtn").on("click", function () {
        var isButtonPressed = $("#likeBtn").attr("pressed");
        if (isButtonPressed == "true") {
            // Se actualizan los contadores manualmente en caso de que las acciones en el controller demoren demasiado.
            // De no hacerlo así, la parte visual de los botones solamente se actualizará cuando el "control" retorne al ajax.
            var actualLikes = parseInt($("#likeBtn").text()[1]);
            actualLikes = actualLikes == 0 ? 0 : actualLikes - 1;
            $("#likeBtn").text(" " + (actualLikes).toString());

            DeactivateLikeButton();

            $.ajax({
                type: 'POST',
                url: '/Vote/SaveVote',
                data: { "postId": $("#postId").val(), "goodVote": false, "badVote" : false },
                success: function (response) {
                    if (response != null && response.success) {
                        
                        $("#likeBtn").text(" " + response.goodVotes);
                        $("#dislikeBtn").text(" " + response.badVotes);
                    }
                    else {
                    }
                },
                error: function (response) {
                }
            });
        }
        else
        {
            // Se actualizan los contadores manualmente en caso de que las acciones en el controller demoren demasiado.
            // De no hacerlo así, la parte visual de los botones solamente se actualizará cuando el "control" retorne al ajax.
            if ($("#dislikeBtn").attr("pressed") == "true") {
                var actualDislikes = parseInt($("#dislikeBtn").text()[1]);
                actualDislikes = actualDislikes == 0 ? 0 : actualDislikes - 1;
                $("#dislikeBtn").text(" " + (actualDislikes).toString());
            }

            DeactivateDislikeButton();
            ActivateLikeButton();

            var actualLikes = parseInt($("#likeBtn").text()[1]);
            $("#likeBtn").text(" " + (actualLikes + 1).toString());

            $.ajax({
                type: 'POST',
                url: '/Vote/SaveVote',
                data: { "postId": $("#postId").val(), "goodVote" : true, "badVote" : false },
                success: function (response) {
                    if (response != null && response.success) {
                        $("#likeBtn").text(" " + response.goodVotes);
                        $("#dislikeBtn").text(" " + response.badVotes);
                    }
                    else {
                    }
                },
                error: function (response) {
                }
            });
        }
        return false
    })

    // Botón "Dislike"
    $("#dislikeBtn").on("click", function () {
        var isButtonPressed = $("#dislikeBtn").attr("pressed");
        if (isButtonPressed == "true") {
            // Se actualizan los contadores manualmente en caso de que las acciones en el controller demoren demasiado.
            // De no hacerlo así, la parte visual de los botones solamente se actualizará cuando el "control" retorne al ajax.
            var actualDislikes = parseInt($("#dislikeBtn").text()[1]);
            actualDislikes = actualDislikes == 0 ? 0 : actualDislikes - 1;
            $("#dislikeBtn").text(" " + (actualDislikes).toString());

            DeactivateDislikeButton();

            $.ajax({
                type: 'POST',
                url: '/Vote/SaveVote',
                data: { "postId": $("#postId").val(), "goodVote": false, "badVote": false },
                success: function (response) {
                    if (response != null && response.success) {
                        $("#likeBtn").text(" " + response.goodVotes);
                        $("#dislikeBtn").text(" " + response.badVotes);
                    }
                    else {
                    }
                },
                error: function (response) {
                }
            });
        }
        else {
            // Se actualizan los contadores manualmente en caso de que las acciones en el controller demoren demasiado.
            // De no hacerlo así, la parte visual de los botones solamente se actualizará cuando el "control" retorne al ajax.
            if ($("#likeBtn").attr("pressed") == "true") {
                var actualLikes = parseInt($("#likeBtn").text()[1]);
                actualLikes = actualLikes == 0 ? 0 : actualLikes - 1;
                $("#likeBtn").text(" " + (actualLikes).toString());
            }
            
            var actualDislikes = parseInt($("#dislikeBtn").text()[1]);
            $("#dislikeBtn").text(" " + (actualDislikes + 1).toString());

            DeactivateLikeButton();
            ActivateDislikeButton();

            $.ajax({
                type: 'POST',
                url: '/Vote/SaveVote',
                data: { "postId": $("#postId").val(), "goodVote": false, "badVote": true },
                success: function (response) {
                    if (response != null && response.success) {
                        $("#likeBtn").text(" " + response.goodVotes);
                        $("#dislikeBtn").text(" " + response.badVotes);
                    }
                    else {
                    }
                },
                error: function (response) {
                }
            });
        }
        return false
    })
})

function InitializeVoteButtonsColor() {
    // Inicializa el color de los botones de los votos en función del voto del usuario
    // que estaba registrado al momento de cargar la página.

    var isLikeBtnPressed = $("#likeBtn").attr("pressed");
    var isDislikeBtnPressed = $("#dislikeBtn").attr("pressed");
    if (isLikeBtnPressed == "true")
    {
        // El usuario había votado positivamente.
        // Entonces se colorea el botón "Like".
        $("#likeBtn").css("background-color", "#357ebd");
        $("#likeBtn").css("color", "white");
    }

    if (isDislikeBtnPressed == "true") {
        // El usuario había votado negativamente.
        // Entonces se colorea el botón "Dislike".
        $("#dislikeBtn").css("background-color", "#e03333");
        $("#dislikeBtn").css("color", "white");
    }
}

// Botón "Like"
function ActivateLikeButton() {
    $("#likeBtn").css("background-color", "#357ebd");
    $("#likeBtn").css("color", "white");
    $("#likeBtn").attr("pressed", "true");
}

function DeactivateLikeButton() {
    $("#likeBtn").css("background-color", "white");
    $("#likeBtn").css("color", "black");
    $("#likeBtn").attr("pressed", "false");
}

// Botón "Dislike"
function ActivateDislikeButton() {
    $("#dislikeBtn").css("background-color", "#e03333");
    $("#dislikeBtn").css("color", "white");
    $("#dislikeBtn").attr("pressed", "true");
}

function DeactivateDislikeButton() {
    $("#dislikeBtn").css("background-color", "white");
    $("#dislikeBtn").css("color", "black");
    $("#dislikeBtn").attr("pressed", "false");
}


// Posts Region

function UpdatePostComplaintModalWithWaitMessage()
{
    // Se quita todo el formulario y se muestra el mensaje de exito.
    $("#postComplaintResultText").text("Su denuncia está siendo registrada. Por favor espere...");
    $("#postComplaintResultText").addClass("text-success");
    $("#postComplaintCommentary").hide();
    $("#postComplaintSubmit").remove();
    $("#postComplaintResult").fadeIn(300);
}

function UpdatePostComplaintModalWithSuccess(response) {
    // Se quita todo el formulario y se muestra el mensaje de exito.
    $("#postComplaintResultText").text('');
    $("#postComplaintResultText").append(response.Message);
    $("#postComplaintResultText").addClass("text-success");
    $("#postComplaintCommentary").remove();
    $("#postComplaintSubmit").remove();
    $("#postComplaintResult").fadeIn(300);
    $("#postGoBackBtn").show();

    // Se elimnina el botón para denunciar el post recientemente denunciado.
    $("#denunciatePost").remove();
}

function UpdatePostComplaintModalWithError(response) {
    // Se quita todo el formulario y se muestra el mensaje de error.

    var message = "";
    if (response.Message != "") {
        message = response.Message;
    }
    else {
        message = "Ha ocurrido un error al procesar la solicitud.";
    }

    $("#postComplaintResultText").text(message);
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

function UpdateCommentaryComplaintModalWithWaitMessage() {
    // Se quita todo el formulario y se muestra el mensaje de exito.
    $("#commentaryComplaintResultText").text("Su denuncia está siendo registrada. Por favor espere...");
    $("#commentaryComplaintResultText").addClass("text-success");
    $("#commentaryComplaintComment").hide();
    $("#commentaryComplaintSubmit").hide();
    $("#commentaryComplaintResult").fadeIn(300);
}

function UpdateCommentaryComplaintModalWithSuccess(response) {
    // Se quita todo el formulario y se muestra el mensaje de exito.
    $("#commentaryComplaintResultText").text('');
    $("#commentaryComplaintResultText").append(response.Message);
    $("#commentaryComplaintResultText").addClass("text-success");
    $("#commentaryComplaintComment").hide();
    $("#commentaryComplaintSubmit").hide();
    $("#commentaryComplaintResult").fadeIn(300);
    $("#commentaryGoBackBtn").show();

    // Se elimnina el botón para denunciar el comentario recientemente denunciado.
    var commentaryId = $("#commentaryIdFormField").val();
    $("[commentary-id = " + commentaryId + "]").remove();
}

function UpdateCommentaryComplaintModalWithError(response) {
    // Se quita todo el formulario y se muestra el mensaje de error.

    var message = "";
    if (response.Message != "") {
        message = response.Message;
    }
    else {
        message = "Ha ocurrido un error al procesar la solicitud.";
    }

    $("#commentaryComplaintResultText").text(message);
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