$(document).ready(function ($) {
    "use strict";

    var fullHeight = function () {
        $('.js-fullheight').css('height', $(window).height());
        $(window).resize(function () {
            $('.js-fullheight').css('height', $(window).height());
        });
    };
    fullHeight();

    var carousel = function () {
        if ($.fn.owlCarousel) {
            $('.home-slider').owlCarousel({
                loop: true,
                autoplay: true,
                margin: 0,
                animateOut: 'fadeOut',
                animateIn: 'fadeIn',
                nav: true,
                dots: true,
                autoplayHoverPause: false,
                items: 1,
                navText: ["<span class='ion-ios-arrow-back'></span>", "<span class='ion-ios-arrow-forward'></span>"],
                responsive: {
                    0: {
                        items: 1
                    },
                    600: {
                        items: 1
                    },
                    1000: {
                        items: 1
                    }
                }
            });
        }
    };
    carousel();

});

$(".product__details__pic__slider").owlCarousel({
    loop: false,
    margin: 0,
    items: 1,
    dots: false,
    nav: true,
    navText: ["<i class='arrow_carrot-left'></i>", "<i class='arrow_carrot-right'></i>"],
    smartSpeed: 1200,
    autoHeight: false,
    autoplay: false,
    mouseDrag: false,
    startPosition: 'URLHash'
}).on('changed.owl.carousel', function (event) {
    var indexNum = event.item.index + 1;
    product_thumbs(indexNum);
});

function product_thumbs(num) {
    var thumbs = document.querySelectorAll('.product__thumb a');
    thumbs.forEach(function (e) {
        e.classList.remove("active");
        if (e.hash.split("-")[1] == num) {
            e.classList.add("active");
        }
    })
}

