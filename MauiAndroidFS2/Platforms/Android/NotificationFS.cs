using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;

namespace MauiAndroidFS2.Platforms.Android;

[Service(ForegroundServiceType = global::Android.Content.PM.ForegroundService.TypeSpecialUse)]
internal class NotificationFS : Service
{
    Timer _timer = null;
    int _id = (new object()).GetHashCode();
    int BadgeNumber = 0;

    public override IBinder? OnBind(Intent? intent)
    {
        return null;
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent,
        [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        var input = intent.GetStringExtra("inputExtra");

        var notificationIntent = new Intent(this, typeof(MainActivity));
        var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent,
            PendingIntentFlags.Immutable);

        var notification = new NotificationCompat.Builder(this,
            MainActivity.ChannelId)
            .SetContentText(input)
            .SetSmallIcon(Resource.Drawable.AppIcon)
            .SetContentIntent(pendingIntent);

        _timer = new Timer(TimerElapsed, notification, 0, 10000);

        //StartForeground(_id, notification.Build());

        return StartCommandResult.Sticky;
    }

    void TimerElapsed(object state)
    {
        BadgeNumber++;

        string timeString = $"Time: {DateTime.Now.ToLongTimeString()}";
        var notification = (NotificationCompat.Builder)state;
        notification.SetNumber(BadgeNumber);
        notification.SetContentTitle(timeString);
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
        {
            StartForeground(_id, notification.Build(), ForegroundService.TypeSpecialUse);
        }
        else
        {
            StartForeground(_id, notification.Build());
        }
    }
}
