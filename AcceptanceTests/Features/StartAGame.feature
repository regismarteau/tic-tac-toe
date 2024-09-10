Feature: Start a game
	As a new player
	I want to start a game
	In order to play against a computer


Scenario: Start a game should initialize a new tic tac toe
	When I start a new game
	Then The game displayed is like
	|  |  |  |
	|  |  |  |
	|  |  |  |