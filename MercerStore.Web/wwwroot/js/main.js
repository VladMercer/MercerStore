//window.addEventListener('scroll', function () {
//    document.getElementById('header-nav').classList.toggle('headernav-scroll', window.scrollY > 133);
//});


$(document).ready(function () {
    $(".event-carousel").owlCarousel({
        items: 1,
        loop: true,
        nav: true,
        dots: true,
        autoplay: true,
        autoplayTimeout: 5000,

    });
});


$(document).ready(function () {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 300) {
            $('#top').fadeIn();
        } else {
            $('#top').fadeOut();
        }
    });

    $('#top').click(function () {
        $('html, body').animate({scrollTop: 0}, 500);
        return false;
    });

    $(".product-carousel").owlCarousel({
        loop: true,
        margin: 20,
        dots: true,
        autoplay: false,
        autoplayTimeout: 5000,
        responsive: {
            0: {
                items: 1
            },
            500: {
                items: 2
            },
            700: {
                items: 3
            }
        }
    });
});