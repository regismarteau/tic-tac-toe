---
paths:
  - ".github/workflows/**"
  - "**/*.sln"
  - "global.json"
---

# Intégration continue (GitHub Actions)

Le dépôt a un workflow `.github/workflows/ci.yml`, déclenché sur les pull requests et sur la branche principale. S'il n'existe pas, le créer. Il rejoue ce que les rules exigent en local :

1. restauration des dépendances
2. build sans avertissement (en .NET : `dotnet build <solution> --no-restore`, `TreatWarningsAsErrors` de `strict-build.md` fait échouer le build)
3. tests (voir `run-tests.md`)
4. scan de vulnérabilités des dépendances. En .NET, `dotnet list <solution> package --vulnerable --include-transitive` sort avec le code 0 même s'il trouve quelque chose : capturer la sortie et faire échouer le job si elle contient `has the following vulnerable packages`.

Règles de maintenance :

- Toute nouvelle vérification exigée localement par une rule est ajoutée au workflow dans la même PR.
- Un projet de test ajouté est exécuté par la CI (la commande de test sur la solution suffit).
- Les secrets passent par les secrets du dépôt GitHub, jamais en clair dans le workflow. Permissions du workflow minimales (`permissions: contents: read`).
- Actions épinglées sur une version majeure explicite (`actions/checkout@v4`), SDK lu depuis `global.json` (`global-json-file`). Un `.github/dependabot.yml` (nuget et github-actions) propose les mises à jour de dépendances.
- Ne pas fusionner une PR dont la CI est rouge.
