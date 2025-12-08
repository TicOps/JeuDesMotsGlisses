public class Plateau
{
    private char[,] grille; 
    private int lignes; 
    private int colonnes; 
    
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
            s += " " + grille[i, j] + " ";  
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



}