using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Vacum
{
    public class Manga : IComparable
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

        public int HigherChap { get; set; }

        public Manga(){
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
            HigherChap = 0;
        }

        /// <summary>
        /// a.compareTo(b) => [-1, a < b][0, a=b][1, a>b]
        /// 1 < a = A < b
        /// </summary>
        /// <param name="obj"></param>
        public int CompareTo(object obj)
        {
            Manga manga = (Manga)obj;
            return String.Compare(MangaTitleUrl, manga.MangaTitleUrl, true);
        }
    }
}