using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public enum State { Patrol, Chase }

    //[Tooltip("Path use to patrol")]
    private Transform[] waypoints;

    [Tooltip("Range for enemy to be able to detect player")]
    [SerializeField, Range(.1f, 10f)] private float catchRange = 5f;
    [Tooltip("Layer for define target to catch")]
    [SerializeField] private LayerMask targetLayer;

    /// <summary>
    /// Event listen for catch player
    /// </summary>
    public System.Action<bool> OnCatch { get; set; } = delegate { };

    private EnemyStats data;
    private State currentState;
    private GameObject target;

    private int currentPathIndex = 0;
    private int direction = 1;

    private void OnEnable()
    {
        currentPathIndex = 0;
    }

    private void Update()
    {
        TransitionHandle();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }

    /// <summary>
    /// First initialization data before use
    /// </summary>
    /// <param name="data">Data for initial value</param>
    public void Initialize(EnemyStats data) => this.data = data;

    private bool PlayerDetected()
    {
        Collider2D target = Physics2D.OverlapCircle(transform.position, catchRange, targetLayer);
        if (target)
        {
            this.target = target.gameObject;
            return true;
        }

        this.target = null;
        return false;
    }

    private bool ReachedPoint()
    {
        return Vector2.Distance(transform.position, waypoints[currentPathIndex].position) < 0.1f;
    }

    private void TransitionHandle()
    {
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            default:
                break;
        }
    }

    private void Move(Vector2 destination) =>
        transform.position = Vector2.MoveTowards(transform.position, destination, data.moveSpeed * Time.deltaTime);

    private void Patrol()
    {
        if (PlayerDetected())
        {
            currentState = State.Chase;
            OnCatch(true);
            return;
        }

        Move(waypoints[currentPathIndex].position);

        if (ReachedPoint())
        {
            direction = currentPathIndex == 0 ? 1 : currentPathIndex == waypoints.Length - 1 ? -1 : direction;
            currentPathIndex += direction;
        }
    }

    private void Chase()
    {
        if(!PlayerDetected())
        {
            currentState = State.Patrol;
            OnCatch(false);
            return;
        }

        Move(target.transform.position);
    }

    /// <summary>
    /// Give waypoints to Enemy for patrol purposes
    /// </summary>
    /// <param name="waypoints">Array of path</param>
    public void SetWaypoints(Transform[] waypoints) => this.waypoints = waypoints;
}