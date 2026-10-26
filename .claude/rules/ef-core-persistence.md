---
paths:
  - "**/*.cs"
  - "**/Migrations/**"
---

# Persistance (EF Core et migrations)

## Requêtes

- `AsNoTracking()` sur une query de lecture qui renvoie des entités mappées. Inutile quand la requête se termine par un `.Select(...)` vers un DTO ou un type anonyme : EF ne suit pas ces résultats.
- Projeter vers ce dont on a besoin (`Select`) plutôt que charger l'entité complète quand on ne lit que quelques colonnes.
- Ne pas contourner les filtres globaux par utilisateur (voir `security.md`) ; un `IgnoreQueryFilters` se justifie par le besoin.
- Tout appel EF asynchrone reçoit le `CancellationToken` (voir `cancellation-token.md` si présente).

## Index et schéma

- Indexer les clés étrangères et les colonnes de filtre, de tri ou de jointure des tables qui vont grossir (y compris la colonne de propriétaire des filtres globaux) ; inutile sur une table minuscule, un index coûte à l'écriture. Préférer un index composite `(propriétaire, colonne)` quand la requête filtre sur les deux.
- Un changement de schéma = une migration (voir le skill `new-migration`). Ne jamais modifier une migration déjà appliquée.
- Le modèle EF (entités, contexte) et les migrations restent synchronisés : une entité ou une propriété ajoutée sans migration est un défaut.

## Outil de migration

- L'outil qui applique les migrations sort avec un code non nul quand une migration échoue : ne pas avaler l'exception dans un `catch` qui ne fait qu'afficher.
