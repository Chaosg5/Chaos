/**
 * @module Notify
 * @copyright Copyright (c) EVRY. All rights reserved.
 * @summary Messenger module for notifications.
 * @requires jQuery
 */

/*global $, JSON, callNotify, document, notification, sessionStorage*/
/*property
    addEventListener, align, animate, delay, enter, exit, from, getItem, icon,
    message, mouse_over, notify, offset, parse, placement, removeItem, setItem,
    spacing, stringify, type, x, y, z_index
*/

/*jslint browser:true, fudge:true, single:true */


/**
 * @summary Checking if there are notifications in storage after the DOM has loaded and showing them if there are
 */
document.addEventListener('DOMContentLoaded', function () {
    "use strict";
    var sessionNotify = JSON.parse(sessionStorage.getItem("notification"));
    if (sessionNotify !== null) {
        notification(sessionNotify[0].type, sessionNotify[0].title, sessionNotify[0].message);
        sessionStorage.removeItem("notification");
    }
});

/**
 * @summary Saving the notification data to session storage
 * @param {string} type - The type of notification to be sent (info/success/warning/danger)
 * @param {string} title - The title for the notification
 * @param {string} message - The message text for the notification
 * @param {int} delay - The delay time in milliseconds
 */
function callNotify(type, title, message, delay) {
    "use strict";
    var notify = [{
        type: type,
        title: typeof title === "undefined" ? "" : title,
        message: typeof message === "undefined" ? "" : message,
        delay: typeof delay === "undefined" ? 0 : delay
    }];
    sessionStorage.setItem("notification", JSON.stringify(notify));
}

/**
 * @summary Main function for calling notifications
 * @param {string} type - The type of notification to be sent (info/success/warning/danger)
 * @param {string} title - The title for the notification
 * @param {string} message - The message-text for the notification
 * @param {int} delay - The delay time in milliseconds
 */
function notification(type, title, message, delay) {
    "use strict";

    if (type) {
        var data = {
            type: type,
            title: typeof title === "undefined" ? "" : title,
            message: typeof message === "undefined" ? "" : message,
            delay: typeof delay === "undefined" ? 0 : delay,
            icon: undefined
        };

        switch (type) {
            case "success":
                data.icon = "fa fa-check-circle";
                break;
            case "info":
                data.icon = "fa fa-info-circle";
                break;
            case "warning":
                data.icon = "fa fa-exclamation-circle";
                break;
            case "danger":
                data.icon = "fa fa-exclamation-triangle";
                break;
            default:
                console.log("%cNotification type has to be defined. %cnotification(%ctype%c, title, message)", "color: red", "color: black", "font-weight: bold", "color: black");
        }

        notify(data);

    } else {
        console.log("%cNotification type has to be defined. %cnotification(%ctype%c, title, message)", "color: red", "color: black", "font-weight: bold", "color: black");
    }
}

/**
 * @summary Calling the $.notify function with passed data
 * @param {Object} data - The data to be sent in the notification. (type, icon, title, message)
 * @param {int} delay - The delay before the notification closes in milliseconds. (0 means never close)
 */
function notify(data) {
    $.notify({
        icon: data.icon,
        title: data.title,
        message: data.message
    },
        {
            type: data.type,
            animate: {
                enter: "animated fadeInDown",
                exit: "animated fadeOutRight"
            },
            placement: {
                from: "top",
                align: "right"
            },
            offset: {
                x: 30,
                y: 80
            },
            spacing: 10,
            z_index: 1031,
            delay: data.delay
        });
}