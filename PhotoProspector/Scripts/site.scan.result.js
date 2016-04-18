﻿'use strict';

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
    initScanResultWindowResize();

    kendoToolTip();
});

function eventBinding() {
    resetButton.click(function (e) {
        e.preventDefault();
        window.location.href = document.location.origin;
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