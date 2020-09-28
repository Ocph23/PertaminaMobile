using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MobileApp.Models.Datas
{
   public class Kejadian
    {
        public int Id { get; set; }
        public string Deskripsi { get; set; }
        public DateTime Waktu { get; set; }

        public int PelaporId { get; set; }
        public Karyawan Pelapor { get; set; }

        public ObservableCollection<BuktiKejadian> Files { get; set; } = new ObservableCollection<BuktiKejadian>();
    }
}
