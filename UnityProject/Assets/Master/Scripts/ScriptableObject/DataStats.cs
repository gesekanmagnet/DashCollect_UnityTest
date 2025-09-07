using UnityEngine;

public abstract class DataStats : ScriptableObject
{
    [Range(1, 3)] public int maxHealth;
    [Min(.1f)] public float moveSpeed = 8f;
}