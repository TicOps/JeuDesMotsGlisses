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
            TriFusion(dico[i]);
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

    /// <summary>
    /// Trie une liste de mots en utilisant l'algorithme du tri fusion.
    /// Cette méthode modifie directement la liste passée en paramètre.
    /// </summary>
    /// <param name="liste">Liste de chaînes de caractères à trier</param>
    private void TriFusion(List<string> liste)
    {
        // Si la liste contient 0 ou 1 élément,
        // elle est déjà triée, on arrête
        if (liste.Count <= 1)
            return;

        // On lance le tri fusion récursif
        List<string> triee = TriFusionRec(liste);

        // On vide la liste d'origine
        liste.Clear();

        // On remet les éléments triés dans la liste d'origine
        liste.AddRange(triee);
    }

    /// <summary>
    /// Fonction récursive du tri fusion.
    /// Elle divise la liste en deux parties, trie chaque partie,
    /// puis les fusionne pour obtenir une liste triée.
    /// </summary>
    /// <param name="liste">Liste à trier</param>
    /// <returns>Nouvelle liste triée</returns>
    private List<string> TriFusionRec(List<string> liste)
    {
        // Cas de base :
        // une liste de 0 ou 1 élément est déjà triée
        if (liste.Count <= 1)
            return liste;

        // On calcule le milieu de la liste
        int milieu = liste.Count / 2;

        // On sépare la liste en deux sous-listes
        List<string> gauche = liste.GetRange(0, milieu);
        List<string> droite = liste.GetRange(milieu, liste.Count - milieu);

        // On trie récursivement chaque sous-liste
        gauche = TriFusionRec(gauche);
        droite = TriFusionRec(droite);

        // On fusionne les deux listes triées
        return Fusion(gauche, droite);
    }

    /// <summary>
    /// Fusionne deux listes déjà triées en une seule liste triée.
    /// Les éléments sont comparés un par un.
    /// </summary>
    /// <param name="gauche">Première liste triée</param>
    /// <param name="droite">Deuxième liste triée</param>
    /// <returns>Liste fusionnée et triée</returns>
    private List<string> Fusion(List<string> gauche, List<string> droite)
    {
        // Liste qui contiendra le résultat final
        List<string> resultat = new List<string>();

        // Tant que les deux listes contiennent des éléments
        while (gauche.Count > 0 && droite.Count > 0)
        {
            // On compare le premier élément de chaque liste
            if (string.Compare(gauche[0], droite[0]) <= 0)
            {
                // L'élément de gauche est le plus petit
                resultat.Add(gauche[0]);
                gauche.RemoveAt(0); // on le retire de la liste gauche
            }
            else
            {
                // L'élément de droite est le plus petit
                resultat.Add(droite[0]);
                droite.RemoveAt(0); // on le retire de la liste droite
            }
        }

        // S'il reste des éléments dans la liste gauche,
        // on les ajoute tous à la fin
        resultat.AddRange(gauche);

        // S'il reste des éléments dans la liste droite,
        // on les ajoute tous à la fin
        resultat.AddRange(droite);

        return resultat;
    }



}


