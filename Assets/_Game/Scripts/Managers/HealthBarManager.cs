using UnityEngine;

namespace LaserTennis
{
	public class HealthBarManager : MonoBehaviour
	{
		[SerializeField] private HealthBar _player1;
		[SerializeField] private HealthBar _player2;

		public void Awake()
		{
			EventBus<HealthUpdated>.AddListener(OnHealthUpdated);
		}

		private void OnHealthUpdated(object sender, HealthUpdated e)
		{
			switch (e.PlayerType)
			{
				case PlayerType.Player1:
					_player1.Health(e.Health, e.MaxHealth);
					break;
				case PlayerType.Player2:
					_player2.Health(e.Health, e.MaxHealth);
					break;
			}
		}

		public void OnDestroy()
		{
			EventBus<HealthUpdated>.RemoveListener(OnHealthUpdated);
		}
	}
}