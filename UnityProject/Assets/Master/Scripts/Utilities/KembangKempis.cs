using UnityEngine;

using DG.Tweening;

public class KembangKempis : MonoBehaviour
{
    [SerializeField] private float scale = 1.2f, duration = .5f;

    private void Start()
    {
        transform.DOScale(scale, duration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}