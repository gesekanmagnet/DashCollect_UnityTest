using UnityEngine;

using DG.Tweening;

public abstract class SpreadSprite : MonoBehaviour
{
    [SerializeField] protected Transform[] sprites;

    protected System.Collections.Generic.List<Vector2> spriteInitPositions { get; private set; } = new();

    private void Awake()
    {
        spriteInitPositions.Clear();
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteInitPositions.Add(sprites[i].localPosition);
        }
    }
}