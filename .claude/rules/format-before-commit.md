---
paths:
  - "**/*.cs"
  - "**/*.csproj"
  - ".editorconfig"
---

# Conformité à l'editorconfig avant commit / PR

Appliqué par `/review-modified-files` (voir `before-commit.md`), qui lance `dotnet format` sur les fichiers modifiés. Avant chaque commit, création de PR ou push :

- Vérifier que le code produit respecte le `.editorconfig` du dépôt (indentation, `var`, namespaces, accolades, nommage, fins de ligne, etc.).
- Lancer le formateur du projet. En .NET : `dotnet format <solution> --severity info`. Les règles de style en `suggestion` ne sont souvent pas corrigées par `dotnet format` : pour celles-là, passer temporairement leurs diagnostics en `warning` dans l'`.editorconfig` (`dotnet_diagnostic.IDExxxx.severity = warning`), lancer `dotnet format style <solution> --diagnostics IDExxxx --severity warn --include <fichiers modifiés>`, puis retirer ces lignes temporaires.
- Les modifications du formatage sont incluses dans le commit.
- Ne pas pousser tant que le formatage n'a pas été appliqué.
