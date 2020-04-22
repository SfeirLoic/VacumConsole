using System;
using System.IO;

namespace Vacum
{
    internal class Picture : Chapitre
    {
        public String PicUrl { get; set; }
        public String PicFullName { get; set; }
        public String PicNameNoExt { get; set; }
        public String PicExt { get; set; }
        public String PicPath { get; set; }


        public Picture(string imgUrl)
        {
            PicUrl = imgUrl;
            PicFullName = Path.GetFileName(imgUrl);
            PicNameNoExt = Path.GetFileNameWithoutExtension(imgUrl);
            PicExt = Path.GetExtension(imgUrl);
            PicPath = "";
        }
    }
}