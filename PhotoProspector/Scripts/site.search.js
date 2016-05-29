'use strict';

var searchContainer,
    aliasInput,
    ImageUploadBox,
    filesLabel,
    uploadImagesInput,
    uploadingLabel,
    searchSubmitButton;

$(function () {
    searchContainer = $('#search_container');
    aliasInput = $('#alias_input');
    ImageUploadBox = $('#img_upload_box');
    filesLabel = $('#files_label');
    uploadImagesInput = $('#upload_images_input');
    uploadingLabel = $('#box__uploading');
    searchSubmitButton = $('#search_submit_button');

    searchEventBinding();

    $('body').css('display', 'none');
    $('body').fadeIn();
});

function searchEventBinding() {
    uploadImagesInput.change(function (e) {
        ImageUploadBox.css('outline', '2px dashed #92b0b3');
        var selectedFiles = e.target.files;
        for (var i = 0; i < selectedFiles.length; ++i) {
            if (!isImageFile(selectedFiles[i].name)) {
                alert("Only image with type .jpeg, .jpg. or .png are accepted.");
                uploadImagesInput.val('');
                filesLabel.text("Choose files");
                return;
            }
        }

        showFiles(e.target.files);
    });

    searchSubmitButton.click(function () {
        var alias = aliasInput.val();
        var willSubmit = true;
        if (!alias.trim()) {
            aliasInput.addClass('input-validation-error');
            willSubmit = false;
        }
        if (uploadImagesInput.get(0).files.length == 0) {
            ImageUploadBox.css('outline', '2px dashed #b03535');
            willSubmit = false;
        }

        if (!willSubmit) return;

        filesLabel.css('display', 'none');
        uploadingLabel.css('display', 'block');

        uploadAndSearch();
    });

    $('input[type="text"]').keypress(function () {
        $(this).removeClass('input-validation-error');
    });
}

function uploadAndSearch() {
    var alias = aliasInput.val();

    if (window.FormData !== undefined) {
        var formData = new FormData();
        for (var i = 0; i < uploadImagesInput.get(0).files.length; ++i) {
            formData.append(uploadImagesInput.get(0).files[i].name, uploadImagesInput.get(0).files[i]);
        }

        $.ajax({
            type: 'POST',
            url: webroot + 'Search/UploadBatchSearchByAlias?alias=' + alias,
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                searchContainer.fadeOut(function () { // result transition animation
                    searchContainer.html(data);
                    searchContainer.fadeIn();
                });
            },
            error: function (xhr, textStatus, errorThrown) {
                alert(xhr.responseText);
                alert('Server is busy now, please try again later.”');
            }
        });
    } else {
        alert("This browser doesn't support HTML5 file uploads!");
    }
}

function showFiles(files) {
    filesLabel.text(files.length > 1 ? (uploadImagesInput.attr('data-multiple-caption') || '').replace('{count}', files.length) : files[0].name);
}

function isImageFile(filename) {
    switch (filename.substring(filename.lastIndexOf('.') + 1).toLowerCase()) {
        case 'jpeg': case 'jpg': case 'png':
            return true;
        default:
            return false;
    }
}