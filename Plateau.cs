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
    private void GenererAleatoire(string fichierLettres)
    {
        var lignesCSV = File.ReadAllLines(fichierLettres);

        Dictionary<char, int> maxLettres = new Dictionary<char, int>();

        foreach (var ligne in lignesCSV)
        {
            var t = ligne.Split(',');
            char lettre = char.ToUpper(t[0][0]);
            int max = int.Parse(t[1]);
            maxLettres[lettre] = max;
        }

        int totalMax = 0;
        foreach (var val in maxLettres.Values)
            totalMax += val;

        if (totalMax < lignes * colonnes)
            throw new Exception("Impossible de remplir le plateau : contraintes insuffisantes.");

        List<char> pool = new List<char>();
        foreach (var kvp in maxLettres)
        {
            for (int i = 0; i < kvp.Value; i++)
                pool.Add(kvp.Key);
        }

        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }

        int index = 0;
        for (int i = 0; i < lignes; i++)
        {
            for (int j = 0; j < colonnes; j++)
            {
                grille[i, j] = pool[index++];
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
    public List<Position> Recherche_Mot(string mot)
    {
        mot = mot.ToUpper();
        int i = lignes - 1;

        for (int j = 0; j < colonnes; j++)
        {
            if (grille[i, j] == mot[0])
            {
                bool[,] utilise = new bool[lignes, colonnes];
                List<Position> chemin = new List<Position>();

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
        List<Position> chemin)
    {
        if (i < 0 || i >= lignes || j < 0 || j >= colonnes)
            return false;

        if (utilise[i, j])
            return false;

        if (grille[i, j] != mot[index])
            return false;

        utilise[i, j] = true;
        chemin.Add(new Position(i, j));

        if (index == mot.Length - 1)
            return true;

        if (
            ChercheVoisins(i, j - 1, mot, index + 1, utilise, chemin) ||
            ChercheVoisins(i, j + 1, mot, index + 1, utilise, chemin) ||
            ChercheVoisins(i - 1, j, mot, index + 1, utilise, chemin) ||
            ChercheVoisins(i - 1, j - 1, mot, index + 1, utilise, chemin) ||
            ChercheVoisins(i - 1, j + 1, mot, index + 1, utilise, chemin)
        )
        {
            return true;
        }

        utilise[i, j] = false;
        chemin.RemoveAt(chemin.Count - 1);

        return false;
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
    public void Maj_Plateau(List<Position> positions)
    {
        foreach (var p in positions)
            grille[p.I, p.J] = '\0';

        for (int j = 0; j < colonnes; j++)
        {
            List<char> lettres = new List<char>();

            for (int i = lignes - 1; i >= 0; i--)
            {
                if (grille[i, j] != '\0')
                    lettres.Add(grille[i, j]);
            }

            int index = 0;
            for (int i = lignes - 1; i >= 0; i--)
            {
                if (index < lettres.Count)
                    grille[i, j] = lettres[index++];
                else
                    grille[i, j] = '\0';
            }
        }
    }

    /// <summary>
    /// Sauvegarde le plateau courant dans un fichier CSV.
    /// </summary>
    /// <param name="nomfile">Nom du fichier de sortie</param>
    public void ToFile(string nomfile)
    {
        using (StreamWriter sw = new StreamWriter(nomfile))
        {
            for (int i = 0; i < lignes; i++)
            {
                List<string> cells = new List<string>();

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

        lignes = lignesCSV.Length;
        colonnes = lignesCSV[0].Split(',').Length;

        grille = new char[lignes, colonnes];

        for (int i = 0; i < lignes; i++)
        {
            var cases = lignesCSV[i].Split(',');

            for (int j = 0; j < colonnes; j++)
            {
                if (string.IsNullOrEmpty(cases[j]))
                    grille[i, j] = '\0';
                else
                    grille[i, j] = cases[j][0];
            }
        }
    }
}
