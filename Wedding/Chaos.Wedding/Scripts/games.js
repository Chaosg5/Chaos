function gamesCallAjax(url, data) {
    $.ajax({
        url: url,
        data: data,
        type: "POST",
        success: function(result) {
            if (result && result.action === "redirect" && result.url) {
                window.location = result.url;
            } else if (result && result.action === "notification" && result.title) {
                if (result.delay > 0) {
                    window.notification(result.level, result.title, result.message, result.delay);
                }
            } else {
                window.notification("success", "The action was successful, but not response message was received.", "", 30000);
            }
        },
        error: function(xhr, ajaxOptions, thrownError) {
            window.notification("danger", thrownError, xhr.responseText, 60000);
        }
    });
}