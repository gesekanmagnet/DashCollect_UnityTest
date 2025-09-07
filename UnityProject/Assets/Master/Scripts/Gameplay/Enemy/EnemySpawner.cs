using UnityEngine;

[System.Serializable]
public struct Path
{
    public Transform[] path;
}

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("List of enemy path")]
    [SerializeField] private System.Collections.Generic.List<Path> path;

    private void OnEnable()
    {
        EventCallback.OnEnemyDead += Spawn;
        EventCallback.OnGameStart += FirstSpawn;
    }

    private void OnDisable()
    {
        EventCallback.OnEnemyDead -= Spawn;
        EventCallback.OnGameStart -= FirstSpawn;
    }

    private void FirstSpawn()
    {
        for (int i = 0; i < path.Count; i++)
        {
            Spawn(path[i].path);
        }
    }

    private void Spawn()
    {
        Spawn(path[Random.Range(0, path.Count)].path);
    }

    private void Spawn(Transform[] path)
    {
        EnemyMove enemy = PoolingHandle.EnemyPool.GetItem().GetComponent<EnemyMove>();
        enemy.SetWaypoints(path);
        enemy.transform.position = path[0].position;
    }
}