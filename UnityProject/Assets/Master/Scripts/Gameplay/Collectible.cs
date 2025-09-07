using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnEnable()
    {
        EventCallback.OnGameOver += Return;
    }

    private void OnDisable()
    {
        EventCallback.OnGameOver -= Return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<PlayerControl>(out var player))
        {
            AudioEmitter.PlayOneShot(GameController.Config.collectClip);
            Return(gameResult: GameResult.Win);
        }
    }

    private void Return(GameResult gameResult) => PoolingHandle.CollectiblePool.ReturnItem(this);
}