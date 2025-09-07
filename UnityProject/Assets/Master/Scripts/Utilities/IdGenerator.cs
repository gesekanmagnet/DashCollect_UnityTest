using System;

public static class IdGenerator
{
    public static string GenerateId()
    {
        string timePart = DateTime.UtcNow.Ticks.ToString("x");
        string randomPart = UnityEngine.Random.Range(1000, 9999).ToString();
        return $"{timePart}{randomPart}";
    }
}