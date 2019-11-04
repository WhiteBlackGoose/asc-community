function on_area_click(event, me) {
    if (event.keyCode === 9) {
        var v = me.value, s = me.selectionStart, e = me.selectionEnd;
        me.value = v.substring(0, s) + "    " + v.substring(e);
        me.selectionStart = me.selectionEnd = s + 4;
        return false;
    }
    else if (event.keyCode === 13) {
        var v = me.value, s = me.selectionStart, e = me.selectionEnd;
        if (s != e) return true;
        i = s;
        while (i > 0 && v[i] != "\n")
            i--;
        if(i != 0)
            i++;
        subs = v.substring(i, s);
        i = 0;
        var r = "";
        while (i < subs.length && subs[i] == " ") {
            i++;
            r += " ";
        }
        
        //me.selectionEnd = me.selectionStart;
        me.value = v.substring(0, s) + "\n" + r + v.substring(s);
        me.selectionStart = me.selectionEnd = s + i;
        return false;
    }
}