namespace LaserTennis
{
	public struct HealthUpdated
	{
		public PlayerType PlayerType { get; private set; }
		public int Health { get; private set; }
		public int MaxHealth { get; private set; }

		public HealthUpdated(PlayerType playerType, int health, int maxHealth)
		{
			PlayerType = playerType;
			Health = health;
			MaxHealth = maxHealth;
		}
	}
}