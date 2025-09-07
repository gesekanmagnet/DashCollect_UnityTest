using UnityEngine;

using Nakama;

public class Connector : MonoBehaviour
{
    public static Connector Instance { get; private set; }

    private string connectionStatus;

    public static IClient client { get; private set; }
    public static ISession session { get; private set; }

    private ISocket socket;

    private void Awake()
    {
        Instance = this;
    }

    private void UpdateStatus(string status)
    {
        connectionStatus = status;
        EventCallback.OnConnectStatus(status);
    }

    /// <summary>
    /// Create client and socket then connect to the session
    /// </summary>
    public async void Log()
    {
        client = new Client("defaultkey");

        await AuthenticateWithDevice();

        socket = client.NewSocket();

        await ConnectingSession();
    }

    public async System.Threading.Tasks.Task AuthenticateWithDevice()
    {
        UpdateStatus("Connecting");
        // If the user's device ID is already stored, grab that - alternatively get the System's unique device identifier.
        var deviceId = PlayerPrefs.GetString("deviceId", SystemInfo.deviceUniqueIdentifier);

        // If the device identifier is invalid then let's generate a unique one.
        if (deviceId == SystemInfo.unsupportedIdentifier)
        {
            deviceId = System.Guid.NewGuid().ToString();
        }

        // Save the user's device ID to PlayerPrefs so it can be retrieved during a later play session for re-authenticating.
        PlayerPrefs.SetString("deviceId", deviceId);

        // Authenticate with the Nakama server using Device Authentication.
        try
        {
            session = await client.AuthenticateDeviceAsync(deviceId);
            var account = await client.GetAccountAsync(session);
            Debug.Log("Username: " + account.User.Username);
            UpdateStatus("Authenticated");
            //Debug.Log("Authenticated with Device ID");
        }
        //catch (ApiResponseException)
        //{
        //    UpdateStatus("Error authenticating device");
        //    //Debug.LogFormat("Error authenticating with Device ID: {0}", ex.Message);
        //    return;
        //}
        catch (System.Exception)
        {
            UpdateStatus("Error authenticating device");
            return;
        }
    }

    /// <summary>
    /// Set the username of player
    /// </summary>
    /// <param name="username">New username</param>
    public async void SetUsername(string username)
    {
        try
        {
            await client.UpdateAccountAsync(session, username);
            Debug.Log("Username updated: " + username);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to update username: " + e.Message);
        }
    }

    public async System.Threading.Tasks.Task ConnectingSession()
    {
        UpdateStatus("Connecting");

        try
        {
            await socket.ConnectAsync(session, true);
            UpdateStatus("Connected");
            EventCallback.OnLoggedin();
        }
        catch (System.Exception)
        {
            UpdateStatus("Failed to connect");
            EventCallback.OnFailedConnect();
            return;
        }
    }
}