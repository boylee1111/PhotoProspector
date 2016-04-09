'use strict';

var personTable,
    personTableDataRow;

$(function () {
    personTable = $('#person_info_table');
    personTableDataRow = $('#person_info_table tbody tr td');

    kendoToolTip();
});

function kendoToolTip() {
    personTableDataRow.bind('mouseenter', function () {
        var $this = $(this);

        if (this.offsetWidth < this.scrollWidth && !$this.attr('title')) {
            $this.attr('title', $this.text());
        }
    });
}