public class Jeu
{
    private Joueur joueur1;
    private Joueur joueur2;
    private Plateau plateau;
    private Dictionnaire dictionnaire;
    private DateTime debutPartie;
    private TimeSpan dureePartie = TimeSpan.FromSeconds(60); // exemple


    private TimeSpan tempsPartie;
    private TimeSpan tempsParTour;
    private Dictionary<char, int> poidsLettres;


    public Jeu(Joueur j1, Joueur j2, Plateau plateau, Dictionnaire dict)
{
    joueur1 = j1;
    joueur2 = j2;
    this.plateau = plateau;
    dictionnaire = dict;

    tempsPartie = TimeSpan.FromMinutes(2);
    tempsParTour = TimeSpan.FromSeconds(20);

    ChargerPoids("Lettres.txt");
}


   public void Demarrer()
{
    Console.WriteLine("Début de la partie !");
    debutPartie = DateTime.Now;

    Joueur actif = joueur1;

    while (DateTime.Now - debutPartie < tempsPartie && !plateau.EstVide())
    {
        bool tourOK = JeuUnTour(actif);

        if (!tourOK)
            break;

        actif = actif == joueur1 ? joueur2 : joueur1;
    }

    Console.WriteLine("Fin de partie !");
    AfficherScores();
}


  private bool JeuUnTour(Joueur joueur)
{
    while (true)
    {
        TimeSpan tempsRestantPartie = tempsPartie - (DateTime.Now - debutPartie);

        if (tempsRestantPartie <= TimeSpan.Zero)
            return false;

        Console.Clear();
        Console.WriteLine(plateau.ToString());
        Console.WriteLine($"Au tour de {joueur.Nom}");
        Console.WriteLine($"Vous avez {(int)tempsRestantPartie.TotalSeconds} secondes pour jouer");

        if (tempsRestantPartie.TotalSeconds <= 10)
            Console.WriteLine("⚠️ Attention, il vous reste moins de 10 secondes !");

        Console.Write("Votre mot : ");
        string mot = Console.ReadLine();

        if (mot.Length < 2)
        {
            Console.WriteLine("Mot trop court !");
            Console.ReadKey();
            continue;
        }

        if (joueur.Contient(mot))
        {
            Console.WriteLine("Mot déjà trouvé !");
            Console.ReadKey();
            continue;
        }

        if (!dictionnaire.RechDichoRecursif(mot))
        {
            Console.WriteLine("Mot absent du dictionnaire !");
            Console.ReadKey();
            continue;
        }

        var resultat = plateau.Recherche_Mot(mot);
if (resultat == null)
{
    Console.WriteLine("Mot introuvable sur le plateau !");
}
else
{
    plateau.Maj_Plateau(resultat);
}


        plateau.Maj_Plateau(resultat);

        int score = CalculScore(mot);
        joueur.Add_Mot(mot);
        joueur.Add_Score(score);

        Console.WriteLine($"Mot validé ! Score +{score}");
        Console.ReadKey();
        return true;
    }
}



    private void ChargerPoids(string fichier)
    {
        poidsLettres = new Dictionary<char, int>();

        string[] lignes = File.ReadAllLines(fichier);

        foreach (string ligne in lignes)
        {
            string[] t = ligne.Split(',');
            char lettre = t[0][0];
            int poids = int.Parse(t[2]);

            poidsLettres[lettre] = poids;
        }
    }

    private int CalculScore(string mot)
    {
        int somme = 0;

        foreach (char c in mot.ToUpper())
        {
            if (poidsLettres.ContainsKey(c))
                somme += poidsLettres[c];
        }

        return somme * mot.Length;
    }

    private void AfficherScores()
    {
        Console.Clear();
        Console.WriteLine("=== Fin de partie ===\n");

        Console.WriteLine($"{joueur1.Nom} : {joueur1.Score} points");
        Console.WriteLine($"{joueur2.Nom} : {joueur2.Score} points\n");

        if (joueur1.Score > joueur2.Score)
        {
            Console.WriteLine($"🏆 Le gagnant est {joueur1.Nom} !");
        }
        else if (joueur2.Score > joueur1.Score)
        {
            Console.WriteLine($"🏆 Le gagnant est {joueur2.Nom} !");
        }
        else
        {
            Console.WriteLine("🤝 Égalité parfaite !");
        }

        Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
        Console.ReadKey();
    }



    }
