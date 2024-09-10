using DG.Tweening;
using UnityEngine;

namespace LaserTennis
{
	public class LaserGenerator : MonoBehaviour
	{
		public bool IsActive { get; private set; }
		[SerializeField] private Vector3 _laserRotation;
		[SerializeField] private GameObject _laserPrefab;
		[SerializeField] private Transform _model;
		[SerializeField] private Transform _laserPoint;
		[SerializeField] private Transform _target;
		[SerializeField] private Transform _activePos;
		[SerializeField] private Material _laserMaterial;

		public void Start()
		{
			IsActive = false;
		}

		public void Generate()
		{
			if (IsActive)
			{
				Instantiate(_laserPrefab, _laserPoint.position, Quaternion.identity).TryGetComponent(out Laser laser);
				laser?.Init(_laserRotation, _target, _laserMaterial);
			}

		}

		public void Activate()
		{
			Sequence seq = DOTween.Sequence();
			seq.Append(_model.DOMove(_activePos.position + new Vector3(0, 0.05f, 0), 0.4f));
			seq.Append(_model.DOMove(_activePos.position, 0.1f).OnComplete(() => IsActive = true));
		}
	}
}