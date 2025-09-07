using UnityEngine;

public class SaveData
{
    private string deviceId;

    /// <summary>
    /// Create new SaveData instance
    /// </summary>
    /// <param name="deviceId">Currently logged device ID</param>
    public SaveData(string deviceId)
    {
        this.deviceId = deviceId;
    }

    /// <summary>
    /// Save the new player data
    /// </summary>
    /// <param name="completionTime">Time player finished the level</param>
    /// <param name="damageTaken">Total damage taken by player</param>
    public void Save(float completionTime, int damageTaken)
    {
        PlayerPrefs.SetFloat("completionTime", completionTime);
        PlayerPrefs.SetInt("damageTaken", damageTaken);
    }

    /// <summary>
    /// Time player finished the level
    /// </summary>
    /// <returns>Time in seconds</returns>
    public float CompletionTime() => PlayerPrefs.GetFloat("completionTime", 0f);
    /// <summary>
    /// Total damage taken by player
    /// </summary>
    /// <returns>Value of damage taken</returns>
    public float DamageTaken() => PlayerPrefs.GetInt("damageTaken", 0);
}