var sendDate;

function loadCartSummary() {
    console.log('star loadCartSummary');
    $.ajax({
        type: 'GET',
        url: '/customer/product/findAllCart',
        success: function (cart) {
            console.log(cart);
            if (cart != null && cart.length > 0) {
                $('.cart__summary-title').text(`Tóm Tắt Mặt Hàng(${cart.length})`);

                var totalSave = 0;
                var total = 0;

                for (let i = 0; i < cart.length; i++) {
                    var priceFormatted = cart[i].price.toLocaleString('it-IT', { style: 'currency', currency: 'VND' })
                    if (cart[i].isCheck) {
                        total += cart[i].price * cart[i].quantity;
                        totalSave += cart[i].savePrice * cart[i].quantity;
                    }

                    else {
                        $('.paid-check-all').prop("checked", false);
                    }
                    var subTotal = (cart[i].price * cart[i].quantity).toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                }


                // Tong tien gio hang

                $('#cart-temp-total').text(total);
                $('#totalSave').text('Tiết kiệm: '+totalSave.toLocaleString('it-IT', { style: 'currency', currency: 'VND' }));

            } else {
                var emptyCart = document.getElementById("empty-cart");
                emptyCart.style.display = "block";
                emptyCart.style.marginBottom = "20px";

                var cartDetail = document.getElementById("cart-detail");
                cartDetail.display = "none";
            }
        }
    });

    console.log('End loadCart');
}

// CHECK COMMIT PAID ON CART

function checkAllPaidProduct() {
    console.log('check all');
    // Kiem tra trang thai truoc khi nhan

    $('#pre-loader').show();

    setTimeout(function () {
        if ($('.paid-check-all').is(':checked')) {
            // Lap qua cac checkbox va set trang thai la checked cho moi checkbox

            loopCheckBox(true);

            // Tinh lai tong tien gio hang

            loadCartSummary();
        }
        else {
            console.log("uncheck");
            // Lap qua cac checkbox va set trang thai la checked cho moi checkbox

            loopCheckBox(false);

            // Tinh lai tong tien gio hang

            loadCartSummary();
        }

        $('#pre-loader').hide();
    }, 800);
}

// Lap qua va check cac checkbox

function loopCheckBox(isCheck) {

    $('.paid-product').each(function () {
        var productDetailId = $(this).attr('data-id');

        // Set state  checked or not for checkbox

        $(this).prop("checked", isCheck);

        $.ajax({
            type: 'POST',
            async: false,
            data: {
                product_detail_id: productDetailId,
                isCheck: isCheck
            },
            url: '/customer/cart/updatePaidProduct',
            success: function (result) {
                console.log(result);
            }
        });
    });
}

// Kiem tra nguoi dung chon san pham nao de thanh toan

function checkPaidProduct(id) {

    $('#pre-loader').show();

    var isAllCheck = true;

    // Kiem tra set tick cho nut check all

    $('.paid-product').each(function () {
        if (!($(this).is(':checked'))) {
            console.log('1 un check');
            isAllCheck = false;
        }
    });

    console.log(isAllCheck);

    tickTheCheckAll(isAllCheck);

    // Kiem tra nguoi dung chon san pham nao de thanh toan

    $('.paid-product').each(function () {
        if ($(this).attr('data-id') == id) {
            // Cap nhat trang thai isCheck trong cart

            if ($(this).is(':checked')) {
                $.ajax({
                    type: 'POST',
                    async: false,
                    data: {
                        product_detail_id: id,
                        isCheck: true
                    },
                    url: '/customer/cart/updatePaidProduct',
                    success: function (result) {
                        $('#pre-loader').hide();
                        loadCartSummary();
                        console.log(result);
                    }
                });
            }
            else {
                $.ajax({
                    type: 'POST',
                    async: false,
                    data: {
                        product_detail_id: id,
                        isCheck: false
                    },
                    url: '/customer/cart/updatePaidProduct',
                    success: function (result) {
                        $('#pre-loader').hide();
                        loadCartSummary();
                        console.log(result);
                    }
                });
            }
        }
    });
}

// Chon vao check all

function tickTheCheckAll(isCheck) {
    $('.paid-check-all').prop("checked", isCheck);
}

