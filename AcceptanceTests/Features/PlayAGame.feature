Feature: Play a game
	As a player
	I want to play to Tic Tac Toe
	'cause this is the best game ever made

Scenario: Start a game should initialize a new tic tac toe
	When I start a new game
	Then the game looks like
	|  |  |  |
	|  |  |  |
	|  |  |  |
	
Scenario: Everytime I play, the computer plays also afterwards
	Given a game started
	When I play on top left cell
	Then the game looks like
	| X |   |  |
	|   | O |  |
	|   |   |  |

@ErrorHandling
Scenario: Play the same cell twice is not possible
	Given a game started
	When I play on top left cell
	And I play on top left cell
	Then an This cell is already marked error occured

@ErrorHandling
Scenario: Playing to a game that doesn't exist is not possible
	When I start a new game
	But I attempt to play an unknown game
	Then an Game not found error occured