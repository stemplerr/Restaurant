$(function () {
    var MyHub = $.connection.MyHub;
    $.connection.hub.start();

    MyHub.client.statusChanged = function (status) {
        $('#status').text(status.statusName);
        $('.mark-delivered').attr('disabled', true);
    }
});