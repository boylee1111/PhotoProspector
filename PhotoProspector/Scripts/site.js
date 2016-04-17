'use strict';

var maximumBrowserHeight,
    navBar,
    footer,
    maximumContentHeight;

$(function () {
    maximumBrowserHeight = window.screen.availHeight - (window.outerHeight - window.innerHeight);
    navBar = $('#global_nav_bar');
    footer = $('#global_footer');
    maximumContentHeight = maximumBrowserHeight - navBar.height() - footer.height();
});