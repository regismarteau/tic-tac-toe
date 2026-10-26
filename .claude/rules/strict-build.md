---
paths:
  - "**/*.csproj"
  - "**/Directory.*.props"
  - ".editorconfig"
---

# Build strict

La configuration de compilation est centralisée et stricte, dans un `Directory.Build.props` à la racine de la solution.

- `Nullable`, `ImplicitUsings`, `TargetFramework` et les options communes sont définis une seule fois dans `Directory.Build.props`, pas répétés dans chaque `.csproj`.
- `TreatWarningsAsErrors` est activé : un avertissement casse le build. Corriger la cause ; ne pas supprimer l'avertissement avec `#pragma` ni `NoWarn` sans raison écrite dans la PR.
- Analyseurs activés : `AnalysisLevel` à `latest`, `EnforceCodeStyleInBuild` à `true`, et `IDE0005` (using inutile) au niveau `warning` dans le `.editorconfig`, avec `GenerateDocumentationFile` pour que le compilateur le détecte ; si la documentation XML n'est pas exigée, ajouter `CS1591` à `NoWarn` dans `Directory.Build.props`.
- Les versions de packages restent centralisées dans `Directory.Packages.props` ; un `.csproj` ne porte jamais de `Version=` sur un `PackageReference`.
- Après un changement de configuration de build, vérifier `dotnet build <solution> --no-incremental` sans avertissement, puis lancer les tests (voir `run-tests.md`).
