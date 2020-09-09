using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models.Datas
{

    public class Pemenang
    {
        public int Id { get; set; }

        public int PeriodeId { get; set; }
        public Periode Periode { get; set; }

        public int KaryawanId { get; set; }
        public Karyawan Karyawan{get;set;}

    }
}