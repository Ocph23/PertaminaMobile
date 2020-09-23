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
        public string DeviceId { get; set; }

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

        public string PhotoView
        {
            get
            {
                if(!string.IsNullOrEmpty(Photo))
                    return $"{Helper.Url}/images/profiles/{Photo}";
                return "noimage.png";
            }
        }
    }



    public interface IKaryawan
    {

        int Id { get; set; }
        string KodeKaryawan { get; set; }

        string NamaKaryawan { get; set; }

        string Alamat { get; set; }

        string Kontak { get; set; }

        string Email { get; set; }

        string UserId { get; set; }

        string Photo { get; set; }

        bool Status { get; set; }

        byte[] DataPhoto { get; set; }


    }
}