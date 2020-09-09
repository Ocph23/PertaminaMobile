using System.Collections.Generic;

namespace MobileApp.Models.Datas
{
    public class Level
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<DetailLevel> Datas { get; set; }
    }
}