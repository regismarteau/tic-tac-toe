# Avant de commiter ou d'ouvrir une PR

Quand le commit ou la PR touche du code (pas pour de la documentation, des rules ou des skills), lancer `/review-modified-files` : revue clean code des fichiers modifiés (corrections incluses), `dotnet format`, build et tests (`run-tests.md`).

Les tests ne sont pas relancés si la suite complète (sans filtre) vient de réussir sur l'état exact des fichiers ; toute modification depuis, `dotnet format` compris, impose de les relancer. Le script de revue fait cette vérification lui-même.

Ce n'est pas facultatif : un hook Claude Code refuse `git commit` tant que la revue n'a pas été enregistrée sur l'état courant des fichiers, dans le checkout principal comme dans un worktree. Commiter automatiquement une fois la revue passée ; le push reste soumis à une demande explicite.

Hors Claude, le hook git `pre-commit` ne fait que `dotnet format` sur les `.cs` indexés.
