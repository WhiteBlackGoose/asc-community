function on_area_click(event, me) {
    if (event.keyCode === 9) {
        var v = me.value, s = me.selectionStart, e = me.selectionEnd;
        me.value = v.substring(0, s) + "\t" + v.substring(e);
        me.selectionStart = me.selectionEnd = s + 1;
        return false;
    }
    else if (event.keyCode === 13) {
        var v = me.value, s = me.selectionStart, e = me.selectionEnd;
        if (s != e) return true;
        i = s;
        while (i > 0 && v[i] != "\n")
            i--;

        fill = ""
        while (v[i] == "\t") {
            fill += "\t";
            i++;
        }

        me.value = v.substring(0, s) + "\n" + fill + v.substring(e);

        me.selectionStart = me.selectionEnd = s + 1 + (i - s);
        return false;
    }
    else {
        return true;
    }
}