# Un bug corrigé reçoit son test

Corriger un bug, c'est écrire d'abord le test qui le reproduit, puis la correction. Un correctif vérifié à la main (navigateur, console) ne suffit pas, et le test n'est pas une option à proposer : il fait partie de la correction.

1. **Reproduire par un test**, au niveau le plus bas qui voit le bug (unitaire, intégration, acceptation), et le voir échouer pour la bonne raison sans la correction (désactiver temporairement le code de production).
2. **Corriger**, puis relancer tous les tests (`run-tests.md`).
3. **Annoncer** dans la réponse le test ajouté et la preuve qu'il échouait avant.

Si les tests existants ne peuvent pas voir le bug, étendre l'outillage de test (ex. vérifier la disparition d'un élément, pas seulement le changement d'état). Si c'est réellement impossible (rendu visuel, comportement lié à l'environnement), le dire et indiquer comment le vérifier.

**Après une migration ou un changement de bibliothèque**, les tests peuvent rester verts alors que l'application est cassée : exercer pour de vrai les parcours qui enchaînent plusieurs états, et tester la **fin** de tout élément à cycle de vie propre (fermeture, désabonnement, libération), pas seulement son début.
