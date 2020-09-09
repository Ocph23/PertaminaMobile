using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{

    public interface IMessageInfo
    {

    }
    public class MessagingCenterAlert
    {
        public string Title { get; internal set; }
        public string Message { get; internal set; }
        public string Cancel { get; internal set; }
    }
}
