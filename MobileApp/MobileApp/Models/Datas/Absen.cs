using System;

namespace MobileApp.Models.Datas
{
    public class Absen
    {
        public int Id { get; set; }

        public AbsenType AbsenType { get; set; }

        public DateTime? Masuk { get; set; }

        public DateTime? Pulang { get; set; }


        public string Deskripsi { get; set; }
        public int KaryawanId { get; set; }
        public virtual Karyawan Karyawan { get; set; }


        public string DisplayName => getDisplayName() ;

        private string getDisplayName()
        {
            if (Masuk != null && Pulang != null)
            {
                return $"{Masuk.Value.ToShortDateString()} - {Pulang.Value.ToShortDateString()}";
            }
            return string.Empty;
        }
    }
}