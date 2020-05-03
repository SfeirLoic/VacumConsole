using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace Vacum
{
    public static class Outils
    {
        /// <summary>
        /// Permet de nettoyer une chaine de caractère
        /// </summary>
        public static string cleanString(String thing)
        {
            //Attention, ne pas supprimer "." car extensions, remplacement de , pour les chapitres intermédiaires etc ....
            String thingClean = thing.Replace("-", " ")
                                     .Replace("_", " ")
                                     .Replace("*", " ")
                                     .Replace("&#039;", "'")
                                     ;
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            return myTI.ToTitleCase(thingClean);
        }

        /// <summary>
        /// Permet de savoir si une URL est valide ou non
        /// </summary>
        public static bool testUrl(string urlManga)
        {
            bool answer = false;
            Uri ourUri = new Uri(urlManga);
            //On test si l'url est atteignable 
            try
            {
                WebRequest myWebRequest = WebRequest.Create(urlManga);
                WebResponse myWebResponse = myWebRequest.GetResponse();
                // Use "ResponseUri" property to get the actual Uri from where the response was attained.
                answer = ourUri.Equals(myWebResponse.ResponseUri) ? true : false;
                myWebResponse.Close();
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                        Menu.Menu.next("404: Titre mal tapé ou la page n'existe plus");
                    else
                        Menu.Menu.next(resp.StatusCode.ToString());
                }
            }

            return answer;
        }

        /// <summary>
        /// Télécharge un fichier depuis l'url à l'adresse local en paramètre
        /// </summary>
        public static void downloadFile(String url, String path)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(url, path);
            }
            catch (Exception e) { 
                Console.WriteLine("Error {0]", e.Message); 
            }
        }

        internal static void zipAndDel(Manga m1)
        {
            foreach (Chapitre chap in m1.MangaChapToDlLst)
            {
                string startPath = chap.ChapPath;
                string zipPath = chap.ChapPath + @".cbr";
                ZipFile.CreateFromDirectory(startPath, zipPath);
                Directory.Delete(chap.ChapPath, true);
            }
        }

    }
}
