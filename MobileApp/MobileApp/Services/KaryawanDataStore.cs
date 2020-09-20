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

    public interface IKaraywanDataStore<T> : IDataStore<T>
    {
        Task<DataScore> GetDataScore(int karyawanId);
    }


    public class KaryawanDataStore : IKaraywanDataStore<Karyawan>
    {

       private List<Karyawan> items;
        private DataScore _score;

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
            using (var client = new RestService())
            {
                var result = await client.PutAsync($"/api/karyawan/{item.Id}", client.GenerateHttpContent(item));
                if (result.IsSuccessStatusCode)
                {
                    var resultString = await result.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Karyawan>(resultString);
                    if (item != null)
                        return true;
                    throw new SystemException("Data tidak berhasil diubah !");
                }
                else
                {
                    throw new SystemException(await client.Error(result));
                }
            }
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

        public async Task<DataScore> GetDataScore(int karyawanId)
        {
            if (_score == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync($"/api/karyawan/pointbykaryawanid/{karyawanId}");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        _score = JsonConvert.DeserializeObject<DataScore>(resultString);
                        return _score;
                    }
                    else
                    {
                        throw new SystemException(await client.Error(result));
                    }
                }
            }
            else
                return _score;
        }
    }


    public class DataScore
    {
        public double Score { get; set; }
        public double Potongan { get; set; }
        public double Pengaduan{ get; set; }
    }
}
