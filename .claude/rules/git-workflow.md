# Branches et pull requests

**Après un merge** dans la branche principale :

- Repasser dessus et la mettre à jour (`git checkout <principale> && git pull`), puis supprimer la branche mergée, localement (`git branch -d`) et à distance (`git push origin --delete`, inutile si GitHub l'a déjà supprimée : `git fetch --prune`).
- Après un squash ou un rebase-merge, git ne voit pas la branche comme mergée : vérifier `gh pr view <branche> --json state`, puis `-D`.
- Ne supprimer que la branche qui vient d'être mergée. Si un worktree l'utilise, le retirer d'abord.

**Description de PR** : elle décrit l'état réel de la branche, pas la chronologie des commits.

- Après chaque push sur une PR ouverte, comparer la description (`gh pr view <n> --json body`) aux changements (comportements, choix techniques, tests, points de déploiement) et la corriger si elle est fausse ou incomplète (`gh pr edit <n> --body ...`), sans attendre de demande. Garder la ligne d'attribution de fin et signaler la mise à jour dans la réponse.
