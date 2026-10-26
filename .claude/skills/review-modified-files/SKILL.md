---
name: review-modified-files
description: Revue boy scout des fichiers modifiés avant un commit ou une PR : sécurité, suppression du code mort et des commentaires superflus, code court et lisible, selon les rules du projet. À lancer avant de commiter ou d'ouvrir/mettre à jour une PR, ou quand on demande de "repasser sur les fichiers modifiés".
argument-hint: "[branche de base, par défaut la branche principale]"
---

# Revue des fichiers modifiés (boy scout)

Appliquer les rules présentes dans `.claude/rules/` (par exemple `security.md`, `clean-code.md`, `cancellation-token.md`) à tous les fichiers modifiés ou ajoutés. Le skill ne commite pas : le commit relève de `before-commit.md`.

## Procédure

1. Lister les fichiers concernés : `git diff $(git merge-base <base> HEAD) --name-only` (base = `$ARGUMENTS`, sinon la branche principale du dépôt ; le merge-base évite de lister ce qui a changé sur la base depuis le départ de la branche), plus les fichiers non suivis (`git status --short`). Ignorer les fichiers supprimés et générés (`obj`, `bin`, `*.feature.cs`, migrations générées).
2. Lire chaque fichier en entier, pas seulement les lignes touchées : le laisser plus propre qu'à l'arrivée.
3. Corriger, fichier par fichier :
   - **Sécurité** : appliquer `security.md`.
   - **Commentaires** : supprimer le code commenté, la documentation XML (`///`) et tous les commentaires, sauf ceux qui documentent un comportement non contrôlable d'un service externe ; un commentaire qui semble nécessaire pour expliquer une règle est un signal pour simplifier le code.
   - **Code inutile** : `using`, méthodes, attributs, propriétés, paramètres de constructeur, classes, fichiers, références de projet et enregistrements DI non utilisés, ainsi que le code dupliqué.
   - **CancellationToken** (.NET) : appliquer `cancellation-token.md` si présente.
   - **Lisibilité** : appliquer `clean-code.md` si présente (méthodes courtes, peu de paramètres, noms cohérents).
4. En .NET, détecter les `using` inutiles avec le compilateur plutôt qu'avec une heuristique. Si `IDE0005` est déjà en `warning` (voir `strict-build.md`), le build suffit. Sinon, ajouter temporairement `dotnet_diagnostic.IDE0005.severity = warning` à la section `[*.cs]` de l'`.editorconfig` du dépôt (le créer avec `[*.cs]` s'il n'existe pas), lancer `dotnet build <solution> --no-incremental -p:GenerateDocumentationFile=true -p:EnforceCodeStyleInBuild=true -p:TreatWarningsAsErrors=false`, relever les `IDE0005`, rétablir l'`.editorconfig` tel qu'il était (retirer la ligne ajoutée, ou supprimer le fichier créé ; vérifier avec `git diff`), puis retirer les `using` signalés dans les fichiers modifiés (garder le BOM s'il existe). Les fichiers non modifiés se signalent seulement.
5. **En dernier**, une fois tout corrigé : `powershell -NoProfile -ExecutionPolicy Bypass -File .claude/skills/review-modified-files/review-gate.ps1 -Mark` (timeout de la commande à 10 minutes). Le script lance `dotnet format` sur les fichiers modifiés puis `dotnet test` sur la solution, sauf si un `dotnet test` complet (sans `--filter`) vient de réussir sur l'état exact des fichiers (un hook PostToolUse l'enregistre) : les tests ne sont alors pas relancés. Il n'enregistre la revue que si tout passe. En cas d'échec, corriger et relancer. Le hook PreToolUse refuse `git commit` sans cette empreinte, et toute modification ultérieure l'invalide (relancer la revue).
6. Rendre compte : ce qui a été corrigé, ce qui a volontairement été laissé (avec la raison), le résultat des tests.

## Garde-fous

- Le boy scout est un refactoring : ne pas changer le comportement, les tests doivent rester verts.
- Ne pas casser les contrats externes lors d'un renommage : noms de files de messages ou de consumers, routes HTTP, noms de tables ou de colonnes, messages publiés, contrats d'API. Ne pas les renommer sans demande explicite.
- Ne pas élargir le périmètre aux fichiers non modifiés ; signaler simplement les problèmes repérés ailleurs.
