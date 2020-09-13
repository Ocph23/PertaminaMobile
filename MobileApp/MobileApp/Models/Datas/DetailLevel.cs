using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models.Datas
{
    public class DetailLevel
    {

        public int Id { get; set; }

        public string Nama { get; set; }

        public double NilaiKaryawan { get; set; }

        public double NilaiPerusahaan { get; set; }

        public double Penambahan { get; set; }

        public int LevelId { get; set; }

        public bool Selected { get; set; }

        public virtual Level Level { get; set; }


    }
}