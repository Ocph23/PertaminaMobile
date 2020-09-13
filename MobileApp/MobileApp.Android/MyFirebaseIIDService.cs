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
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            SendRegistrationToServer(FirebaseInstanceId.Instance);
        }

        private void SendRegistrationToServer(FirebaseInstanceId instance)
        {
            MobileApp.Services.RestService.DeviceToken = instance.Token;
            MobileApp.Services.RestService.DeviceID= instance.Id;
        }

    }
}