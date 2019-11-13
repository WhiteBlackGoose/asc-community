function calc_sample_onclick(sample) {
    $("#prompt").val(sample.text);
}

function latexres_onclick() {
    $("#latex-res").toggle();
    $("#plain-res").toggle();
}