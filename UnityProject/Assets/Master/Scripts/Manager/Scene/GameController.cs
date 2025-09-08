using UnityEngine;

public class GameController : MonoBehaviour
{
    [Tooltip("Game config for data manage")]
    [SerializeField] private GameConfig config;

    [Tooltip("Object for activate/deactivate when game start/ends")]
    [SerializeField] private GameObject entity;

    private float currentTime;
    private int playerHit = 0;
    private bool gamePaused = true, onlineMatch = false;

    /// <summary>
    /// Game configuration data contains
    /// </summary>
    public static GameConfig Config { get; private set; }

    private void Awake()
    {
        Debug.developerConsoleVisible = true;
    }

    private void OnEnable()
    {
        Config = config;

        EventCallback.OnCollect += CollectibleCheck;
        EventCallback.OnGameStart += GameStart;
        EventCallback.OnGameOver += GameEnd;
        EventCallback.OnPlayerHit += PlayerHit;
        EventCallback.OnMatchStart += OnlineMatch;
    }

    private void OnDisable()
    {
        EventCallback.OnCollect -= CollectibleCheck;
        EventCallback.OnGameStart -= GameStart;
        EventCallback.OnGameOver -= GameEnd;
        EventCallback.OnPlayerHit -= PlayerHit;
        EventCallback.OnMatchStart -= OnlineMatch;
    }

    private void Update()
    {
        if (gamePaused)
            currentTime = 0;
        else
            currentTime += Time.deltaTime;

        EventCallback.OnTimer(currentTime);
    }

    private void CollectibleCheck(int count)
    {
        //collectCount++;
        if (count >= config.collectibleCount)
        {
            if(onlineMatch)
            {
                EventCallback.OnMatchEnd(Connector.session);
                return;
            }

            EventCallback.OnGameOver(GameResult.Win);
        }
    }

    /// <summary>
    /// Start the game
    /// </summary>
    private void GameStart()
    {
        gamePaused = false;
        //EventCallback.OnGameStart();
        entity.SetActive(true);
    }

    private void PlayerHit() => playerHit++;

    private void OnlineMatch()
    {
        onlineMatch = true;
        EventCallback.OnGameStart();
    }

    private void GameEnd(GameResult gameResult)
    {
        gamePaused = true;

        //if(gameResult.Equals(GameResult.Win))
            //EventCallback.OnScore(currentTime, playerHit);
        
        currentTime = 0;
        playerHit = 0;
        onlineMatch = false;

        entity.SetActive(false);
    }
}