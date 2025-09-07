using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class PlayerControl : MonoBehaviour
{
    [Tooltip("PlayerStats for player's data")]
    [SerializeField] private PlayerStats data;

    private Health health;
    private PlayerMove move;

    private int collectCount = 0;
    private bool godMode = false;

    private void Awake()
    {
        health = new(data);
        move = GetComponent<PlayerMove>();
    }

    private void OnEnable()
    {
        ResetGame();

        health.Initialize();
        move.Initialize(data);

        health.Register(Dead);

        EventCallback.OnCheatGod += GodMode;
    }

    private void OnDisable()
    {
        health.Unregister(Dead);

        EventCallback.OnGameStart -= ResetGame;
        EventCallback.OnCheatGod -= GodMode;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyControl>(out var enemy))
            TakeDamage(collision);

        if (collision.TryGetComponent<Collectible>(out var collectible))
        {
            collectCount += 1;
            EventCallback.OnCollect(collectCount);
        }
    }

    private void GodMode(bool active) => godMode = active;

    private void TakeDamage(Collider2D collider)
    {
        if (godMode) return;

        if (move.isDashing)
        {
            PoolingHandle.ParticlePool.GetItem().transform.position = collider.transform.position;
            AudioEmitter.PlayOneShot(GameController.Config.breakClip);
            return;
        }

        health.SubtractHealth(1);
        AudioEmitter.PlayOneShot(data.hitClip);
        EventCallback.OnPlayerHit();
    }

    private void ResetGame()
    {
        collectCount = 0;
        godMode = false;
        EventCallback.OnCollect(collectCount);
    }
        
    private void Dead() => EventCallback.OnGameOver(GameResult.Lose);
}