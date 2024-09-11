Feature: Play
	As a player
	I want to mark a cross
	In order to attempt to make a line

	
Scenario: Everytime I play, the computer plays also afterwards
	Given a game started
	When I play on top left cell
	Then the game displayed is like
	| X |   |  |
	|   | O |  |
	|   |   |  |

Scenario: The computer will attempt to win if I play it wrong
	Given a game started
	When I play on top left cell
	And I play on bottom right cell
	And I play on top right cell
	Then the game displayed is like
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
	Then the game displayed is like
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
	Then the game displayed is like
	| X | O | X |
	| X | O | O |
	| O | X | X |