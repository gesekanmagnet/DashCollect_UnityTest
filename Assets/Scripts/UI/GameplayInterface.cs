using UnityEngine;
using TMPro;

using DG.Tweening;

public class GameplayInterface : MonoBehaviour
{
    [Tooltip("Panel to display after game win")]
    [SerializeField] private CanvasGroup winPanel;
    [Tooltip("Panel to display after game lose")]
    [SerializeField] private CanvasGroup losePanel;

    [Tooltip("Text for display the current time")]
    [SerializeField] private TMP_Text timerText;

    [Tooltip("Text for display current collectible count")]
    [SerializeField] private TMP_Text countText;

    private RectTransform timerPanel, countPanel;
    private Vector2 timerInitialPosition, countInitialPosition;

    private void Awake()
    {
        timerPanel = timerText.transform.parent.GetComponent<RectTransform>();
        countPanel = countText.transform.parent.GetComponent<RectTransform>();

        timerInitialPosition = timerPanel.anchoredPosition;
        countInitialPosition = countPanel.anchoredPosition;
    }

    private void OnEnable()
    {
        EventCallback.OnTimer += SetTimer;
        EventCallback.OnCollect += Collect;
        EventCallback.OnGameStart += DisplayUI;
        EventCallback.OnGameOver += GameFinished;
    }

    private void OnDisable()
    {
        EventCallback.OnTimer -= SetTimer;
        EventCallback.OnCollect -= Collect;
        EventCallback.OnGameStart -= DisplayUI;
        EventCallback.OnGameOver -= GameFinished;
    }

    private void DisplayUI()
    {
        timerPanel.DOAnchorPos(new(0, 0), 1f);
        countPanel.DOAnchorPos(new(0, 0), 1f);
    }

    private void UndisplayUI()
    {
        timerPanel.DOAnchorPos(timerInitialPosition, 1f);
        countPanel.DOAnchorPos(countInitialPosition, 1f);
    }

    private void SetTimer(float time)
    {
        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(time);
        timerText.SetText(string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds));
    }

    private void Collect(int count)
    {
        countText.SetText(count.ToString());
        countText.rectTransform.DOScale(1.5f, .3f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }

    private void GameFinished(GameResult result)
    {
        switch (result)
        {
            case GameResult.Lose:
                AudioEmitter.PlayMusic(GameController.Config.loseClip);
                losePanel.DOFade(1, .5f).OnComplete(() =>
                {
                    losePanel.DOFade(0, 1f).SetDelay(2f);
                });
                break;
            case GameResult.Win:
                AudioEmitter.PlayMusic(GameController.Config.winClip);
                winPanel.DOFade(1f, .5f).OnComplete(() =>
                {
                    winPanel.DOFade(0, 1f).SetDelay(2f);
                });
                break;
            default:
                break;
        }
        UndisplayUI();
    }
}