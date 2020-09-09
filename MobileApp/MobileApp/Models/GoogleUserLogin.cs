using MobileApp.Models.Datas;
using System;
using System.Collections.Generic;
using System.Text;

namespace MobileApp.Models
{
    public class UserProfile
    {
        public  string ServerAuthCode { get; set; }
        public  Uri PhotoUrl { get; set;}
        public  bool IsExpired { get; set;}
        public  string IdToken { get; set;}
        public  string Id { get; set;}
        public  string GivenName { get; set;}
        public  string FamilyName { get; set;}
        public  string DisplayName { get; set;}
        public  string Email { get; set;}


    }

    public class Profile
    {
        public string UserName{ get; set; }
        public UserIdentity User{ get; set; }
        public List<string> Roles{ get; set; }
        public Karyawan Karyawan { get; set; }


    }


    public class UserIdentity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }

    public enum AuthProvider
    {
        UserAndPassword,
        Google,
        Facebook, 
        Windows
    }
}
