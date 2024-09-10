using TMPro;
using UnityEngine;

namespace LaserTennis
{
	public class Timer : MonoBehaviour
	{
		private float _pausedTime;
		private bool _timerActive;
		private float _startTime;
		[SerializeField] private TMP_Text _timerText;

		public void Awake()
		{
			EventBus<GameStart>.AddListener(OnGameStart);
			EventBus<GamePaused>.AddListener(OnGamePaused);
			EventBus<GameResumed>.AddListener(OnGameResumed);
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		private void Update()
		{
			if (_timerActive)
			{
				float t = Time.time - _startTime;

				int minutes = (int)(t / 60);
				int seconds = (int)(t % 60);

				string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
				_timerText.text = timerString;
			}
		}

		private void StartTimer()
		{
			_startTime += Time.time - _pausedTime;
			_timerActive = true;
		}

		private void StopTimer()
		{
			_timerActive = false;
		}

		private void ResetTimer()
		{
			_startTime = Time.time;
			_pausedTime = 0;
			_timerText.text = "00:00";
			_timerActive = true;
		}

		private void OnGameStart(object sender, GameStart e)
		{
			ResetTimer();
		}

		private void OnGameResumed(object sender, GameResumed e)
		{
			StartTimer();
		}

		private void OnGamePaused(object sender, GamePaused e)
		{
			StopTimer();
			_pausedTime = Time.time;
		}

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			StopTimer();
		}

		public void OnDestroy()
		{
			EventBus<GameStart>.RemoveListener(OnGameStart);
			EventBus<GamePaused>.RemoveListener(OnGamePaused);
			EventBus<GameResumed>.RemoveListener(OnGameResumed);
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
		}
	}
}