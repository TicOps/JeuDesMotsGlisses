using System;

public class Program
{
    public static void Main(string[] args)
    {
        bool quitter = false;

        while (!quitter)
        {
            Console.Clear();
            Console.WriteLine("=== MENU ===");
            Console.WriteLine("1. Jouer à partir d’un fichier CSV");
            Console.WriteLine("2. Jouer avec un plateau généré aléatoirement");
            Console.WriteLine("3. Quitter");
            Console.Write("\nVotre choix : ");

            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    JouerDepuisFichier();
                    break;

                case "2":
                    JouerAvecPlateauAleatoire();
                    break;

                case "3":
                    quitter = true;
                    Console.WriteLine("Fermeture du programme...");
                    break;

                default:
                    Console.WriteLine("Choix invalide !");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // ================================
    //  LANCEMENT DES FICHIERS
    // ================================

    private static void JouerDepuisFichier()
    {
        Console.Clear();
        Console.WriteLine("=== Jouer à partir d’un fichier CSV ===");

        Console.Write("Nom du fichier CSV (ex: Test1.csv) : ");
        string nomFichier = Console.ReadLine();

        // On charge le plateau
        Plateau p;
        try
        {
            p = new Plateau(nomFichier);
        }
        catch(Exception ex)
        {
            Console.WriteLine("Erreur lors de la lecture du fichier : " + ex.Message);
            Console.WriteLine("Appuyez sur une touche pour revenir au menu");
            Console.ReadKey();
            return;
        }

        // On charge le dictionnaire
        Dictionnaire d = new Dictionnaire("MotsFrancais.txt");

        Console.Write("Nom joueur 1 : ");
        Joueur j1 = new Joueur(Console.ReadLine());

        Console.Write("Nom joueur 2 : ");
        Joueur j2 = new Joueur(Console.ReadLine());

        Jeu jeu = new Jeu(j1, j2, p, d);
        jeu.Demarrer();

        Console.WriteLine("Appuyez sur une touche pour revenir au menu");
        Console.ReadKey();
    }


    private static void JouerAvecPlateauAleatoire()
    {
        Console.Clear();

        // génération
        Plateau p = new Plateau("Lettres.txt", 8, 8);
        Dictionnaire d = new Dictionnaire("MotsFrancais.txt");

        Console.Write("Nom joueur 1 : ");
        Joueur j1 = new Joueur(Console.ReadLine());

        Console.Write("Nom joueur 2 : ");
        Joueur j2 = new Joueur(Console.ReadLine());

        Jeu jeu = new Jeu(j1, j2, p, d);
        jeu.Demarrer();

        Console.WriteLine("Appuyez sur une touche pour revenir au menu");
        Console.ReadKey();
    }

}
