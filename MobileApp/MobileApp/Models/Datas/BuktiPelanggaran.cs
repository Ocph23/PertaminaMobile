
namespace MobileApp.Models.Datas
{

    public class BuktiPelanggaran
    {
        public int Id { get; set; }

        public int PelanggaranId { get; set; }

        public string FileType { get; set; }

        public string FileName { get; set; }

        public string Thumb { get; set; }


        public byte[] Data { get; set; }


    }
}