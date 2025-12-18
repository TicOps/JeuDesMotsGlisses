// using System;
// using System.Collections.Generic;

// public class TestUnitaire
// {
//     public static void Main()
//     {
//         Console.WriteLine("=== TESTS UNITAIRES ===\n");

//         Test_Joueur_AddScore();
//         Test_Joueur_Contient();
//         Test_Dictionnaire_Recherche();
//         Test_Plateau_EstVide();
//         Test_Plateau_MajPlateau();

//         Console.WriteLine("\nFin des tests.");
//         Console.ReadKey();
//     }

//     // ============================
//     // TEST 1 : Add_Score
//     // ============================
//     static void Test_Joueur_AddScore()
//     {
//         Joueur j = new Joueur("Test");
//         j.Add_Score(5);

//         if (j.Score == 5)
//             Console.WriteLine("Test Add_Score : OK");
//         else
//             Console.WriteLine("Test Add_Score : ERREUR");
//     }

//     // ============================
//     // TEST 2 : Contient
//     // ============================
//     static void Test_Joueur_Contient()
//     {
//         Joueur j = new Joueur("Test");
//         j.Add_Mot("CHAT");

//         if (j.Contient("CHAT"))
//             Console.WriteLine("Test Contient : OK");
//         else
//             Console.WriteLine("Test Contient : ERREUR");
//     }

//     // ============================
//     // TEST 3 : Dictionnaire
//     // ============================
//     static void Test_Dictionnaire_Recherche()
//     {
//         Dictionnaire d = new Dictionnaire("MotsFrancais.txt");

//         if (d.RechDichoRecursif("CHAT"))
//             Console.WriteLine("Test Dictionnaire : OK");
//         else
//             Console.WriteLine("Test Dictionnaire : ERREUR");
//     }

//     // ============================
//     // TEST 4 : Plateau.EstVide
//     // ============================
//     static void Test_Plateau_EstVide()
//     {
//         Plateau p = new Plateau("TestPlateauVide.csv");

//         if (p.EstVide())
//             Console.WriteLine("Test EstVide : OK");
//         else
//             Console.WriteLine("Test EstVide : ERREUR");
//     }

//     // ============================
//     // TEST 5 : Plateau.Maj_Plateau
//     // ============================
//     static void Test_Plateau_MajPlateau()
//     {
//         Plateau p = new Plateau("TestPlateau.csv");

//         List<Position> positions = new List<Position>();
//         positions.Add(new Position(2, 0)); // on supprime une lettre

//         p.Maj_Plateau(positions);

//         if (!p.EstVide())
//             Console.WriteLine("Test Maj_Plateau : OK");
//         else
//             Console.WriteLine("Test Maj_Plateau : ERREUR");
//     }
// }
