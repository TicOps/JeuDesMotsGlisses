using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Gère le déroulement complet d'une partie du jeu des Mots Glissés.
/// Cette classe contrôle les tours de jeu, la gestion du temps,
/// la validation des mots et le calcul des scores.
/// </summary>
public class Jeu
{
    // Attributs principaux du jeu
    private Joueur joueur1;
    private Joueur joueur2;
    private Plateau plateau;
    private Dictionnaire dictionnaire;

    // Gestion du temps
    private DateTime debutPartie;
    private readonly TimeSpan dureePartie = TimeSpan.FromMinutes(2);
    private readonly TimeSpan tempsParTour = TimeSpan.FromSeconds(20);

    // Poids des lettres
    private Dictionary<char, int> poidsLettres;

    /// <summary>
    /// Initialise une nouvelle partie du jeu.
    /// Associe les joueurs, le plateau et le dictionnaire,
    /// puis charge les poids des lettres.
    /// </summary>
    /// <param name="j1">Premier joueur</param>
    /// <param name="j2">Deuxième joueur</param>
    /// <param name="plateau">Plateau de jeu utilisé</param>
    /// <param name="dict">Dictionnaire contenant les mots valides</param>
    public Jeu(Joueur j1, Joueur j2, Plateau plateau, Dictionnaire dict)
    {
        joueur1 = j1;
        joueur2 = j2;
        this.plateau = plateau;
        dictionnaire = dict;

        ChargerPoids("Lettres.txt");
    }

    /// <summary>
    /// Démarre la partie et gère l'alternance des joueurs
    /// jusqu'à la fin du jeu.
    /// </summary>
    public void Demarrer()
    {
        Console.Clear();
        Console.WriteLine("=================================");
        Console.WriteLine("       DÉBUT DE LA PARTIE");
        Console.WriteLine("=================================\n");

        debutPartie = DateTime.Now;
        Joueur joueurActif = joueur1;

        while (!PartieTerminee())
        {
            bool tourValide = JouerUnTour(joueurActif);
            if (!tourValide)
                break;

            joueurActif = ChangerJoueur(joueurActif);
        }

        AfficherScores();
    }

    /// <summary>
    /// Gère un tour de jeu pour un joueur donné.
    /// Vérifie le temps restant, valide le mot proposé
    /// et met à jour le plateau et le score.
    /// </summary>
    /// <param name="joueur">Joueur dont c'est le tour</param>
    /// <returns>
    /// true si la partie peut continuer,
    /// false si le temps total de jeu est écoulé
    /// </returns>
    private bool JouerUnTour(Joueur joueur)
    {
        TimeSpan tempsRestant = dureePartie - (DateTime.Now - debutPartie);

        if (tempsRestant <= TimeSpan.Zero)
            return false;

        Console.Clear();
        AfficherPlateauEtInfos(joueur, tempsRestant);

        DateTime debutTour = DateTime.Now;  //Définition de début tour

        Console.Write("Mot proposé : ");
        string mot = Console.ReadLine().ToUpper();

        if (DateTime.Now - debutTour > tempsParTour)
        {
            Console.WriteLine("\n⏱ Temps écoulé pour ce tour !");
            Pause();
            return true; // on passe au joueur suivant
        }

        if (!MotValide(joueur, mot))
            return true;

        var positions = plateau.Recherche_Mot(mot);
        if (positions == null)
        {
            MessageErreur("Mot introuvable sur le plateau !");
            return true;
        }

        plateau.Maj_Plateau(positions);

        int score = CalculScore(mot);
        joueur.Add_Mot(mot);
        joueur.Add_Score(score);

        Console.WriteLine($"\n✅ Mot validé ! +{score} points");
        Pause();
        return true;
    }
    /// <summary>
    /// Vérifie si un mot proposé est valide selon les règles du jeu.
    /// </summary>
    /// <param name="joueur">Joueur ayant proposé le mot</param>
    /// <param name="mot">Mot saisi par le joueur</param>
    /// <returns>
    /// true si le mot est valide,
    /// false sinon
    /// </returns>
    private bool MotValide(Joueur joueur, string mot)
    {
        if (mot.Length < 2)
            return MessageErreur("Mot trop court.");

        if (joueur.Contient(mot))
            return MessageErreur("Mot déjà trouvé.");

        if (!dictionnaire.RechDichoRecursif(mot))
            return MessageErreur("Mot absent du dictionnaire.");

        return true;
    }

    /// <summary>
    /// Affiche le plateau de jeu ainsi que les informations
    /// du joueur actif et le temps restant.
    /// </summary>
    /// <param name="joueur">Joueur dont c'est le tour</param>
    /// <param name="tempsRestant">Temps restant avant la fin de la partie</param>
    private void AfficherPlateauEtInfos(Joueur joueur, TimeSpan tempsRestant)
    {
        Console.WriteLine(plateau.ToString());
        Console.WriteLine($"Joueur : {joueur.Nom}");
        Console.WriteLine($"Temps restant : {(int)tempsRestant.TotalSeconds} secondes");

        if (tempsRestant.TotalSeconds <= 10)
            Console.WriteLine("Attention : fin de partie imminente.");

        Console.WriteLine(new string('-', 40));
    }

    /// <summary>
    /// Affiche les scores finaux et annonce le résultat de la partie.
    /// </summary>
    private void AfficherScores()
    {
        Console.Clear();
        Console.WriteLine("=================================");
        Console.WriteLine("         FIN DE PARTIE");
        Console.WriteLine("=================================\n");

        Console.WriteLine($"{joueur1.Nom} : {joueur1.Score} points");
        Console.WriteLine($"{joueur2.Nom} : {joueur2.Score} points\n");

        if (joueur1.Score > joueur2.Score)
            Console.WriteLine($"Victoire de {joueur1.Nom}");
        else if (joueur2.Score > joueur1.Score)
            Console.WriteLine($"Victoire de {joueur2.Nom}");
        else
            Console.WriteLine("Égalité.");

        Pause();
    }

    /// <summary>
    /// Indique si la partie est terminée.
    /// </summary>
    /// <returns>
    /// true si le temps est écoulé ou si le plateau est vide,
    /// false sinon
    /// </returns>
    private bool PartieTerminee()
    {
        return plateau.EstVide() || DateTime.Now - debutPartie >= dureePartie;
    }

    /// <summary>
    /// Change le joueur actif.
    /// </summary>
    /// <param name="actuel">Joueur actuellement actif</param>
    /// <returns>Le joueur suivant</returns>
    private Joueur ChangerJoueur(Joueur actuel)
    {
        return actuel == joueur1 ? joueur2 : joueur1;
    }

    /// <summary>
    /// Affiche un message d'erreur standardisé.
    /// </summary>
    /// <param name="message">Message d'erreur à afficher</param>
    /// <returns>false</returns>
    private bool MessageErreur(string message)
    {
        Console.WriteLine("\n" + message);
        Pause();
        return false;
    }

    /// <summary>
    /// Met le programme en pause jusqu'à une action utilisateur.
    /// </summary>
    private void Pause()
    {
        Console.WriteLine("\nAppuyez sur une touche...");
        Console.ReadKey();
    }

    /// <summary>
    /// Charge le poids des lettres depuis un fichier texte.
    /// </summary>
    /// <param name="fichier">Nom du fichier contenant les lettres et leurs poids</param>
    private void ChargerPoids(string fichier) /// fonction qui a pour but de remplir une structure de données 
    {
        poidsLettres = new Dictionary<char, int>(); /// on créer un dictionnaire vide avec clé qui est un char(par exemple A) et sa valeur en int

        foreach (string ligne in File.ReadAllLines(fichier))  /// boucle parcourant chaque ligne du fichier
        {
            string[] t = ligne.Split(',');  
            char lettre = char.ToUpper(t[0][0]);
            int poids = int.Parse(t[2]);

            poidsLettres[lettre] = poids;
        }
    }

    /// <summary>
    /// Calcule le score d'un mot en fonction du poids de ses lettres
    /// et de sa longueur.
    /// </summary>
    /// <param name="mot">Mot validé par le joueur</param>
    /// <returns>Score associé au mot</returns>
    private int CalculScore(string mot)
    {
        int score = 0;

        foreach (char c in mot)
        {
            if (poidsLettres.ContainsKey(c))
                score += poidsLettres[c];
        }

        return score * mot.Length;
    }
}
