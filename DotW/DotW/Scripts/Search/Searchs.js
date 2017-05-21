$(function () {
    $("#searchText").bind("enterKey", function (e) {
        $("#searchForm").submit();
    });
})