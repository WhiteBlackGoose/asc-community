function loadSample(sampleName) {
    var text = $("#" + sampleName).text();
    cm.setValue(text);
}

document.addEventListener('DOMContentLoaded', function () {
    var width = (window.innerWidth > 0) ? window.innerWidth : screen.width;
    if (width < 800) {
        var dw = parseFloat($(".container").css("margin-left")) +
            parseFloat($(".container").css("padding-left")) + 50;
        width = parseFloat($(".navbar").css("width")) - dw;
        $(".code-editor-container").css("max-width", width);
        $(".code-editor-container").css("width", width);
        $(".code-editor-container").css("max-height", width);
        $(".code-editor-container").css("height", width);
        $(".horizontal-align-container").css("display", "inline");
        $("#samples-div").insertAfter($("#msl-output"));
        $("ul").css("padding-left", 0);
        $("ul").css("max-width", width);
        $("h6").css("padding-left", 0);
        cm.setOption("lineNumbers", false);
    }
    $("#msl-output").css("max-width", width);

});