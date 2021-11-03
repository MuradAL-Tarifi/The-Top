var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (fromUser, message, photo) {

    
    var html_code = '<div class="answer left">';
    html_code += '<div class="avatar"><img src="' + photo +'"></div >';
    html_code += '<div class="name">' + fromUser +'</div>';
    html_code += '<div class="text">' + message +'</div >';
    html_code += '<div class="time">&ensp;</div>';
    html_code += "</div>";

    

    $("#list").append(html_code);

});

connection.start();

$("#btnSend").on("click", function () {

    var fromUser = $("#txtUser").val();
    var message = $("#txtMessage").val();
    var photo = $("#photo").prop('src');
    connection.invoke("SendMessage", fromUser, message, photo);
});