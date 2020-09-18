using System;
namespace MobileApp.Models.Datas
{
    public class Notification
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
