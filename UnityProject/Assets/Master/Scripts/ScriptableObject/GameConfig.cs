using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "SO/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Min(1)] public int collectibleCount = 20;
    public AudioClip winClip, loseClip, collectClip, breakClip;
}