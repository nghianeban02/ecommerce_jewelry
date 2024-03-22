function applyPromotion(e, code, total) {
    e.preventDefault();
    var sendDate = (new Date()).getTime();

    console.log("code promo: "+code);

    $.ajax({
        type: 'POST',
        data: {
            promoCode: code,
            cartTotal:total
        },
        url: '/customer/cart/ApplyPromotion',
        success: function (result) {
            var receiveDate = (new Date()).getTime();
            var responseTimeMs = receiveDate - sendDate;

            console.log(responseTimeMs);
            if (typeof result === 'string') {
                alert(result);
            }
            else {
                $('#promotion-txb').val(code);
                // Lay ve tong tien thuc apply promo

                let temp_total = $('#total-cart').val();
                var total_before_promo = parseFloat(temp_total);
                console.log("Total before promo: " + total_before_promo);
                var total_after_promo = 0;

                if (result.discountUnit.trim() == '%') {
                    var actual_discount = total_before_promo * parseFloat(result.discountValue / 100);
                    console.log("Muc giam that su: " + actual_discount);

                    // Neu vuot qua max discount cua promo

                    if (actual_discount > result.maxDiscount) {
                        actual_discount = result.maxDiscount;
                    }
                    total_after_promo = parseFloat(total_before_promo - actual_discount).toFixed(2);
                    console.log(total_after_promo);
                }
                else if (result.discountUnit == '$') {
                    total_after_promo = total_before_promo - result.discountValue; 
                }

                // Hien thi gia tong tien sau khi giam

                $('#total-booking-price').text(total_after_promo + " $");

                // Hien thi gia truoc khi giam

                $('#total-before-promo').text(total_before_promo + " $");

                // Chinh sua total khi thanh toan paypal hoac cod

                $('.total-cart-paid').val(total_after_promo);

                console.log("Gia da giam sau promo: "+total_after_promo);

                console.log(result);
            }
        }
    });
}

function loadShippingInfo() {
    $('#pre-loader').show();
    $.ajax({
        type: 'GET',
        async: false,
        url: '/customer/cart/isShipInformation',
        success: function (result) {
            if (result != null) {
                console.log(result);
                var shippingInfo = `<div class="letter-information-wrapper">
                        <div class="name-and-phone">
                            <span>${result.fullname}</span> <span style="color:#999;">${result.phone}</span>
                        </div>

                        <div class="mail-info">
                            <span>Dia chi mail: ${result.mail}</span>
                        </div>

                        <div class="address-info">
                            <span>${result.address}</span>
                        </div>

                        <div class="default-address">
                            <div>Địa chỉ mặc định</div>
                            <span>Chỉnh sửa</span>
                        </div>
                    </div>`;
                setTimeout(function () {
                    $('#pre-loader').hide();
                    $('.information-form__wrapper').html(shippingInfo);
                }, 800);
            }
            else {
                console.log('shipping session has not been created');
                var shippingForm = `<div class="area__wrapper" style="margin-top:26px;">
                                <span>Vị trí*</span>
                                <span style="margin-top:10px;">Vietnam</span>
                            </div>

                            <div class="name__wrapper">
                                <div class="full_name">
                                    <input type="text" id="full_name-input" required class="full_name-input" />
                                    <span class="full_name-holder">Họ và tên*</span>
                                </div>

                                <div class="customer_mail">
                                    <input type="text" id="gmail-input" required class="customer_mail-input" />
                                    <span class="customer_mail-holder">Địa chỉ gmail*</span>
                                </div>
                            </div>

                            <div class="phone__wrapper">
                                <div class="zip__code">
                                    <span>VN +84</span>
                                </div>

                                <div class="phone__wrapper-box">
                                    <div class="phone__wrapper-txb">
                                        <input type="text" class="phone-input" required id="phone-input" />
                                        <span class="phone-holder" style="padding-top:3px;">Số điện thoại liên lạc*</span>
                                    </div>
                                </div>
                            </div>

                            <div class="address__wrapper">
                                <span class="address_title">Địa chỉ đường phố*</span>
                                <textarea id="address-input" placeholder="Địa chỉ đường phố ,căn hộ, Suĩte, Đơn vi,v.v."></textarea>
                            </div>

                            <div class="policy__wrapper">
                                <a href="#">Chính sách bảo mật & Cookie</a>
                                <a href="#">ghi chú địa chỉ chung</a>
                            </div>
                            <div class="shipping-information__wrapper">
                                <button class="btn btn-confirm-information" onclick="saveCardShippingInfo(event);">Xác nhận và lưu</button>
                            </div>`;
                setTimeout(function () {
                    $('#pre-loader').hide();
                    $('.information-form__wrapper').html(shippingForm);
                }, 800);
            }
        }
    });
}

function saveCardShippingInfo(e) {
    e.preventDefault();
    var fullname = $('#full_name-input').val();
    var phone = $('#phone-input').val();
    var gmail = $('#gmail-input').val();
    var address = $('#address-input').val();

    $.ajax({
        type: 'POST',
        async:false,
        data: {
            fullname: fullname,
            mail: gmail,
            phone: phone,
            address: address
        },
        url: '/customer/cart/saveShipInformation',
        success: function (result) {
            alert(result.message);
            loadShippingInfo();
        }
    });
}

// Hide payment modal

function closePaymentModal(e) {
    e.preventDefault();
    var element = document.getElementById("paymentModal");
    element.classList.remove('show-modal');
    element.classList.add('hide');
}

// Show payment modal

function showPaymentModal(e) {
    e.preventDefault();
    var element = document.getElementById("paymentModal");
    element.classList.remove('hide');
    element.classList.add('show-modal');
}
