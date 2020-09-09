using System;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Com.Airbnb.Lottie;

namespace MobileApp.Droid
{
    [Activity(Label = "SplashScreen", MainLauncher =false, Theme = "@style/Theme.Splash") ]
    public class SplashScreen : Activity, Animator.IAnimatorListener
    {
        private int loop;

        public void OnAnimationCancel(Animator animation)
        {
           
        }

        public void OnAnimationEnd(Animator animation)
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }

        public void OnAnimationRepeat(Animator animation)
        {
           
        }

        public void OnAnimationStart(Animator animation)
        {
          
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashScreen);
            LottieAnimationView lottie = FindViewById<LottieAnimationView>(Resource.Id.animation_view);
            lottie.AddAnimatorListener(this);
        }
    }
}