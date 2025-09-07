using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardItem : MonoBehaviour
{
    [Tooltip("TMP References")]
    [SerializeField] private TMP_Text rankText, usernameText, completionTimeText, damageTakenText;

    [Tooltip("UI background leaderboard")]
    [SerializeField] private Image background;

    /// <summary>
    /// Apply all leaderboard content
    /// </summary>
    /// <param name="rank">Rank of player</param>
    /// <param name="username">Username of player</param>
    /// <param name="completionTime">Completion time by player</param>
    /// <param name="damageTaken">Damage taken by player</param>
    /// <param name="backgroundColor">Background color of the leaderboard</param>
    public void SetContent(string rank, string username, string completionTime, string damageTaken, Color backgroundColor)
    {
        rankText.SetText(rank);
        usernameText.SetText(username);
        completionTimeText.SetText(completionTime);
        damageTakenText.SetText(damageTaken);
        background.color = backgroundColor;
    }
}