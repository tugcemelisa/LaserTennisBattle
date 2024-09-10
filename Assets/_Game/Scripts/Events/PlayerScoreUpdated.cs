namespace LaserTennis
{
	public struct PlayerScoreUpdated
	{
		public int Player1Score { get; private set; }
		public int Player2Score { get; private set; }

		public PlayerScoreUpdated(int player1Score, int player2Score)
		{
			Player1Score = player1Score;
			Player2Score = player2Score;
		}
	}
}