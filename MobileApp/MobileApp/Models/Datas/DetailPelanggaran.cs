using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models.Datas
{
    public class DetailPelanggaran
    {
        public int Id { get; set; }
        public double NilaiKaryawan { get; set; }
        public double NilaiPerusahaan { get; set; }
        public double Penambahan { get; set; }
        public int DetailLevelId { get; set; }
        public virtual DetailLevel DetailLevel { get; set; }

    }
}
