using System;
namespace MobileApp.Models.Datas
{
    public class Notification
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime Created { get; set; }
    }
}
