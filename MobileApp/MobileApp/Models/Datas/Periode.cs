using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Models.Datas
{
    public class Periode
    {
        public int Id { get; set; }

        public DateTime Mulai { get; set; }

        public DateTime Selesai { get; set; }

        public DateTime Undian { get; set; }

        public bool Status { get; set; }


        public string DisplayName => $"{Mulai:dd/MM/yyyy} - {Selesai:dd/MM/yyyy}";

    }
}