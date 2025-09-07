using DG.Tweening;

public class EnemySpreadSprite : SpreadSprite
{
    private EnemyMove move;

    private void Awake()
    {
        spriteInitPositions.Clear();
        for (int i = 0; i < sprites.Length; i++)
        {
            spriteInitPositions.Add(sprites[i].localPosition);
        }

        move = GetComponentInParent<EnemyMove>();
    }

    private void OnEnable()
    {
        MoveTo(false);
        transform.DORotate(new UnityEngine.Vector3(0, 0, 360), 1f, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);

        move.OnCatch += MoveTo;
    }

    private void OnDisable()
    {
        move.OnCatch -= MoveTo;
        DOTween.Kill(transform);
    }

    private void MoveTo(bool isCatch)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            //UnityEngine.Debug.Log(spriteInitPositions[i]);
            sprites[i].DOLocalMove(isCatch ? spriteInitPositions[i] : new(0, 0), .3f);
        }
    }
}