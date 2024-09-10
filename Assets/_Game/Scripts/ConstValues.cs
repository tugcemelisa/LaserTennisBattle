using UnityEngine;

namespace LaserTennis
{
	public static class ConstValues
	{
		public static readonly int Walk = Animator.StringToHash("Walk");
		public static readonly int Jump = Animator.StringToHash("Jump");
		public static readonly int Die = Animator.StringToHash("Die");
		public static readonly int Win = Animator.StringToHash("Win");

		public static readonly string Player1Score = "LaserTennisPlayer1Score";
		public static readonly string Player2Score = "LaserTennisPlayer2Score";

		public static readonly string GameScene = "Game";
		public static readonly string MainMenu = "MainMenu";

	}
}