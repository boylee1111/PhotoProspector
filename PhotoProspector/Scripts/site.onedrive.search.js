'use strict';

var oneDriveSearchContainer,
    aliasInput,
    oneDriveSearchSubmitButton;

$(function () {
    oneDriveSearchContainer = $('#onedrive_auth_container');
    aliasInput = $('#alias_input');
    oneDriveSearchSubmitButton = $('#onedrive_search_submit_button');

    oneDriveSearchEventBinding();

    $('body').css('display', 'none');
    $('body').fadeIn();
});

function oneDriveSearchEventBinding() {
    oneDriveSearchSubmitButton.click(function () {
        var alias = aliasInput.val();
        if (!alias.trim()) {
            aliasInput.addClass('input-validation-error');
            return;
        }

        $.ajax({
            type: 'POST',
            url: webroot + 'OneDriveSearch/OneDriveSearchByAlias?alias=' + alias,
            contentType: false,
            processData: false,
            success: function (data) {
                oneDriveSearchContainer.fadeOut(function () { // result transition animation
                    oneDriveSearchContainer.html(data);
                    oneDriveSearchContainer.fadeIn();
                });
            },
            error: function (xhr, textStatus, errorThrown) {
                alert(xhr.responseText);
                alert('Server is busy now, please try again later.”');
            }
        });
    });

    $('input[type="text"]').keypress(function () {
        $(this).removeClass('input-validation-error');
    });
}
