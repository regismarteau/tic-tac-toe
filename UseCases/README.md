# UseCases
As the second part of the domain, this project's responsability is to orchestrate and manipulate the domain through commands.
To do so, it needs to communicate with repositories and finders. It doesn't deal with direct technical implementations thought.
`Ports` are here to expose contracts that has to be defined on the Infrastructure layer. 

Two differents concepts are used here:
- Commands
- Side effect

For example here, the smallest business transaction of this game is a move done by one of the two players. It means that if the user plays, we don't need to let the computer play in the same transaction.
In that case, the computer's move is more like a `reaction` to the user move and can be performed afterwards in a asynchronous process.

## Notes
To handle easily commands and side effect processes, I use [MediatR](https://github.com/jbogard/MediatR) which is a pretty good package.