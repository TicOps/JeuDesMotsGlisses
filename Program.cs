using System;

/// <summary>
/// Point d’entrée principal du programme.
/// Gère l’affichage du titre, des règles et du menu,
/// puis lance les différentes parties du jeu.
/// </summary>
public class Program
{
    /// <summary>
    /// Méthode principale exécutée au lancement du programme.
    /// Affiche le menu et gère les choix de l’utilisateur.
    /// </summary>
    public static void Main(string[] args)
    {
        bool quitter = false;

        while (!quitter)
        {
            Console.Clear();

            // ==========================
            //        TITRE ASCII
            // ==========================
            Console.WriteLine(@"     ____.              ________                     _____          __             ________.__  .__                             
    |    | ____  __ __  \______ \   ____   ______   /     \   _____/  |_  ______  /  _____/|  | |__| ______ ______ ____   ______
    |    |/ __ \|  |  \  |    |  \_/ __ \ /  ___/  /  \ /  \ /  _ \   __\/  ___/ /   \  ___|  | |  |/  ___//  ___// __ \ /  ___/
 /\__|    \  ___/|  |  /  |    `   \  ___/ \___ \  /    Y    (  <_> )  |  \___ \  \    \_\  \  |_|  |\___ \ \___ \\  ___/ \___ \ 
 \________|\___  >____/  /_______  /\___  >____  > \____|__  /\____/|__| /____  >  \______  /____/__/____  >____  >\___  >____  >
               \/                \/     \/     \/          \/                 \/          \/             \/     \/     \/     \/ ");

            // ==========================
            //           RÈGLES
            // ==========================
            Console.WriteLine("\n\nREGLES DU JEU");
            Console.WriteLine("────────────────────────────────────");
            Console.WriteLine("• Le jeu se joue à 2 joueurs.");
            Console.WriteLine("• Les joueurs jouent chacun leur tour.");
            Console.WriteLine("• Un mot doit :");
            Console.WriteLine("   - exister dans le dictionnaire");
            Console.WriteLine("   - être présent sur le plateau");
            Console.WriteLine("   - contenir au moins 2 lettres");
            Console.WriteLine("• Les lettres utilisées disparaissent.");
            Console.WriteLine("• Les lettres au-dessus glissent vers le bas.");
            Console.WriteLine("• Chaque lettre rapporte des points.");
            Console.WriteLine("• La partie se termine après 2 minutes ou si le plateau est vide.");

            // ==========================
            //     TEXTE ASCII SECONDAIRE
            // ==========================
            Console.WriteLine(@"
░████████                                                             ░██                                                    
░██    ░██                                                            ░██                                                    
░██    ░██   ░███████  ░████████  ░████████   ░███████      ░███████  ░████████   ░██████   ░████████   ░███████   ░███████  
░████████   ░██    ░██ ░██    ░██ ░██    ░██ ░██    ░██    ░██    ░██ ░██    ░██       ░██  ░██    ░██ ░██    ░██ ░██    ░██ 
░██     ░██ ░██    ░██ ░██    ░██ ░██    ░██ ░█████████    ░██        ░██    ░██  ░███████  ░██    ░██ ░██        ░█████████ 
░██     ░██ ░██    ░██ ░██    ░██ ░██    ░██ ░██           ░██    ░██ ░██    ░██ ░██   ░██  ░██    ░██ ░██    ░██ ░██        
░█████████   ░███████  ░██    ░██ ░██    ░██  ░███████      ░███████  ░██    ░██  ░█████░██ ░██    ░██  ░███████   ░███████  
");

            Console.WriteLine("Appuyez sur une touche pour accéder au menu...");
            Console.ReadKey();
            Console.Clear();

            // ==========================
            //            MENU
            // ==========================
            Console.WriteLine("\n\n\n╔════════════════════════════════════╗");
            Console.WriteLine("║               MENU                 ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║  1  Jouer depuis un fichier CSV    ║");
            Console.WriteLine("║  2  Plateau généré aléatoirement   ║");
            Console.WriteLine("║  3  Quitter                        ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("Entrez votre choix : ");

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
                    Console.WriteLine("Choix invalide.");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    /// <summary>
    /// Lance une partie à partir d’un plateau chargé depuis un fichier CSV.
    /// </summary>
    private static void JouerDepuisFichier()
    {
        Console.Clear();
        Console.WriteLine("Jouer à partir d’un fichier CSV");

        Console.Write("Nom du fichier CSV : ");
        string nomFichier = Console.ReadLine();

        Plateau plateau;
        try
        {
            plateau = new Plateau(nomFichier);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur lors de la lecture du fichier : " + ex.Message);
            Console.ReadKey();
            return;
        }

        Dictionnaire dictionnaire = new Dictionnaire("MotsFrancais.txt");

        Console.Write("Nom joueur 1 : ");
        Joueur j1 = new Joueur(Console.ReadLine());

        Console.Write("Nom joueur 2 : ");
        Joueur j2 = new Joueur(Console.ReadLine());

        Jeu jeu = new Jeu(j1, j2, plateau, dictionnaire);
        jeu.Demarrer();

        Console.WriteLine("Appuyez sur une touche pour revenir au menu");
        Console.ReadKey();
    }

    /// <summary>
    /// Lance une partie avec un plateau généré aléatoirement.
    /// </summary>
    private static void JouerAvecPlateauAleatoire()
    {
        Console.Clear();
        Console.Write("Choisissez la taille du plateau : ");

        int taille;
        while (!int.TryParse(Console.ReadLine(), out taille))
        {
            Console.Write("Veuillez entrer un entier valide : ");
        }

        Plateau plateau = new Plateau("Lettres.txt", taille, taille);
        Dictionnaire dictionnaire = new Dictionnaire("MotsFrancais.txt");

        Console.Write("Nom joueur 1 : ");
        Joueur j1 = new Joueur(Console.ReadLine());

        Console.Write("Nom joueur 2 : ");
        Joueur j2 = new Joueur(Console.ReadLine());

        Jeu jeu = new Jeu(j1, j2, plateau, dictionnaire);
        jeu.Demarrer();

        Console.WriteLine("Appuyez sur une touche pour revenir au menu");
        Console.ReadKey();
    }
}
