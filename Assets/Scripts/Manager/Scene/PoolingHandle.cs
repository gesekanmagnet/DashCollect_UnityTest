using UnityEngine;

public class PoolingHandle : MonoBehaviour
{
    [Tooltip("Enemy object for pooling")]
    [SerializeField] private EnemyControl enemy;
    [Tooltip("Collectible object for pooling")]
    [SerializeField] private Collectible collectible;
    [Tooltip("Particle object for pooling")]
    [SerializeField] private ParticleCallback particle;
    [Tooltip("Count of object to spawn")]
    [SerializeField, Min(1)] private int count = 10;
    //[Tooltip("Pool parent")]
    //[SerializeField] private Transform parent;

    /// <summary>
    /// Pooling instance reference for EnemyControl
    /// </summary>
    public static PoolingInstance<EnemyControl> EnemyPool { get; private set; }
    public static PoolingInstance<Collectible> CollectiblePool { get; private set; }
    public static PoolingInstance<ParticleCallback> ParticlePool { get; private set; }

    private void Awake()
    {
        EnemyPool = new(enemy, count, transform);
        CollectiblePool = new(collectible, count, transform);
        ParticlePool = new(particle, count, transform);
    }
}