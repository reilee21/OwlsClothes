function updateQuantity(sku,maxquantity,change=0) {
    var quanInput = document.getElementById('pro-qtty-input-' + sku);
    var inputValue = parseInt(quanInput.value);

    if (isNaN(inputValue)) {
        quanInput.value = 1;
        return;
    }

    inputValue += change;

    if (inputValue < 1) {
        inputValue = 1;
    } else if (inputValue > maxquantity) {
        inputValue = maxquantity;
    }
    quanInput.value = inputValue;

   
    UpdateCart(sku,quanInput.value);

}
function UpdateCart(sku, newQuantity) {
    $.ajax({
        url: '/Cart/UpdateCartItem',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            Sku: sku,
            Quantity: newQuantity
        }),
        success: function (response) {
            var p = document.getElementById('price-' + sku).innerText;
            var price = parseInt(p);
            var totalPrice = response.updatedItem.quantity * price;
            document.getElementById('cart-total-' + sku).innerText = totalPrice.toLocaleString('en-US', {
                style: 'currency',
                currency: 'VND'
            });

            $('#cartCount').empty();
            if (response.totalQuantity > 0) {
                $('#cartCount').html('<div class="tip">' + response.totalQuantity + '</div>');
            }
            document.getElementById('totalp').innerText = "Tổng tiền : "+ response.total.toLocaleString('en-US', {
                style: 'currency',
                currency: 'VND'
            });
        }
    })
}
function RemoveItem(sku) {
    $.ajax({
        url: '/Cart/RemoveCartItem',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            Sku: sku,
            Quantity: 0
        }),
        success: function (response) {
            document.getElementById('row-' + sku).remove();
            $('#cartCount').empty();
            if (response.totalQuantity > 0) {
                $('#cartCount').html('<div class="tip">' + response.totalQuantity + '</div>');
                document.getElementById('totalp').innerText = "Tổng tiền : " + response.total.toLocaleString('en-US', {
                    style: 'currency',
                    currency: 'VND'
                });
            } else {
                document.getElementById('cart-empty').style.display = "flex";
                document.getElementById('shop-cart').style.display = "none";

            }
        }
    })
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function Details(name) {
    window.location.href = "/Products/" + name;
}