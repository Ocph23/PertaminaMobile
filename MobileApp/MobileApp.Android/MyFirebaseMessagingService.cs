using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Firebase.Messaging;
using MobileApp.Models;
using Xamarin.Forms;

namespace MobileApp.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService, MobileApp.Services.INotification
    {

        /*  public override void OnCreate()
          {
              base.OnCreate();
              if (MainActivity.IsBacground)
              {
                  var powerManager = (PowerManager)GetSystemService(PowerService);
                  var wakeLock = powerManager.NewWakeLock(WakeLockFlags.ScreenDim | WakeLockFlags.AcquireCausesWakeup, "Info Tsunami");
                  wakeLock.Acquire();
                  AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
                  Intent myIntent = new Intent(this, typeof(NotifyBroadcastReceived));
                  PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, myIntent, PendingIntentFlags.OneShot);
                  myIntent.SetFlags(ActivityFlags.ClearTop);
                  manager.Set(AlarmType.RtcWakeup, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), pendingIntent);
              }
          }*/

        public override void OnMessageReceived(RemoteMessage remoteMessahe)
        {
            var message = remoteMessahe.GetNotification();

            var created = Convert.ToInt64(remoteMessahe.Data.Where(x => x.Key == "Created").FirstOrDefault().Value);
            var msg = new Models.Datas.Notification()
            {
                Created = new DateTime(created),
                Sender = remoteMessahe.Data.Where(x=>x.Key.ToLower()=="sender").FirstOrDefault().Value,
                Title = message.Title,
                Body = message.Body,
                NotificationType=(NotificationType)Enum.Parse(typeof(NotificationType), remoteMessahe.Data.Where(x => x.Key == "NotificationType").FirstOrDefault().Value)
            };

            if (remoteMessahe.From== "/topics/all")
            {
                AlarmManager manager = (AlarmManager)GetSystemService(Context.AlarmService);
                Intent myIntent = new Intent(this, typeof(NotifyBroadcastReceived));
                myIntent.PutExtra("message", remoteMessahe.Data.Where(x => x.Key == "message").FirstOrDefault().Value);
                myIntent.PutExtra("role", remoteMessahe.Data.Where(x => x.Key == "role").FirstOrDefault().Value);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, myIntent, PendingIntentFlags.OneShot);
                myIntent.SetFlags(ActivityFlags.ClearTop);
                manager.Set(AlarmType.RtcWakeup, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), pendingIntent);
            }
            else
            {
                SendNotification(remoteMessahe.GetNotification().Title, remoteMessahe.GetNotification().Body, remoteMessahe.Data);
            }

          
           

            MessagingCenter.Send<Services.INotification, Models.Datas.Notification>(this, "notification", msg); 
            base.OnMessageReceived(remoteMessahe);
        }

        void SendNotification(string title, string messageBody, IDictionary<string, string> data)
        {
            var bigStyle = new NotificationCompat.BigTextStyle().BigText(messageBody);
            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            var context = MainActivity.MainActivityInstance;
            NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

            NotificationCompat.Builder builder = new NotificationCompat.Builder(context, MainActivity.CHANNEL_ID)
                //  .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(messageBody)
                .SetAutoCancel(true)
                .SetStyle(bigStyle)
                .SetPriority(NotificationCompat.PriorityMax)
                .SetSmallIcon(Resource.Drawable.xamarin_logo);

            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Lollipop)
            {
                builder.SetVisibility(NotificationCompat.VisibilityPublic);
            }

            Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(Helper.Url));
            PendingIntent pendingIntent = PendingIntent.GetActivity(context, MainActivity.NOTIFICATION_ID, intent, PendingIntentFlags.UpdateCurrent);



            builder.AddAction(Resource.Drawable.abc_ic_menu_overflow_material, "VIEW", pendingIntent);

            
            notificationManager.Notify(MainActivity.NOTIFICATION_ID, builder.Build());
        }

        public override void OnNewToken(string p0)
        {
            base.OnNewToken(p0);
        }
    }
}