using HtmlAgilityPack;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Vacum
{
    /// <summary>
    /// Objet contenant les fonctions pour télécharger sur LelScan
    /// </summary>
    internal class LelScan
    {

        /**********   GET / SET  ***********/
        public HtmlDocument LelScanHtmlPage { get; set; }
        public HtmlWeb LelScanHtmlWeb { get; set; }
        public String LelScanUrlRoot { get; set; }
        public String LelScanUrl { get; set; }
        public String LelScanUrlMangaRoot { get; set; }

        /**********   fin GET / SET  ***********/


        /**********   Constructeurs ***********/
        /// <summary>
        /// Constructeur avec Url par deffaut
        /// </summary>
        public LelScan()
        {
            LelScanUrlRoot = "https://www.lelscan-vf.com";
            LelScanUrl = "";
            LelScanUrlMangaRoot = LelScanUrlRoot + "/manga";
            LelScanHtmlWeb = new HtmlWeb();
            LelScanHtmlPage = LelScanHtmlWeb.Load(LelScanUrlRoot);
        }
        /// <summary>
        /// Constructeur avec Url en param
        /// </summary>
        public LelScan(string url)
        {
            LelScanUrlRoot = "https://www.lelscan-vf.com";
            LelScanUrl = url;
            LelScanUrlMangaRoot = LelScanUrl + "/manga";
            LelScanHtmlWeb = new HtmlWeb();
            LelScanHtmlPage = LelScanHtmlWeb.Load(LelScanUrl);
        }
        /**********   Fin Constructeurs ***********/


        /**********   Méthodes ***********/
        /// <summary>
        /// TODO: lorsque d'autres sites seront fonctionnels, à voir ce qui est mutualisable
        /// Construit un objet manga complet à partir d'un titre tel que défini dans l'url
        /// </summary>
        /// <param name="manga"></param>
        /// <returns></returns>
        internal Manga getInfosManga(String titleUrl)
        {
            Manga manga = new Manga(titleUrl);
            manga.MangaUrl = LelScanUrlMangaRoot + "/" + manga.MangaTitleUrl;
            manga.MangaPath = Program._rootPath + "/" + manga.MangaTitleClean;
            //manga.MangaPath = Path.Combine(Program._rootPath, manga.MangaTitleClean);
            Console.WriteLine("Récupération des informations du manga {0}.", manga.MangaTitleClean);
            Console.WriteLine("Veuillez patienter ...");
            getChapsFromManga(manga);
            getChapsToDlList(manga);
            Console.WriteLine("   {0} chapitre(s) à télécharger / {1}, soit {2} page(s) / {3}.", manga.MangaChapToDlLst.Count, manga.MangaChapCompleteLst.Count , manga.MangaNbrPagesToDl, manga.MangaNbrPagesTot);
            return manga;
        }

        /// <summary>
        /// Complète l'objet Manga avec la liste des tous les chapitres trouvés
        /// </summary>
        /// <param name="manga"></param>
        /// <returns></returns>
        private void getChapsFromManga(Manga manga)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument mangaPage = web.Load(manga.MangaUrl);
            var chapters = mangaPage.DocumentNode.Descendants("h5").Where(u => u.HasAttributes);

            foreach (var c in chapters)
            {
                String ChapTitleUrl = c.Descendants("a").First().InnerText;
                String ChapUrl = c.Descendants("a").First().GetAttributeValue("href", null);
                Chapitre chap = new Chapitre(manga, ChapTitleUrl, ChapUrl);
                manga.MangaChapCompleteLst.Add(chap);
            }
        }

        /// <summary>
        /// Complète l'objet Manga avec la liste des tous les chapitres à télécharger
        /// </summary>
        /// <param name="manga"></param>
        private void getChapsToDlList(Manga manga)
        {
            //si le dossier existe, on à déjà téléchargé des choses pour ce manga
            //on va analyser pour exclure ce qu'on à déjà téléchargé
            //sinon on prends toute la liste
            if (Directory.Exists(manga.MangaPath))
            {
                //pour chaque chapitre on vérifie s'il existe ou pas
                foreach (var chap in manga.MangaChapCompleteLst)
                {
                    checkFicChap(chap);
                    //si le chapitre à déjà été rappatrié, on s'arrête la, on prends le partie pris de considérer que si le dossier / fichier existe, il est complet
                    if (!chap.chapAlreadyDled)
                    {
                        //Sinon, on récupère la liste des pages du chapitre et on ajoute le chapitre dans la liste à dl
                        getChapPages(chap);
                        manga.MangaChapToDlLst.Add(chap);
                        manga.MangaNbrPagesToDl += chap.ChapNbrPage;
                    }
                }
            }
            else
                manga.MangaChapToDlLst = manga.MangaChapCompleteLst;
        }

        private void checkFicChap(Chapitre chap)
        {
            //le chapitre à déjà été dl si:
            //- un dossier avec son nom existe
            if (Directory.Exists(chap.ChapPath))
            {
                chap.chapAlreadyDled = true;
            } 

            //- un fichier avec son nom existe
            if (File.Exists(chap.ChapPath))
            {
                chap.chapAlreadyDled = true;
            // il peut avoir été préfixé par un 0
            } else
            {
                var files = Directory.GetFiles(chap.MangaPath, "*.*");
                foreach (var f in files)
                {
                    //on prends le numero sans l'extension
                    int.TryParse(f.Split("\\").Last().Split(".").First().Split(" ").Last(), out int numChap);
                    if (numChap == chap.ChapNumber)
                    {
                        chap.chapAlreadyDled = true;
                        break;
                    }
                }
                ;
            }
        }

        /// <summary>
        /// Complète la liste de Picture d'un chapitre contenant l'ensemble des images de celui-ci.
        /// </summary>
        private void getChapPages(Chapitre chap)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument chapterPage = web.Load(chap.ChapUrl);
            var imgsUrl =
                from truc in chapterPage.DocumentNode.Descendants("img")
                let data = truc.GetAttributeValue("data-src", null)
                where data != null
                select data;
            foreach (var imgUrl in imgsUrl)
            {
                Picture pic = new Picture(imgUrl);
                pic.PicPath = chap.ChapPath + "/" + pic.PicFullName;
                chap.ChapPicLst.Add(pic);
            }
            chap.ChapNbrPage = chap.ChapPicLst.Count;
        }

        /// <summary>
        /// TODO: lorsque d'autres sites seront fonctionnels, à voir ce qui est mutualisable
        /// Construit un objet manga complet à partir d'un titre tel que défini dans l'url
        /// </summary>
        /// <param name="manga"></param>
        /// <returns></returns>
        internal Manga getInfosMangaThread(String titleUrl)
        {
            Manga manga = new Manga(titleUrl);
            manga.MangaUrl = LelScanUrlMangaRoot + "/" + manga.MangaTitleUrl;
            //manga.MangaPath = Path.Combine(Program._rootPath, manga.MangaTitleClean);
            manga.MangaPath = Program._rootPath + "/" + manga.MangaTitleClean;
            Console.WriteLine("Récupération des informations du manga {0}.", manga.MangaTitleClean);
            Console.WriteLine("Veuillez patienter ...");
            getChapsFromMangaThread(manga);
            getChapsToDlListThread(manga);
            Console.WriteLine("   {0} chapitre(s) à télécharger / {1}, soit {2} page(s) / {3}.", manga.MangaChapToDlLst.Count, manga.MangaChapCompleteLst.Count, manga.MangaNbrPagesToDl, manga.MangaNbrPagesTot);
            return manga;
        }

        /// <summary>
        /// Complète l'objet Manga avec la liste des tous les chapitres trouvés
        /// </summary>
        /// <param name="manga"></param>
        /// <returns></returns>
        private void getChapsFromMangaThread(Manga manga)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument mangaPage = web.Load(manga.MangaUrl);
            var chapters = mangaPage.DocumentNode.Descendants("h5").Where(u => u.HasAttributes);

            Parallel.ForEach(chapters, (c) =>
            //foreach (var c in chapters)
            {
                String ChapTitleUrl = c.Descendants("a").First().InnerText;
                String ChapUrl = c.Descendants("a").First().GetAttributeValue("href", null);
                Chapitre chap = new Chapitre(manga, ChapTitleUrl, ChapUrl);
                manga.MangaChapCompleteLst.Add(chap);
            //}
            });
        }

        /// <summary>
        /// Complète l'objet Manga avec la liste des tous les chapitres à télécharger
        /// </summary>
        /// <param name="manga"></param>
        private void getChapsToDlListThread(Manga manga)
        {
            //Parallel.ForEach(manga.MangaChapCompleteLst, (chap) =>
            foreach (Chapitre chap in manga.MangaChapCompleteLst)
            {
                int nbPicDled = 0;

                if (Directory.Exists(chap.ChapPath))
                    nbPicDled = Directory.GetFiles(chap.ChapPath, "*.*", SearchOption.AllDirectories).Length;
                getChapPagesThread(chap);
                manga.MangaNbrPagesTot += chap.ChapNbrPage;
                // si on à plus d'images à dl que ce qu'on à déjà, on mets le chap dans la liste des chaps à dl (pas parfait comme méthode, on peut avoir des images qui n'ont rien à voir dans notre dossier ...)
                if (nbPicDled < chap.ChapNbrPage)
                {
                    manga.MangaChapToDlLst.Add(chap);
                    manga.MangaNbrPagesToDl += chap.ChapNbrPage;
                }
            }
            //});
        }

        /// <summary>
        /// Complète la liste de Picture d'un chapitre contenant l'ensemble des images de celui-ci.
        /// </summary>
        private void getChapPagesThread(Chapitre chap)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument chapterPage = web.Load(chap.ChapUrl);
            var imgsUrl =
                from truc in chapterPage.DocumentNode.Descendants("img")
                let data = truc.GetAttributeValue("data-src", null)
                where data != null
                select data;
            Parallel.ForEach(imgsUrl, (imgUrl) =>
            {
                Picture pic = new Picture(imgUrl);
                pic.PicPath = chap.ChapPath + "/" + pic.PicFullName;
                chap.ChapPicLst.Add(pic);
            });
            chap.ChapNbrPage = chap.ChapPicLst.Count;
        }

        /// <summary>
        /// Téléchargement un manga à partir d'un objet Manga
        /// </summary>
        internal void dlMangaWebClient(Manga manga)
        {
            Console.WriteLine("Téléchargement en cours, veuillez partienter");
            // Création du chronomètre.
            //Stopwatch timeDl = new Stopwatch();
            // Démarrage du chronomètre.
            //timeDl.Start();
            DirectoryInfo mangaDir = Directory.CreateDirectory(manga.MangaPath);
            // on veux les télécharger dans l'ordre ascendant
            foreach (var c in manga.MangaChapToDlLst) dlChapWebClient(c);
            // Arrêt du chronomètre.
            //timeDl.Stop();
            // IHM.
            //Menu.Menu.next("Téléchargement terminé, durée du téléchargement: " + timeDl.Elapsed.TotalSeconds + " secondes");
        }

        /// <summary>
        /// Télécharge un chapitre à partir d'un objet Chapitre
        /// </summary>
        /// <param name="chap"></param>
        /// <returns></returns>
        internal void dlChapWebClient(Chapitre chap)
        {
            Console.WriteLine("Chapitre {0} en cours", chap.ChapTitleClean);
            DirectoryInfo chapDir = Directory.CreateDirectory(chap.ChapPath);
            foreach (var p in chap.ChapPicLst)
            {
                Outils.downloadFile(p.PicUrl, p.PicPath);
            }
        }

        /**********   Fin Méthodes ***********/
        /**********   Méthodes  obsolètes - non fonctionnelles ***********/
        /// <summary>
        /// KO / Récupère la liste de tous les mangas du site
        /// </summary>
        /// <returns></returns>
        /*internal List<Manga> getAllMangaList()
        {
            List<Manga> allManga = new List<Manga>();
            LelScanUrl = "https://www.lelscan-vf.com/manga-list";
            LelScanHtmlPage = LelScanHtmlWeb.Load(LelScanUrl);

            var nodeH5 = LelScanHtmlPage.DocumentNode.Descendants("h5");

            foreach (HtmlNode title in nodeH5)
            {
                String urlManga = title.FirstChild.GetAttributeValue("href", null);
                String titreManga = title.InnerText;
                Manga manga = new Manga(titreManga, urlManga);
                allManga.Add(manga);
            }

            return allManga;
        }*/

        /// <summary>
        /// Récupère la liste de tous les manga populaires de lelscan
        /// </summary>
        /// <returns></returns>
        /*internal List<Manga> getAllPopularMangaList()
        {
            List<Manga> allManga = new List<Manga>();

            //On récupère toutes les divs qui ont pour class manga-name
            var divs =
                from truc in LelScanHtmlPage.DocumentNode.Descendants("div")
                let classe = truc.GetAttributeValue("class", null)
                where classe == "manga-name"
                select truc;
            int i = 0;
            foreach (var div in divs)
            {
                var a = div.Descendants("a").First();
                String urlManga = a.GetAttributeValue("href", null);
                String titreManga = a.InnerText;
                Manga manga = new Manga(titreManga, urlManga);
                allManga.Add(manga);
                i++;
            }
            return allManga;
        }*/

        /// <summary>
        /// Télécharge la liste de manga en param à un chemin système
        /// </summary>
        /// <param name="mangaList"></param>
        /// <param name="rootPath"></param>
        /*internal void DLAllMangaWithUrlAndPath(List<Manga> mangaList, String rootPath)
        {
            foreach (Manga manga in mangaList)
            {
                String mangaPath = rootPath + "\\" + manga.MangaTitleUrl;
                //Création du répertoire du manga
                DirectoryInfo di = Directory.CreateDirectory(mangaPath);
                HtmlWeb web = new HtmlWeb();
                HtmlDocument mangaPage = web.Load(manga.MangaUrl);
                var lesChapitres = mangaPage.DocumentNode.Descendants("h5");
                int nbChap = 0;
                int nbPageTotal = 0;
                foreach (var leChapitre in lesChapitres)
                {
                    if (leChapitre.HasAttributes)
                    {
                        String titreChapitre = leChapitre.Descendants("a").First().InnerText;
                        String urlChapter = leChapitre.Descendants("a").First().GetAttributeValue("href", null);

                        Chapitre chap = new Chapitre(titreChapitre, urlChapter);
                        String chapPath = mangaPath + "\\" + chap.ChapTitleUrl;
                        nbPageTotal += this.DownloadChapter(chap, chapPath);
                        nbChap++;
                    }
                }
                Console.WriteLine("Pour {0}, il y a {1} chapitres, {2} pages", manga.MangaTitleUrl, nbChap, nbPageTotal);
            }
        }*/

        /// <summary>
        /// Télécharge une image à partir d'un objet image et d'un chemin système
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /*private async Task dlPicAsync(Picture image, String path)
        {
            String localFilename = image.PicExt;
            String urlImg = image.PicUrl;

            try
            {
                var httpClient = new HttpClient();
                byte[] imageBytes = await httpClient.GetByteArrayAsync(urlImg);
                string localPath = Path.Combine(path, localFilename);
                if (!File.Exists(localPath))
                    File.WriteAllBytes(localPath, imageBytes);
            }
            catch (Exception e) { Console.WriteLine("Error {0]", e.Message); }
        }*/
        /**********   Fin Méthodes  obsolètes - non fonctionnelles ***********/
    }
}