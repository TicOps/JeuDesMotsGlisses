using System;
using System.Collections.Generic;
using System.IO;

public class Jeu
{
    // ==========================
    //        ATTRIBUTS
    // ==========================
    private Joueur joueur1;
    private Joueur joueur2;
    private Plateau plateau;
    private Dictionnaire dictionnaire;

    private DateTime debutPartie;

    private readonly TimeSpan dureePartie = TimeSpan.FromMinutes(2);
    private readonly TimeSpan tempsParTour = TimeSpan.FromSeconds(20);

    private Dictionary<char, int> poidsLettres;

    // ==========================
    //       CONSTRUCTEUR
    // ==========================
    public Jeu(Joueur j1, Joueur j2, Plateau plateau, Dictionnaire dict)
    {
        joueur1 = j1;
        joueur2 = j2;
        this.plateau = plateau;
        dictionnaire = dict;

        ChargerPoids("Lettres.txt");
    }

    // ==========================
    //      LANCEMENT JEU
    // ==========================
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

    // ==========================
    //       TOUR DE JEU
    // ==========================
    private bool JouerUnTour(Joueur joueur)
    {
        TimeSpan tempsRestant = dureePartie - (DateTime.Now - debutPartie);

        if (tempsRestant <= TimeSpan.Zero)
            return false;

        Console.Clear();
        AfficherPlateauEtInfos(joueur, tempsRestant);

        Console.Write("Mot proposé : ");
        string mot = Console.ReadLine().ToUpper();

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

    // ==========================
    //       VALIDATION
    // ==========================
    private bool MotValide(Joueur joueur, string mot)
    {
        if (mot.Length < 2)
            return MessageErreur("Mot trop court !");

        if (joueur.Contient(mot))
            return MessageErreur("Mot déjà trouvé !");

        if (!dictionnaire.RechDichoRecursif(mot))
            return MessageErreur("Mot absent du dictionnaire !");

        return true;
    }

    // ==========================
    //      AFFICHAGES
    // ==========================
    private void AfficherPlateauEtInfos(Joueur joueur, TimeSpan tempsRestant)
    {
        Console.WriteLine(plateau.ToString());

        Console.WriteLine($"🎮 Joueur : {joueur.Nom}");
        Console.WriteLine($"⏱ Temps avant la fin de la partie : {(int)tempsRestant.TotalSeconds} s");

        if (tempsRestant.TotalSeconds <= 10)
            Console.WriteLine("⚠️  Attention : fin imminente !");
        
        Console.WriteLine(new string('-', 40));
    }

    private void AfficherScores()
    {
        Console.Clear();
        Console.WriteLine("=================================");
        Console.WriteLine("         FIN DE PARTIE");
        Console.WriteLine("=================================\n");

        Console.WriteLine($"{joueur1.Nom} : {joueur1.Score} points");
        Console.WriteLine($"{joueur2.Nom} : {joueur2.Score} points\n");

        if (joueur1.Score > joueur2.Score)
            Console.WriteLine($"🏆 Victoire de {joueur1.Nom} !");
        else if (joueur2.Score > joueur1.Score)
            Console.WriteLine($"🏆 Victoire de {joueur2.Nom} !");
        else
            Console.WriteLine("🤝 Égalité parfaite !");

        Pause();
    }

    // ==========================
    //      UTILITAIRES
    // ==========================
    private bool PartieTerminee()
    {
        return plateau.EstVide() || DateTime.Now - debutPartie >= dureePartie;
    }

    private Joueur ChangerJoueur(Joueur actuel)
    {
        return actuel == joueur1 ? joueur2 : joueur1;
    }

    private bool MessageErreur(string message)
    {
        Console.WriteLine($"\n❌ {message}");
        Pause();
        return false;
    }

    private void Pause()
    {
        Console.WriteLine("\nAppuyez sur une touche...");
        Console.ReadKey();
    }

    // ==========================
    //        SCORE
    // ==========================
    private void ChargerPoids(string fichier)
    {
        poidsLettres = new Dictionary<char, int>();

        foreach (string ligne in File.ReadAllLines(fichier))
        {
            string[] t = ligne.Split(',');
            char lettre = char.ToUpper(t[0][0]);
            int poids = int.Parse(t[2]);

            poidsLettres[lettre] = poids;
        }
    }

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
