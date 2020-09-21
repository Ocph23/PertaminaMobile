using MobileApp.Models.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    public class AbsenDataStore : IDataStore<Absen>
    {

        List<Absen> items;
        private readonly string controller = "/api/datakaryawan";
        private readonly string controllerAbsen = "/api/absen";



        public async Task<bool> AddItemAsync(Absen item)
        {
            using (var client = new RestService())
            {
                try
                {
                   var result =await client.PostAsync(controllerAbsen,client.GenerateHttpContent(item));
                    if (result.IsSuccessStatusCode)
                    {
                        Helper.InfoMessage("Anda Berhasil Absen");
                        if(items==null)
                            items = new List<Absen>();
                        items.Add(item);
                        return await Task.FromResult(true);
                    }
                    else
                    {
                        throw new SystemException(await client.Error(result));
                    }
                }
                catch (Exception ex)
                {
                    throw new SystemException(ex.Message);
                }
            }
        }

        public async Task<bool> UpdateItemAsync(Absen item)
        {
            var oldAbsen = items.Where((Absen arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldAbsen);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldAbsen = items.Where((Absen arg) => arg.Id == Convert.ToInt32(id)).FirstOrDefault();
            items.Remove(oldAbsen);

            return await Task.FromResult(true);
        }

        public async Task<Absen> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        public async Task<IEnumerable<Absen>> GetItemsAsync(bool forceRefresh = false)
        {
            if (items == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync(controller + "/absen");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Absen>>(resultString);
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
