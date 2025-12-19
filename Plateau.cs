using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Représente le plateau de jeu.
/// Cette classe gère la grille de lettres, la recherche de mots,
/// la mise à jour du plateau après un mot trouvé
/// ainsi que la sauvegarde et le chargement depuis un fichier.
/// </summary>
public class Plateau
{
    private char[,] grille;
    private int lignes;
    private int colonnes;

    private static Random random = new Random();

    /// <summary>
    /// Construit un plateau à partir d'un fichier CSV.
    /// </summary>
    /// <param name="fichierCSV">Nom du fichier CSV contenant le plateau</param>
    public Plateau(string fichierCSV)
    {
        ToRead(fichierCSV);
    }

    /// <summary>
    /// Construit un plateau aléatoire à partir du fichier des lettres.
    /// </summary>
    /// <param name="fichierLettres">Fichier contenant les lettres et leurs contraintes</param>
    /// <param name="lignes">Nombre de lignes du plateau</param>
    /// <param name="colonnes">Nombre de colonnes du plateau</param>
    public Plateau(string fichierLettres, int lignes, int colonnes)
    {
        this.lignes = lignes;
        this.colonnes = colonnes;
        grille = new char[lignes, colonnes];

        GenererAleatoire(fichierLettres);
    }

    /// <summary>
    /// Génère un plateau aléatoire en respectant
    /// les contraintes du fichier des lettres.
    /// </summary>
    /// <param name="fichierLettres">Fichier contenant les lettres autorisées</param>
    private void GenererAleatoire(string fichierLettres)  /// appel uniquement dans plateau 
    {
        var lignesCSV = File.ReadAllLines(fichierLettres); /// lit toutes les lignes dans le fichier et renvoie un tableau de string

        Dictionary<char, int> maxLettres = new Dictionary<char, int>(); /// création d'un dico (clé + valeur pour l'instant vide)

        foreach (var ligne in lignesCSV) /// on parcout chaque ligne du texte 
        {
            var t = ligne.Split(',');  /// découpe la ligne en colonnes 
            char lettre = char.ToUpper(t[0][0]);
            int max = int.Parse(t[1]); /// on prend la deuxième valeur du fichier lettre.txt
            maxLettres[lettre] = max; /// on remplit le dico 
        }

        int totalMax = 0;
        foreach (var val in maxLettres.Values)
            totalMax += val;

        if (totalMax < lignes * colonnes)
            throw new Exception("Impossible de remplir le plateau : contraintes insuffisantes.");

        List<char> pool = new List<char>(); /// une liste qui va contenir toutes les lettres de mon lettre.texte ( par exemple 8 fois A...)
        foreach (var kvp in maxLettres) /// on parcout chaque couple de valeur du dico
        {
            for (int i = 0; i < kvp.Value; i++)  /// si dans dico la valeur de A c'est 3 on va faire une boucle de longeur 3
                pool.Add(kvp.Key); /// on rajoute dans la liste l'élément
        }

        for (int i = pool.Count - 1; i > 0; i--) /// on parcourt la liste à l'envers
        {
            int j = random.Next(i + 1); /// un nombre random dans la liste
            (pool[i], pool[j]) = (pool[j], pool[i]); /// on intervit les valeurs de liste en i et en j
        }

        int index = 0;
        for (int i = 0; i < lignes; i++)
        {
            for (int j = 0; j < colonnes; j++)
            {
                grille[i, j] = pool[index++]; /// on remplit la grille avec avec chaque élément de la liste
            }
        }
    }

    /// <summary>
    /// Retourne une représentation textuelle du plateau
    /// sous forme de tableau encadré.
    /// </summary>
    /// <returns>Chaîne représentant le plateau</returns>
    public override string ToString()
    {
        string s = "";

        s += "┌";
        for (int j = 0; j < colonnes; j++)
        {
            s += "───";
            if (j < colonnes - 1)
                s += "┬";
        }
        s += "┐\n";

        for (int i = 0; i < lignes; i++)
        {
            s += "│";
            for (int j = 0; j < colonnes; j++)
            {
                char c = grille[i, j];
                if (c == '\0') c = ' ';
                s += " " + c + " │";
            }
            s += "\n";

            if (i < lignes - 1)
            {
                s += "├";
                for (int j = 0; j < colonnes; j++)
                {
                    s += "───";
                    if (j < colonnes - 1)
                        s += "┼";
                }
                s += "┤\n";
            }
        }

        s += "└";
        for (int j = 0; j < colonnes; j++)
        {
            s += "───";
            if (j < colonnes - 1)
                s += "┴";
        }
        s += "┘\n";

        return s;
    }

    /// <summary>
    /// Recherche un mot sur le plateau à partir de la base.
    /// </summary>
    /// <param name="mot">Mot à rechercher</param>
    /// <returns>
    /// Liste des positions du mot si trouvé,
    /// null sinon
    /// </returns>
    public List<Position> Recherche_Mot(string mot)  /// cette méthode essaye de construire spatialement le mot 
    {
        mot = mot.ToUpper();
        int i = lignes - 1; /// point de départ de la recherche en bas du plateau 

        for (int j = 0; j < colonnes; j++) /// parcourt toutes les colonnes
        {
            if (grille[i, j] == mot[0]) 
            {
                bool[,] utilise = new bool[lignes, colonnes];  /// matrice de la même taille que le plateau pour savoir si une case est déjà utilisé dans le chemin courant( pour éviter les boucles infinis)
                List<Position> chemin = new List<Position>(); /// liste qui contiendra les coordonnées des lettres trouvées

                if (ChercheVoisins(i, j, mot, 0, utilise, chemin))
                    return chemin;
            }
        }

        return null;
    }

    /// <summary>
    /// Recherche récursive des lettres voisines constituant un mot.
    /// </summary>
    /// <param name="i">Indice de ligne</param>
    /// <param name="j">Indice de colonne</param>
    /// <param name="mot">Mot recherché</param>
    /// <param name="index">Indice courant dans le mot</param>
    /// <param name="utilise">Cases déjà utilisées</param>
    /// <param name="chemin">Chemin courant du mot</param>
    /// <returns>true si le mot est trouvé, false sinon</returns>
    private bool ChercheVoisins(
        int i, int j,
        string mot,
        int index,
        bool[,] utilise,
        List<Position> chemin) /// index sert à savoir où on est dans le mot (par exemple mot[0])
    {
        if (i < 0 || i >= lignes || j < 0 || j >= colonnes)
            return false;

        if (utilise[i, j]) /// permet d'éviter les boucles infini
            return false;

        if (grille[i, j] != mot[index]) /// si la case ne contient pas la lettre alors c'est pas le bon chemin
            return false;

        utilise[i, j] = true;
        chemin.Add(new Position(i, j)); /// si toutes les conditions sont vérfiés on ajoute la position dans chemin et on met à 1 le utilise (i,j)

        if (index == mot.Length - 1) /// condition d'arrêt de l'appel récursif 
            return true;

        if (
            ChercheVoisins(i, j - 1, mot, index + 1, utilise, chemin) || /// teste pour toutes les lettres suivantes ici gauche
            ChercheVoisins(i, j + 1, mot, index + 1, utilise, chemin) || /// droite
            ChercheVoisins(i - 1, j, mot, index + 1, utilise, chemin) ||/// haut
            ChercheVoisins(i - 1, j - 1, mot, index + 1, utilise, chemin) || /// haut gauche
            ChercheVoisins(i - 1, j + 1, mot, index + 1, utilise, chemin) /// haute droite
        )
        {
            return true;
        }

        utilise[i, j] = false; /// si aucun voisin ne marche on libère la case pour d'autres chemins possibles
        chemin.RemoveAt(chemin.Count - 1); /// on retire la dernière positon enlevée 

        return false; /// return false si il n'y a pas de possibilité de finir le mot 
    }

    /// <summary>
    /// Indique si le plateau est entièrement vide.
    /// </summary>
    /// <returns>true si le plateau est vide, false sinon</returns>
    public bool EstVide()
    {
        for (int i = 0; i < lignes; i++)
        {
            for (int j = 0; j < colonnes; j++)
            {
                if (grille[i, j] != '\0')
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Met à jour le plateau après la validation d'un mot.
    /// Supprime les lettres du mot et fait glisser les colonnes.
    /// </summary>
    /// <param name="positions">Positions des lettres du mot</param>
    public void Maj_Plateau(List<Position> positions) /// prend en argument la liste des cases à supprimer
    {
        foreach (var p in positions) /// on parcout toutes les lettres du mot trouvé 
            grille[p.I, p.J] = '\0'; /// on met un caractère vide à la bonne place 

        for (int j = 0; j < colonnes; j++) /// parcours colonne par colonne (gravité fonctionne verticalement)
        {
            List<char> lettres = new List<char>(); /// liste contient toutes les lettres présentes 

            for (int i = lignes - 1; i >= 0; i--) /// on parcout la colonne du bas vers le haut 
            {
                if (grille[i, j] != '\0') /// si la case n'est pas vide on ajoute la letrre à la liste
                    lettres.Add(grille[i, j]);
            }

            int index = 0;
            for (int i = lignes - 1; i >= 0; i--) /// on parcourt la liste
            {
                if (index < lettres.Count)
                    grille[i, j] = lettres[index++];
                else
                    grille[i, j] = '\0'; /// le reste des élements non placés 
            }
        }
    }

    /// <summary>
    /// Sauvegarde le plateau courant dans un fichier CSV.
    /// </summary>
    /// <param name="nomfile">Nom du fichier de sortie</param>
    public void ToFile(string nomfile)
    {
        using (StreamWriter sw = new StreamWriter(nomfile)) /// ouvrir un ficher en écriture 
        {
            for (int i = 0; i < lignes; i++) /// on parcourt la grille ligne par ligne
            {
                List<string> cells = new List<string>(); /// liste qui va contenir les valeurs de la ligne

                for (int j = 0; j < colonnes; j++)
                {
                    if (grille[i, j] == '\0')
                        cells.Add("");
                    else
                        cells.Add(grille[i, j].ToString());
                }

                sw.WriteLine(string.Join(",", cells));
            }
        }
    }

    /// <summary>
    /// Charge un plateau depuis un fichier CSV.
    /// </summary>
    /// <param name="nomfile">Nom du fichier CSV à lire</param>
    public void ToRead(string nomfile)
    {
        var lignesCSV = File.ReadAllLines(nomfile);

        lignes = lignesCSV.Length; /// nombre de ligne du fichier 
        colonnes = lignesCSV[0].Split(',').Length; /// on regarde la première ligne qu'on découpe -> nombre de morceux 

        grille = new char[lignes, colonnes]; /// création de notre grille 

        for (int i = 0; i < lignes; i++)
        {
            var cases = lignesCSV[i].Split(','); 

            for (int j = 0; j < colonnes; j++) /// reconstruction de chaque cellule 
            {
                if (string.IsNullOrEmpty(cases[j])) /// si la case est vide 
                    grille[i, j] = '\0';
                else
                    grille[i, j] = cases[j][0]; /// on stocje le premier caractère dans la grille 
            }
        }
    }
}
