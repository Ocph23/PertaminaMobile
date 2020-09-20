using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Models.Datas;

namespace MobileApp.Services
{
    public class NotificationDataStore : IDataStore<Notification>,INotification
    {

        List<Notification> items;
       
        public Task<bool> AddItemAsync(Notification item)
        {
            try
            {
                if (items == null)
                    items = new List<Notification>();
                items.Add(item);
                return Task.FromResult(true);
            }
            catch 
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Notification> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Notification>> GetItemsAsync(bool forceRefresh = false)
        {
            if (items == null)
            {
                using (var client = new RestService())
                {
                    var result = await client.GetAsync("/api/user/notifications");
                    if (result.IsSuccessStatusCode)
                    {
                        var resultString = await result.Content.ReadAsStringAsync();
                        items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Notification>>(resultString);
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

        public Task<bool> UpdateItemAsync(Notification item)
        {
            throw new NotImplementedException();
        }
    }


    public interface INotification
    {

    }
}
