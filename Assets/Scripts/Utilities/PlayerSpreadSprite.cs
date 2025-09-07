using UnityEngine;

using DG.Tweening;

public class PlayerSpreadSprite : SpreadSprite
{
    [SerializeField] private Transform target;

    private int hit;

    private void OnEnable()
    {
        RestartSprite();

        EventCallback.OnDashStarted += Dash;
        EventCallback.OnPlayerHit += DisableSprite;
    }

    private void OnDisable()
    {
        EventCallback.OnDashStarted -= Dash;
        EventCallback.OnPlayerHit -= DisableSprite;
    }

    private void RestartSprite()
    {
        foreach (var item in sprites)
        {
            item.gameObject.SetActive(true);
        }
    }

    private void DisableSprite()
    {
        if (hit >= sprites.Length) return;
        sprites[hit].gameObject.SetActive(false);
        hit++;
    }

    private void Dash()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            int index = i;
            Vector2 targetPos = target.position + target.up * 3f;
            sprites[index].DOMove(targetPos, .3f).OnComplete(() =>
            {
                sprites[index].DOLocalMove(spriteInitPositions[index], .3f);
            });
        }
    }
}