$(function () {
    $(".returnImage").click("click", function (e) {
        var urlImage = $(this).attr("data-url");
        window.opener.UpdateValue(urlImage);
        window.close();
    });
});