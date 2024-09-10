using DG.Tweening;
using UnityEngine;

namespace LaserTennis
{
	public class Laser : MonoBehaviour
	{
		private Sequence _seq;
		[SerializeField] private float _scaleUp;
		[SerializeField] private float _scaleUpDuration;
		[SerializeField] private float _moveDuration;
		[SerializeField] private Transform _model;

		public void OnEnable()
		{
			EventBus<GamePaused>.AddListener(OnGamePaused);
			EventBus<GameResumed>.AddListener(OnGameResumed);
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		public void Init(Vector3 rot, Transform target, Material material)
		{
			var targetPos = target.position;
			targetPos.y = transform.position.y;
			transform.Rotate(rot);
			GetComponentInChildren<MeshRenderer>().material = material;
			_seq = DOTween.Sequence();
			_seq.Append(_model.DOScaleY(_scaleUp, _scaleUpDuration));
			_seq.Append(transform.DOMove(targetPos, _moveDuration).SetEase(Ease.InOutQuad).OnComplete(() => Destroy(gameObject)));
		}

		private void OnGameResumed(object sender, GameResumed e)
		{
			_seq.Play();
		}

		private void OnGamePaused(object sender, GamePaused e)
		{
			_seq.Pause();
		}

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			Destroy(gameObject);
		}

		public void OnDisable()
		{
			EventBus<GamePaused>.RemoveListener(OnGamePaused);
			EventBus<GameResumed>.RemoveListener(OnGameResumed);
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
		}

		public void OnDestroy()
		{
			if (_seq.active)
			{
				_seq.Kill();
			}
		}
	}
}