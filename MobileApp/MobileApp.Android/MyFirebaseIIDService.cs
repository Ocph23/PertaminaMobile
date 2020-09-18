using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;

namespace MobileApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [Obsolete]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";

        [Obsolete]
        public override void OnTokenRefresh()
        {
            SendRegistrationToServer(FirebaseInstanceId.Instance.Token, FirebaseInstanceId.Instance.Id);
        }

        private void SendRegistrationToServer(string token, string id)
        {
            Xamarin.Essentials.SecureStorage.SetAsync("DeviceId", id);
            Xamarin.Essentials.SecureStorage.SetAsync("DeviceToken", token);
        }
    }
}