# AcceptanceTests
Probably the most important test stack overall.
Taking distance with the implementation, its main responsability is to verify that the application is working as attended by the business whatever the architecture and the implementations.
This is for me the best way to have confidence in the quality of the application, and it allows major refactoring and evolution at a minimum risk.

They also fit really well with `Behaviour Driven Development` (or `Acceptance Test Driven Development`) and offer the opportunity to validate the interaction between services, layers, sub-systems, etc.
For example here, they represent a good way to test `queries` by dispatching `commands` first without the need to populate data by hand in a mock (which can ends to unexpected behaviour if the data hasn't been verified)?

Well designed, it can provide a living documentation about all of your application's features. This is why I personally use [Reqnroll](https://github.com/reqnroll/Reqnroll) to have the most understable scenarios possible. In a optimistic way, it can lead also to a collaboration with the business to define them and make sure that we are all aligned with the expectations.

## Notes
- They are slower than Unit tests so it has to be taken into account when defining tests. They doesn't fit really well with exhaustivity.
- There are quite a lot of necessary configuration and tooling to make it usable and sustainable, and it depends a lot on what the framework can provide.
- With ORM like Entity Framework, it offers good opportunities to test quickly your application without the need to mock all of you repositories, by switching to a InMemory provider.
	- Also, it can easily be reuse for integration tests without switching the database provider.