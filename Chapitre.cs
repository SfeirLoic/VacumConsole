using System;
using System.Collections.Generic;

namespace Vacum
{
    internal class Chapitre : Manga
    {
        private String _chapTitleUrl;

        public String ChapTitleUrl
        {
            get { return _chapTitleUrl; }
            set
            {
                _chapTitleUrl = value;
                ChapTitleClean = Outils.cleanString(_chapTitleUrl);
            }
        }
        public String ChapTitleClean { get; private set; }

        public List<Picture> ChapPicLst { get; set; }
        public String ChapUrl { get; set; }
        public String ChapPath { get; set; }
        public int ChapNbrPage { get; set; }

        public Chapitre()
        {
            ChapTitleUrl = "";
            ChapUrl = "";
            ChapPicLst = new List<Picture>();
            ChapPath = "";
            ChapNbrPage = 0;
        }
        public Chapitre(string titreChapUrl, string urlChap)
        {
            ChapTitleUrl = titreChapUrl;
            ChapUrl = urlChap;
            ChapPicLst = new List<Picture>();
            ChapPath = "";
            ChapNbrPage = 0;
        }
        public Chapitre(string titreChapUrl, string urlChap, List<Picture> picLstChap)
        {
            ChapTitleUrl = titreChapUrl;
            ChapUrl = urlChap;
            ChapPicLst = picLstChap;
            ChapPath = "";
            ChapNbrPage = 0;
        }
    }
}