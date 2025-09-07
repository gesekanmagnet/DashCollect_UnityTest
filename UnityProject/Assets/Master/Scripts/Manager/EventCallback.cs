using System;

public enum GameResult { Lose, Win }

public static class EventCallback
{
    public static Action OnGameStart { get; set; } = delegate { };
    public static Action<float> OnTimer { get; set; } = delegate { };
    public static Action<GameResult> OnGameOver { get; set; } = delegate { };
    public static Action OnPlayerHit { get; set; } = delegate { };
    public static Action<int> OnCollect { get; set; } = delegate { };
    public static Action OnEnemyDead { get; set; } = delegate { };
    public static Action<float, int> OnScore { get; set; } = delegate { };
    public static Action OnDashStarted { get; set; } = delegate { };
    public static Action OnDashEnded { get; set; } = delegate { };

    public static Action OnLoggedin { get; set; } = delegate { };
    public static Action OnFailedConnect { get; set; } =delegate { };
    public static Action<string> OnConnectStatus { get; set; } = delegate { };
    public static Action<bool> OnCheatGod { get; set; } = delegate { };

    public static Action OnMatchStart { get; set; } = delegate { };
    public static Action OnMatchFound { get; set; } = delegate { };
    public static Action<Nakama.ISession> OnMatchEnd { get; set; } = delegate { };
}