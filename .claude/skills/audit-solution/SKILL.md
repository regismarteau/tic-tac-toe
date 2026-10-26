---
name: audit-solution
description: Audit complet d'une solution .NET par rapport aux rules du dépôt et aux bonnes pratiques (build, tests, format, vulnérabilités, secrets, CancellationToken, taille des méthodes, sécurité, persistance, CI). À lancer sur demande d'audit, de revue globale ou d'état des lieux de la solution.
disable-model-invocation: true
argument-hint: "[périmètre : chemin ou projet, par défaut toute la solution]"
---

# Audit de la solution

Lecture seule : ne rien modifier. Produire un rapport, puis demander pour chaque constat s'il faut corriger, créer une rule ou un skill.

## Procédure

1. **Découvrir** : repérer le fichier de solution, les projets, la branche principale et les rules en vigueur (`.claude/rules/*.md`).
2. **Outillé** (en parallèle) :
   - `dotnet build <solution> --no-incremental` : avertissements et erreurs.
   - `dotnet test <solution>` : nombre de tests réussis / échoués.
   - `dotnet format <solution> --severity info --verify-no-changes` : écarts de style.
   - `dotnet list <solution> package --vulnerable --include-transitive` et `--deprecated`.
3. **Recherches ciblées** (fichiers `.cs` versionnés, hors `bin`, `obj`, code généré, migrations) :
   - Sécurité : points d'entrée sans protection d'autorisation, accès anonymes, contournements de filtres globaux, SQL brut, désérialisation polymorphe, secrets (`password`, `secret`, `apikey`, `Bearer`) dans le code, la config versionnée, les fichiers locaux et l'historique (`git log -S`), détail d'exception renvoyé au client, CORS, `!` sur des entrées client.
   - `CancellationToken` : méthodes `Task` sans token, appels de base, HTTP, consumers et itérations asynchrones qui ne le propagent pas, `CancellationToken.None`.
   - Clean code : méthodes de plus de 10 lignes, plus de 3 paramètres hors constructeurs, `return await` inutile, commentaires, code mort, `using` inutiles (IDE0005).
   - Persistance : index manquants, `AsNoTracking` sur lectures d'entités, entités sans migration, `catch` qui avale une erreur de migration.
   - Exploitation : CI (`.github/`), logger, health checks, README, `Directory.Build.props`.
4. **Rapport** par catégorie, avec fichier:ligne, sévérité (🔴 critique, 🟠 important, 🟡 mineur) et impact. Séparer « écarts aux rules existantes » et « bonnes pratiques non couvertes par une rule ».
5. **Questions** : pour chaque bonne pratique non couverte, demander à l'utilisateur (`AskUserQuestion`) s'il faut créer une rule ou un skill. Ne rien créer sans réponse.

## Garde-fous

- Ne pas afficher la valeur d'un secret trouvé : donner le fichier et le nom de la clé.
- Distinguer ce qui est versionné de ce qui est seulement local (vérifier avec `git ls-files` et `git check-ignore`).
- Mesurer plutôt que deviner : un constat s'appuie sur une sortie d'outil ou une ligne de code citée.
