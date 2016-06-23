'use strict';

var personTable,
    personTableDataRow,
    resetOptionContainer,
    scanResultImg,
    resetButton;

$(function () {
    personTable = $('#person_info_table');
    personTableDataRow = $('#person_info_table tbody tr td');
    resetOptionContainer = $('#uploaded_options');
    scanResultImg = $('#scan_result_img');
    resetButton = $('#upload_button');

    console.log(resetOptionContainer.height());
    eventBinding();
    if (!isMobile()) {
        //initScanResultWindowResize();
    }

    //kendoToolTip();
});

function eventBinding() {
    resetButton.click(function (e) {
        e.preventDefault();
        window.location.href = webroot;
    });

    $('body').on('click', '.modal-link', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });
    // Attach listener to .modal-close-btn's so that when the button is pressed the modal dialog disappears
    $('body').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });
    //clear modal cache, so that new content can be loaded
    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
    $('#CancelModal').on('click', function () {
        return false;
    });
}

function initScanResultWindowResize() {
    var resizedImgHeight = maximumContentHeight - resetOptionContainer.height() - 60; // 60 is the img padding-top(40) plus padding-bottom(20px)
    scanResultImg.css('max-height', resizedImgHeight);
}

function kendoToolTip() {
    personTableDataRow.bind('mouseenter', function () {
        var $this = $(this);

        if (this.offsetWidth < this.scrollWidth && !$this.attr('title')) {
            $this.attr('title', $this.text());
        }
    });
}