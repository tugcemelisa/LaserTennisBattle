namespace LaserTennis
{
	public struct PlayerDied
	{
		public PlayerType PlayerType { get; private set; }

		public PlayerDied(PlayerType playerType)
		{
			PlayerType = playerType;
		}
	}
}