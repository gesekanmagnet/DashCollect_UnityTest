using UnityEngine;

[RequireComponent(typeof(EnemyMove))]
public class EnemyControl : MonoBehaviour
{
    [Tooltip("EnemyStats for enemy's data")]
    [SerializeField] private EnemyStats data;

    private Health health;
    private EnemyMove move;

    private void Awake()
    {
        health = new(data);
        move = GetComponent<EnemyMove>();
    }

    private void OnEnable()
    {
        health.Register(Dead);

        health.Initialize();
        move.Initialize(data);

        EventCallback.OnGameOver += Dead;
    }

    private void OnDisable()
    {
        health.Unregister(Dead);

        EventCallback.OnGameOver -= Dead;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerControl>(out var player))
            health.SubtractHealth(1);
    }

    private void Dead() => PoolingHandle.EnemyPool.ReturnItem(this);
    private void Dead(GameResult gameResult) => Dead();
}