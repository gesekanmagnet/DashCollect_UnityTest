using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ConnectInterface : MonoBehaviour
{
    [Tooltip("Text for display the current connection status")]
    [SerializeField] private TMP_Text statusText;
    [Tooltip("Button for connect and play, also for show connection status")]
    [SerializeField] private Button accessButton;
    [Tooltip("Text for find match")]
    [SerializeField] private TMP_Text matchText, joinText;
    [Tooltip("Event trigger for access button")]
    [SerializeField] private EventTrigger eventTrigger;

    private void Awake()
    {
        accessButton.onClick.AddListener(() =>
        {
            Connector.Instance.Log();
        });
    }

    private void OnEnable()
    {
        EventCallback.OnConnectStatus += Log;
        EventCallback.OnMatchFound += MatchFound;
        EventCallback.OnLoggedin += AccessToPlayButton;
        EventCallback.OnFailedConnect += FailedConnect;
        EventCallback.OnGameStart += DeactivatePlayButton;
        EventCallback.OnGameOver += PlayButton;
    }

    private void OnDisable()
    {
        EventCallback.OnConnectStatus -= Log;
        EventCallback.OnLoggedin -= AccessToPlayButton;
        EventCallback.OnMatchFound -= MatchFound;
        EventCallback.OnFailedConnect -= FailedConnect;
        EventCallback.OnGameStart -= DeactivatePlayButton;
        EventCallback.OnGameOver -= PlayButton;
    }

    private void Log(string status) => statusText.SetText(status);
    private void PlayButton(GameResult gameResult) => AccessToPlayButton();
    private void MatchFound() => matchText.SetText("Match Found!");
    
    private void DeactivatePlayButton()
    {
        accessButton.gameObject.SetActive(false);
        matchText.transform.parent.gameObject.SetActive(false);
        joinText.transform.parent.gameObject.SetActive(false);
    }

    private void FailedConnect()
    {
        accessButton.interactable = true;
        eventTrigger.enabled = true;
    }

    private void AccessToPlayButton()
    {
        matchText.SetText("FIND MATCH");
        joinText.SetText("JOIN MATCH");
        matchText.transform.parent.gameObject.SetActive(true);
        //joinText.transform.parent.gameObject.SetActive(true);

        eventTrigger.enabled = false;
        
        accessButton.gameObject.SetActive(true);
        accessButton.interactable = true;
        accessButton.onClick.RemoveAllListeners();
        accessButton.onClick.AddListener(() =>
        {
            EventCallback.OnGameStart();
        });
        statusText.SetText("PLAY");
    }
}