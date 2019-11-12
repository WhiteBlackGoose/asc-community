

var editor = CodeMirror.fromTextArea(document.getElementById("demotext"), {
    lineNumbers: true,
    matchBrackets: true,
    mode: "text/x-csharp",
    indentUnit: 4,
});

var mac = CodeMirror.keyMap.default == CodeMirror.keyMap.macDefault;
CodeMirror.keyMap.default[(mac ? "Cmd" : "Ctrl") + "-Space"] = "autocomplete";

var symsToComplete = [190, 188]

editor.on("keyup", function (cm, e) {
    if (symsToComplete.indexOf(e.keyCode) >= 0) {
        CodeMirror.commands.autocomplete(cm);
    }
})

var dictionary =
    [
        "class",
        "for",
        "if",
        "else",
        "elif",
        "while",
        "static",
        "var",
        "function",
        "const",
        "public",
        "private",
        "internal",
        "abstract",
        "namespace",
        "interface",
        "return",
        "lambda",
        "this",
        "in",
        "foreach",
        "true",
        "false",
        "null",
        "using",
        "System",
        "Console",
        "Integer",
        "Float",
        "String",
        "True",
        "False",
        "Reflection",
        "Type",
        "Print",
        "PrintLine",
        "GetType",
        "Utils",
        "Math",
        "Vector2",
        "Vector3",
        "x",
        "y",
        "z",
        "Array",
        "Size",
    ];

CodeMirror.commands.autocomplete = function (cm) {
    CodeMirror.showHint(cm, function (editor) {
        var cur = editor.getCursor();
        var curLine = editor.getLine(cur.line);
        var start = cur.ch;
        var end = start;
        while (end < curLine.length && /[\w$]/.test(curLine.charAt(end)))++end;
        while (start && /[\w$]/.test(curLine.charAt(start - 1)))--start;
        var curWord = start !== end && curLine.slice(start, end);
        var regex = new RegExp('^' + curWord, 'i');
        return {
            list: (!curWord ? dictionary : dictionary.filter(function (item) {
                return item.match(regex);
            })).sort(),
            from: CodeMirror.Pos(cur.line, start),
            to: CodeMirror.Pos(cur.line, end)
        }
    });
};

function onpushsuccess(data) {
    $("#io-line").prop("disabled", true);
    $("#msl-output").text($("#msl-output").text() + "\n[input]: " + $("#io-line").val());
    $("#io-line").val("");
}

function onpullsuccess(data) {
    if (data == "")
        return;
    $("#io-line").prop("disabled", false);
    $("#msl-output").text($("#msl-output").text() + data);
}

function makereq(arg, val, onok) {
    $.get("/msl/msl?" + arg + "=" + val + "&ReturnedId=" + MSLID).done(onok);
}

function textarea_onkeyup(event) {
    if (event.which === 13) {
        req = $("#io-line").val();
        makereq("InputMSL", req, onpushsuccess);
    }
}

function pullnew() {
    if (MSLID == "")
        return;
    req = "1";
    makereq("IfPull", req, onpullsuccess);
}

$("#io-line").keyup(textarea_onkeyup);
//window.setInterval(pullnew, 1000);