---
paths:
  - "**/*.cs"
---

# Code lisible et simple

## Méthodes courtes

- Viser une dizaine de lignes par méthode (repère, pas dogme : ne pas fragmenter un code déjà lisible). Au-delà, extraire une méthode privée au nom clair (verbe + intention métier).
- Une méthode = un niveau d'abstraction. Extraire les boucles, les mappings et les blocs `if/else` qui mélangent des niveaux.
- Exception : un mapping pur (un seul initialiseur d'objet, sans logique) ou une configuration déclarative peut dépasser la limite.

## Peu de paramètres

- Maximum 3 paramètres par méthode, `CancellationToken` non compté. Au-delà, créer un `record` dédié qui porte un concept métier et y déplacer la logique qui s'y rapporte.
- Cette limite ne s'applique pas aux constructeurs de classes et de records (queries, commands, événements, DTO, injection de dépendances) : un record dédié serait redondant avec le type lui-même. Ne pas créer de record intermédiaire pour les alléger.
- Supprimer en revanche les dépendances injectées mais inutilisées.

## Noms

- Le nom d'une classe dit ce qu'elle fait et reste cohérent avec ce qu'elle traite. Suivre la convention de nommage déjà en place dans le projet (ex. `<Requête>QueryHandler`, `<Commande>CommandHandler`, `On<Événement>`).
- Un handler de requête ne porte pas un nom de commande, et son paramètre non plus (`query`, pas `command`).
- Deux méthodes de controller qui font des choses différentes n'ont pas le même nom.
- Pas de `this.` inutile, pas de variable ou de paramètre à un nom sans sens (`x`, `data`, `tmp`) hors lambdas triviales.

## Simplicité

- Préférer les retours anticipés et les expressions simples aux imbrications.
- Une méthode `async` qui ne fait que `await` un seul appel final retourne directement le `Task`, sauf si un `using` ou un `try` doit encadrer l'appel.
- Ne pas laisser de code mort : méthode non appelée, enregistrement DI en double, `using` inutile, code commenté.
- Pas de commentaire par défaut : le code dit la règle et l'intention (noms, méthodes extraites, types).
- Seule exception : l'intégration d'un service externe, pour documenter un comportement que l'on ne contrôle pas (contrainte, bug, contournement).
- Une règle difficile à comprendre n'est pas une raison de commenter : simplifier le code (renommer, extraire, restructurer) jusqu'à ce qu'elle se lise seule.
- Pas de commentaire ni de documentation XML (`///`) sur les contrats entre modules d'un projet (interfaces, records, ports) : le type et les noms suffisent.
- Trier les `using` par ordre alphabétique, sans doublon.

## Style de l'editorconfig

- Lire le `.editorconfig` du dépôt avant d'écrire du code .NET et appliquer **toutes** ses règles dès la première écriture, y compris celles en `suggestion` (corps d'expression, `var`, namespaces fichier, accolades, `using`, pattern matching, collection expressions, constructeurs primaires, etc.). Ne pas compter sur le formateur pour corriger après coup (voir `format-before-commit.md` pour la vérification avant commit).
- Quand l'utilisateur change une règle de l'`.editorconfig`, l'appliquer aux fichiers modifiés de la session et à tout le code écrit ensuite.
