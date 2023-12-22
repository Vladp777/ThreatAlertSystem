// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function DisplayGeneralNotification(message, title) {
    setTimeout(function () {
        toastr.options = {
            closeButton: true,
            progressBar: true,
            showMethod: 'slideDown',
        };

        // Add a reload button to the notification
        var notificationWithReload = toastr.info(message, title, {
            timeOut: 0, // Set timeOut to 0 to make the notification sticky
            extendedTimeOut: 0,
            tapToDismiss: false,
            closeButton: false,
            progressBar: false,
            positionClass: 'toast-bottom-right', // Adjust the position as needed
            onclick: function () {
                // Reload the page when the notification is clicked
                location.reload(true);
            },
        });

        // Add the reload button manually
        var reloadButton = $('<button>', {
            text: '🔄',
            class: 'btn btn-sm btn-primary',
            click: function () {
                // Reload the page when the button is clicked
                location.reload(true);
            },
        });

        $(notificationWithReload).append(reloadButton);
    }, 1300);
}