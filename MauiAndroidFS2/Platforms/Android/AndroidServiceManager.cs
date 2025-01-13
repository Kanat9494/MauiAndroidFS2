namespace MauiAndroidFS2.Platforms.Android;

public static class AndroidServiceManager
{
    static AndroidServiceManager()
    {
        _connectivity = Connectivity.Current;
        _connectivity.ConnectivityChanged += ConnectivityChanged;
    }

    private static IConnectivity _connectivity;
    public static MainActivity MainActivity { get; set; }
    public static bool IsRunning { get; set; }

    public static void StartFService()
    {
        AndroidServiceManager.IsRunning = true;
        if (MainActivity == null) return;
        MainActivity.StartService();
    }

    public static void StopFService()
    {
        if (MainActivity == null) return;
        MainActivity.StopService();
        IsRunning = false;
    }

    private static void ConnectivityChanged(object? sender, ConnectivityChangedEventArgs? e)
    {
        if (e?.NetworkAccess == NetworkAccess.Internet)
        {
            if (!IsRunning)
            {
                StartFService();
            }
        }
        else
        {
            if (IsRunning)
            {
                StopFService();
            }
        }
    }
}
