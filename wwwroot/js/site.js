// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
};

addToDeck = (url, cardId) => {
    $.ajax({
        type: 'POST',
        url: url,
        data: { id: cardId },
        success: function (res) {
        }
    })
};

saveDeck = (url) => {
    $.ajax({
        type: 'POST',
        url: url,
        ddata: {
            name: $("#deckName").val()
        },
        success: function (res) {
            if (res == "Failure") {
                alert("Deck could not be saved. Verify length and number of cards");
            }
            else if (res == "Deck Exists"){
                alert("Deck already exists. Changes were not saved");
            }
            else {
                alert("Deck Saved Successfully");
            }
        }
    })
};