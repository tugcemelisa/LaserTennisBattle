namespace LaserTennis
{
	public struct LaserButtonPressed
	{
		public PlayerType Player { get; private set; }
		public int LaserCount { get; private set; }

		public LaserButtonPressed(PlayerType player, int laserCount)
		{
			Player = player;
			LaserCount = laserCount;
		}
	}
}