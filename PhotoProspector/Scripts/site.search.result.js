'use strict';

var imageContainer;

$(function () {
    imageContainer = $('#waterfall_img_container');

    imageContainer.imagesLoaded(function () {
        imageContainer.masonry({
            itemSelector: '.waterfall_item',
            isAnimated: true,
            isFitWidth: true
        });
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

    $('body').css('display', 'none');
    $('body').fadeIn();
});