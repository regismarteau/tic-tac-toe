# Queries
Queries represent around 80% of the actions asked by users. Their main objective is to be as efficient as possible. It means that data has to be retrievable quickly without the need to compute or validate it.
This is one of the main advantage of the CQRS pattern, as it allows to go straight to the data by admitting that everything went fine when commands were performed.

For example, the result of a game (depending on marks played) doesn't need to be evaluated inside a query, as it has already been determined by the domain which raised the appropriate event. The information has been persisted so we don't have to evaluate it again when asking for the result.

Queries are also decoupled from Domain and Database model to fit the most to the need that they are refering.

## Notes
- Because we don't need any abstraction, calculation nor isolation, we can directly use the Database and project the data as we want. This is more a `N-tier` architecture approach. 
- On more complex, big data and low latency systems, some dedicated projections might be necessary to be computed on the `Command` side to ease the data retrieval.