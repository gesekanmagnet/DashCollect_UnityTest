using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    [Tooltip("Location for collectible after spawn")]
    [SerializeField] private Transform[] spawnLocation;

    private void OnEnable()
    {
        EventCallback.OnGameStart += Spawn;
    }

    private void OnDisable()
    {
        EventCallback.OnGameStart -= Spawn;
    }

    private void Spawn()
    {
        for (int i = 0; i < GameController.Config.collectibleCount; i++)
        {
            var collectible = PoolingHandle.CollectiblePool.GetItem();
            collectible.transform.position = spawnLocation[i].position;
        }
    }
}