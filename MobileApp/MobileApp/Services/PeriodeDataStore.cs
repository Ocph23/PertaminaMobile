using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MobileApp.Models.Datas;

namespace MobileApp.Services
{
    public class PeriodeDataStore : IDataStore<Periode>
    {
        private readonly string controller= "/api/periode";
        private List<Periode> items;

        public async Task<bool> AddItemAsync(Periode item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Periode item)
        {
            var oldPeriode= items.Where((Periode arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldPeriode);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldPeriode= items.Where((Periode arg) => arg.Id == Convert.ToInt32(id)).FirstOrDefault();
            items.Remove(oldPeriode);

            return await Task.FromResult(true);
        }

        public async Task<Periode> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        public async Task<IEnumerable<Periode>> GetItemsAsync(bool forceRefresh = false)
        {
            if (items == null || forceRefresh)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync(controller);
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Periode>>(resultString);
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
