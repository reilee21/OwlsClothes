let city_edit;
let district_edit;
let ward_edit;


document.addEventListener('DOMContentLoaded', function () {
    city_edit = document.getElementById('province_edit');
    district_edit = document.getElementById('district_edit');
    ward_edit = document.getElementById('village_edit');

    fetch('/data.json')
        .then((response) => response.json())
        .then((data) => renderCity(data));


    function renderCity(data) {
        window.init = function (cityId, districtId, wardId) {
            if (cityId) {
                var c = data.filter((a) => a.Id == cityId)[0];
                city_edit.value = cityId;
                for (var item of c.Districts) {
                    district_edit.options[district_edit.options.length] = new Option(item.Name, item.Id);
                }
                if (districtId) {
                    var d = c.Districts.filter((d) => d.Id == districtId)[0];
                    district_edit.value = districtId;
                    for (var item of d.Wards) {
                        ward_edit.options[ward_edit.options.length] = new Option(item.Name, item.Id);
                    }
                    if (wardId) {
                        ward_edit.value = wardId;
                    }
                }

            }
        }

        city_edit.onchange = () => {
            district_edit.length = 1;
            ward_edit.length = 1;


            if (city_edit.value != '' && city_edit.value !== "invalid") {
                const result = data.filter((n) => n.Id === city_edit.value);
                for (var item of result[0].Districts) {
                    district_edit.options[district_edit.options.length] = new Option(item.Name, item.Id);
                }
            }

        };

        district_edit.onchange = () => {
            ward_edit.length = 1;
            const result = data.filter((el) => el.Id === city_edit.value);
            if (district_edit.value != ' ') {
                const resultDistrict = result[0].Districts.filter((el) => el.Id === district_edit.value);

                for (var item of resultDistrict[0].Wards) {
                    ward_edit.options[ward_edit.options.length] = new Option(item.Name, item.Id);
                }
            }
        };
    }
});

$(document).ready(function () {
    $('#editAccountModal').on('hide.bs.modal', function () {
        $('#editaccountForm')[0].reset();
        $('#editaccountForm input').removeClass('is-invalid');
        $('#editaccountForm .invalid-feedback').remove();

        $('#editaccountForm select').each(function () {
            $(this).val($(this).find('option:first').val());
        });
    });
});
$("#editaccountForm").on("submit", function (event) {
    event.preventDefault();
    if ($(this).valid()) {
        var formdata = $(this).serialize();
        $.ajax({
            url: "/Admin/Accounts/UpdateAccount",
            type: 'post',
            data: formdata,
            success: function (rs) {
                $('#editAccountModal').modal('hide');
                location.reload();
            },
            error: function (xhr, status, error) {
            }
        })
    }
});