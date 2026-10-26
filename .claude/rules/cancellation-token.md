---
paths:
  - "**/*.cs"
---

# Propagation du CancellationToken

Tout code asynchrone fait circuler un `CancellationToken` jusqu'à l'appel qui fait réellement de l'I/O.

- Point d'entrée : action de controller (`CancellationToken cancellationToken` en dernier paramètre), consumer ou handler de messages (le token du contexte de message), handler de commande/requête, middleware, tâche de fond.
- Le transmettre à chaque appel qui l'accepte : dispatch de commandes et requêtes, publication de messages, méthodes des ports, appels de base de données (`ToListAsync`, `SingleAsync`, `AnyAsync`, `FindAsync`, `AddAsync`, `SaveChangesAsync`), appels HTTP, API de clients tiers (`ExecuteAsync(cancellationToken)`), itérations asynchrones.
- Les ports (interfaces d'infrastructure) qui font de l'I/O prennent `CancellationToken cancellationToken` en dernier paramètre. Mettre à jour ensemble le port, ses implémentations et les substituts de test.
- Ne pas ajouter de token à une méthode qui ne peut pas l'exploiter (pas d'I/O en aval, API tierce sans support) : un paramètre inutilisé est du code inutile.
- Nom du paramètre : `cancellationToken`. Ne pas le rendre optionnel ni nullable dans nos propres signatures.
- `CancellationToken.None` uniquement quand l'opération doit aller au bout même si l'appelant est annulé (ex. enregistrer l'état d'échec d'un traitement). Le choix doit se lire dans le code, sans commentaire.
