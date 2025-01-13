namespace MauiAndroidFS2;

public partial class MainPage : ContentPage
{
    int count = 0;
    string Message = string.Empty;


    public MainPage()
    {
        InitializeComponent();
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
#if ANDROID
    if (!MauiAndroidFS2.Platforms.Android.AndroidServiceManager.IsRunning) 
    {
        MauiAndroidFS2.Platforms.Android.AndroidServiceManager.StartFService();
        Message = "Service has started";
    }
    else
    {
        Message = "Service is running";
    }
#endif
    }

    private void StopService(object sender, EventArgs e)
    {
#if ANDROID
    MauiAndroidFS2.Platforms.Android.AndroidServiceManager.StopFService();
    Message = "Service has stopped";
#endif
    }
}
