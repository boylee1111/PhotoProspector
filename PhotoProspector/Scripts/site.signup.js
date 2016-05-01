'use strict';

var signUpButon,
    signUpForm,
    photoPathInput,
    avatarUploadEditor,
    avatarPreviewImg,
    signupResultDiv,
    signupSuccessMessage,
    signupFailureMessage;

$(function () {
    signUpButon = $('#signup_buton');
    signUpForm = $('#sign_up_form');
    photoPathInput = $('#photoPath');
    avatarUploadEditor = $('#avatar_upload_editor');
    avatarPreviewImg = $('#avatar_preview_img');
    signupResultDiv = $('#signup_result');
    signupSuccessMessage = $('.success_message');
    signupFailureMessage = $('.failure_message');

    eventBinding();

    $('body').css('display', 'none');
    $('body').fadeIn();

    $("input").attr("maxlength", 25);
});

function eventBinding() {
    photoPathInput.change(function () {
        if (photoPathInput.get(0).files && photoPathInput.get(0).files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                avatarPreviewImg.attr('src', e.target.result);
            }

            reader.readAsDataURL(photoPathInput.get(0).files[0]);
        }
    });

    signUpButon.click(function (e) {
        e.preventDefault();
        if (photoPathInput.get(0).files.length == 0) { // if no file is selected, show input error
            avatarUploadEditor.addClass('input-file-validation-error');
        } else {
            avatarUploadEditor.removeClass('input-file-validation-error');
            signUpForm.submit();
        }
    });

    $('input[type="text"]').keypress(function () {
        $(this).removeClass('input-validation-error');
    });
}