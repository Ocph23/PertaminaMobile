using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using MobileApp.Models.Datas;
using Xamarin.Forms;

namespace MobileApp.Services
{
    public class NotificationDataStore : IDataStore<Notification>,INotification
    {

        List<Notification> list;
        public NotificationDataStore()
        {
            list = new List<Notification> {
            new Notification { Created=DateTime.Now, Id=1, Message="Test Message", Topic="Pelanggaran", Sender="Keuangan" },
            new Notification { Created=DateTime.Now, Id=2, Message="its long Messange its long Messange its long Messange its long Messange its long Messange its long Messange its long Messange its long Messange its long Messange its long Messange its long Messange ", Topic="Pelanggaran", Sender="System" },
            new Notification { Created=DateTime.Now, Id=3, Message="Test Message", Topic="Pelanggaran", Sender="Admin" },
            new Notification { Created=DateTime.Now, Id=4, Message="Test Message", Topic="Pelanggaran", Sender="HRD" },
            new Notification { Created=DateTime.Now, Id=5, Message="Test Message", Topic="Pelanggaran", Sender="Apakek" },
            };
          
        }
       
        public Task<bool> AddItemAsync(Notification item)
        {
            try
            {
                if (list == null)
                    list = new List<Notification>();
                list.Add(item);
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
            await Task.Delay(100);
            if(list==null || forceRefresh)
            {
                return null;
            }

            return list;
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
