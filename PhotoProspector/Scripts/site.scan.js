'use strict';

var scanContainer,
    previewImg,
    buttonsContainer,
    scanProgress,
    scanButton,
    resetButton;

$(function () {
    scanContainer = $('#scan_container');
    previewImg = $('#preview_img');
    buttonsContainer = $('#buttons_container');
    scanProgress = $('#scan_progress');
    scanButton = $('#scan_button');
    resetButton = $('#upload_button');

    eventBinding();

    $('body').css('display', 'none');
    $('body').fadeIn();
});

function eventBinding() {
    scanButton.click(function (e) {
        e.preventDefault();
        previewImg.loadgo({
            'filter': 'grayscale'
        });

        // set interval used to uplaod progress
        var intervalId = setInterval(function () {
            $.post('/Scan/ScanProgress', function (progress) {
                if (progress >= 100) {
                    updateScanProgress();
                    clearInterval(intervalId);
                }
                else {
                    updateScanProgress(progress);
                }
            });
        }, 100);

        scanProgress.fadeIn();
        buttonsContainer.fadeOut();
        // scan handling start
        $.ajax({
            type: 'POST',
            url: '/Scan/Scan',
            data: {
                filePath: previewImg.attr('src')
            }
        }).done(function (data) {
            clearInterval(intervalId);
            updateScanProgress();

            // replace scan preview with scan result
            setTimeout(function () {
                scanContainer.fadeOut(function () {
                    scanContainer.html(data);
                    scanContainer.fadeIn();
                });
            }, 200);
        }).fail(function (e) {
            alert('Erro: Scan failed');
        });
    });

    resetButton.click(function (e) {
        e.preventDefault();
        document.location.host;
    });
}

function updateScanProgress(progress) {
    progress = typeof progress !== 'undefined' ? progress : '100%';
    previewImg.loadgo('setprogress', parseFloat(progress));
    scanProgress.html(progress);
}