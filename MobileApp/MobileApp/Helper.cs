using MobileApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MobileApp
{
    public class Helper
    {
        private static string url= "http://192.168.1.5/";
      //  private static string url= "https://192.168.1.10:44308/";

        public static string Url
        {
            get { return url; }
            set { url = value; }
        }

        public static UserProfile Account { get; set; } 
        public static Profile Profile{ get; set; } 


        public static void ErrorMessage(string message)
        {
            MessagingCenter.Send(new MessagingCenterAlert { Cancel = "OK", Message = message, Title = "Error" }, "message");
        }

    }
}
