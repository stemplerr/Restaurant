$(function () {
    $('#mark-delivered').on('click', function () {
        var id = $(this).data('order-id');
        var self = $(this);
        var button = document.getElementById('mark-delivered');
        $.post('/admin/MarkAsDelivered', { orderId: id }, function () {
            $(alert('made it in'));
            button.disabled = true;
        });
    });
});