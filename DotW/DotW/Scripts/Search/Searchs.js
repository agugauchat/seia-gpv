$(function () {
    $("#searchText").on("click", function () {

        var textToSearch = $("#searchInput").val();

        var url = '/Search/Index?text=' + textToSearch;

        location.href = url;
    })
})