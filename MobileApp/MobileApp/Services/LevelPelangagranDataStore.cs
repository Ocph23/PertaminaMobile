using MobileApp.Models.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileApp.Services
{
    public class LevelPelangagranDataStore : IDataStore<Level>
    {

        List<Level> items;
        string controller = "/api/level";
        public LevelPelangagranDataStore()
        {
            //items = new List<Level>();
            //var level = new Level() { Id = 1, Nama = "Ringan" };
            //level.Datas = new List<DetailLevel>();
            //level.Datas.Add(new DetailLevel { Id = 1, LevelId = 1, Nama = "Tidak Pake Helm" });
            //level.Datas.Add(new DetailLevel { Id = 2, LevelId = 1, Nama = "Tidak Bawa kartu", Selected=true });
            //level.Datas.Add(new DetailLevel { Id = 3, LevelId = 1, Nama = "Tidak Bawa kartu" });
            //level.Datas.Add(new DetailLevel { Id = 4, LevelId = 1, Nama = "Tidak Bawa kartu" });
            //level.Datas.Add(new DetailLevel { Id = 5, LevelId = 1, Nama = "Tidak Bawa kartu" });

            //var level2 = new Level() { Id = 2, Nama = "Berat" };
            //level2.Datas = new List<DetailLevel>();
            //level2.Datas.Add(new DetailLevel { Id = 6, LevelId = 2, Nama = "Tidak Pake Helm", Selected=true });
            //level2.Datas.Add(new DetailLevel { Id = 7, LevelId = 2, Nama = "Tidak Bawa kartu"  });

            //items.Add(level);
            //items.Add(level2);
        }
        public async Task<bool> AddItemAsync(Level item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateItemAsync(Level item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Level> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == Convert.ToInt32(id)));
        }

        public async Task<IEnumerable<Level>> GetItemsAsync(bool forceRefresh = false)
        {
            if (items == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync("/api/level");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Level>>(resultString);
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
