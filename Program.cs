using System;

public class Program
{
    public static void Main(string[] args)
    {
        bool quitter = false;

        while (!quitter)
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine(@"     ____.              ________                     _____          __             ________.__  .__                             
    |    | ____  __ __  \______ \   ____   ______   /     \   _____/  |_  ______  /  _____/|  | |__| ______ ______ ____   ______
    |    |/ __ \|  |  \  |    |  \_/ __ \ /  ___/  /  \ /  \ /  _ \   __\/  ___/ /   \  ___|  | |  |/  ___//  ___// __ \ /  ___/
/\__|    \  ___/|  |  /  |    `   \  ___/ \___ \  /    Y    (  <_> )  |  \___ \  \    \_\  \  |_|  |\___ \ \___ \\  ___/ \___ \ 
\________|\___  >____/  /_______  /\___  >____  > \____|__  /\____/|__| /____  >  \______  /____/__/____  >____  >\___  >____  >
              \/                \/     \/     \/          \/                 \/          \/             \/     \/     \/     \/ ");
            
            Console.WriteLine("\n\n📜 RÈGLES DU JEU");
            Console.WriteLine("────────────────────────────────────");
            Console.WriteLine("• Le jeu se joue à 2 joueurs.");
            Console.WriteLine("• Les joueurs jouent chacun leur tour.");
            Console.WriteLine("• Un mot doit :");
            Console.WriteLine("   - exister dans le dictionnaire");
            Console.WriteLine("   - être présent sur le plateau");
            Console.WriteLine("   - faire au moins 2 lettres");
            Console.WriteLine("• Les lettres utilisées disparaissent.");
            Console.WriteLine("• Les lettres au-dessus glissent vers le bas.");
            Console.WriteLine("• Chaque lettre rapporte des points.");
            Console.WriteLine("• La partie s’arrête au bout de 2 minutes ou quand le plateau est vide.");            

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
                        
            
            Console.WriteLine("\n\n\n╔════════════════════════════════════╗");
            Console.WriteLine("║              🎮 MENU 🎮            ║");
            Console.WriteLine("╠════════════════════════════════════╣");
            Console.WriteLine("║  1️⃣  Jouer depuis un fichier CSV    ║");
            Console.WriteLine("║  2️⃣  Plateau généré aléatoirement   ║");
            Console.WriteLine("║  3️⃣  Quitter                        ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine();
            Console.Write("➤ Entrez votre choix : ");


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
        int taille;
        Console.Write("Choisissez la taille du plateau : ");

        while (!int.TryParse(Console.ReadLine(), out taille))
        {
            Console.Write("Veuillez entrer un entier valide : ");
        }

        Plateau p = new Plateau("Lettres.txt", taille, taille);
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
