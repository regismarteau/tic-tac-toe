Feature: Play versus an unbeatable computer
	As a player
	I want to play versus a computer that can't be defeated
	In order to attempt to be better than him

Scenario: The computer will attempt to win if I play it wrong
	Given a game started
	When I play on top left cell
	And I play on bottom right cell
	And I play on top right cell
	Then the game has been won by the computer
	| X | O | X |
	|   | O |   |
	|   | O | X |

Scenario: Even if I try as hard as possible, I can't win
	Given a game started
	When I play on middle cell
	And I play on left cell
	And I play on bottom middle cell
	And I play on top right cell
	And I play on bottom right cell
	Then the game ends in a draw
	| O | O | X |
	| X | X | O |
	| O | X | X |

Scenario: Definitively, it's impossible ...
	Given a game started
	When I play on top left cell
	And I play on bottom right cell
	And I play on bottom middle cell
	And I play on top right cell
	And I play on left cell
	Then the game ends in a draw
	| X | O | X |
	| X | O | O |
	| O | X | X |