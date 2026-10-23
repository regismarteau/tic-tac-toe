---
paths:
  - "**/*.cs"
  - "**/*.csproj"
  - "**/Directory.Packages.props"
  - "**/global.json"
---

# Exécution des tests .NET (xunit.v3 sur Microsoft.Testing.Platform)

Les projets sont sur xunit.v3 avec le runner Microsoft.Testing.Platform (MTP), activé dans `global.json` (`"test": { "runner": "Microsoft.Testing.Platform" }`, SDK 10). Voir `run-tests.md` pour le principe (tests ciblés pendant le développement, suite complète avant commit).

- **Jamais `--filter`** (syntaxe VSTest, non supportée par xunit.v3 en MTP). Utiliser les options xUnit, passées directement à `dotnet test` (pas de `--` intermédiaire) :
  - `--filter-class "Ns.MaClasse"` (plusieurs valeurs possibles : `--filter-class "Ns.A" "Ns.B"`)
  - `--filter-namespace "Ns.Module"`
  - `--filter-method "*.Should_do_x_when_y"`
  - `--filter-trait "Category=Unit"`
  - `--filter-query "/*/Ns/MaClasse/*"` (chemin `/assembly/namespace/classe/méthode`, traits entre crochets)
  - exclusions : `--filter-not-class`, `--filter-not-namespace`, `--filter-not-method`, `--filter-not-trait`.
- Noms pleinement qualifiés, joker `*` accepté ; mettre les valeurs entre guillemets.
- Tests impactés : retrouver les tests par le namespace ou la classe miroir du code modifié (`Module.UseCases.X` → `--filter-namespace "*Module*"`, ou la feature d'acceptation du cas d'usage). En cas de doute sur la couverture, lancer la suite complète.
- Pas de `Microsoft.NET.Test.Sdk`, `xunit.runner.visualstudio` ni `coverlet.collector` (VSTest uniquement) ; la couverture passe par `Microsoft.Testing.Extensions.CodeCoverage` si elle est demandée.
- `--help` sur l'exécutable de tests liste les options disponibles pour la version installée.
- Un filtre qui ne retient aucun test dans un des projets de la solution fait sortir `dotnet test` en code 8 (« zero tests ran ») : ce n'est pas un échec des tests, ajouter `--ignore-exit-code 8` pour les runs ciblés.
