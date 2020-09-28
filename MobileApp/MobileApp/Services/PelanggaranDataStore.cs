using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MobileApp.Models.Datas;

namespace MobileApp.Services
{
    public interface IPelanggaranDataStore<T>:IDataStore<T>
    {
        Task<IEnumerable<T>> GetItemsmelaporkanAsync(bool forceRefresh = false);
        Task<IEnumerable<Kejadian>> GetItemsKejadianAsync(bool forceRefresh = false);
    }


    public class PelanggaranDataStore : IPelanggaranDataStore<Pelanggaran>
    {

        List<Pelanggaran> items;
        List<Pelanggaran> melaporkan;
        List<Kejadian> kejadian;
        string controller = "/api/datakaryawan";

        public PelanggaranDataStore()
        {
           
        }

        public async Task<bool> AddItemAsync(Pelanggaran item)
        {
            try
            {
                using (var client = new RestService())
                {
                    var response = await client.PostAsync("api/pelanggaran", client.GenerateHttpContent(item));
                    if (response.IsSuccessStatusCode)
                    {
                        var stringResponse = await response.Content.ReadAsStringAsync();
                        var pelanggaran = Newtonsoft.Json.JsonConvert.DeserializeObject<Pelanggaran>(stringResponse);
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

        public async Task<bool> UpdateItemAsync(Pelanggaran item)
        {
            var oldPelanggaran= items.Where((Pelanggaran arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldPelanggaran);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldPelanggaran= items.Where((Pelanggaran arg) => arg.Id == Convert.ToInt32(id)).FirstOrDefault();
            items.Remove(oldPelanggaran);

            return await Task.FromResult(true);
        }

        public async Task<Pelanggaran> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        public async Task<IEnumerable<Pelanggaran>> GetItemsAsync(bool forceRefresh = false)
        {
            if (items == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync(controller+"/pelanggaran");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pelanggaran>>(resultString);
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

        public async Task<IEnumerable<Pelanggaran>> GetItemsmelaporkanAsync(bool forceRefresh = false)
        {
            if (melaporkan == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync(controller + "/pelaporan");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        melaporkan= Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pelanggaran>>(resultString);
                        return melaporkan;
                    }
                    else
                    {
                        throw new SystemException(await client.Error(result));
                    }
                }
            }
            else
            {
                return melaporkan;
            }
        }

        public async Task<IEnumerable<Kejadian>> GetItemsKejadianAsync(bool forceRefresh = false)
        {
            if (kejadian == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync(controller + "/pelaporan");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        kejadian= Newtonsoft.Json.JsonConvert.DeserializeObject<List<Kejadian>>(resultString);
                        return kejadian;
                    }
                    else
                    {
                        throw new SystemException(await client.Error(result));
                    }
                }
            }
            else
            {
                return kejadian;
            }
        }
    }
}
