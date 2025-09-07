using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "SO/DataStats/Enemy")]
public class EnemyStats : DataStats
{
    [Min(1)] public int damageAmount = 1;
    [Range(.1f, 10f)] public float catchRange = 8f;
    public LayerMask targetLayer;
}