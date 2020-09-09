using System;
using System.Collections.Generic;

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

    }
}