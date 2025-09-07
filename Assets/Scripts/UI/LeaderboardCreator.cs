using System.Threading.Tasks;

using UnityEngine;

using DG.Tweening;

public class LeaderboardCreator : MonoBehaviour
{
    [Tooltip("Template for leaderboard item")]
    [SerializeField] private LeaderboardItem itemPrefab;
    [Tooltip("Top Leaderboard parent with layout element")]
    [SerializeField] private Transform topItemContainer;
    [Tooltip("Around Leaderboard parent with layout element")]
    [SerializeField] private Transform arroundItemContainer;
    [Tooltip("Panel of all leaderboards")]
    [SerializeField] private RectTransform panel;

    [Tooltip("Indicator for content information, shows up when theres no current index on leaderboard")]
    [SerializeField] private GameObject indexIndicator;

    private Vector2 initialPos;

    private void Awake()
    {
        initialPos = panel.anchoredPosition;
    }

    private void OnEnable()
    {
        EventCallback.OnScore += SendDataToLeaderboard;
        EventCallback.OnLoggedin += ActivateLeaderboard;
        EventCallback.OnLoggedin += RefreshLeaderboard;
        EventCallback.OnGameStart += DeactivateLeaderboard;
        EventCallback.OnGameOver += ActivateLeaderboard;
    }

    private void OnDisable()
    {
        EventCallback.OnScore -= SendDataToLeaderboard;
        EventCallback.OnLoggedin -= ActivateLeaderboard;
        EventCallback.OnLoggedin -= RefreshLeaderboard;
        EventCallback.OnGameStart -= DeactivateLeaderboard;
        EventCallback.OnGameOver -= ActivateLeaderboard;
    }

    [NaughtyAttributes.Button]
    private void AutoFinished() => EventCallback.OnGameOver(GameResult.Win);

    private async void SendDataToLeaderboard(float completionTime, int damageTaken) => await SubmitRun(completionTime, damageTaken);

    private async Task SubmitRun(float completionTime, int damageTaken)
    {
        var payload = new Payload
        {
            completionTime = Mathf.RoundToInt(completionTime),
            damageTaken = damageTaken,
            clientVersion = Application.version
        };

        string json = JsonUtility.ToJson(payload);

        try
        {
            await Connector.client.RpcAsync(Connector.session, "SubmitRun", json);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Submit run failed: " + e.Message);
            throw;
        }
    }

    private async void CreateLeaderboard()
    {
        await TopRecord();
        await ArroundPlayer();
    }

    private void DeactivateLeaderboard() => DisplayLeaderboard(false);
    private void RefreshLeaderboard() => StartCoroutine(UpdateLeaderboard());
    
    private void ActivateLeaderboard()
    {
        CreateLeaderboard();
        DisplayLeaderboard(true);
    }

    private void ActivateLeaderboard(GameResult gameResult)
    {
        CreateLeaderboard();
        DisplayLeaderboard(true);
    }

    private void ClearLeaderboard(Transform container)
    {
        if (container.childCount <= 0) return;

        for (int i = 0; i < container.childCount; i++)
        {
            Destroy(container.GetChild(i).gameObject);
        }
    }

    private async Task TopRecord()
    {
        //var payload = "{}"; // kosong aja
        //var gg = await Connector.client.RpcAsync(Connector.session, "SubmitRun", payload);
        //Debug.Log(gg.Payload);
        ClearLeaderboard(topItemContainer);

        var result = await Connector.client.ListLeaderboardRecordsAsync(Connector.session, "unity_test", limit: 10);
        foreach (var r in result.Records)
        {
            indexIndicator.SetActive(false);
            LeaderboardItem item = Instantiate(itemPrefab, topItemContainer);
            Color color = Color.white;
            if(r.OwnerId.Equals(Connector.session.UserId)) color = Color.yellow;
            item.SetContent(r.Rank, r.Username, r.Score, r.Subscore ?? "0", color);
            item.gameObject.SetActive(true);
        }
    }

    private async Task ArroundPlayer()
    {
        ClearLeaderboard(arroundItemContainer);

        var result = await Connector.client.ListLeaderboardRecordsAroundOwnerAsync(Connector.session, "unity_test", Connector.session.UserId, limit: 3);
        foreach (var r in result.Records)
        {
            LeaderboardItem item = Instantiate(itemPrefab, arroundItemContainer);
            Color color = Color.white;
            if (r.OwnerId.Equals(Connector.session.UserId)) color = Color.yellow;
            item.SetContent(r.Rank, r.Username, r.Score, r.Subscore ?? "0", color);
            item.gameObject.SetActive(true);
        }
    }

    private void DisplayLeaderboard(bool active)
    {
        if (active)
            panel.DOAnchorPos(new(0, 0), 1f);
        else
            panel.DOAnchorPos(initialPos, 1f);
        //Debug.Log("Leaderboard: " + active);
    }

    private System.Collections.IEnumerator UpdateLeaderboard()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            CreateLeaderboard();
        }

    }
}

[System.Serializable]
public struct Payload
{
    public int completionTime;
    public int damageTaken;
    public string clientVersion;
}
