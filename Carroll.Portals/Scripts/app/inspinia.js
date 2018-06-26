/*
 *
 *   INSPINIA - Responsive Admin Theme
 *   version 2.7.1
 *
 */


$(document).ready(function () {

    //$('#test').on('click', test);

    // Add body-small class if window less than 768px
    if ($(this).width() < 769) {
        $('body').addClass('body-small')
    } else {
        $('body').removeClass('body-small')
    }

    // MetsiMenu
    $('#side-menu').metisMenu();

    // Collapse ibox function
    $('.collapse-link').click(function () {
        var ibox = $(this).closest('div.ibox');
        var button = $(this).find('i');
        var content = ibox.children('.ibox-content');
        content.slideToggle(200);
        button.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
        ibox.toggleClass('').toggleClass('border-bottom');
        setTimeout(function () {
            ibox.resize();
            ibox.find('[id^=map-]').resize();
        }, 50);
    });

    // Close ibox function
    $('.close-link').click(function () {
        var content = $(this).closest('div.ibox');
        content.remove();
    });

    // Fullscreen ibox function
    $('.fullscreen-link').click(function () {
        var ibox = $(this).closest('div.ibox');
        var button = $(this).find('i');
        $('body').toggleClass('fullscreen-ibox-mode');
        button.toggleClass('fa-expand').toggleClass('fa-compress');
        ibox.toggleClass('fullscreen');
        setTimeout(function () {
            $(window).trigger('resize');
        }, 100);
    });

    // Close menu in canvas mode
    $('.close-canvas-menu').click(function () {
        $("body").toggleClass("mini-navbar");
        SmoothlyMenu();
    });

    // Run menu of canvas
    $('body.canvas-menu .sidebar-collapse').slimScroll({
        height: '100%',
        railOpacity: 0.9
    });

    // Open close right sidebar
    $('.right-sidebar-toggle').click(function () {
        $('#right-sidebar').toggleClass('sidebar-open');
    });

    // Initialize slimscroll for right sidebar
    $('.sidebar-container').slimScroll({
        height: '100%',
        railOpacity: 0.4,
        wheelStep: 10
    });

    // Open close small chat
    $('.open-small-chat').click(function () {
        $(this).children().toggleClass('fa-comments').toggleClass('fa-remove');
        $('.small-chat-box').toggleClass('active');
    });

    // Initialize slimscroll for small chat
    $('.small-chat-box .content').slimScroll({
        height: '234px',
        railOpacity: 0.4
    });

    // Small todo handler
    $('.check-link').click(function () {
        var button = $(this).find('i');
        var label = $(this).next('span');
        button.toggleClass('fa-check-square').toggleClass('fa-square-o');
        label.toggleClass('todo-completed');
        return false;
    });

    // Minimalize menu
    $('.navbar-minimalize').on('click', function (event) {
        event.preventDefault();
        $("body").toggleClass("mini-navbar");
        SmoothlyMenu();

    });

    // Tooltips demo
    $('.tooltip-demo').tooltip({
        selector: "[data-toggle=tooltip]",
        container: "body"
    });

    // Move modal to body
    // Fix Bootstrap backdrop issu with animation.css
    $('.modal').appendTo("body");

    // Full height of sidebar
    function fix_height() {
        var heightWithoutNavbar = $("body > #wrapper").height() - 61;
        $(".sidebard-panel").css("min-height", heightWithoutNavbar + "px");

        var navbarHeight = $('nav.navbar-default').height();
        var wrapperHeight = $('#page-wrapper').height();

        if (navbarHeight > wrapperHeight) {
            $('#page-wrapper').css("min-height", navbarHeight + "px");
        }

        if (navbarHeight < wrapperHeight) {
            $('#page-wrapper').css("min-height", $(window).height() + "px");
        }

        if ($('body').hasClass('fixed-nav')) {
            if (navbarHeight > wrapperHeight) {
                $('#page-wrapper').css("min-height", navbarHeight + "px");
            } else {
                $('#page-wrapper').css("min-height", $(window).height() - 60 + "px");
            }
        }

    }

    fix_height();

    // Fixed Sidebar
    $(window).bind("load", function () {
        if ($("body").hasClass('fixed-sidebar')) {
            $('.sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: 0.9
            });
        }
    });

    // Move right sidebar top after scroll
    $(window).scroll(function () {
        if ($(window).scrollTop() > 0 && !$('body').hasClass('fixed-nav')) {
            $('#right-sidebar').addClass('sidebar-top');
        } else {
            $('#right-sidebar').removeClass('sidebar-top');
        }
    });

    $(window).bind("load resize scroll", function () {
        if (!$("body").hasClass('body-small')) {
            fix_height();
        }
    });

    $("[data-toggle=popover]")
        .popover();

    // Add slimscroll to element
    $('.full-height-scroll').slimscroll({
        height: '100%'
    })
});


// Minimalize menu when screen is less than 768px
$(window).bind("resize", function () {
    if ($(this).width() < 769) {
        $('body').addClass('body-small')
    } else {
        $('body').removeClass('body-small')
    }
});

// Local Storage functions
// Set proper body class and plugins based on user configuration
$(document).ready(function () {
    if (localStorageSupport) {

        var collapse = localStorage.getItem("collapse_menu");
        var fixedsidebar = localStorage.getItem("fixedsidebar");
        var fixednavbar = localStorage.getItem("fixednavbar");
        var boxedlayout = localStorage.getItem("boxedlayout");
        var fixedfooter = localStorage.getItem("fixedfooter");

        var body = $('body');

        if (fixedsidebar == 'on') {
            body.addClass('fixed-sidebar');
            $('.sidebar-collapse').slimScroll({
                height: '100%',
                railOpacity: 0.9
            });
        }

        if (collapse == 'on') {
            if (body.hasClass('fixed-sidebar')) {
                if (!body.hasClass('body-small')) {
                    body.addClass('mini-navbar');
                }
            } else {
                if (!body.hasClass('body-small')) {
                    body.addClass('mini-navbar');
                }

            }
        }

        if (fixednavbar == 'on') {
            $(".navbar-static-top").removeClass('navbar-static-top').addClass('navbar-fixed-top');
            body.addClass('fixed-nav');
        }

        if (boxedlayout == 'on') {
            body.addClass('boxed-layout');
        }

        if (fixedfooter == 'on') {
            $(".footer").addClass('fixed');
        }
    }
});

// check if browser support HTML5 local storage
function localStorageSupport() {
    return (('localStorage' in window) && window['localStorage'] !== null)
}

// For demo purpose - animation css script
function animationHover(element, animation) {
    element = $(element);
    element.hover(
        function () {
            element.addClass('animated ' + animation);
        },
        function () {
            //wait for animation to finish before removing classes
            window.setTimeout(function () {
                element.removeClass('animated ' + animation);
            }, 2000);
        });
}

function SmoothlyMenu() {
    if (!$('body').hasClass('mini-navbar') || $('body').hasClass('body-small')) {
        // Hide menu in order to smoothly turn on when maximize menu
        $('#side-menu').hide();
        // For smoothly turn on menu
        setTimeout(
            function () {
                $('#side-menu').fadeIn(400);
            }, 200);
    } else if ($('body').hasClass('fixed-sidebar')) {
        $('#side-menu').hide();
        setTimeout(
            function () {
                $('#side-menu').fadeIn(400);
            }, 100);
    } else {
        // Remove all inline style from jquery fadeIn function to reset menu state
        $('#side-menu').removeAttr('style');
    }
}

// Dragable panels
function WinMove() {
    var element = "[class*=col]";
    var handle = ".ibox-title";
    var connect = "[class*=col]";
    $(element).sortable(
        {
            handle: handle,
            connectWith: connect,
            tolerance: 'pointer',
            forcePlaceholderSize: true,
            opacity: 0.8
        })
        .disableSelection();
}


//function test() {
//    $.ajax({
//        type: "GET",
//        url: "http://localhost:54620/api/user/Getusers",
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        async: false,
//        beforeSend: function (xhr) {
//            xhr.setRequestHeader('Authorization', 'Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6InowMzl6ZHNGdWl6cEJmQlZLMVRuMjVRSFlPMCIsImtpZCI6InowMzl6ZHNGdWl6cEJmQlZLMVRuMjVRSFlPMCJ9.eyJhdWQiOiJodHRwczovL2dyYXBoLndpbmRvd3MubmV0IiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvMWQ1OTNmODctYjY4Ni00NzA0LTkyNmEtZGIwNGMxNGI4ZDE4LyIsImlhdCI6MTQ5Mzc4MDE1MCwibmJmIjoxNDkzNzgwMTUwLCJleHAiOjE0OTM3ODQwNTAsImFjciI6IjEiLCJhaW8iOiJBU1FBMi84REFBQUFJOHp3U3ljTXNaMFh6THNJOUVRam1ZeDJhTldvZllJNElNWlF2YjFMOTZzPSIsImFtciI6WyJwd2QiXSwiYXBwaWQiOiI0NDhmZWU2OC1jMGRkLTRjNDctYTdjNC1lYjk0MGZkODY3NTkiLCJhcHBpZGFjciI6IjEiLCJlX2V4cCI6MjYyODAwLCJmYW1pbHlfbmFtZSI6Ik5hbmR1cmkiLCJnaXZlbl9uYW1lIjoiUGF2YW4iLCJpcGFkZHIiOiIzOC4xMDQuMC4yNiIsIm5hbWUiOiJQYXZhbiBOYW5kdXJpIiwib2lkIjoiMjExNGM4MjEtYzFjYi00MWZmLWJhNDctMmY5NmU0NWVhOWI2IiwicGxhdGYiOiIzIiwicHVpZCI6IjEwMDMzRkZGOEIwRUZCMDMiLCJzY3AiOiJEaXJlY3RvcnkuUmVhZC5BbGwgVXNlci5SZWFkIiwic3ViIjoiME50LVRsUjhvaVlRY2d1ZXRSSVhJY3A0bXVlUWZaRVZtTkRFaGc5VkpUVSIsInRpZCI6IjFkNTkzZjg3LWI2ODYtNDcwNC05MjZhLWRiMDRjMTRiOGQxOCIsInVuaXF1ZV9uYW1lIjoiUGF2YW4uTmFuZHVyaUBjYXJyb2xsb3JnLmNvbSIsInVwbiI6IlBhdmFuLk5hbmR1cmlAY2Fycm9sbG9yZy5jb20iLCJ1dGkiOiJTV3V1ODZudlJFV1pBTGtyNWJ3REFBIiwidmVyIjoiMS4wIn0.LvCYm3-DzaUzBh-6c0aCsirpU8kvqF3ZRuxyl-o0bTUZCpRSaXO-pfe_dTmmnB7aXKcOqsmU-Ef-nXh0z9qsPHgsRxr4tOH05BE7CZlANALADaLYxp_XUbJhFCF8BMs_Fwkl2GV6k4prnFuSnb-7XFMAPNsfyno2ftKIZY0HCOUQdpNK2aEe4WWU5drDpQVJjVNEnhlcndEerEQwKqKNpPXwJ3uZ3cPlsmPyuPDqrXJyClhH7_E90K9rHruGHeyKBQxnQM6j9cjWYW2TqjXVcI8U3xCvA0Fm_PurQJujIMxCuWh1MFuzJmDj0AsVApVqkm6_Q1PO33KUy_9HC8YJNg');
//        },
//        success: function (result) {
//            var data = result;//$.parseJSON(result);


//        },
//        error: function (msg) {
//            alert('Error getting info..');
//        }
//    });
//}