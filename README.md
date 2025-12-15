# 🧩 Jeu des Mots Glissés

## 📌 Présentation générale

**Jeu des Mots Glissés** est un jeu de réflexion en **C# (console)** pour deux joueurs.  
Le but est de former des mots valides à partir d’un plateau de lettres, dans un temps limité.

À chaque mot trouvé :
- les lettres utilisées disparaissent,
- les lettres situées au-dessus **glissent vers le bas**,
- le joueur marque des points selon la valeur des lettres.

La partie se termine lorsque :
- le temps total est écoulé,
- ou que le plateau est vide.

---

## 🎯 Objectif du jeu

- Trouver un maximum de mots valides
- Optimiser son score
- Battre l’autre joueur avant la fin du temps imparti

---

## 📜 Règles du jeu

- Le jeu se joue à **2 joueurs**
- Les joueurs jouent **à tour de rôle**
- Un mot est valide si :
  - il contient **au moins 2 lettres**
  - il est présent dans le **dictionnaire**
  - il peut être formé sur le plateau
  - chaque lettre est utilisée **une seule fois**
- Les lettres peuvent être enchaînées :
  - horizontalement
  - verticalement
  - en combinant les directions
- Les lettres supprimées sont remplacées par des cases vides
- Les lettres au-dessus tombent vers le bas (effet de gravité)

---

## 🧮 Système de score

- Chaque lettre a un **poids** (défini dans `Lettres.txt`)
- Le score d’un mot est :
  

---

## ⏱️ Gestion du temps

- La partie possède une durée totale
- Chaque joueur dispose d’un temps limité par tour
- Le temps restant est affiché en direct pendant le jeu

---

## 🗂️ Architecture du projet

### 📁 Structure des fichiers

- JeuDesMotsGlisses/
    - Program.cs → Menu principal et lancement du jeu
    - Jeu.cs → Gestion de la partie (tours, scores, temps)
    - Plateau.cs → Gestion du plateau et recherche des mots
    -  Joueur.cs → Données et actions d’un joueur
    - Dictionnaire.cs → Chargement et recherche des mots
    - Position.cs → Représente une position (ligne, colonne)

    - Lettres.txt → Lettres disponibles + poids
    - MotsFrancais.txt → Dictionnaire de mots français

    - README.md → Documentation du projet


---

## 🧠 Description des classes principales

### 🔹 `Program.cs`
- Affiche le titre et les règles
- Gère le menu principal
- Lance une partie :
  - depuis un fichier CSV
  - ou avec un plateau généré aléatoirement

---

### 🔹 `Jeu.cs`
- Gère le déroulement de la partie
- Alterne les tours des joueurs
- Vérifie les règles
- Calcule les scores
- Affiche le gagnant en fin de partie

---

### 🔹 `Plateau.cs`
- Représente la grille de lettres
- Génère un plateau aléatoire
- Charge un plateau depuis un CSV
- Recherche des mots sur le plateau
- Met à jour la grille après suppression des lettres

---

### 🔹 `Dictionnaire.cs`
- Charge les mots depuis `MotsFrancais.txt`
- Classe les mots par première lettre
- Effectue une recherche dichotomique rapide

---

### 🔹 `Joueur.cs`
- Stocke :
  - le nom du joueur
  - son score
  - les mots déjà trouvés
- Empêche la réutilisation d’un mot

---

### 🔹 `Position.cs`
- Classe simple représentant :
  - une ligne
  - une colonne
- Utilisée pour mémoriser le chemin d’un mot

---

## 📄 Fichiers de données

### 📘 `MotsFrancais.txt`
- Contient les mots autorisés
- Classés par longueur

### 🔠 `Lettres.txt`
- Définit :
  - le nombre maximal de chaque lettre
  - le poids de chaque lettre

---

## ▶️ Lancer le jeu

### Depuis un terminal :

```bash
dotnet run



# 📦 Commandes utiles

## 👉 Cloner le dépôt distant (GitHub)
<pre>
git clone https://github.com/TicOps/JeuDesMotsGlisses.git
cd JeuDesMotsGlisses </pre>

## 👉 Générer le projet (VsCode)
## -> pour pouvoir créer le projet C#
<pre>dotnet new console -n JeuDesMotsGlisses </pre>

## -> lancer le programme.cs
<pre>bash dotnet run </pre>

<pre>git add .  git commit -m"message"  git push </pre>


# Fonctions à implémenter : 
- Fonction pour trier le fichier mots.txt