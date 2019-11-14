function calc_sample_onclick(sample) {
    $("#prompt").val(sample.text);
}

function latexres_onclick() {
    $("#latex-res").toggle();
    $("#plain-res").toggle();
}
var width = 0;

document.addEventListener('DOMContentLoaded', function () {
    width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
    if (width < 640) {
        $(".asc-calc-title").hide();
        $("#inter-as").hide();
        $("#sendButton").hide();
        $("#example-block-calc").hide();
    }
});