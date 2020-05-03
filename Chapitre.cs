using System;
using System.Collections.Generic;
using System.Linq;

namespace Vacum
{
    public class Chapitre : Manga
    {
        private String _chapTitleUrl;

        public String ChapTitleUrl
        {
            get { return _chapTitleUrl; }
            set
            {
                _chapTitleUrl = value;
                ChapTitleClean = Outils.cleanString(_chapTitleUrl).Replace(".",",");
            }
        }
        public String ChapTitleClean { get; private set; }

        public List<Picture> ChapPicLst { get; set; }
        public String ChapUrl { get; set; }
        public String ChapPath { get; set; }

        public int ChapNumber { get; set; }
        public int ChapNbrPage { get; set; }
        public int ChapNbrPageAlreadyDled { get; set; }

        public bool chapAlreadyDled { get; set; }

        public Chapitre(Manga manga, string chapTitleUrl, string chapUrl)
        {
            MangaPath = manga.MangaPath;
            ChapTitleUrl = chapTitleUrl;
            ChapUrl = chapUrl;
            ChapPicLst = new List<Picture>();
            ChapPath = manga.MangaPath + "/" + ChapTitleClean;
            int.TryParse(chapTitleUrl.Split(" ").Last(), out int i);
            ChapNumber = i;
            ChapNbrPage = 0;
            ChapNbrPageAlreadyDled = 0;
            chapAlreadyDled = false;
            if (ChapNumber > HigherChap)
                HigherChap = ChapNumber;
        }
        public Chapitre() { }
    }
}