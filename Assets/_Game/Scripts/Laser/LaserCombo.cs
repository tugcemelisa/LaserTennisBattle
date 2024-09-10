using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace LaserTennis
{
	public class LaserCombo : MonoBehaviour
	{
		private bool _gameActive;
		private bool _isActive;
		private float _timer;
		[SerializeField] private int _laserCount;
		[SerializeField] private float _activateTime;
		[SerializeField] private Material _activeMaterial;
		[SerializeField] private Material _deactiveMaterial;
		[SerializeField] private MeshRenderer _meshRenderer;
		[SerializeField] private Transform _active;

		public void OnEnable()
		{
			EventBus<GameStart>.AddListener(OnGameStart);
			EventBus<GamePaused>.AddListener(OnGamePaused);
			EventBus<GameResumed>.AddListener(OnGameResumed);
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		private void Start()
		{
			_isActive = false;
		}

		private void OnGameStart(object sender, GameStart e)
		{
			_gameActive = true;
			StartCoroutine(Activate());
			_meshRenderer.material = _deactiveMaterial;
		}

		public void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out PlayerController player))
			{
				_active.DOLocalMoveY(-0.08f, 0.2f);
				if (_isActive && player.JumpLand)
				{
					_active.DOLocalMoveY(-0.17f, 0.2f);
					Deactivate();
					EventBus<LaserButtonPressed>.Emit(this, new LaserButtonPressed(player.PlayerType, _laserCount));
				}
			}
		}

		public void OnTriggerExit(Collider other)
		{
			if (other.TryGetComponent(out PlayerController player))
			{
				_active.DOLocalMoveY(0, 0.2f);
			}
		}

		public IEnumerator Activate()
		{
			while (_isActive == false)
			{
				if (_gameActive)
				{
					_timer += Time.deltaTime;
				}
				if (_timer >= _activateTime)
				{
					_isActive = true;
					_timer = 0;
					_meshRenderer.material = _activeMaterial;
				}
				yield return null;
			}
		}

		public void Deactivate()
		{
			_isActive = false;
			StartCoroutine(Activate());
			_meshRenderer.material = _deactiveMaterial;
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