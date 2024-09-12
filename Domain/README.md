# Domain
This project contains all the business logic.
There is no technical implementation here, only pure concepts defined by the business' experts. It means that this project doesn't reference any other one to ensure its isolation.

A `Game` act as an (_very small_) aggregate and will be in charge of taking decision about the `Events` to raise.
To take these decisions, it will be helped by `Value objects` such as `TicTacToe`. These ones are concepts defined by their values and are always consistant which means that they can't be invalid by design.

Services helps also the business in the decision-making process. For example, the responsability of `UnbeatablePlayFinder` is here to help the computer to never loose by finding the correct cell to play.

## Pros
This kind of design and isolation helps a lot to guarantee consistency over time and reducing accidental complexity.
Also it ease the testability of all business logic without the need to mock / stub technical concept that doesn't apply here. 
See [UnitTests](https://github.com/regismarteau/tic-tac-toe/tree/main/UnitTests) for related tests.

## Trade-offs
You might notice the use of [MediatR.Contracts](https://www.nuget.org/packages/MediatR.Contracts) and specially the `INotification` interface that will help to handle the propagation of events through the system. This is completely arguable and in a more complex system I would suggest to decouple inbox and outbox events.
See [UseCases](https://github.com/regismarteau/tic-tac-toe/tree/main/UseCases) for more detail.