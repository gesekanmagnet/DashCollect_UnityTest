using UnityEngine;

public class LateMoveSprite : MonoBehaviour
{
    [SerializeField] private Transform player;

    private float smoothTime = .1f;
    private Vector3 velocity;

    private void LateUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, player.rotation, 100 * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothTime);
    }
}