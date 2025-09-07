using UnityEngine;

public class ParticleCallback : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        PoolingHandle.ParticlePool.ReturnItem(this);
    }
}