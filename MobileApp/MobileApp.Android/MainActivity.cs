using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Auth.Api;
using Firebase.Auth;
using Firebase;
using Android.Gms.Tasks;
using Android.Content.PM;
using MobileApp.Services;
using Android.Widget;
using Android.Content;
using MobileApp.Droid;
using Xamarin.Forms;
using MobileApp.Models;
using System;
using Plugin.Media;
using Android.Support.V4.App;
using Android.Gms.Common;

[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]
namespace MobileApp.Droid
{

    [Activity(Label = "MobileApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthService, IOnSuccessListener, IOnFailureListener
    {

        GoogleSignInOptions gso;
        private GoogleApiClient googleApiClient;
        private FirebaseAuth firebaseAuth;
        public static MainActivity MainActivityInstance { get; private set; }
        public static bool IsBackground { get; private set; }
        public static readonly string CHANNEL_ID = "ChannelId1";
        public static readonly int NOTIFICATION_ID = 100;
        public UserProfile Profile { get; set; }

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            await CrossMedia.Current.Initialize();


            Firebase.Messaging.FirebaseMessaging.Instance.SubscribeToTopic("all");
            CreateNotificationChannel();



            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
             .RequestIdToken("505524341439-7uau390b9vq18qb11r0lcuh3h9cdeiup.apps.googleusercontent.com")
             //.RequestId()
             .RequestEmail()
             //.RequestProfile()
             .Build();

            googleApiClient = new GoogleApiClient.Builder(this)
                           .AddApi(Auth.GOOGLE_SIGN_IN_API, gso).Build();
            googleApiClient.Connect();
            firebaseAuth = GetFirebaseAuth();

            MainActivityInstance = this;
            LoadApplication(new App());
        }

        private FirebaseAuth GetFirebaseAuth()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseAuth mAuth;

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("pertamina-emloyee-score")
                    .SetApplicationId("pertamina-emloyee-score")
                    .SetApiKey("AIzaSyCSteRJZJTHMUe3hVG3ORXH-BLVO_F_jeY")
                   .SetDatabaseUrl("https://pertamina-emloyee-score.firebaseio.com")
                    .SetStorageBucket("pertamina-emloyee-score.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(this, options);
                mAuth = FirebaseAuth.Instance;
            }
            else
            {
                mAuth = FirebaseAuth.Instance;
            }
            return mAuth;
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Android.Content.Intent intent)
        {
            base.OnNewIntent(intent);

        }



        public System.Threading.Tasks.Task Login(AuthProvider provider)
        {



            if (MainActivityInstance.firebaseAuth.CurrentUser != null)
            {
                MainActivityInstance.firebaseAuth.SignOut();
            }

            switch (provider)
            {
                case AuthProvider.UserAndPassword:
                    break;
                case AuthProvider.Google:
                    var intent = Auth.GoogleSignInApi.GetSignInIntent(MainActivityInstance.googleApiClient);
                    ((Activity)Forms.Context).StartActivityForResult(intent, 42);
                    break;
                case AuthProvider.Facebook:
                    break;
                case AuthProvider.Windows:
                    break;
                default:
                    break;
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            Toast.MakeText(this, resultCode.ToString(), ToastLength.Long).Show();
           if (requestCode == 42)
            {
                GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                if (result.IsSuccess)
                {
                    GoogleSignInAccount account = result.SignInAccount;
                    LoginWithFirebase(account);
                }
            }
        }

        private void LoginWithFirebase(GoogleSignInAccount account)
        {
            var credentials = GoogleAuthProvider.GetCredential(account.IdToken, null);
            firebaseAuth.SignInWithCredential(credentials).AddOnSuccessListener(this)
                .AddOnFailureListener(this);

            var datas = new UserProfile
            {
                PhotoUrl = new Uri(account.PhotoUrl.ToString()),
                DisplayName = account.DisplayName,
                Email = account.Email,
                 FamilyName=account.FamilyName,
                  GivenName=account.GivenName,
                   Id=account.Id, IdToken=account.IdToken, IsExpired=account.IsExpired, ServerAuthCode=account.ServerAuthCode
            };

            MessagingCenter.Send(datas, "UserLogin");
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            Toast.MakeText(this, "Login successful", ToastLength.Short).Show();
        }

        public void OnFailure(Java.Lang.Exception e)
        {
            Toast.MakeText(this, "Login Failed", ToastLength.Short).Show();
        }

        public bool IsPlayServicesAvailable()
        {
            string text = "";
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    text = GoogleApiAvailability.Instance.GetErrorString(resultCode);
                else
                {
                    text = "This device is not supported";
                    Finish();
                }
                Toast.MakeText(this, text, ToastLength.Long).Show();
                return false;
            }
            else
            {
                text = "Google Play Services is available.";
                Toast.MakeText(this, text, ToastLength.Long).Show();
                return true;
            }
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var bigStyle = new NotificationCompat.BigTextStyle().BigText("Pemberitahuan");
            NotificationManager notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, CHANNEL_ID.ToString())
                //  .SetContentIntent(pendingIntent)
                .SetContentTitle("Pertamina")
                .SetContentText("Pemberitahuan")
                .SetAutoCancel(true)
                .SetStyle(bigStyle)
                .SetPriority(NotificationCompat.PriorityMax)
                .SetSmallIcon(Resource.Drawable.xamarin_logo);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                builder.SetVisibility(NotificationCompat.VisibilityPublic);
            }

            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(""));
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.UpdateCurrent);

            builder.AddAction(Resource.Drawable.abc_ic_menu_overflow_material, "VIEW", pendingIntent);

            notificationManager.Notify(MainActivity.NOTIFICATION_ID, builder.Build());

        }

    }
}