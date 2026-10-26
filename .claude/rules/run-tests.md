# Exécution des tests

Quand une unité de travail touche du code ou de la configuration (source, fichiers de projet, dépendances), lancer les tests du projet avant de la déclarer terminée et avant tout commit, pas après chaque édition intermédiaire. Pas de tests pour les rules, skills ou la documentation.

- Utiliser la commande du projet (README, CI, fichiers de build : `dotnet test`, `npm test`, `pytest`...). S'il n'y en a pas, le dire.
- Pendant le développement, lancer en priorité les tests concernés par la modification (filtre par classe, namespace ou trait selon l'outil du projet) ; la suite complète reste obligatoire avant de déclarer terminé et avant tout commit.
- Avant un commit, ne pas relancer la suite complète si elle vient de réussir sur l'état exact des fichiers (`/review-modified-files` le détecte).
- Un test qui échoue est signalé avec sa sortie et corrigé avant de continuer.
- Indiquer le nombre de tests réussis / échoués dans la réponse.
