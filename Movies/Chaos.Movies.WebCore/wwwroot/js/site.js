﻿// Write your Javascript code.
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
    return rating;
}