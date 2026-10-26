---
paths:
  - "**/*.cs"
---

# Erreurs et journalisation

## Erreurs renvoyées au client (si le projet expose une API)

- Ne jamais renvoyer `Exception.Message`, le nom du type d'exception ni une stack trace dans une réponse HTTP. Une erreur inattendue (500) renvoie une réponse générique (ex. `ProblemDetails`).
- Une entrée client invalide est une erreur 4xx (400, 404, 409...), jamais un 500 : la valider avant de construire une query ou une commande (voir `api-input-validation.md` si présente), ou la traduire dans le gestionnaire d'erreurs central.
- Un jeton d'authentification invalide ou expiré est un 401 : attraper l'exception de la bibliothèque de validation de jeton à cet endroit.
- Les exceptions métier peuvent exposer un message maîtrisé, conçu pour le client ; les autres types, non.

## Journalisation

- Utiliser le logger injecté (`ILogger<T>` en .NET), avec des messages structurés (`"Traitement échoué pour {UserId}"`), jamais de concaténation ni d'interpolation.
- Journaliser à l'endroit où l'erreur est traitée (gestionnaire d'erreurs, consumer, point d'entrée), pas à chaque niveau de la pile. Pas de `catch` qui avale une exception : soit on la journalise et on la relance ou on la traduit, soit on ne l'attrape pas.
- Niveaux : `Error` pour un échec inattendu, `Warning` pour une situation dégradée récupérable, `Information` pour les événements métier importants.
- Ne jamais journaliser de token, de mot de passe ni de donnée personnelle ou de contenu utilisateur, ni la requête brute. Identifier par identifiants techniques.
- Pas de sortie console (`Console.WriteLine`) hors outil en ligne de commande ; et un outil en ligne de commande qui échoue sort avec un code non nul.
