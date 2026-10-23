---
name: cpm
description: Commit, merge et push sur main en une commande (/cpm) : commite les changements en cours, merge la branche courante dans la branche principale, pousse, nettoie la branche, puis fait un pull de la principale. À lancer quand on demande de "commit, merge et push sur main".
disable-model-invocation: true
argument-hint: "[message de commit]"
---

# Commit, merge, push sur main

L'invocation de `/cpm` vaut demande explicite de commit, de merge et de push : pas de confirmation intermédiaire, sauf blocage décrit ci-dessous.

## Procédure

1. **État** : `git status --short`, `git branch --show-current`, et déterminer la branche principale (`git symbolic-ref --short refs/remotes/origin/HEAD`, sinon `main`).
2. **Commit** : s'il y a des changements, appliquer `before-commit.md` si la rule est présente et que le commit touche du code (format, `/review-modified-files`, build, tests), puis commiter. Message = `$ARGUMENTS` s'il est fourni, sinon un message court qui décrit le changement. Ajouter uniquement les fichiers pertinents ; ne jamais commiter de secret ni de fichier généré.
3. **Merge** :
   - Déjà sur la branche principale : passer directement au push.
   - Sinon : `git fetch origin`, `git checkout <principale> && git pull --ff-only` (la principale doit être à jour avant le merge), puis `git rev-list --count <branche>..<principale>` pour retenir si la principale a des commits que la branche n'a pas, puis `git merge <branche>` (fast-forward ou commit de merge). Si la branche est utilisée par un worktree, merger depuis le dépôt principal.
4. **Tests** après le merge, **si et seulement si** la principale avait des commits de retard sur la branche (le compte de l'étape 3 est supérieur à 0) : le merge produit alors un état que personne n'a testé (voir `run-tests.md`) ; ne pas pousser un état rouge. Sinon le merge est un fast-forward (ou la branche est déjà la principale) : l'état est celui déjà testé à l'étape 2, ne pas relancer les tests.
5. **Push** : `git push origin <principale>`.
6. **Nettoyage** : si une branche a été mergée, appliquer `git-workflow.md` (branche locale, puis distante).
7. **Mise à jour** : rester sur la branche principale et lancer `git pull --ff-only` pour qu'elle soit à jour avec `origin`. Si la branche d'origine existe encore (worktree, suppression impossible), la mettre aussi à jour avec la principale.
8. Rendre compte : commit créé, branche mergée, résultat du push.

## Garde-fous

- Conflit de merge, tests rouges ou push refusé : s'arrêter, expliquer, ne jamais utiliser `--force`, `--no-verify` ni `reset --hard` pour s'en sortir. Un conflit se résout seulement s'il est trivial ; sinon `git merge --abort` et demander.
- Ne pousser que la branche principale et supprimer que la branche qui vient d'être mergée.
- S'il existe une PR ouverte pour la branche, la merger via `gh pr merge` plutôt que localement, puis suivre `git-workflow.md`.
