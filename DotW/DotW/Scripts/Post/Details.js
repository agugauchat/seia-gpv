$(function () {
    // Inicializa el componente para el resaltado de sintaxis de codigo.
    hljs.initHighlightingOnLoad();

    $("#denunciate").on("click", function () {
        $("#denouncementModal").modal('show');
    })

    $('#complaintForm').submit(function (e) {
        e.preventDefault();
        $.ajax({
            type: 'POST',
            url: '/Post/Complaint',
            data: $("#complaintForm").serialize(),
            success: function (response) {
                if (response != null && response.success) {
                    $("#resultText").text(response.Message);
                    $("#resultText").addClass("text-success");
                    $("#commentary").hide();
                    $("#complaintSubmit").hide();
                    $("#result").fadeIn(300);
                    $("#goBackBtn").show();
                }
                else {
                    $("#resultText").text(response.Message);
                    $("#resultText").addClass("text-danger");
                    $("#commentary").hide();
                    $("#complaintSubmit").hide();
                    $("#result").fadeIn(300);
                    $("#goBackBtn").show();
                }
            },
            error: function (response) {
                $("#resultText").text("Ha ocurrido un error al procesar la solicitud.");
                $("#resultText").addClass("text-danger");
                $("#commentary").hide();
                $("#complaintSubmit").hide();
                $("#result").fadeIn(300);
                $("#goBackBtn").show();
            }
        });
    });
})