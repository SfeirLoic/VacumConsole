using System;
using System.Diagnostics;
using System.IO;
using Vacum.Menu;

namespace Vacum
{
    class Program
    {
        public const String _rootPath = "C:/Manga";
        public static void Main(string[] args)
        {
            Menu.Menu.showMenuGeneral();
            while (true)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        Console.Clear();
                        lelScan();
                        Menu.Menu.showMenuGeneral();
                        break;
                    case ConsoleKey.Q:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("");
                        Console.WriteLine("Veuillez entrer une autre touche SVP");
                        break;
                }
            }
        }

        public static void lelScan()
        {
            bool sortie = false;
            Menu.Menu.showMenuLelScan();
            while (!sortie)
            {
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.NumPad1:
                    case ConsoleKey.D1:
                        break;
                    case ConsoleKey.NumPad2:
                    case ConsoleKey.D2:
                        LelScanDlWithTitleOrUrl();
                        Menu.Menu.showMenuLelScan();
                        break;
                    case ConsoleKey.R:
                        Console.Clear();
                        sortie = true;
                        break;
                    default:
                        Console.WriteLine("Veuillez entrer une autre touche SVP");
                        break;
                }
            }
        }

        /// <summary>
        /// Téléchargement sur LelScan, avec le titre du manga en paramètre
        /// </summary>
        private static void LelScanDlWithTitleOrUrl()
        {
            bool correctTitle = false;
            while (!correctTitle)
            {
                Console.Clear();
                Menu.Menu.showMenuByTitle();
                String titleUrl = Console.ReadLine().Trim().ToLower();
                LelScan lel = new LelScan();
                if (titleUrl == "r")
                    break;
                if (titleUrl.Contains(lel.LelScanUrlRoot))
                    titleUrl = Path.GetFileName(titleUrl);
                if (writeAndCheckTitle(titleUrl, lel))
                {
                    Manga manga = lel.getInfosManga(titleUrl);
                    if (manga.MangaNbrPagesToDl <= 0)
                        Menu.Menu.next("Vous possédez déjà l'intégralité des chapitres pour " + manga.MangaTitleUrl + ".");
                    else 
                        if (Menu.Menu.confirm("Voulez vous télécharger ce manga ? (Enter pour confirmer)", ConsoleKey.Enter)) lel.dlMangaWebClient(manga);
                }
            }
        }

        /// <summary>
        /// Vérifie que la saisie est correcte pour un titre de manga.
        /// </summary>
        /// <param name="titleUrl"></param>
        /// <returns></returns>
        private static bool writeAndCheckTitle(String titleUrl, LelScan lel)
        {
            if (titleUrl.Length <= 0) { Console.WriteLine("Le titre saisi ne peut être vide"); return false; }
            if (!Outils.testUrl(lel.LelScanUrlMangaRoot + "/" + titleUrl))
                return false;
            return true;
        }
    }
}


