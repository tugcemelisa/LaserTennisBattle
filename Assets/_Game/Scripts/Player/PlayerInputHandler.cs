using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LaserTennis
{
	public class PlayerInputHandler : MonoBehaviour
	{
		private PlayerInput _playerInput;
		private PlayerController _playerController;

		private void Awake()
		{
			_playerInput = GetComponent<PlayerInput>();
			var playerIndex = _playerInput.playerIndex;
			var playerControllers = FindObjectsOfType<PlayerController>();
			_playerController = playerControllers.FirstOrDefault(p => (int)p.PlayerType == playerIndex);
			_playerInput.transform.SetParent(_playerController.transform);
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			_playerController.Move(context);
		}

		public void OnJump(InputAction.CallbackContext context)
		{
			_playerController.Jump(context);
		}

		public void OnJumpDown(InputAction.CallbackContext context)
		{
			_playerController.JumpDown(context);
		}

		public void OnSprint(InputAction.CallbackContext context)
		{
			_playerController.Sprint(context);
		}
	}
}