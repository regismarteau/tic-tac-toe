Feature: Play
	As a player
	I want to mark a cross
	In order to attempt to make a line


Scenario: Everytime the player plays, the computer play also afterwards
	Given a game started
	When I play on top left cell
	Then The game displayed is like
	| X |   |  |
	|   | O |  |
	|   |   |  |