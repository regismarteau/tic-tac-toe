---
paths:
  - "**/*.cs"
---

# Validation des entrées de l'API

Toute donnée qui vient du client est validée avant d'atteindre la logique métier. Les bornes se justifient par le besoin : ne pas en ajouter par réflexe (pas de longueur maximale systématique sur les chaînes), mais en poser une quand l'absence de borne coûte quelque chose (chargement de toute la base, volume stocké ou traité sans limite).

- **DTO et paramètres** : champs obligatoires déclarés comme tels (propriété `required` ou `[Required]` sur les DTO et paramètres de query ; les paramètres de route sont typés et contraints). Longueurs maximales et bornes numériques seulement quand le métier ou la sécurité l'exige (ex. un champ indexé, une quantité qui pilote un traitement).
- **Pagination** : tout `take` / `pageSize` a une valeur par défaut et un maximum, pour ne jamais charger toute la table ; une valeur hors bornes est refusée en 400, jamais acceptée telle quelle.
- **Pas de `!` sur une donnée client** : la validation du modèle doit garantir la valeur en amont, et le type du DTO doit refléter cette garantie.
- **Pas d'exception technique pour une entrée invalide** : un constructeur de query ou de commande ne lève pas `InvalidOperationException` pour une donnée client incohérente. Valider dans le DTO pour répondre 400.
- **URL fournies par le client** (ex. URL de redirection) : ne les transmettre à un tiers qu'après comparaison à une liste blanche configurée.
- **CORS** : lister explicitement les origines autorisées dans la configuration. Pas d'autorisation de toute origine, surtout combinée à l'envoi de credentials.
- Les identifiants de ressource sont typés (ex. `Guid`, contrainte de route) et la ressource est toujours lue via le contexte utilisateur (voir `security.md`).
