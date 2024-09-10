using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LaserTennis
{
	public class LaserGeneratorManager : MonoBehaviour
	{
		private bool _gameActive;
		private float _timer;
		[SerializeField] private float[] _generatorStartTimes;
		[SerializeField] private LaserGenerator[] _laserGenerators1; // Player 1 generators
		[SerializeField] private LaserGenerator[] _laserGenerators2;
		[SerializeField] private ComboQueue[] _comboQueue;
		[SerializeField] private float _delay;

		private void OnEnable()
		{
			EventBus<LaserButtonPressed>.AddListener(OnLaserButtonPressed);
			EventBus<GameStart>.AddListener(OnGameStart);
			EventBus<GamePaused>.AddListener(OnGamePaused);
			EventBus<GameResumed>.AddListener(OnGameResumed);
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		private void Start()
		{
			_timer = 0;
		}

		private void OnGameStart(object sender, GameStart e)
		{
			_gameActive = true;
			StartCoroutine(ActivateGenerator());
		}

		private IEnumerator ActivateGenerator()
		{
			int i = 1;
			_laserGenerators1[0].Activate();
			_laserGenerators2[0].Activate();
			while (i + 1 <= _laserGenerators1.Length)
			{
				if (_gameActive)
				{
					_timer += Time.deltaTime;
				}
				if (_timer > _generatorStartTimes[i])
				{
					_laserGenerators1[i].Activate();
					_laserGenerators2[i].Activate();
					i++;
				}
				yield return null;
			}
		}

		private void OnLaserButtonPressed(object sender, LaserButtonPressed e)
		{
			switch (e.Player)
			{
				case PlayerType.Player1:
					GenerateLaser1(e.LaserCount);
					break;
				case PlayerType.Player2:
					GenerateLaser2(e.LaserCount);
					break;
			}
		}

		private void GenerateLaser1(int laserCount)
		{
			List<LaserGenerator> activeLasers = new List<LaserGenerator>();
			for (int i = 0; i < _laserGenerators1.Length; i++)
			{
				if (_laserGenerators1[i].IsActive)
				{
					activeLasers.Add(_laserGenerators1[i]);
				}
			}

			Generate(laserCount, activeLasers);
		}

		private void GenerateLaser2(int laserCount)
		{
			List<LaserGenerator> activeLasers = new List<LaserGenerator>();

			for (int i = 0; i < _laserGenerators2.Length; i++)
			{
				if (_laserGenerators2[i].IsActive)
				{
					activeLasers.Add(_laserGenerators2[i]);
				}
			}

			Generate(laserCount, activeLasers);
		}

		private async void Generate(int laserCount, List<LaserGenerator> activeLasers)
		{
			if (laserCount == 1)
			{
				activeLasers[Random.Range(0, activeLasers.Count)].Generate();
			}
			else
			{
				ComboQueue combo = _comboQueue[activeLasers.Count - 1];
				int generatorCount = combo.Queue[combo.IndexCounter % combo.Queue.Length];
				combo.IndexCounter++;
				List<LaserGenerator> laserGenerated = new List<LaserGenerator>();

				for (int i = 0; i < generatorCount; i++)
				{
					int rnd = Random.Range(0, activeLasers.Count);
					laserGenerated.Add(activeLasers[rnd]);
					activeLasers[rnd].Generate();
					activeLasers.RemoveAt(rnd);
					await Task.Delay((int)(_delay * 1000));
				}

				laserCount -= generatorCount;
				for (int i = 0; i < laserCount; i++)
				{
					laserGenerated[Random.Range(0, laserGenerated.Count)].Generate();
					await Task.Delay((int)(_delay * 1000));
				}
			}
		}

		private void OnGamePaused(object sender, GamePaused e)
		{
			_gameActive = false;
		}

		private void OnGameResumed(object sender, GameResumed e)
		{
			_gameActive = true;
		}

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			_gameActive = false;
		}

		private void OnDisable()
		{
			EventBus<LaserButtonPressed>.RemoveListener(OnLaserButtonPressed);
			EventBus<GameStart>.RemoveListener(OnGameStart);
			EventBus<GamePaused>.RemoveListener(OnGamePaused);
			EventBus<GameResumed>.RemoveListener(OnGameResumed);
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
		}
	}

	[Serializable]
	public class ComboQueue
	{
		public int IndexCounter;
		public int[] Queue;
	}
}