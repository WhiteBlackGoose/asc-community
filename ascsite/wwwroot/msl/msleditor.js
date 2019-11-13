var cm = CodeMirror.fromTextArea(document.getElementById("msl-editor"), {
    lineNumbers: true,
    mode: "text/x-csharp",
    lineWrapping: true,
    matchBrackets: true,
});

function loadSample(sampleName) {
    var text = $("#" + sampleName).text();
    cm.setValue(text);
}