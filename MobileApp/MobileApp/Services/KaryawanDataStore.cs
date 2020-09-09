using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MobileApp.Models.Datas;

namespace MobileApp.Services
{
    public class KaryawanDataStore : IDataStore<Karyawan>
    {

        readonly List<Karyawan> items;

        public KaryawanDataStore()
        {
            items = new List<Karyawan>()
            {
                new Karyawan{ Id = 1,  NamaKaryawan="Budi", KodeKaryawan="125251"},
                new Karyawan{ Id = 2,  NamaKaryawan="Antonius Sanjaya", KodeKaryawan="125252"},
                new Karyawan{ Id = 3,  NamaKaryawan="Chandra Budi", KodeKaryawan="125253"},
                new Karyawan{ Id = 4,  NamaKaryawan="Diamna Saja", KodeKaryawan="125254"}
            };
        }

        public async Task<bool> AddItemAsync(Karyawan item)
        {
            items.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Karyawan item)
        {
            var oldKaryawan= items.Where((Karyawan arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldKaryawan);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldKaryawan= items.Where((Karyawan arg) => arg.Id == Convert.ToInt32(id)).FirstOrDefault();
            items.Remove(oldKaryawan);

            return await Task.FromResult(true);
        }

        public async Task<Karyawan> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        public async Task<IEnumerable<Karyawan>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
