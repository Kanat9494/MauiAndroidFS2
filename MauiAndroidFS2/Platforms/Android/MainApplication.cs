using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using AndroidX.Core.Content;

namespace MauiAndroidFS2;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    //public static readonly string ChannelId = "backgroundServiceChannel";

    //public override void OnCreate()
    //{
    //    base.OnCreate();

    //    try
    //    {
    //        if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
    //        {
    //            if (ContextCompat.CheckSelfPermission(this, Android.Manifest.Permission.PostNotifications)
    //                != Permission.Granted)
    //            {
    //                //ActivityCompat.RequestPermissions(this, new[] { Android.Manifest.Permission.PostNotifications }, 1);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("Ошибка при запросе разрешения на уведомления: " + ex.Message);
    //    }

    //    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
    //    {
    //        var serviceChannel = new NotificationChannel(ChannelId,
    //            "Background Service Channel",
    //            NotificationImportance.High);

    //        if (GetSystemService(NotificationService) is NotificationManager manager)
    //        {
    //            manager.CreateNotificationChannel(serviceChannel);
    //        }
    //    }
    //}
}
