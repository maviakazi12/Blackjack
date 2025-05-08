using BlackjackGame.UI;
using BlackjackGame.Engine;
using BlackjackGame.Services;

var deck = new Deck();
var player = new Player();
var dealer = new Dealer();
var inputOutput = new IO();
var scoring = new ScoringService();

// Initialize GameController
var gameController = new GameController(
    deck,
    player,
    dealer,
    2,            // initial cards to deal
    inputOutput,
    scoring
);

// Start Game
gameController.Run();

