using System;

namespace MobileApp.Models.Datas
{
    public class PerusahaanKaryawan
    {
        public int Id { get; set; }
        public string Jabatan { get; set; }
        public DateTime MulaiKerja { get; set; }
        public DateTime? SelesaiKerja { get; set; }
        public bool Status { get; set; } = true;

        public int KaryawanId { get; set; }
        public virtual Karyawan Karyawan { get; set; }

        public int PerusahaanId { get; set; }
        public virtual Perusahaan Perusahaan { get; set; }


        public string SelesaiView
        {
            get
            {
                if(SelesaiKerja== null)
                {
                    return "Sekarang";
                }

                return SelesaiKerja.Value.ToString("dd MMMM yyyyy");
            }
        }


    }
}