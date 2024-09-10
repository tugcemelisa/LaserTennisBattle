using Cinemachine;
using UnityEngine;

namespace LaserTennis
{
	public class CamManager : MonoBehaviour
	{
		[SerializeField] private CinemachineVirtualCamera _player1WinCam;
		[SerializeField] private CinemachineVirtualCamera _player2WinCam;

		private void OnEnable()
		{
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
		}

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			switch (e.PlayerType)
			{
				case PlayerType.Player1:
					_player2WinCam.Priority = 100;
					break;
				case PlayerType.Player2:
					_player1WinCam.Priority = 100;
					break;
			}
		}

		private void OnDisable()
		{
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
		}
	}
}