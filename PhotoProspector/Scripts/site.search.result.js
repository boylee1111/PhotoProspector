'use strict';

var imageContainer,
    imgLoadControl;

$(function () {
    imageContainer = $('#waterfall_img_container');
    imgLoadControl = $('#img_load');

    imageContainer.imagesLoaded(function () {
        setTimeout(function () {
            imgLoadControl.css('display', 'none');
            imageContainer.fadeIn();
            imageContainer.masonry({
                itemSelector: '.waterfall_item',
                isAnimated: true,
                isFitWidth: true
            });
        }, 1500);
    });

    $('.fancybox').fancybox({
        padding: 0,
        openEffect: 'elastic',
        closeBtn: false,
        helpers: {
            title: {
                type: 'inside'
            },
            overlay: {
                css: {
                    'background': 'rgba(0, 0, 0, 0.5)'
                }
            },
        }
    });
});