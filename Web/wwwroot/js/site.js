﻿// Write your Javascript code.
$(document).ready(function () {
    $.validator.setDefaults({
        highlight: function (element) {
            $(element).closest('.bmd-form-group').addClass('has-error');
        },
        unhighlight: function (element) {
            $(element).closest('.bmd-form-group').removeClass('has-error');
        },
        errorElement: 'div',
        errorClass: 'help-block',
        errorPlacement: function (error, element) {
            $(element).closest('.bmd-form-group').children('.help-block').remove();
            if (element.parent('.input-group').length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    hljs.configure({ useBR: true });

    var deploy = function (msg) {
        $('.deploy-message').text(msg);
    };

    hub.on('deploy', deploy);

    $.connection.hub.disconnected && $.connection.hub.disconnected(function () {
        if ($.connection.hub.lastError)
        { alert("Disconnected. Reason: " + $.connection.hub.lastError.message); }
    });

    connection.start();
});

var connection = $.hubConnection();
var hub = connection.createHubProxy("AuctusDemo");

$('.form-control').focus(function () { $(this).parent().addClass('is-focused'); });
$('.form-control').blur(function () { $(this).parent().removeClass('is-focused'); });