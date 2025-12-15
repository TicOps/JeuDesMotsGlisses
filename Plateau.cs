public class Plateau
{
    private char[,] grille; 
    private int lignes; 
    private int colonnes; 
    // Charge un plateau depuis un fichier CSV
    public Plateau(string fichierCSV)
    {
        ToRead(fichierCSV);
    }

    // Génère un plateau aléatoire depuis Lettres.txt
    public Plateau(string fichierLettres, int lignes, int colonnes)
    {
        this.lignes = lignes;
        this.colonnes = colonnes;
        grille = new char[lignes, colonnes];

        GenererAleatoire(fichierLettres);
    }

    private static Random random = new Random(); // une seule fois pour tout le programme

private void GenererAleatoire(string fichierLettres)
{
    // 1. Lire le fichier
    var lignesCSV = File.ReadAllLines(fichierLettres);

    // 2. Dictionnaire : lettre -> max autorisé
    Dictionary<char, int> maxLettres = new Dictionary<char, int>();

    foreach (var ligne in lignesCSV)
    {
        var t = ligne.Split(',');
        char lettre = char.ToUpper(t[0][0]);
        int max = int.Parse(t[1]);
        maxLettres[lettre] = max;
    }

    // 3. Vérifier que le total est suffisant
    int totalMax = 0;
    foreach (var x in maxLettres.Values)
        totalMax += x;

    if (totalMax < lignes * colonnes)
        throw new Exception("Impossible de remplir le plateau : pas assez de lettres possibles dans Lettres.txt");

    // 4. Créer une liste avec les lettres répétées selon le max
    List<char> pool = new List<char>();
    foreach (var kvp in maxLettres)
    {
        char lettre = kvp.Key;
        int max = kvp.Value;
        for (int i = 0; i < max; i++)
            pool.Add(lettre);
    }

    // 5. Mélanger le pool aléatoirement
    for (int i = pool.Count - 1; i > 0; i--)
    {
        int j = random.Next(i + 1);
        (pool[i], pool[j]) = (pool[j], pool[i]);
    }

    // 6. Remplir la grille en piochant dans le pool
    int index = 0;
    for (int i = 0; i < lignes; i++)
    {
        for (int j = 0; j < colonnes; j++)
        {
            grille[i, j] = pool[index];
            index++;
        }
    }
}

public string ToString()
{
    string s = "";   // On va construire progressivement la chaîne finale du plateau

    // ================
    // LIGNE DU HAUT
    // ================

    s += "┌";   // Coin supérieur gauche du tableau

    for (int j = 0; j < colonnes; j++)
    {
        s += "───";  // Le haut d’une case (3 tirets)

        // Si ce n'est pas la dernière colonne
        if (j < colonnes - 1)
            s += "┬";   // Séparateur entre colonnes dans la ligne du haut
    }

    s += "┐\n";  // Coin supérieur droit + retour à la ligne



    // ====================
    // LIGNES DU PLATEAU
    // ====================
    for (int i = 0; i < lignes; i++)
    {
        s += "│";   // Bordure gauche de la ligne

        for (int j = 0; j < colonnes; j++)
        {
            char c = grille[i, j];
            if (c == '\0') c = ' ';   // afficher un espace visible

            s += " " + c + " ";

            // Ajoute la lettre entourée d'espaces pour la centrer visuellement
            // Exemple : " A "

            s += "│";  // Bordure verticale entre 2 cases
        }

        s += "\n";   // Retour à la ligne après avoir affiché toutes les colonnes



        // ==========================
        // LIGNES DE SÉPARATION (entre chaque ligne)
        // ==========================

        if (i < lignes - 1) // Ne fait PAS de séparation après la dernière ligne
        {
            s += "├";  // Bord gauche de la ligne de séparation

            for (int j = 0; j < colonnes; j++)
            {
                s += "───"; // Séparateur horizontal d’une case

                if (j < colonnes - 1)
                    s += "┼"; // Intersection entre les cases
            }

            s += "┤\n"; // Bord droit de la ligne de séparation
        }
    }



    // ====================
    // LIGNE DU BAS
    // ====================

    s += "└";   // Coin inférieur gauche

    for (int j = 0; j < colonnes; j++)
    {
        s += "───";  // Bas de chaque case

        if (j < colonnes - 1)
            s += "┴";  // Séparateur bas entre colonnes
    }

    s += "┘\n";  // Coin inférieur droit + saut de ligne



    return s;  // On renvoie la chaîne complète contenant le plateau
}

//----------------------------------- Modification du plateau --------------------------------------------------

// Fonction pour voir si le plateau est vide (cela permet d'arrêter la partie avant la fin du temps)
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

// Rechercher si un mot est bien dans le plateau
public List<Position> Recherche_Mot(string mot)
{
    mot = mot.ToUpper();

    for (int i = 0; i < lignes; i++)
    {
        for (int j = 0; j < colonnes; j++)
        {
            var v = ChercheVertical(i, j, mot);
            if (v != null) return v;

            var g = ChercheGauche(i, j, mot);
            if (g != null) return g;

            var d = ChercheDroite(i, j, mot);
            if (d != null) return d;

            var diag1 = ChercheDiagHautGauche(i, j, mot);
            if (diag1 != null) return diag1;

            var diag2 = ChercheDiagHautDroite(i, j, mot);
            if (diag2 != null) return diag2;

            var diag3 = ChercheDiagBasGauche(i, j, mot);
            if (diag3 != null) return diag3;

            var diag4 = ChercheDiagBasDroite(i, j, mot);
            if (diag4 != null) return diag4;
        }
    }

    return null;
}


private List<Position> ChercheVertical(int i, int j, string mot)
{
    if (i - (mot.Length - 1) < 0) return null; // sort de la grille

    List<Position> pos = new List<Position>();

    for(int k = 0; k < mot.Length; k++)
    {
        if(grille[i - k, j] != mot[k])
            return null;

        pos.Add(new Position(i - k, j));
    }

    return pos;
}


private List<Position> ChercheGauche(int i, int j, string mot)
{
    // si le mot dépasse à gauche -> impossible
    if (j - (mot.Length - 1) < 0)
        return null;

    List<Position> pos = new List<Position>();

    for (int k = 0; k < mot.Length; k++)
    {
        // grille[i, j - k] car on va à gauche
        if (grille[i, j - k] != mot[k])
            return null;

        pos.Add(new Position(i, j - k));
    }

    return pos;
}

private List<Position> ChercheDroite(int i, int j, string mot)
{
    // si le mot dépasse à droite -> impossible
    if (j + (mot.Length - 1) >= colonnes)
        return null;

    List<Position> pos = new List<Position>();

    for (int k = 0; k < mot.Length; k++)
    {
        // grille[i, j + k] car on va à droite
        if (grille[i, j + k] != mot[k])
            return null;

        pos.Add(new Position(i, j + k));
    }

    return pos;
}

private List<Position> ChercheDiagHautGauche(int i, int j, string mot)
{
    if (i - (mot.Length - 1) < 0 || j - (mot.Length - 1) < 0)
        return null;

    List<Position> pos = new List<Position>();

    for (int k = 0; k < mot.Length; k++)
    {
        if (grille[i - k, j - k] != mot[k])
            return null;

        pos.Add(new Position(i - k, j - k));
    }

    return pos;
}

private List<Position> ChercheDiagHautDroite(int i, int j, string mot)
{
    if (i - (mot.Length - 1) < 0 || j + (mot.Length - 1) >= colonnes)
        return null;

    List<Position> pos = new List<Position>();

    for (int k = 0; k < mot.Length; k++)
    {
        if (grille[i - k, j + k] != mot[k])
            return null;

        pos.Add(new Position(i - k, j + k));
    }

    return pos;
}

private List<Position> ChercheDiagBasGauche(int i, int j, string mot)
{
    if (i + (mot.Length - 1) >= lignes || j - (mot.Length - 1) < 0)
        return null;

    List<Position> pos = new List<Position>();

    for (int k = 0; k < mot.Length; k++)
    {
        if (grille[i + k, j - k] != mot[k])
            return null;

        pos.Add(new Position(i + k, j - k));
    }

    return pos;
}

private List<Position> ChercheDiagBasDroite(int i, int j, string mot)
{
    if (i + (mot.Length - 1) >= lignes || j + (mot.Length - 1) >= colonnes)
        return null;

    List<Position> pos = new List<Position>();

    for (int k = 0; k < mot.Length; k++)
    {
        if (grille[i + k, j + k] != mot[k])
            return null;

        pos.Add(new Position(i + k, j + k));
    }

    return pos;
}




// Mise à jour du plateau, ici l'iée clé est de traiter colone par colone et de supprimer le \0
public void Maj_Plateau(List<Position> positions)
{
    // 1. Effacer les lettres du mot
    foreach (var p in positions)
    {
        grille[p.I, p.J] = '\0';
    }

    // 2. Faire glisser chaque colonne
    for(int j = 0; j < colonnes; j++)
    {
        // On collecte toutes les cases non vides dans cette colonne
        List<char> lettres = new List<char>();

        for(int i = lignes - 1; i >= 0; i--)
        {
            if(grille[i, j] != '\0')
            {
                lettres.Add(grille[i, j]);
            }
        }

        // Remplissage de bas en haut
        int index = 0;

        for(int i = lignes - 1; i >= 0; i--)
        {
            if(index < lettres.Count)
            {
                grille[i, j] = lettres[index];
                index++;
            }
            else
            {
                grille[i, j] = '\0';
            }
        }
    }
}

// ------------------------------------ Méthodes de sauvegarde et d'écriture des plateaux ---------------
public void ToFile(string nomfile)
{
    using (StreamWriter sw = new StreamWriter(nomfile))
    {
        for (int i = 0; i < lignes; i++)
        {
            List<string> cells = new List<string>();

            for (int j = 0; j < colonnes; j++)
            {
                // Si la case est vide
                if (grille[i, j] == '\0')
                    cells.Add(""); // vide
                else
                    cells.Add(grille[i, j].ToString());
            }

            // écrire : A,B,C,D
            sw.WriteLine(string.Join(",", cells));
        }
    }
}

public void ToRead(string nomfile)
{
    var lignesCSV = File.ReadAllLines(nomfile);

    this.lignes = lignesCSV.Length;
    this.colonnes = lignesCSV[0].Split(',').Length;

    grille = new char[this.lignes, this.colonnes];

    for (int i = 0; i < lignes; i++)
    {
        var cases = lignesCSV[i].Split(',');

        for (int j = 0; j < colonnes; j++)
        {
            if (string.IsNullOrEmpty(cases[j]))
                grille[i, j] = '\0';
            else
                grille[i, j] = cases[j][0]; // premier caractère
        }
    }
}





}