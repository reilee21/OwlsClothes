﻿var selectedSize = "";
var selectedColor = "Mặc định";
var selectedSKU = "";
var cartquantity = 0;


function updateSelectedSize(size) {

    selectedSize = size;
    updateUIbySize();
    updatePrice();

    var labels = document.querySelectorAll('.size__btn label');

    labels.forEach(function (label) {

        if (!label.querySelector('input').disabled) {


            if (label.getAttribute('for') === size) {
                label.classList.add('active');
            } else {
                label.classList.remove('active');
            }
        }
    });
}
function updateSelectedColor(color) {
    selectedColor = color;
    updateUIbyColor(selectedColor);
    updatePrice();

    var labels = document.querySelectorAll('.color__checkbox label');
    labels.forEach(function (label) {      

        if (label.getAttribute('for') === 'color-' + color) {
            label.classList.add('active');
        } else {
            label.classList.remove('active');
        }
    });
}

function updatePrice() {
    var pro = getProductVariant();
    if (pro == null) return;

    var sale_price = document.getElementById('sale_price');
    var sale_price_discount = document.getElementById('dis_sale_price');
    var pro_discount = document.getElementById('pro_discount');
    var price = pro.salePrice;
    if (pro.totalDiscount != 0) {
        sale_price.textContent = price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
        price = pro.salePrice * (1 - pro.totalDiscount / 100);
        sale_price_discount.textContent = price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
        pro_discount.textContent = pro.totalDiscount + ' %';
        pro_discount.classList.add('px-3')
        pro_discount.classList.add('py-1')


    } else {
        sale_price_discount.textContent = price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
        sale_price.textContent = '';
        pro_discount.textContent = '';
        pro_discount.classList.remove('px-3')
        pro_discount.classList.remove('py-1')


    }
}
function AddToCart() {
    var pro = getProductVariant(); 
    var quanInput = document.getElementById('inputquan');
    var inputValue = parseInt(quanInput.value);
    cartquantity = inputValue;
    var statusErrorsModal = new bootstrap.Modal(document.getElementById('statusErrorsModal'));
    if (pro == null) {
        statusErrorsModal.show();
        setTimeout(function () {
            statusErrorsModal.hide();
        }, 1500);
        return;
    }
    selectedSKU = pro.sku;

    $.ajax({
        url: '/Cart/AddToCartItem',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            Sku: selectedSKU,
            Quantity: cartquantity
        }),
        success: function (response) {

            var statusSuccessModal = new bootstrap.Modal(document.getElementById('statusSuccessModal'));
            statusSuccessModal.show();
            setTimeout(function () {
                statusSuccessModal.hide();
            }, 2000);
            $('#cartCount').empty();
            if (response > 0) {
                $('#cartCount').html('<div class="tip">' + response + '</div>');
            }
        },
        error: function (xhr, status, error) {
            
            var ct = document.getElementById('error-content');
            ct.innerText = "Không còn đủ số lượng sản phẩm."
            statusErrorsModal.show();
            setTimeout(function () {
                statusErrorsModal.hide();

            }, 2000); 
        }
    });
}

function checkQuantity() {
    var quanInput = document.getElementById('inputquan');
    var inputValue = parseInt(quanInput.value);


    if (isNaN(inputValue) || inputValue < 1 || inputValue > 50) {
        quanInput.value = 1;
    }
}
function updateUIbySize() {

    var colorInputs = document.querySelectorAll('.color__checkbox input');  
    var rs = getProductVariantBySize(selectedSize);
    colorInputs.forEach(function (input) { 
        var color = input.value; 
        var variantWithColor = rs.find(function (variant) {
            if (color == 'Mặc định') color = null;
            return variant.colorName === color;
        });

        if (!variantWithColor || variantWithColor?.quantity === 0) {
            input.disabled = true; 
        } else {
            input.disabled = false; 
        }
    });
}
function updateUIbyColor() {

    var sizeInputs = document.querySelectorAll('.size__btn input');
    var rs = getProductVariantByColor(selectedColor);
    sizeInputs.forEach(function (input) {
        let size = '';
        if (!input.value.includes("Size")) {
            size = 'Size '
        }
         size += input.value;
        var variantWithSize = rs.find(function (variant) {
            return variant.size === size;
        });

        if (!variantWithSize || variantWithSize?.quantity === 0) {
            input.disabled = true;
        } else {
            input.disabled = false;
        }
    });
}
function getProductVariantBySize(size) {
    if (!size.includes("Size")) {
        size = 'Size ' + size;
    }
    return productVariants.filter(function (variant) {
        if (size == variant.size)
            return variant;
    });
}
function getProductVariantByColor(color) {
    if (color == 'Mặc định') color = null;
    return productVariants.filter(function (variant) {
        if (color == variant.colorName)
            return variant;
    });
}
function getProductVariant() {
    var x = "";
    if (selectedColor == null || selectedColor == 'Mặc định') x = null;
    else x = selectedColor;

    var stemp = null;
    if (selectedSize != null && selectedSize.length > 0) {
        if (selectedSize.startsWith('Size')){
            stemp = selectedSize;
        }
        else {
            stemp = 'Size ' + selectedSize;

        }
    }


    return productVariants.find(function (variant) {
        return variant.colorName === x && (variant.size === stemp || variant.size === '');
    });

}


