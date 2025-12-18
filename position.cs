/// <summary>
/// Représente une position dans le plateau du jeu.
/// Une position est définie par une ligne (I) et une colonne (J).
/// </summary>
public class Position
{
    /// <summary>
    /// Indice de la ligne dans la grille.
    /// </summary>
    public int I { get; set; }

    /// <summary>
    /// Indice de la colonne dans la grille.
    /// </summary>
    public int J { get; set; }

    /// <summary>
    /// Constructeur de la classe Position.
    /// Initialise la ligne et la colonne.
    /// </summary>
    /// <param name="i">Numéro de la ligne</param>
    /// <param name="j">Numéro de la colonne</param>
    public Position(int i, int j)
    {
        I = i;
        J = j;
    }
}
