using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using MauiAndroidFS2.Platforms.Android;

namespace MauiAndroidFS2;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public MainActivity()
    {
        AndroidServiceManager.MainActivity = this;
    }

    public static readonly string ChannelId = "backgroundServiceChannel";


    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        try
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
            {
                if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications)
                    != Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.PostNotifications }, 1);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при запросе разрешения на уведомления: " + ex.Message);
        }

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var serviceChannel = new NotificationChannel(ChannelId,
                "Background Service Channel",
                NotificationImportance.High);

            if (GetSystemService(NotificationService) is NotificationManager manager)
            {
                manager.CreateNotificationChannel(serviceChannel);
            }
        }
    }

    public void StartService()
    {
        var serviceIntent = new Intent(this, typeof(NotificationFS));
        serviceIntent.PutExtra("inputExtra", "Background Service");
        StartService(serviceIntent);
    }

    public void StopService()
    {
        var serviceIntent = new Intent(this, typeof(NotificationFS));
        StopService(serviceIntent);
    }
}
