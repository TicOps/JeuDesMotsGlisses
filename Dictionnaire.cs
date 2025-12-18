using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Représente un dictionnaire de mots organisé par première lettre.
/// Les mots sont stockés dans un tableau de 26 listes (A à Z),
/// ce qui permet une recherche rapide par recherche dichotomique.
/// </summary>
public class Dictionnaire
{
    /// <summary>
    /// Tableau de 26 listes de mots.
    /// Chaque case correspond à une lettre de A à Z.
    /// </summary>
    private List<string>[] dico;

    /// <summary>
    /// Constructeur du dictionnaire.
    /// Initialise les 26 listes et charge les mots depuis un fichier texte.
    /// </summary>
    /// <param name="filename">Nom du fichier contenant les mots du dictionnaire</param>
    public Dictionnaire(string filename)
    {
        dico = new List<string>[26];

        for (int i = 0; i < 26; i++)
            dico[i] = new List<string>();

        LoadFile(filename);
    }

    /// <summary>
    /// Charge les mots du dictionnaire depuis un fichier texte.
    /// Chaque ligne peut contenir plusieurs mots séparés par des espaces.
    /// Les mots sont rangés dans la liste correspondant à leur première lettre.
    /// </summary>
    /// <param name="nomFichier">Nom du fichier texte du dictionnaire</param>
    public void LoadFile(string nomFichier)
    {
        using (StreamReader sr = new StreamReader(nomFichier))
        {
            string ligne;

            while ((ligne = sr.ReadLine()) != null)
            {
                ligne = ligne.Trim();

                if (ligne.Length == 0)
                    continue;

                string[] mots = ligne.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (mots.Length == 0)
                    continue;

                char lettre = char.ToUpper(mots[0][0]);
                int index = lettre - 'A';

                if (index < 0 || index >= 26)
                    continue;

                foreach (string mot in mots)
                {
                    dico[index].Add(mot.ToUpper());
                }
            }
        }

        // Tri des listes pour permettre la recherche dichotomique
        for (int i = 0; i < 26; i++)
            dico[i].Sort();
    }

    /// <summary>
    /// Retourne une description du dictionnaire (nombre total de mots chargés).
    /// </summary>
    /// <returns>Chaîne décrivant le dictionnaire</returns>
    public override string ToString()
    {
        int total = 0;

        for (int i = 0; i < 26; i++)
            total += dico[i].Count;

        return $"Dictionnaire chargé : {total} mots";
    }

    /// <summary>
    /// Vérifie si un mot est présent dans le dictionnaire
    /// en utilisant une recherche dichotomique récursive.
    /// </summary>
    /// <param name="mot">Mot à rechercher</param>
    /// <returns>true si le mot est trouvé, false sinon</returns>
    public bool RechDichoRecursif(string mot)
    {
        if (string.IsNullOrWhiteSpace(mot))
            return false;

        mot = mot.ToUpper();
        int index = mot[0] - 'A';

        if (index < 0 || index >= 26)
            return false;

        return DichoRec(dico[index], mot, 0, dico[index].Count - 1);
    }

    /// <summary>
    /// Effectue une recherche dichotomique récursive dans une liste de mots.
    /// </summary>
    /// <param name="liste">Liste triée de mots</param>
    /// <param name="mot">Mot recherché</param>
    /// <param name="debut">Indice de début de la zone de recherche</param>
    /// <param name="fin">Indice de fin de la zone de recherche</param>
    /// <returns>true si le mot est trouvé, false sinon</returns>
    private bool DichoRec(List<string> liste, string mot, int debut, int fin)
    {
        if (debut > fin)
            return false;

        int milieu = (debut + fin) / 2;
        int comparaison = string.Compare(liste[milieu], mot);

        if (comparaison == 0)
            return true;
        else if (comparaison > 0)
            return DichoRec(liste, mot, debut, milieu - 1);
        else
            return DichoRec(liste, mot, milieu + 1, fin);
    }
}
