---
paths:
  - "**/*Tests.cs"
  - "**/*UnitTests/**/*.cs"
---

# Tests unitaires

Un test unitaire lit comme un scénario : les services sont déjà câblés, le test ne fait qu'injecter des données, appeler le code testé et vérifier.

## Fakes plutôt que classes statiques ou mocks de comportement

- Pour une dépendance externe (API tierce, horloge, contexte utilisateur, fabrique de client), écrire un **fake instanciable** qui garde son propre état et simule le comportement réel (ex. un faux Gmail qui répond en HTTP selon les messages qu'on lui a donnés). Pas de classe statique partagée entre tests, pas de `Substitute.For` configuré ligne à ligne dans chaque test.
- Le comportement se règle par des méthodes à l'intention claire (`ExpireHistory()`, `AddNewMail(...)`, `AddMessage(...)`), pas par des lambdas passées au constructeur dans chaque test.
- Le fake enregistre ce qu'il reçoit (requêtes, appels) quand un test doit le vérifier.
- Fakes dans un dossier `Fakes/` à côté des tests, en `internal sealed`. Réutiliser un fake existant avant d'en créer un.
- Préférer un fake à un mock pour tout ce qui a un état ou un protocole. `NSubstitute` reste acceptable pour une interface triviale à une méthode, jamais pour reconstituer un protocole.
- Base de données : un `DbContext` en mémoire réel, pas un faux repository, quand le code testé interroge le contexte.
- **Ne pas modifier le code de production pour les tests** : pas de `virtual`, de classe non scellée ou de `internal` ouvert uniquement pour pouvoir hériter ou surcharger dans un fake. Quand une dépendance concrète bloque (ex. une fabrique de client tiers), extraire une interface, faire dépendre les consommateurs de l'interface, l'enregistrer en DI, et écrire le fake comme une implémentation de cette interface.

## Mini conteneur de dépendances de test

- Une **classe de base** (`<Sujet>TestBase : IAsyncLifetime`) construit dans `InitializeAsync` un `ServiceCollection` qui enregistre les fakes et les vrais services testés, puis le libère dans `DisposeAsync`.
- Le conteneur câble les constructeurs : un test n'instancie jamais à la main le service testé ni ses dépendances.
- Le test récupère un service précis avec `Get<T>()`, exposé en propriété privée de la classe de test (`private GoogleMailProvider Provider => Get<GoogleMailProvider>();`).
- Une instance neuve par test (xUnit crée une classe par test) : aucun état partagé, pas de dépendance à l'ordre d'exécution.
- La base ne contient que le câblage commun. Le comportement propre à un scénario se règle dans le test, en début d'« arrange », pour qu'il soit visible (ex. `Gmail.ExpireHistory();` dans le test, pas dans l'initialisation).

## Données de test

- Les objets du domaine ou de l'API tierce se construisent avec un **builder fluide** (`AMessage("id").WithHistoryId(100).ReceivedAt(date).Unread().Build()`), avec des valeurs par défaut neutres. Le test ne précise que ce qui compte pour le scénario.
- Builders dans un dossier `Builders/`. Les heures viennent d'un fake d'horloge, jamais de l'horloge système.

## Structure d'un test

- Nom `Should_<résultat>_when_<condition>`, qui dit la règle testée.
- Arrange (données injectées dans les fakes et la base), Act (un seul appel du code testé), Assert (FluentAssertions). Pas de logique conditionnelle dans le test.
- Un test par comportement. Pour un bug corrigé, vérifier que le test échoue sans la correction (désactiver temporairement la correction).
- Voir `acceptance-tests.md` ou `blazor-acceptance-tests.md` (si présentes) pour ce qui relève d'un scénario d'acceptation et `run-tests.md` pour l'exécution.
