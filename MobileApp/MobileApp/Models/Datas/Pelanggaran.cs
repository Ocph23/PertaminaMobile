using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileApp.Models.Datas
{
    public class Pelanggaran
    {
        public int Id { get; set; }
        public PelanggaranType Jenis { get; set; }
        public DateTime TanggalKejadian { get; set; }
        public int PerusahaanId { get; set; }
        public int TerlaporId { get; set; }
        public virtual Karyawan Terlapor { get; set; }
        public int PelaporId { get; set; }
        public virtual Karyawan Pelapor { get; set; }
        public DateTime Tanggal { get; set; }
        public string Deskripsi { get; set; }
        public StatusPelanggaran Status { get; set; }
        public virtual ICollection<DetailPelanggaran> ItemPelanggarans { get; set; }
        public virtual ICollection<BuktiPelanggaran> Files { get; set; }

        [Newtonsoft.Json.JsonIgnore]
        public string PelanggaranDisplay
        {
            get
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var item in ItemPelanggarans)
                {
                    if (item.DetailLevel != null)
                        sb.Append($"-{item.DetailLevel.Nama}\n");
                }

                return sb.ToString();
            }
        }



        [Newtonsoft.Json.JsonIgnore]
        public Uri Photo
        {
            get
            {
                if (Files != null && Files.Count > 0)
                {
                    var images = Files.FirstOrDefault();
                    return new Uri($"{Helper.Url}/bukti/thumbs/{images.Thumb}");
                } else
                    return new Uri($"noimage.png");
            }
        }

        [Newtonsoft.Json.JsonIgnore]

        public double TotalPengurangan
        {
            get
            {
                if (ItemPelanggarans != null)
                {
                    return ItemPelanggarans.Sum(x => x.NilaiKaryawan);
                }

                return 0;
            }
        }

    }
}