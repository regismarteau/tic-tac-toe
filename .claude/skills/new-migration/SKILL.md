---
name: new-migration
description: Crée une migration de schéma de base de données avec l'outil déjà utilisé par le dépôt (FluentMigrator, EF Core, etc.), alignée sur les entités et avec les index nécessaires. À utiliser dès qu'une entité, une colonne, une table ou un index change.
argument-hint: "<description du changement de schéma>"
---

# Nouvelle migration

## Étapes

1. **Découvrir** l'outil de migration et ses conventions : dossier des migrations, schéma de nommage (souvent un horodatage), type de classe de base, migrations « forward-only » ou avec `Down`. Lire les migrations existantes et l'entité concernée (et le contexte de données) pour reprendre les types de colonnes, les noms de tables et la colonne de propriétaire si les tables sont filtrées par utilisateur.
2. **Nommer** la migration selon la convention repérée, avec un horodatage UTC strictement postérieur à la dernière migration. Un seul changement de schéma par migration.
3. **Écrire** la migration en reprenant la forme des existantes (même classe de base, même namespace, même style de création de tables, colonnes et index).
4. **Index** (voir `ef-core-persistence.md`) : index sur les clés étrangères, la colonne de propriétaire (si le projet en a une) et les colonnes de filtre, de tri ou de jointure des tables qui vont grossir ; index composite `(propriétaire, colonne)` quand les deux sont filtrés ; unicité quand le métier l'impose.
5. **Synchroniser** le modèle d'accès aux données : entité, `DbSet`, configuration du contexte (dont le filtre global par utilisateur si le projet en utilise).
6. **Ne jamais** modifier une migration déjà commitée sur la branche principale : en créer une nouvelle.
7. **Vérifier** : build sans avertissement, tests (voir `run-tests.md`). Si les tests n'exercent pas les migrations (base en mémoire), relire la migration avec soin ; si une base locale est disponible, appliquer la migration et vérifier le code de sortie de l'outil.

## Garde-fous

- Pas de suppression de colonne ou de table sans l'accord explicite de l'utilisateur : donnée perdue.
- Migration sur une grosse table : le signaler (verrou, durée).
