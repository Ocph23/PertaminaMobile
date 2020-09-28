using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MobileApp.Models.Datas;

namespace MobileApp.Services
{
    public class KejadianDataStore : IDataStore<Kejadian>
    {

        List<Kejadian> items;
        string controller = "/api/kejadian";

        public KejadianDataStore()
        {
            items = new List<Kejadian>() { new Kejadian { Deskripsi="Jalan Rusah", Pelapor= new Karyawan { NamaKaryawan="Budi" }, Waktu=DateTime.Now, Id=1 } };
        }

        public async Task<bool> AddItemAsync(Kejadian item)
        {
            try
            {
                using (var client = new RestService())
                {
                    var response = await client.PostAsync(controller, client.GenerateHttpContent(item));
                    if (response.IsSuccessStatusCode)
                    {
                        var stringResponse = await response.Content.ReadAsStringAsync();
                        var pelanggaran = Newtonsoft.Json.JsonConvert.DeserializeObject<Kejadian>(stringResponse);
                        if (pelanggaran != null)
                        {
                            items.Add(pelanggaran);
                        }
                        return true;
                    }
                    else
                    {
                        throw new SystemException( await client.Error(response));
                        
                    }

                }
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }

        public async Task<bool> UpdateItemAsync(Kejadian item)
        {
            var oldKejadian= items.Where((Kejadian arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldKejadian);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldKejadian= items.Where((Kejadian arg) => arg.Id == Convert.ToInt32(id)).FirstOrDefault();
            items.Remove(oldKejadian);

            return await Task.FromResult(true);
        }

        public async Task<Kejadian> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        public async Task<IEnumerable<Kejadian>> GetItemsAsync(bool forceRefresh = false)
        {
            if (items == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync(controller);
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Kejadian>>(resultString);
                        return items;
                    }
                    else
                    {
                        throw new SystemException(await client.Error(result));
                    }
                }
            }else
            {
                return items;
            }
        }

    }
}
