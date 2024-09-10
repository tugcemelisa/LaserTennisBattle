using TMPro;
using UnityEngine;

namespace LaserTennis
{
	public class UIManager : MonoBehaviour
	{
		[SerializeField] private TMP_Text _player1Score;
		[SerializeField] private TMP_Text _player2Score;
		[SerializeField] private TMP_Text _playerWon;
		[SerializeField] private GameObject _endPanel;
		[SerializeField] private GameObject _player1Won;
		[SerializeField] private GameObject _player2Won;
		[SerializeField] private GameObject[] _closeAtStart;

		private void OnEnable()
		{
			EventBus<GameStart>.AddListener(OnGameStart);
			EventBus<PlayerScoreUpdated>.AddListener(OnPlayerScoreUpdated);
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		private void Start()
		{
			foreach (var go in _closeAtStart)
			{
				go.SetActive(false);
			}
		}

		private void OnGameStart(object sender, GameStart e)
		{
			foreach (var go in _closeAtStart)
			{
				go.SetActive(true);
			}
		}

		public void PlayAgain()
		{
			SceneLoader.Instance.LoadAddressableScene(ConstValues.GameScene);
		}

		public void Pause()
		{
			EventBus<GamePaused>.Emit(this, new GamePaused());
		}

		public void Resume()
		{
			EventBus<GameResumed>.Emit(this, new GameResumed());
		}

		public void MainMenu()
		{
			SceneLoader.Instance.LoadAddressableScene(ConstValues.MainMenu);
		}

		public void Quit()
		{
			Application.Quit();
		}

		private void OnPlayerScoreUpdated(object sender, PlayerScoreUpdated e)
		{
			_player1Score.text = e.Player1Score.ToString();
			_player2Score.text = e.Player2Score.ToString();
		}

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			switch (e.PlayerType)
			{
				case PlayerType.Player1:
					_playerWon.text = "PLAYER 2 WON";
					_player1Won.SetActive(false);
					_player2Won.SetActive(true);
					break;
				case PlayerType.Player2:
					_playerWon.text = "PLAYER 1 WON";
					_player1Won.SetActive(true);
					_player2Won.SetActive(false);
					break;
			}
			_endPanel.SetActive(true);
		}

		private void OnDisable()
		{
			EventBus<GameStart>.RemoveListener(OnGameStart);
			EventBus<PlayerScoreUpdated>.RemoveListener(OnPlayerScoreUpdated);
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
		}
	}
}