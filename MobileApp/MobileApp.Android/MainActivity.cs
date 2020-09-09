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

[assembly: Xamarin.Forms.Dependency(typeof(MainActivity))]
namespace MobileApp.Droid
{

    [Activity(Label = "MobileApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthService, IOnSuccessListener, IOnFailureListener
    {

        GoogleSignInOptions gso;
        private GoogleApiClient googleApiClient;
        private FirebaseAuth firebaseAuth;
        public static MainActivity MainActivityInstance { get; private set; }
        public UserProfile Profile { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
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


        public  System.Threading.Tasks.Task Login(AuthProvider provider)
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

      
    }
}