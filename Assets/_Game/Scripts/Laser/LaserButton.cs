using DG.Tweening;
using UnityEngine;

namespace LaserTennis
{
	public class LaserButton : MonoBehaviour
	{
		public bool IsActive { get; private set; }
		private Material _activeMaterial;
		[SerializeField] private int _laserCount;
		[SerializeField] private Material _deactiveMaterial;
		[SerializeField] private MeshRenderer _meshRenderer;
		[SerializeField] private Transform _active;

		public void Awake()
		{
			IsActive = false;
			_meshRenderer.material = _deactiveMaterial;
		}

		public void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out PlayerController player))
			{
				_active.DOLocalMoveY(-0.15f, 0.2f);
				if (IsActive)
				{
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

		public void Activate()
		{
			IsActive = true;
			_meshRenderer.material = _activeMaterial;
		}

		public void Deactivate()
		{
			IsActive = false;
			_meshRenderer.material = _deactiveMaterial;
		}

		public void SetMaterial(Material mat)
		{
			_activeMaterial = mat;
		}
	}
}