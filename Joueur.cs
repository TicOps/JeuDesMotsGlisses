using System;
using System.Collections.Generic;

public class Joueur
{
    // ============================
    //        ATTRIBUTS
    // ============================
    private string nom;
    private List<string> motsTrouves;
    private int score;

    // ============================
    //       CONSTRUCTEUR
    // ============================
    public Joueur(string nom)
    {
        if(nom==null||nom=="")
        {
            Console.WriteLine("Erreur : le nom du joueur est vide."); 
            nom = "Joueur"; 
        }

        this.nom = nom;
        this.motsTrouves = new List<string>();
        this.score = 0;
    }

    // ============================
    //        PROPRIETES
    // ============================
    public string Nom
    {
        get { return nom; }
    }

    public int Score
    {
        get { return score; }
    }

    public List<string> MotsTrouves
    {
        get { return motsTrouves; }
    }

    // ============================
    //        METHODES IMPOSEES
    // ============================

    /// <summary>
    /// Ajoute un mot trouvé par le joueur.
    /// </summary>
    public void Add_Mot(string mot)
    {
        motsTrouves.Add(mot);
    }

    /// <summary>
    /// Ajoute un score (poids du mot trouvé).
    /// </summary>
    public void Add_Score(int val)
    {
        score += val;
    }

    /// <summary>
    /// Vérifie si le joueur a déjà trouvé ce mot.
    /// </summary>
    public bool Contient(string mot)
    {
        return motsTrouves.Contains(mot);
    }

    /// <summary>
    /// Retourne une description du joueur.
    /// </summary>
    public override string ToString()
    {
        return $"Joueur : {nom} | Score : {score} | Mots trouvés : {motsTrouves.Count}";
    }
}
