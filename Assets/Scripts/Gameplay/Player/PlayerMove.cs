using UnityEngine;

using DG.Tweening;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Tooltip("Dash Effect")]
    [SerializeField] private TrailRenderer trailRenderer;

    private PlayerStats data;
    private PlayerInput input;
    private Rigidbody2D rigidBody;

    private bool canDash = true;
    public bool isDashing { get; private set; }
    private float rotationVelocity;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        EnableDash();
    }

    private void Update()
    {
        DashInput();
    }

    private void FixedUpdate()
    {
        RigidbodyMove();
    }

    /// <summary>
    /// First initialization data before use
    /// </summary>
    /// <param name="data">Data for initial value</param>
    public void Initialize(PlayerStats data) => this.data = data;

    private void DashInput()
    {
        if (input.dash)
        {
            input.dash = false;
            if (canDash) StartCoroutine(Dash());
        }
    }

    private void RigidbodyMove()
    {
        if (isDashing) return;

        Vector2 inputDirection = input.move.normalized;
        rigidBody.linearVelocity = inputDirection * data.moveSpeed;
        float rotation = Mathf.Atan2(input.move.y, input.move.x) * Mathf.Rad2Deg - 90f;

        if (input.move != Vector2.zero)
        {
            if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, rotation)) < 0.1f)
                rigidBody.MoveRotation(rotation);
            else
            {
                float rotate = Mathf.SmoothDampAngle(transform.eulerAngles.z, rotation, ref rotationVelocity, data.smoothTime);
                rigidBody.MoveRotation(rotate);
            }
        }
    }

    private void EnableDash()
    {
        canDash = true;
        EventCallback.OnDashEnded();
    }

    private System.Collections.IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        EventCallback.OnDashStarted();
        trailRenderer.emitting = true;
        transform.DOScaleX(.15f, .3f);
        rigidBody.linearVelocity = transform.up * 24f;
        AudioEmitter.PlayOneShot(data.dashClip);
        //Debug.Log("dash");
        yield return new WaitForSeconds(.3f);
        transform.DOScaleX(1f, .3f);
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(data.dashCooldown);
        EnableDash();
    }
}