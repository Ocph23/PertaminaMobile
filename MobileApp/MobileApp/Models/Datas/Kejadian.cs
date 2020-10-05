using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MobileApp.Models.Datas
{
   public class Kejadian
    {
        public int Id { get; set; }
        public string Deskripsi { get; set; }
        public DateTime Waktu { get; set; }

        public int PelaporId { get; set; }
        public Karyawan Pelapor { get; set; }

        public ObservableCollection<BuktiKejadian> Files { get; set; } = new ObservableCollection<BuktiKejadian>();

        [Newtonsoft.Json.JsonIgnore]
        public string DeskripsiShort { get {
                if (Deskripsi.Length > 50)
                    return Deskripsi.Substring(1, 50) + " ......";
                return Deskripsi;
            } }
        [Newtonsoft.Json.JsonIgnore]
        public string Photo
        {
            get
            {
                if (Files != null && Files.Count > 0)
                {
                    var images = Files[0];
                    if (images.FileType.ToLower().Contains("video"))
                        return "pngegg.png";
                    return $"{Helper.Url}/bukti/thumbs/{images.Thumb}";
                }
                else
                    return $"noimage.png";
            }
        }
    }
}
