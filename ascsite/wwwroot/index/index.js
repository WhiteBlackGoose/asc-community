document.addEventListener('DOMContentLoaded', function () {
    var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
    if (width < 640) {
        $(".project-link").css("margin-left", 0);
    }
    var projectBoxWidth = parseFloat($(".project-box").css("width"));
    $(".project-box").css("height", projectBoxWidth / 2);
});