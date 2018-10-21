function SetPersonRating(event, sender, rightToLeft, movieId, personId, sessionId) {
    var rating = GetRatingFromPosition(event, sender, rightToLeft, 200);

    alert("Rating: " + rating);
}

function SetCharacterRating(event, sender, rightToLeft, movieId, characterId, sessionId) {
    var rating = GetRatingFromPosition(event, sender, rightToLeft, 200);
    alert("Rating: " + rating);
}

function GetRatingFromPosition(event, sender, rightToLeft, max) {
    if (!event || !sender) {
        return -1;
    }

    var x = event.clientX;
    var offsetX = $(sender).offset().left;
    var position = x - offsetX;
    if (rightToLeft) {
        position = max - position;
    }

    var rating = Math.floor(((position / max) * 10)) + 1;
    if (position / max < 0.05) {
        rating = 0;
    }

    return rating;
}

function showError(error) {
    document.getElementById("errorContainer").style.display = "block";
    document.getElementById("errorMessage").innerHTML = error;
}

function hideError() {
    document.getElementById("errorContainer").style.display = "none";
    document.getElementById("errorMessage").innerHTML = "";
}