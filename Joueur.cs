using System;
using System.Collections.Generic;

/// <summary>
/// Représente un joueur du jeu.
/// Un joueur possède un nom, un score et une liste de mots trouvés.
/// </summary>
public class Joueur
{
    /// <summary>
    /// Nom du joueur.
    /// </summary>
    private string nom;

    /// <summary>
    /// Liste des mots déjà trouvés par le joueur.
    /// </summary>
    private List<string> motsTrouves;

    /// <summary>
    /// Score total du joueur.
    /// </summary>
    private int score;

    /// <summary>
    /// Constructeur du joueur.
    /// Initialise le nom, le score et la liste des mots trouvés.
    /// </summary>
    /// <param name="nom">Nom du joueur</param>
    public Joueur(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            Console.WriteLine("Erreur : le nom du joueur est vide.");
            nom = "Joueur";
        }

        this.nom = nom;
        motsTrouves = new List<string>();
        score = 0;
    }

    /// <summary>
    /// Récupère le nom du joueur.
    /// </summary>
    public string Nom
    {
        get { return nom; }
    }

    /// <summary>
    /// Récupère le score actuel du joueur.
    /// </summary>
    public int Score
    {
        get { return score; }
    }

    /// <summary>
    /// Récupère la liste des mots trouvés par le joueur.
    /// </summary>
    public List<string> MotsTrouves
    {
        get { return motsTrouves; }
    }

    /// <summary>
    /// Ajoute un mot à la liste des mots trouvés par le joueur.
    /// </summary>
    /// <param name="mot">Mot trouvé</param>
    public void Add_Mot(string mot)
    {
        motsTrouves.Add(mot);
    }

    /// <summary>
    /// Ajoute des points au score du joueur.
    /// </summary>
    /// <param name="val">Nombre de points à ajouter</param>
    public void Add_Score(int val)
    {
        score += val;
    }

    /// <summary>
    /// Vérifie si le joueur a déjà trouvé un mot donné.
    /// </summary>
    /// <param name="mot">Mot à vérifier</param>
    /// <returns>true si le mot a déjà été trouvé, false sinon</returns>
    public bool Contient(string mot)
    {
        return motsTrouves.Contains(mot);
    }

    /// <summary>
    /// Retourne une description du joueur (nom, score et nombre de mots trouvés).
    /// </summary>
    /// <returns>Chaîne descriptive du joueur</returns>
    public override string ToString()
    {
        return $"Joueur : {nom} | Score : {score} | Mots trouvés : {motsTrouves.Count}";
    }
}
