using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace MauiAndroidFS2.Platforms.Android;

[Service(ForegroundServiceType = global::Android.Content.PM.ForegroundService.TypeSpecialUse)]
internal class NotificationFS : Service
{
    Timer _timer = null;
    int _id = (new object()).GetHashCode();
    int BadgeNumber = 0;
    private bool _disposed = false;
    private static IConnectivity _connectivity;

    public override IBinder? OnBind(Intent? intent)
    {
        return null;
    }


    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent,
        [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        var input = intent.GetStringExtra("inputExtra");
        _disposed = false;
        _connectivity = Connectivity.Current;
        _connectivity.ConnectivityChanged += ConnectivityChanged;

        var notificationIntent = new Intent(this, typeof(MainActivity));
        var pendingIntent = PendingIntent.GetActivity(this, 0, notificationIntent,
            PendingIntentFlags.Immutable);

        var notification = new NotificationCompat.Builder(this,
            MainActivity.ChannelId)
            .SetContentText(input)
            .SetSmallIcon(Resource.Drawable.AppIcon)
            .SetContentIntent(pendingIntent);

        //_timer = new Timer(TimerElapsed, notification, 0, 10000);

        //StartForeground(_id, notification.Build());
        ConnectToRTS(notification);

        return StartCommandResult.Sticky;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _disposed = true;
        Console.WriteLine("Service destroyed and task canceled.");
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

    void ConnectToRTS(object state)
    {
        ulong senderId = 1;
        ulong receiverId = 5;
        ClientWebSocket clientWebSocket = new ClientWebSocket();
        var buffer = new byte[1024 * 4];
        string uri = $"ws://localhost:5021/api/Chats/ConnectToWS?userId={senderId}";
        WebSocketReceiveResult receiveResult;
        string jsonString;
        Message message;
        try
        {
            var receiveMessageTask = new Task(async () =>
            {
                await clientWebSocket.ConnectAsync(new Uri(uri), CancellationToken.None);
                byte[] data = new byte[1024 * 4];

                try
                {
                    while (!_disposed)
                    {
                        receiveResult = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(data), CancellationToken.None);
                        StringBuilder messageBuilder = new StringBuilder();
                        messageBuilder.Append(Encoding.UTF8.GetString(data, 0, receiveResult.Count));
                        message = JsonSerializer.Deserialize<Message>(messageBuilder.ToString());
                        Console.WriteLine($"Сообщение от пользователя: {message.SenderId} - {message.Content}");
                        var notification = (NotificationCompat.Builder)state;
                        notification.SetNumber(BadgeNumber);
                        //notification.SetContentTitle($"Сообщение от пользователя: {message.SenderId} - {message.Content}");
                        notification.SetContentTitle($"Сообщение от пользователя: {message.SenderId}");
                        notification.SetContentText(message.Content);
                        notification.SetPriority(NotificationCompat.PriorityHigh);
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
                catch (Exception ex)
                {

                }
                finally
                {
                    if (clientWebSocket.State == WebSocketState.Open)
                    {
                        await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Task stopped", CancellationToken.None);
                    }
                    clientWebSocket.Dispose();
                }
            });
            receiveMessageTask.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    private void ConnectivityChanged(object? sender, ConnectivityChangedEventArgs? e)
    {
        if (e?.NetworkAccess == NetworkAccess.Internet)
        {
            return;
        }
        else
        {
            _disposed = true;
        }
    }
}
