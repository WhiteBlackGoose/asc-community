function loadSample(sampleName) {
    var text = $("#" + sampleName).text();
    cm.setValue(text);
}

document.addEventListener('DOMContentLoaded', function () {
    var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
    if (width < 800) {
        var padding = 30;
        width = width - padding;
        $(".code-editor-container").css("max-width", width);
        $(".code-editor-container").css("width", width);
        $(".code-editor-container").css("max-height", width);
        $(".code-editor-container").css("height", width);
        $(".horizontal-align-container").css("display", "inline");
        $(".huge-asc-button").css("width", width);
        $("ul").css("padding", 0);
        $("h6").css("padding-left", 0);

        $("#samples-div").insertAfter($("#msl-output"));
        cm.setOption("lineNumbers", false);
    }
    $("#msl-output").css("max-width", width);

});