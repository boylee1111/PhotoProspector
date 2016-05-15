'use strict';

var emailInput,
    forgetCoedButton;

$(function () {
    emailInput = $('#MSEmail');
    forgetCoedButton = $('#forget_invitation_code_button');

    eventBinding();

    $('body').css('display', 'none');
    $('body').fadeIn();

    $("input").attr("maxlength", 50);
});

function eventBinding() {
    forgetCoedButton.click(function (e) {
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
        forgetCodeButtonCountDown(60);
    });
}

function forgetCodeButtonCountDown(counter) {
    forgetCoedButton.text('Retry(' + counter + ')');
    forgetCoedButton.prop('disabled', true);
    forgetCoedButton.addClass('btn_disabled');
    var disableTimer = setInterval(function () {
        --counter;
        forgetCoedButton.text('Retry(' + counter + ')');
        if (counter === 0) {
            forgetCoedButton.prop('disabled', false);
            forgetCoedButton.removeClass('btn_disabled');
            forgetCoedButton.text('Forget Code');
            clearInterval(disableTimer);
        }
    }, 1000);
}
