using UnityEngine;
using Unity.Cinemachine;

using DG.Tweening;

public class ZoomCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachine;
    [SerializeField, Min(1)] private float zoomValue = 8f;

    private LensSettings lens;

    private void OnEnable()
    {
        EventCallback.OnGameStart += StartGame;
        EventCallback.OnGameOver += EndGame;
        EventCallback.OnDashStarted += ZoomDash;
    }

    private void OnDisable()
    {
        EventCallback.OnGameStart -= StartGame;
        EventCallback.OnGameOver -= EndGame;
        EventCallback.OnDashStarted -= ZoomDash;
    }

    private void Start()
    {
        lens = cinemachine.Lens;
    }

    private void StartGame() => AnimateZoom(.5f, 13f, 1.5f);
    private void EndGame(GameResult gameResult) => AnimateZoom(13f, .5f, 1.5f);

    private void ZoomDash()
    {
        float initialValue = lens.OrthographicSize;
        AnimateZoom(lens.OrthographicSize, zoomValue, .2f, () =>
        {
            AnimateZoom(lens.OrthographicSize, initialValue, .2f);
        });
    }

    private void AnimateZoom(float from, float to, float duration, TweenCallback callback = null)
    {
        //float lastValue = from;
        DOVirtual.Float(from, to, duration, (value) =>
        {
            lens.OrthographicSize = value;
            cinemachine.Lens = lens;
        }).OnComplete(() =>
        {
            callback?.Invoke();
        });
    }
}
