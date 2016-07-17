$(function () {
    $('.add-to-cart-button').on('click', function () {
        var id = $(this).data('id');
        $.post('/home/AddToCart', { itemId: id }, function (cartId) {
            console.log(cartId);
        });
    });
});