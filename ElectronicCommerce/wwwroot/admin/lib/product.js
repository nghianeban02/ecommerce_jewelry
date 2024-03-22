$(document).ready(function () {
        // su kien on click tren class, . la class, # la id
        $('.best-seller').click(function () {
            console.log("Best seller: " + $(this).val());
            console.log("Product Id: " + $(this).attr('data-id'));

            var productId = $(this).attr('data-id');

            $.ajax({
                type: 'POST',
                data: {
                    product_id: productId
                },
                url: '/admin/product/updatebestseller',
                success: function (result) {
                    console.log(result);
                }
            });
        });
    // homeflag
    $('.home-flag').click(function () {
        console.log("Home flag: " + $(this).val());
        console.log("Product Id: " + $(this).attr('data-id'));

        var productId = $(this).attr('data-id');

        $.ajax({
            type: 'POST',
            data: {
                product_id: productId
            },
            url: '/admin/product/updatehomeflag',
            success: function (result) {
                console.log(result);
            }
        });
    });
    // active
    $('.active').click(function () {
        console.log("Active: " + $(this).val());
        console.log("Product Id: " + $(this).attr('data-id'));

        var productId = $(this).attr('data-id');

        $.ajax({
            type: 'POST',
            data: {
                product_id: productId
            },
            url: '/admin/product/updateactive',
            success: function (result) {
                console.log(result);
            }
        });
    });
});