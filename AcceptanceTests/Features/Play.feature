Feature: Play
	As a player
	I want to mark a cross
	In order to attempt to make a line


Scenario: The player has to make the first move
	Given a game started
	When I play on top left cell
	Then The game displayed is like
	| X |  |  |
	|   |  |  |
	|   |  |  |