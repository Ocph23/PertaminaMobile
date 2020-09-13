using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MobileApp.Models.Datas;
using Newtonsoft.Json;

namespace MobileApp.Services
{
    public class KaryawanDataStore : IDataStore<Karyawan>
    {

       private List<Karyawan> items;

        public KaryawanDataStore()
        {
          
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
            if (items == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync("/api/karyawan" );
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        items = JsonConvert.DeserializeObject<List<Karyawan>>(resultString);
                        return items;
                    }
                    else
                    {
                        throw new SystemException(await client.Error(result));
                    }
                }
            }
            else
            {
                return items;
            }
        }
    }
}
