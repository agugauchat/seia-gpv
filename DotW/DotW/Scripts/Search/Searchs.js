$(function () {
    $("#searchText").on('keyup', function (e) {
        // Enter key.
        if (e.keyCode == 13) {
            Search();
        }
    });

    $("#searchInput").on("click", function () {
        Search();
    })
})

function Search() {
    var textToSearch = $("#searchText").val();

    var url = '/Search/Index?text=' + textToSearch;

    location.href = url;
}