document.addEventListener("DOMContentLoaded", function () {
    const dropdowns = document.querySelectorAll(".dropdown");

    dropdowns.forEach(function (dropdown) {
        const toggle = dropdown.querySelector(".dropdown-toggle");
        const menu = dropdown.querySelector(".dropdown-menu");
        const items = dropdown.querySelectorAll(".dropdown-item");
        const selectedValue = dropdown.querySelector(".selected-value");

        toggle.addEventListener("click", function () {
            menu.classList.toggle("show");
        });

        items.forEach(function (item) {
            item.addEventListener("click", function () {
                toggle.textContent = item.textContent;
                selectedValue.value = item.getAttribute("data-value");
                menu.classList.remove("show");
            });
        });

        document.addEventListener("click", function (event) {
            if (!dropdown.contains(event.target)) {
                menu.classList.remove("show");
            }
        });
    });
});
