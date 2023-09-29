# Welcome to YGWeb!

This is an ASP.NET Core MVC web application that is designed to handle basic deckbuilding functions for the popular card game Yugioh. This app is designed to be very intuitive, but all of the primary features are described before. To run the web application, simply clone the repository and run using dotnet:
>git clone https://github.com/Tkhan2000/YGWeb 
\
>dotnet -watch


# Features

These are the current available features in the application

## Create User Accounts

This application supports basic user authentication. Register and Login information can be seen in the right side of the top navigation bar. To register, Users need to input a Username, Email, and Password.

## View Full Card List

The full Yugioh card list is stored in a SQL database of over 12,000 records. Users are able to see the full card list in a view that uses pagination to limit scrolling. Users can also filter cards using keyword, card type, etc.

## Deck Building

In order to create a deck, users must first be logged into an account. The Deck Builder allows users to create their own Yugioh decks using any of the cards from the full card list. When users are finished building their decks, they can save their deck. If their deck is valid, the deck will be saved and users can later load that deck in the future.
