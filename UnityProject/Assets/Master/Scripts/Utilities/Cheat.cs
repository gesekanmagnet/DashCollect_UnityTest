using UnityEngine;

public class Cheat : MonoBehaviour
{
    private bool pressed = false;

    public void GodMode()
    {
        pressed = !pressed;
        EventCallback.OnCheatGod(pressed);
    }

    public void AutoWin() => EventCallback.OnGameOver(GameResult.Win);
}