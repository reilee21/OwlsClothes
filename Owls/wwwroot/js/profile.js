let city;
let district;
let ward;


document.addEventListener('DOMContentLoaded', function () {
    city = document.getElementById('province');
    district = document.getElementById('district');
    ward = document.getElementById('village');
 
    fetch('/data.json')
        .then((response) => response.json())
        .then((data) => renderCity(data));


    function renderCity(data) {
        for (var item of data) {
            city.options[city.options.length] = new Option(item.Name, item.Id);
            city_edit.options[city_edit.options.length] = new Option(item.Name, item.Id);

        }
        if (oldCity && oldDistrict && oldWard) {

            city.value = c.Id;
            for (var item of c.Districts) {
                district.options[district.options.length] = new Option(item.Name, item.Id);
            }
            district.value = d.Id
            for (var item of d.Wards) {
                ward.options[ward.options.length] = new Option(item.Name, item.Id);
            }
            ward.value = w.Id;
        }
       
        city.onchange = () => {
            district.length = 1;
            ward.length = 1;

            if (city.value != '' && city.value !== "invalid") {
                const result = data.filter((n) => n.Id === city.value);

                for (var item of result[0].Districts) {
                    district.options[district.options.length] = new Option(item.Name, item.Id);
                }
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
