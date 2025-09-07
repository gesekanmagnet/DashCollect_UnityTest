public class Health
{
    private System.Action OnZeroHealth { get; set; } = delegate { };

    private float maxHealth, currentHealth;

    /// <summary>
    /// Create new instance for health and apply initial data
    /// </summary>
    /// <param name="data">Data for initial value</param>
    public Health(DataStats data)
    {
        maxHealth = data.maxHealth;
        Initialize();
    }

    /// <summary>
    /// First initialization data before use
    /// </summary>
    public void Initialize()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Add and subscribe events, called when health becomes zero
    /// </summary>
    /// <param name="action">Events to execute</param>
    public void Register(System.Action action) => OnZeroHealth += action;

    /// <summary>
    /// Remove and unsubscribe events, prevent it from called when health becomes zero
    /// </summary>
    /// <param name="action">Events to execute</param>
    public void Unregister(System.Action action) => OnZeroHealth -= action;

    /// <summary>
    /// Decrease the current health
    /// </summary>
    /// <param name="amount">Amount of health to decrease</param>
    public void SubtractHealth(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0) OnZeroHealth();
    }
}
