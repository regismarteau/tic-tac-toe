---
name: new-use-case
description: Génère le squelette d'un nouveau cas d'usage en suivant l'architecture déjà en place dans le dépôt (domaine, commande/requête et handler, port, implémentation, point d'entrée, scénario d'acceptation). À utiliser pour ajouter une fonctionnalité, un endpoint ou la réaction à un événement.
argument-hint: "<contexte ou module> <nom du cas d'usage>"
---

# Nouveau cas d'usage

Ne pas imposer une architecture : repérer celle du dépôt et la reproduire. Les étapes ci-dessous supposent une séparation domaine / cas d'usage / ports / infrastructure ; en l'absence de ces couches, garder l'ordre (test, logique, point d'entrée) et sauter ce qui n'existe pas.

## Étapes

1. **Découvrir l'architecture** : lire la structure des projets/dossiers et choisir un cas d'usage existant du même module comme modèle (même type : commande, requête, consumer). Repérer où vivent le domaine, les cas d'usage, les ports, l'infrastructure, les points d'entrée (HTTP, messages) et les tests d'acceptation, ainsi que les conventions de nommage.
2. **Cadrer** : commande (écrit) ou requête (lit) ? Déclenché par HTTP, par un message externe ou par un événement de domaine ? Si ambigu, poser la question.
3. **Scénario d'acceptation d'abord** : ajouter ou compléter une feature (vocabulaire métier, steps existants réutilisés), voir `acceptance-tests.md`.
4. **Domaine** : événements et logique métier dans l'agrégat concerné, selon le modèle existant.
5. **Cas d'usage** : commande ou requête + handler, selon les conventions repérées. Les ports d'infrastructure prennent `CancellationToken cancellationToken` en dernier paramètre (voir `cancellation-token.md` si présente).
6. **Infrastructure** : implémenter les ports et les enregistrer dans la configuration DI du module. Mettre à jour les substituts de test si un port externe change.
7. **Point d'entrée** : endpoint protégé par défaut, DTO validé (voir `api-input-validation.md` si présente), ou consumer / listener qui transmet le token du contexte.
8. **Persistance** : si le schéma change, utiliser le skill `new-migration`.
9. **Vérifier** : build sans avertissement, tests (voir `run-tests.md`), puis `/review-modified-files` avant de commiter.

## Garde-fous

- Respecter `clean-code.md` : méthodes courtes, 3 paramètres maximum, noms conformes aux conventions du dépôt.
- Pas de commentaire : le code dit l'intention (voir `clean-code.md`).
- Ne pas créer d'abstraction au-delà du besoin du cas d'usage.
