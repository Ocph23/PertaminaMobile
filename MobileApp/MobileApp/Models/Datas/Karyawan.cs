using System.Linq;
using System.Collections.Generic;


namespace MobileApp.Models.Datas
{
    public class Karyawan
    {

        public int Id { get; set; }
        public string KodeKaryawan { get; set; }

        public string NamaKaryawan { get; set; }

        public string Alamat { get; set; }

        public string Kontak { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }

        public string Photo { get; set; }

        public bool Status { get; set; } = true;

        public byte[] DataPhoto { get; set; }

        public PerusahaanKaryawan Perusahaan
        {
            get
            {
                if (_perusahaan != null)
                {
                    return _perusahaan;
                }

                if (Perusahaans != null && Perusahaans.Count > 0)
                {
                    return Perusahaans.Last();
                }

                return _perusahaan;
            }
            set { _perusahaan = value; }
        }
        public ICollection<Pelanggaran> Pelanggarans { get; set; }
        public ICollection<Absen> Absens { get; set; }
        public virtual ICollection<PerusahaanKaryawan> Perusahaans { get; set; } = new List<PerusahaanKaryawan>();

        public List<string> Roles { get; set; }

        private PerusahaanKaryawan _perusahaan;
    }
}