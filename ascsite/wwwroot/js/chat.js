"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var xhttp = new XMLHttpRequest();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = false;

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {

    
    $.ajax({
        type: 'GET',
        url: '/calc',
        dataType: "text",
        success: function (result) {
            //document.getElementById("response").innerText = "sa";
            location.reload();
            //alert(result);
        }
    });
    
    //document.getElementById("response").innerHTML = req.responseText;
});