# UnitTests
Unit tests are directly plugged to the domain and assert all of the invariants defined by the business.
- They are really fast and so they can be really exhaustive.
- They have to be focused on business logic only, and should not try to test technical detail if not necessary.

## Notes
The main downside of this, is that they are tightly coupled with the domain implementation (even if the domain is as pure as possible), which can leads to difficulty to refactor these concepts when needing. 
So they have to be defined with caution and personally I would rather suggest to write it in an [Acceptance](https://github.com/regismarteau/tic-tac-toe/tree/main/AcceptanceTests) stack if possible.
It remains really useful to ensure consistency over a large amount of possible combinations. For example here, a unit test fits well to ensure that the computer will never loose whatever the user plays by testing all of the possible moves.