using System;

namespace Vacum.Menu
{
    public static class Menu
    {
        /// <summary>
        /// Menu commun général
        /// </summary>
        public static void showMenuGeneral()
        {
            Console.Clear();
            Console.WriteLine(" ---------------------------------------------------- ");
            Console.WriteLine("|Faites votre choix en appuyant sur la bonne touche: |");
            Console.WriteLine(" ---------------------------------------------------- ");
            Console.WriteLine("|  1) LelScan                                        |");
            Console.WriteLine(" ---------------------------------------------------- ");
            Console.WriteLine("|  Q) Fermer le PGM                                  |");
            Console.WriteLine(" ---------------------------------------------------- ");
        }

        /// <summary>
        /// Menu spécifique à lelscan
        /// </summary>
        public static void showMenuLelScan()
        {
            Console.Clear();
            Console.WriteLine(" ---------------------------------------------------- ");
            Console.WriteLine("|Faites votre choix en appuyant sur la bonne touche: |");
            Console.WriteLine(" ---------------------------------------------------- ");
            Console.WriteLine("|  1) Récuperer la liste des mangas populaires       |");
            Console.WriteLine(" ---------------------------------------------------- ");
            Console.WriteLine("|  2) Via Titre ou url                               |");
            Console.WriteLine(" ---------------------------------------------------- ");
            Console.WriteLine("|  R) Retour                                         |");
            Console.WriteLine(" ---------------------------------------------------- ");
        }

        /// <summary>
        /// Menu commun de téléchargement par titre
        /// </summary>
        public static void showMenuByTitle()
        {
            Console.WriteLine(" --------------------------------------------------------------------------- ");
            Console.WriteLine("|Entrez le titre du manga tel qu'indiqué dans l'url puis appuyez sur Enter: |");
            Console.WriteLine("| pour test 'anima' ou 'https://www.lelscan-vf.com/manga/anima'             |");
            Console.WriteLine(" --------------------------------------------------------------------------- ");
            Console.WriteLine("|  R) Retour                                                                |");
            Console.WriteLine(" --------------------------------------------------------------------------- ");
        }

        /// <summary>
        /// Message commun à formater + appuyez sur une touche pour continuer
        /// </summary>
        /// <param name="msg"></param>
        public static void next(String msg)
        {
            Console.WriteLine(msg);
            Console.WriteLine("Appuyez sur une touche pour continuer");
            Console.ReadKey();
        }

        /// <summary>
        /// Confirmation que la lettre saisie est celle que l'on attend
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyConfirm"></param>
        /// <returns></returns>
        public static bool confirm(String msg, ConsoleKey keyConfirm)
        {
            Console.WriteLine(msg);
            if (Console.ReadKey(false).Key == keyConfirm)
                return true;
            return false;
        }
    }
}
