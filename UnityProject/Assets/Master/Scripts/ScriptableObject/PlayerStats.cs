using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "SO/DataStats/Player")]
public class PlayerStats : DataStats
{
    [Min(.1f)] public float dashCooldown = 2f;
    [Range(.1f, 1f)]public float smoothTime = .1f;

    public AudioClip hitClip, dashClip;
}