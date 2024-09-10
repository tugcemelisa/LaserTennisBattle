using System.Linq;
using UnityEngine;

namespace LaserTennis
{
	public class LaserButtonManager : MonoBehaviour
	{
		private bool _gameActive;
		private float _timer;
		[SerializeField] private float _buttonActivateTime;
		[SerializeField] private LaserButton[] _laserButtons1;
		[SerializeField] private LaserButton[] _laserButtons2;
		[SerializeField] private Material[] _materials;

		public void OnEnable()
		{
			EventBus<GameStart>.AddListener(OnGameStart);
			EventBus<GamePaused>.AddListener(OnGamePaused);
			EventBus<GameResumed>.AddListener(OnGameResumed);
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		private void Start()
		{
			for (int i = 0; i < _laserButtons1.Length; i++)
			{
				_laserButtons1[i].SetMaterial(_materials[0]);
				_laserButtons2[i].SetMaterial(_materials[1]);
			}
		}

		private void OnGameStart(object sender, GameStart e)
		{
			_laserButtons1[Random.Range(0, _laserButtons1.Length)].Activate();
			_laserButtons2[Random.Range(0, _laserButtons2.Length)].Activate();
			_gameActive = true;
		}

		private void Update()
		{
			if (_gameActive == false) return;

			ButtonActivate();
		}

		private void ButtonActivate()
		{
			_timer += Time.deltaTime;

			if (_timer >= _buttonActivateTime)
			{
				_timer = 0;

				LaserButton[] temp = _laserButtons1.Where(t => t.IsActive == false).ToArray();
				if (temp.Length > 0)
				{
					temp[Random.Range(0, temp.Length)].Activate();
				}

				LaserButton[] temp2 = _laserButtons2.Where(t => t.IsActive == false).ToArray();
				if (temp2.Length > 0)
				{
					temp2[Random.Range(0, temp2.Length)].Activate();
				}
			}
		}

		private void OnGameResumed(object sender, GameResumed e)
		{
			_gameActive = true;
		}

		private void OnGamePaused(object sender, GamePaused e)
		{
			_gameActive = false;
		}

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			_gameActive = false;
		}

		public void OnDisable()
		{
			EventBus<GameStart>.RemoveListener(OnGameStart);
			EventBus<GamePaused>.RemoveListener(OnGamePaused);
			EventBus<GameResumed>.RemoveListener(OnGameResumed);
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
		}
	}
}