'use strict';

var jcrop_api,
    boundx,
    boundy,
    xsize,
    ysize,
    crop_frame,
    form_input_file,
    upload_form,
    upload_box,
    uploadingContainer,
    uploadingSection,
    logoSection,
    logoSectionCover,
    coverDescription;

var maxSizeAllowed = 2;     // Upload limit in MB
var maxSizeInBytes = maxSizeAllowed * 1024 * 1024;
var keepUploadBox = false;  // Remove if you want to keep the upload box

$(function () {
    if (typeof $('#photo-upload-form') !== undefined) {
        form_input_file = $('#photo-upload-form input:file');
        upload_form = $('#photo-upload-form');
        upload_box = $('#photo-upload-box');
        uploadingContainer = $('#uploading-container');
        uploadingSection = $('#uploading-section');
        logoSection = $('#logo-section');
        logoSectionCover = $('#logo-section-cover');
        coverDescription = $('#cover-description');

        $('#photo-upload-max-size').html(maxSizeAllowed);
        $('#photo-upload-form input:file').on('change', function (e) {
            var files = e.currentTarget.files;
            for (var x in files) {
                if (files[x].name != 'item' && typeof files[x].name != 'undefined') {
                    if (files[x].size <= maxSizeInBytes) {
                        // Submit the selected file
                        $('#photo-upload-form .upload-file-notice').removeClass('bg-danger');

                        homeToUploadAnimation();

                        // set delay for uploading, we can see the animation
                        setTimeout(function () {
                            $('#photo-upload-form').submit();
                        }, 2000);
                    } else {
                        // File too large
                        $('#photo-upload-form .upload-file-notice').addClass('bg-danger');
                    }
                }
            }
        });
        $('#photo-upload-form input:file').on('click', function (e) {
            $(this).blur();
        });
    }

    $('body').css('display', 'none');
    initPhotoUpload();
    $('body').fadeIn();
});

function initPhotoUpload() {
    upload_form.ajaxForm({
        beforeSend: function () {
            updateProgress(0);
            upload_form.addClass('hidden');
        },
        uploadProgress: function (event, position, total, percentComplete) {
            console.log(percentComplete);
            updateProgress(percentComplete);
        },
        success: function (data) {
            updateProgress(100);
            if (data.success === false) {
                upload_form.removeClass('hidden');
                $('.upload-progress').addClass('hidden');
                alert('Error: ' + data.errorMessage + ' We will take you back.');
                window.location.href = '/Upload';

            } else {
                $('#preview-pane .preview-container img').attr('src', data.fileName);
                var img = $('#crop-photo-target');
                img.attr('src', data.fileName);

                if (!keepUploadBox) {
                    $('#photo-upload-box').addClass('hidden');
                }
                savePhoto();
            }
        },
        complete: function (xhr) {
        }
    });
}

function updateProgress(percentComplete) {
    $('.upload-percent-bar').width(percentComplete + '%');
    $('.upload-percent-value').html(percentComplete + '%');
    if (percentComplete === 0) {
        $('#upload-status').empty();
        $('.upload-progress').removeClass('hidden');
    }
}

function savePhoto() {
    var img = $('#preview-pane .preview-container img');

    $.ajax({
        type: 'POST',
        url: '/Upload/Save',
        traditional: true,
        data: {
            fileName: img.attr('src')
        }
    }).done(function (data) {
        if (data.success === true) {
            $.redirect("/Scan", { fileName: data.uploadFileLocation });
        } else {
            alert('Error: ' + data.errorMessage + ' We will take you back.');
            window.location.href('/Upload');
        }
    }).fail(function (e) {
        alert('Error: Cannot upload photo at this time. We will take you back.');
        window.location.href('/Upload');
    });
}

function homeToUploadAnimation() {
    $('.inputfile-circle').addClass('hidden');

    $('#logo-section').fadeOut(function () {
        $('#uploading-section').fadeIn();
        $('#upload-circular').fadeIn();
        $('#upload-control').fadeIn();
    });
}