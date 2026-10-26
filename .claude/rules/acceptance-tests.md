---
paths:
  - "**/*.cs"
  - "**/*.feature"
---

# Tests d'acceptation

- Si le projet a des tests d'acceptation, toute nouvelle fonctionnalité visible de l'extérieur (endpoint, consumer, réaction à un événement) a au moins un scénario dedans, qui passe par l'API et des substituts pour les dépendances externes (fournisseurs tiers, bus de messages, notifications).
- Écrire le scénario avec le vocabulaire métier (`Given` / `When` / `Then`), en réutilisant les steps et les tables de données existants avant d'en créer de nouveaux.
- Les scénarios d'acceptation portent sur les règles fonctionnelles : ce que le système doit faire pour l'utilisateur et les refus qui font partie du métier (ex. une règle de gestion qui interdit une action).
- Ne pas écrire de scénario pour les cas d'erreur qui ne doivent pas arriver (paranoid testing) : entrée client malformée ou hors bornes (paramètre trop grand, jeton ou identifiant invalide), exception technique, panne d'une dépendance, requête qu'un client légitime n'envoie jamais. Ces garde-fous se vérifient en revue (voir `api-input-validation.md`, `errors-and-logs.md` si présentes) ou, si la logique est isolée, par un test unitaire.
- Un bug corrigé reçoit un scénario s'il porte sur une règle fonctionnelle, sinon un test unitaire, qui échoue sans la correction.
- La logique pure et isolée (parsing, validation, algorithmes) se teste en unitaire, en suivant `unit-tests.md` (fakes instanciables, conteneur de test, builders).
- Pas de test dédié « sans jeton → 401 » ni d'isolation multi-utilisateurs sauf demande explicite : ces garanties reposent sur l'autorisation par défaut et les filtres globaux, vérifiés en revue (voir `security.md`).
- Les tests ne dépendent ni du réseau, ni de l'horloge système, ni d'un ordre d'exécution : utiliser l'abstraction d'horloge et les substituts du projet.
- Voir `run-tests.md` pour l'exécution après modification.
