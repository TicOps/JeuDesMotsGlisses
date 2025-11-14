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
    //   FONCTIONS SIMPLIFIÉES / TODO
    // ================================

    private static void JouerDepuisFichier()
    {
        Console.Clear();
        Console.WriteLine("[TODO] Jouer depuis un fichier CSV n’est pas encore implémenté.");
        Console.WriteLine("Appuyez sur une touche pour retourner au menu...");
        Console.ReadKey();
        // TODO : Lire un plateau CSV + lancer une partie
    }

    private static void JouerAvecPlateauAleatoire()
    {
        Console.Clear();
        Console.WriteLine("[TODO] Jouer avec un plateau aléatoire n’est pas encore implémenté.");
        Console.WriteLine("Appuyez sur une touche pour retourner au menu...");
        Console.ReadKey();
        // TODO : Générer un plateau aléatoire + lancer une partie
    }
}
