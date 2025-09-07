using UnityEngine;

using DG.Tweening;

public class RotateSprite : MonoBehaviour
{
    private Tween rotateTween;

    private void OnEnable()
    {
        EventCallback.OnDashEnded += Rotate;
        EventCallback.OnDashStarted += StopRotate;
    }

    private void OnDisable()
    {
        EventCallback.OnDashEnded -= Rotate;
        EventCallback.OnDashStarted -= StopRotate;
    }

    private void Start()
    {
        Rotate();
    }

    private void Rotate()
    {
        rotateTween = transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

    private void StopRotate() => rotateTween.Kill();
}