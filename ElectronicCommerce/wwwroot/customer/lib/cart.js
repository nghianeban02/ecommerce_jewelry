// Calculate total

function calculateTotal() {
    var total = 0;
    var cartQuantity = 0;
    $('.header__cart-item-price-wrap').each(function () {
        var price = parseInt($(this).find('.subPrice').val());
        var quantity = parseInt($(this).find('.subQuantity').val());

        total = total + (price * quantity);

        cartQuantity = cartQuantity + quantity;
    });
    console.log(" Total: " + total);
    $('.checkout-total-money').text(total + ' $');

    $('#cartQuantity').text(cartQuantity);

    // Cap nhat lai so luong tren gio hang


}

// Show modal choose finger size

function removeItemFromCart(e, id) {
    e.preventDefault();
    console.log("product detail id: " + id);
    removeItem(id);
}

function removeItem(id) {
    var sendDate = (new Date()).getTime();
    $('#pre-loader').show();
    $.ajax({
        type: 'POST',
        data: {
            product_detail_id: id
        },
        url: '/customer/product/removeFromCart',
        success: function (result) {
            var receiveDate = (new Date()).getTime();
            var responseTimeMs = receiveDate - sendDate;
            
            setTimeout(function () {
                $('#pre-loader').hide();
                $('#' + id.trim()).closest('li').remove();
                calculateTotal();
            }, responseTimeMs);
        }
    });
}

function showModalFingerSize() {
    var element = document.getElementById("fingerSizeModal");
    element.classList.remove('hide');
    element.classList.add('show-modal');
}

// Show modal choose wrist size
function showModalWristSize() {
    var element = document.getElementById("wristSizeModal");
    element.classList.remove('hide');
    element.classList.add('show-modal');
}

// Confirm add to cart after choose size for product or not !IMPORTANT

function confirmAddToCart(id, size) {
    // Goi ajax nhan ve cart session
    showCartDisplay();
    $('#pre-loader').show();

    $('.header__cart-no-cart').css("display", "none");

    var sendDate = (new Date()).getTime();
    $.ajax({
        type: 'POST',
        async: false,
        data: {
            size: size,
            product_id: id
        },
        url: '/customer/product/AddToCart',
        success: function (result) {
            console.log(result);
            var receiveDate = (new Date()).getTime();
            var responseTimeMs = receiveDate - sendDate;

            if (result == 'Outstock') {
                alert("Out of stock !");
                setTimeout(function () {
                    $('#pre-loader').hide();
                }, responseTimeMs);
                timeOutCartDisplay();
            }
            else if (typeof result == "string" && result.indexOf('Exists') > -1) {
                setTimeout(function () {
                    $('#pre-loader').hide();

                    const data = result.split(" ");

                    let p_detail_id = data[1];

                    console.log("abc: " + p_detail_id);
                    console.log("abc: " + $('#' + p_detail_id.trim()).text());
                    var numb = parseInt($('#' + p_detail_id.trim()).text());
                    numb = numb + 1;

                    $('#' + p_detail_id.trim()).text(numb);

                    $('#hide' + p_detail_id.trim()).val(numb);

                    // Tinh tong tien
                    calculateTotal();

                }, responseTimeMs);
                setTimeout(timeOutCartDisplay, 5000);
            }
            else {

                // Ok

                console.log("add new item");
                setTimeout(function () {
                    $('#pre-loader').hide();

                    var numb = parseInt($('#cartQuantity').text());

                    numb = numb + 1;

                    $('#cartQuantity').text(numb);

                    // Append san pham vao gio hang

                    var newItem =
                        `<li class="header__cart-item">
                            <div class="cart__over-wrapper" style="display:flex;margin-left:18px;margin-right:20px;">
                                <span style="position: relative;">
                                    <label class="checkbox-container">
                                        <input onclick="checkPaidProduct('${result.product_detail_id}');" class="paid-product" data-id="${result.product_detail_id}" id="check-${result.product_detail_id}" type="checkbox" ${result.isCheck ? `checked="checked"`:``} />
                                        <span class="checkmark" style="position: absolute;top:-9px;"></span>
                                    </label>
                                </span>
                            </div>
                            <img src="/admin/images/products/${result.image}" alt="Watch" class="header__cart-img" />
                            <div class="header__cart-item-info">
                                <div class="header__cart-item-head">
                                    <h5 class="header__cart-item-name">${result.name}</h5>
                                    <div class="header__cart-item-price-wrap">
                                        <span class="header__cart-item-price">${result.price} $</span>
                                        <span class="header__cart-item-multiply">x</span>
                                        <span class="header__cart-item-quantity" id="${result.product_detail_id.trim()}" >${result.quantity}</span>
                                        <input type="hidden" value="${result.price}" class="subPrice"/>
                                        <input type="hidden" value="${result.quantity}" class="subQuantity" id="hide${result.product_detail_id.trim()}"/>
                                    </div>
                                </div>

                                <div class="header__cart-item-body">
                                    <span class="header__cart-item-description">
                                        Kích cỡ sản phẩm đã chọn: ${result.size}
                                                                                    </span>
                                    <span class="header__cart-item-remove" onclick="removeItemFromCart(event,'${result.product_detail_id}');">Xoá</span>
                                </div>
                            </div>
                        </li>`;
                    $('.header__cart-list-item').prepend(newItem);

                    // Tinh tong tien
                    calculateTotal();

                }, responseTimeMs);
                setTimeout(timeOutCartDisplay, 5000);
            }
        }
    });
}


$(document).ready(function () {
    // Click on button add to cart

    $('.btn-add-to-cart').click(function () {
        // Lay ve product id

        var productId = $(this).attr('data-id');
        console.log(productId);

        // Kiem tra day co phai la san pham can chon size

        $.ajax({
            type: 'POST',
            async: false,
            data: {
                product_id: productId
            },
            url: '/customer/product/findProductById',
            success: function (result) {
                console.log(result);

                if (result == 'free-size') {
                    console.log("Day khong can chon size");
                    confirmAddToCart(productId, 0);
                }
                else {
                    if (result.name.startsWith("Nhẫn")) {
                        console.log("Show nhan size guide");
                        showModalFingerSize();
                    }
                    else if (result.name.startsWith("Vòng")) {
                        console.log("Show vong size guide");
                        showModalWristSize();
                    }
                    // Ajax hien thi san pham tuong ung vao modal

                    $('.product-name').text(result.name);
                    $('.product-id').text('Mã sản phẩm: ' + result.producT_ID);
                    $('.product-thumbnail').attr('src', '/admin/images/products/' + result.image);
                    $('.product-price').text(result.price +"$");

                    // Tim ve tat ca size cua san pham tuong ung

                    findAllSizeOfProducts(productId);
                }

            }
        });
    });

    // Xac nhan them vao gio hang sau khi chon size

    $('.btn-add-to-cart__confirm').click(function () {
    console.log('click');
    var size = $('#size').val();
    var id = $('#product-id').val();

    if (size == 0) {
        alert('Vui lòng chọn size của sản phẩm theo bảng chỉ dẫn chọn size !');
    }
    else {
        $('#size').val(0);
        $('#product-id').val(0);
        confirmAddToCart(id, size);
    }
});
});


function findAllSizeOfProducts(productId) {
    $.ajax({
        type: 'POST',
        async: false,
        data: {
            product_id: productId
        },
        url: '/customer/product/findAllSizeOfProducts',
        success: function (data) {
            var result = '';
            for (let i = 0; i < data.length; i++) {
                result += `<div onclick="clickChooseSize('${data[i]}','${productId}');" data-id="${data[i]}" class="size-box">
                         <span style="margin: auto;font-weight:lighter;font-size: 14px;">${data[i]}</span>
                         </div>`;
            }

            $('.product-size__box').html(result);

        }
    });
}

function clickChooseSize(size,id) {
    console.log(size + id);
    $('.product-size__box .size-box').each(function () {
        $(this).css("background-color", "transparent");
        $(this).css("color", "#333");
        if ($(this).attr('data-id') == size) {
            $(this).css("background-color", "#8f7d59");
            $(this).css("color", "#FFFFFF");
        }
    });


    $.ajax({
        type: 'POST',
        data: {
            size: size,
            product_id: id
        },
        url: '/customer/product/findPriceBySizeAndId',
        success: function (result) {
            console.log(result);
            var priceFormatted = result.toLocaleString('en-US', {
                style: 'currency',
                currency: 'USD',
            });
            $('.product-price').text(priceFormatted.replace('.', ','));
        }
    });

    $('#size').val(size);
    $('#product-id').val(id);
}

// Close modal finger size

function closeModalFingerSize() {
    var element = document.getElementById("fingerSizeModal");
    element.classList.remove('show-modal');
    element.classList.add('hide');
}

// Close modal wrist size

function closeModalWristSize() {
    var element = document.getElementById("wristSizeModal");
    element.classList.remove('show-modal');
    element.classList.add('hide');
}

// Show finger size guide

function showFingerSizeGuide() {
    var element = document.getElementById("size-guide__table");
    element.classList.remove('hide');
    element.classList.add('show');
}

// Hide finger size guide

function hideFingerSizeGuide(event) {
    event.stopPropagation();
    const element = document.querySelector(".size-guide__table");
    element.classList.remove("show");
    element.classList.add('hide');
}

// Show wrist size guide

function showWristSizeGuide() {
    var element = document.getElementById("size-wrist-guide__table");
    element.classList.remove('hide');
    element.classList.add('show');
}

// Hide wrist size guide

function hideWristSizeGuide(event) {
    event.stopPropagation();
    var element = document.getElementById("size-wrist-guide__table");
    element.classList.remove('show');
    element.classList.add('hide');
}

// Open cart for 3s after add product to cart

function timeOutCartDisplay() {
    var cart = document.getElementById("header__cart-list");
    cart.style.display = "none";

}

function showCartDisplay() {
    var cart = document.getElementById("header__cart-list");
    cart.style.display = "block";
    closeModalFingerSize();
    closeModalWristSize();
}

// Xu ly nguoi dung chon san pham nao de thanh toan
function checkPaidProduct(id) {
    // Kiem tra nguoi dung chon san pham nao de thanh toan
    var sendDate = (new Date()).getTime();
    

    $('.paid-product').each(function () {
        $('#pre-loader').show();
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
                        var receiveDate = (new Date()).getTime();
                        var responseTimeMs = receiveDate - sendDate;
                        $('#')
                        console.log()

                        setTimeout(function () {
                            $('#pre-loader').hide();
                        }, responseTimeMs);
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
                        var receiveDate = (new Date()).getTime();
                        var responseTimeMs = receiveDate - sendDate;

                        setTimeout(function () {
                            $('#pre-loader').hide();
                        }, responseTimeMs);
                    }
                });
            }
        }
    });
}
