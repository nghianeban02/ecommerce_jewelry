$(document).ready(function () {
    // su kien on click tren class, . la class, # la id
    $('.active').click(function () {
        var ProductDiscountId = $(this).attr('data-id');

        $.ajax({
            type: 'post',
            data: {
                ProductDiscount_id: ProductDiscountId
            },
            url: '/admin/productdiscount/updateactive',
            success: function (result) {
                console.log(result);
            }
        });
    });
});