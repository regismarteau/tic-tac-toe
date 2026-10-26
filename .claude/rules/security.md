# Sécurité

Vérifier à chaque revue des fichiers modifiés :

- **Authentification et autorisation** : tout point d'entrée exposé (controller, action, hub) est protégé par défaut (ex. `[Authorize]`) ; l'accès anonyme reste exceptionnel et justifié (inscription, statut). L'identité vient des claims ou du contexte utilisateur, jamais d'une donnée du client. Tant qu'un projet n'a pas d'authentification (squelette de template, prototype), ne pas poser `[Authorize]` ni signaler son absence ; dès qu'elle est introduite, elle devient une tâche à part entière et la règle s'applique à tous les points d'entrée.
- **Isolation des données** (si le projet est multi-utilisateur) : les accès base passent par le contexte utilisateur (filtres par propriétaire). Ne pas contourner les filtres globaux (ex. `IgnoreQueryFilters`, requêtes SQL brutes) sans justification liée au besoin.
- **Validation des entrées** : voir `api-input-validation.md` si présente (API).
- **Injection** : jamais de SQL construit par concaténation, jamais d'URL, de chemin ou de commande construits par interpolation avec une donnée utilisateur sans encodage (`Uri.EscapeDataString`, bibliothèque d'URL) ni liste blanche.
- **Secrets** : aucun mot de passe, token, clé API ou chaîne de connexion réels dans le code, les tests ou les fichiers de configuration versionnés (les valeurs par défaut d'un environnement de développement local jetable, comme `postgres/postgres` ou `guest/guest` d'un `docker-compose`, sont tolérées) ; passer par la configuration, les user-secrets ou les variables d'environnement. Vérifier aussi les fichiers locaux (ex. `launchSettings.json`) et l'historique git.
- **Fuite d'information** : voir `errors-and-logs.md` si présente (.NET).
- **Désérialisation** : pas de désérialisation polymorphe non restreinte (ex. `TypeNameHandling`, `pickle`) sur des données externes.
- **Dépendances** : ne pas ajouter de package sans nécessité ; signaler tout package avec une vulnérabilité connue (en .NET : `dotnet list package --vulnerable`, en Node : `npm audit`).
- **Ressources** : libérer les ressources (`using`, `with`) et borner les traitements longs (annulation, délai d'expiration) pour éviter un déni de service ; en .NET voir `cancellation-token.md` si présente.

Si une faille est trouvée, la corriger quand la correction est locale et sans effet de bord ; sinon la signaler clairement dans la réponse avec le fichier, la ligne et l'impact.
