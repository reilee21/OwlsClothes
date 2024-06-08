let city;
let district;
let ward;
let sfee;
let totalprice;

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
            sfee.innerHTML = '' + shfee.fee.toLocaleString('en-US') +'₫'
            var p = parseInt(stt) + shfee.fee;
            totalprice.innerHTML = '' + p.toLocaleString('en-US') + '₫'
        }
        else
            sfee.innerHTML = '---';
    }

    function renderCity(data) {
        for (var item of data) {
            city.options[city.options.length] = new Option(item.Name, item.Id);
        }
        updateShippingFee('');

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
