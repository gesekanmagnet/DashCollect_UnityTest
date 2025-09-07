using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 move;
    public bool dash;

    public void OnMove(InputValue value) => move = value.Get<Vector2>();
    public void OnDash(InputValue value) => dash = value.isPressed;
}