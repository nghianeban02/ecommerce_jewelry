$(document).ready(function () {
    // su kien on click tren class, . la class, # la id
    $('.activate').click(function () {
        var promotionId = $(this).attr('data-id');
        console.log("Activate: " + $(this).val());
        console.log("Promotion Id: " + $(this).attr('data-id'));


        $.ajax({
            type: 'post',
            data: {
                promotion_id: promotionId
            },
            url: '/admin/promotion/updateactivate',
            success: function (result) {
                console.log(result);
            }
        });
    });
});