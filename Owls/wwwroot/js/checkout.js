let city;
let district;
let ward;
let sfee;
let totalprice;
let shippingfee = 0;

document.addEventListener('DOMContentLoaded', function () {
    city = document.getElementById('province');
    district = document.getElementById('district');
    ward = document.getElementById('village');
    sfee = document.getElementById('shipping-fee');
    totalprice = document.getElementById('totalprice');
    fetch('/data.json')
        .then((response) => response.json())
        .then((data) => renderCity(data));
    function updateShippingFee(ct) {
        var shfee = shipFee.find(f => f.city === ct);
        if (shfee) {
            sfee.innerHTML = '' + shfee.fee.toLocaleString('vi-vn', { style: "currency", currency: "VND" });
            shippingfee = shfee.fee
            updateTotal();
        }
        else {
            shippingfee = 0;
            sfee.innerHTML = '---';
        }
    }
    function updateTotal() {
        var newTotal = stt + shippingfee - vchdiscount;
        totalprice.innerHTML = '' + newTotal.toLocaleString('vi-vn', { style: "currency", currency: "VND" });

    }
    function renderCity(data) {
        for (var item of data) {
            city.options[city.options.length] = new Option(item.Name, item.Id);
        }
        updateShippingFee('');
        if (uscity) {
            var c = data.filter((a) => a.Id == uscity)[0];
            city.value = uscity;
            updateShippingFee(c.Name)
            for (var item of c.Districts) {
                district.options[district.options.length] = new Option(item.Name, item.Id);
            }
            if (usdistrict) {
                var d = c.Districts.filter((d) => d.Id == usdistrict)[0];
                district.value = usdistrict;
                for (var item of d.Wards) {
                    ward.options[ward.options.length] = new Option(item.Name, item.Id);
                }
                if (usward) {
                    var w = d.Wards.filter((w) => w.Id == usward)[0];
                    ward.value = usward;
                }
            }
        }

        city.onchange = () => {
            district.length = 1;
            ward.length = 1;

            if (city.value != '' && city.value !== "invalid") {
                const result = data.filter((n) => n.Id === city.value);
                updateShippingFee(result[0].Name);

                for (var item of result[0].Districts) {
                    district.options[district.options.length] = new Option(item.Name, item.Id);
                }
            } else {
                updateShippingFee('');
            }

        };

        district.onchange = () => {
            ward.length = 1;
            const result = data.filter((el) => el.Id === city.value);
            if (district.value != ' ') {
                const resultDistrict = result[0].Districts.filter((el) => el.Id === district.value);

                for (var item of resultDistrict[0].Wards) {
                    ward.options[ward.options.length] = new Option(item.Name, item.Id);
                }
            }
        };
    }
});
