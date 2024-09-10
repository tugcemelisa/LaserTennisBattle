using UnityEngine;

namespace LaserTennis
{
	public class ScoreManager : MonoBehaviour
	{
		private int _player1Score;
		private int _player2Score;

		private void Start()
		{
			_player1Score = PlayerPrefs.GetInt(ConstValues.Player1Score, 0);
			_player2Score = PlayerPrefs.GetInt(ConstValues.Player2Score, 0);
			UpdateScoreTable();
		}

		public void OnEnable()
		{
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			switch (e.PlayerType)
			{
				case PlayerType.Player1:
					_player2Score++;
					break;
				case PlayerType.Player2:
					_player1Score++;
					break;
			}
			UpdateScoreTable();
		}

		private void UpdateScoreTable()
		{
			PlayerPrefs.SetInt(ConstValues.Player1Score, _player1Score);
			PlayerPrefs.SetInt(ConstValues.Player2Score, _player2Score);
			EventBus<PlayerScoreUpdated>.Emit(this, new PlayerScoreUpdated(_player1Score, _player2Score));
		}

		public void OnDisable()
		{
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
		}
	}
}