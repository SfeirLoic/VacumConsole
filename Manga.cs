using System;
using System.Collections.Generic;

namespace Vacum
{
    internal class Manga
    {
        //Variables cachés
        private String _mangaTitleUrl;

        public String MangaTitleUrl {
            get { return _mangaTitleUrl; }
            set { 
                _mangaTitleUrl = value;
                MangaTitleClean = Outils.cleanString(_mangaTitleUrl);
            }
        }

        public String MangaTitleClean { get; private set; }

        public String MangaUrl { get; set; }
        public String MangaPath { get; set; }
        public List<Chapitre> MangaChapCompleteLst { get; set; }
        public int MangaNbrPagesTot { get; set; }
        public List<Chapitre> MangaChapToDlLst { get; set; }
        public int MangaNbrPagesToDl { get; set; }

        public Manga()
        {
            MangaTitleUrl = "";
            MangaUrl = "";
            MangaPath = "";
            MangaChapCompleteLst = new List<Chapitre>();
            MangaNbrPagesTot = 0;
            MangaChapToDlLst = new List<Chapitre>();
            MangaNbrPagesToDl = 0;
        }
        public Manga(string titleUrl)
        {
            MangaTitleUrl = titleUrl;
            MangaUrl = "";
            MangaPath = "";
            MangaChapCompleteLst = new List<Chapitre>();
            MangaNbrPagesTot = 0;
            MangaChapToDlLst = new List<Chapitre>();
            MangaNbrPagesToDl = 0;
        }
        public Manga(string titleUrl, string url)
        {
            MangaTitleUrl = titleUrl;
            MangaUrl = url;
            MangaPath = "";
            MangaChapCompleteLst = new List<Chapitre>();
            MangaNbrPagesTot = 0;
            MangaChapToDlLst = new List<Chapitre>();
            MangaNbrPagesToDl = 0;
        }

        public Manga(string titleUrl, string url, List<Chapitre> chapterLst)
        {
            MangaTitleUrl = titleUrl;
            MangaUrl = url;
            MangaPath = "";
            MangaChapCompleteLst = chapterLst;
            MangaNbrPagesTot = 0;
            MangaChapToDlLst = new List<Chapitre>();
            MangaNbrPagesToDl = 0;
        }

    }
}