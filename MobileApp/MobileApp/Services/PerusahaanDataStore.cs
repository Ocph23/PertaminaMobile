using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MobileApp.Models.Datas;

namespace MobileApp.Services
{
    public class PerusahaanDataStore : IDataStore<Perusahaan>
    {

        readonly List<Perusahaan> items;

        public PerusahaanDataStore()
        {
            items = new List<Perusahaan>()
            {
                new Perusahaan{ Id = 1,  Nama="PT. Andi Jaya"}
            };
        }

        public async Task<bool> AddItemAsync(Perusahaan item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Perusahaan item)
        {
            var oldPerusahaan= items.Where((Perusahaan arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldPerusahaan);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldPerusahaan= items.Where((Perusahaan arg) => arg.Id == Convert.ToInt32(id)).FirstOrDefault();
            items.Remove(oldPerusahaan);

            return await Task.FromResult(true);
        }

        public async Task<Perusahaan> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        public async Task<IEnumerable<Perusahaan>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
