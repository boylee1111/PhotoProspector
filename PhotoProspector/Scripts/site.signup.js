'use strict';

var emailInput,
    getCodeButton,
    signUpButton,
    signUpForm,
    photoPathInput,
    avatarUploadEditor,
    avatarPreviewImg,
    signupResultDiv,
    signupSuccessMessage,
    signupFailureMessage;

$(function () {
    emailInput = $('#MSEmail');
    getCodeButton = $('#get_invitation_code_button');
    signUpButton = $('#signup_buton');
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

    $("input").attr("maxlength", 50);
});

function eventBinding() {
    photoPathInput.change(function () {
        // File type must be image
        var val = $(this).val();
        switch (val.substring(val.lastIndexOf('.') + 1).toLowerCase()) {
            case 'jpeg': case 'jpg': case 'png':
                break;
            default:
                alert("Only image with type .jpeg, .jpg. or .png are accepted");
                return;
        }

        // If image, show preview
        if (photoPathInput.get(0).files && photoPathInput.get(0).files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                avatarPreviewImg.attr('src', e.target.result);
            }
            reader.readAsDataURL(photoPathInput.get(0).files[0]);
        }
    });

    signUpButton.click(function (e) {
        e.preventDefault();
        if (photoPathInput.get(0).files.length == 0) { // if no file is selected, show input error
            avatarUploadEditor.addClass('input-file-validation-error');
        } else {
            avatarUploadEditor.removeClass('input-file-validation-error');
            signUpForm.submit();
        }
    });

    getCodeButton.click(function (e) {
        var emailAddress = emailInput.val();
        if (!isMicrosoftEmailAccount(emailAddress)) {
            alert('Only Microsoft email is accecpted.');
            return;
        }

        $.ajax({
            type: 'POST',
            url: webroot + 'SignUp/SendInvitationCode',
            traditional: true,
            data: {
                email: emailAddress
            }
        }).done(function (data) {
            if (data.success === true) {
            } else if (data.success === false) {
            }
        });

        alert("Email Sent. Please check your mailbox for invitation code. If you receive email, try again later.");
        getCodeButtonCountDown(60);
    });

    $('input[type="text"]').keypress(function () {
        $(this).removeClass('input-validation-error');
    });
}

function getCodeButtonCountDown(counter) {
    getCodeButton.text('Retry(' + counter + ')');
    getCodeButton.prop('disabled', true);
    getCodeButton.addClass('btn_disabled');
    var disableTimer = setInterval(function () {
        --counter;
        getCodeButton.text('Retry(' + counter + ')');
        if (counter === 0) {
            getCodeButton.prop('disabled', false);
            getCodeButton.removeClass('btn_disabled');
            getCodeButton.text('Get Code');
            clearInterval(disableTimer);
        }
    }, 1000);
}
