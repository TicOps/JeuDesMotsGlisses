using System; // permet l'utilisation des fonctions de base
using System.Collections.Generic;  // permet l'utilisation de list<T>
using System.IO; // permet de lire les fichiers

public class Dictionnaire
{
    private List<string>[] dico;  // création d'un tableau de 26 cases. Chaque case contient une list <string> (une liste de mots)

    public Dictionnaire(string filename)  // le constructeur reçoit le fichier texte contentant tous les mots
    {
        dico = new List<string>[26];

        // Initialisation des 26 listes
        for (int i = 0; i < 26; i++)
            dico[i] = new List<string>();

        LoadFile(filename);  // Charge les mots depuis un fichier .txt
    }

    // ===========================
    // CHARGEMENT DU DICTIONNAIRE
    // ===========================
    private void LoadFile(string filename)
{
    if (!File.Exists(filename))
        throw new FileNotFoundException("Fichier dictionnaire introuvable !");

    string[] lignes = File.ReadAllLines(filename);

    foreach (string mot in lignes)
    {
        string clean = mot.Trim().Trim('\uFEFF');

        if (clean.Length == 0) 
            continue;

        clean = clean.ToUpper();

        char c = clean[0];
        int index = c - 'A';
        if (index < 0 || index >= 26)
            continue;

        dico[index].Add(clean);
    }

    for(int i = 0; i < 26; i++)
        dico[i].Sort(StringComparer.Ordinal);
}


    // ===========================
    // AFFICHAGE
    // ===========================
    public override string ToString()
    {
        int count = 0;
        for (int i = 0; i < 26; i++)
            count += dico[i].Count;   // on compte le nombre de mots au total ont été chargés

        return $"Dictionnaire chargé : {count} mots";
    }

    // ===========================
    // RECHERCHE DICHOTOMIQUE RÉCURSIVE
    // ===========================
    public bool RechDichoRecursif(string mot)
    {
        if (string.IsNullOrWhiteSpace(mot))  // si le mot est vide on retourne false 
            return false;

        mot = mot.ToUpper(); // on convertit le mot en majuscule 
        int index = mot[0] - 'A';  // calcule l'indice de la première lettre 

        if (index < 0 || index > 25)
            return false;

        return DichoRec(dico[index], mot, 0, dico[index].Count - 1); // lancement de la recherche dichotomique 
    }

    private bool DichoRec(List<string> liste, string mot, int debut, int fin)
    {
        if (debut > fin)  // condition d'arrêt: la zone de recherche est vide 
            return false;

        int milieu = (debut + fin) / 2; // on coupe la liste en deux 

        int comparaison = string.Compare(liste[milieu], mot);  // on compare les deux mots string.Compare renvoie un nombre > < ou = à 0 

        if (comparaison == 0)
            return true;
        else if (comparaison > 0)
            return DichoRec(liste, mot, debut, milieu - 1);
        else
            return DichoRec(liste, mot, milieu + 1, fin);
    }
}
