using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models.Datas
{
    public class Perusahaan
    {
        public int Id { get; set; }

        public string Nama { get; set; }

        public string Alamat { get; set; }

        public string Direktur { get; set; }

        public string Kontak { get; set; }

        public string Email { get; set; }

        public string Logo { get; set; }

        public byte[] DataPhoto { get; set; }

        public virtual ICollection<PerusahaanKaryawan> PerusahaansKaryawan { get; set; }

        public virtual ICollection<Pelanggaran> Pelanggarans { get; set; }


    }


}