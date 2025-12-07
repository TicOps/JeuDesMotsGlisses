using System;

public class Jeu
{
    private Dictionnaire dico;
    private Plateau plateau;
    private Joueur joueur1;
    private Joueur joueur2;

    private int tempsTour;
    private int tempsPartie;

    public Jeu()
    {
        // On initialisera tout plus tard
        Console.WriteLine("Jeu initialisé.");
    }

    public void Lancer()
    {
        Console.WriteLine("La partie démarre !");
        // On fera le menu + boucle de jeu ici
    }
}
