
using System.IO;
using Xamarin.Forms;

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

        [Newtonsoft.Json.JsonIgnore]
        public string ThumbView
        {
            get
            {
                return $"{Helper.Url}/bukti/thumbs/{Thumb}";
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public string FileView
        {
            get
            {
                return $"{Helper.Url}/bukti/images/{FileName}";
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public ImageSource DataView
        {
            get
            {

             if (Data == null || Data.Length <= 0)
                    return null;

              return ImageSource.FromStream(() => new MemoryStream(Data));
            }
        }


    }
}