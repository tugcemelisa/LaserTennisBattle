using HighlightPlus;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LaserTennis
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerController : MonoBehaviour
	{
		private bool _gameActive;
		public bool JumpLand { get; private set; }
		public int PlayerIndex;
		private float _animSpeed;
		[SerializeField] private int _maxHealth;
		[SerializeField] private int _currentHealth;
		[SerializeField] private Animator _animator;
		[SerializeField] private HighlightEffect _highlightEffect;
		[SerializeField] private GameObject _hitParticle;
		[SerializeField] private GameObject _winConfetti;
		[field: SerializeField] public PlayerType PlayerType { get; private set; }

		#region Variables: Movement

		private Vector2 _input;
		private CharacterController _characterController;
		private Vector3 _direction;

		[SerializeField] private Movement movement;

		#endregion
		#region Variables: Rotation

		private Camera _mainCamera;
		[SerializeField] private float rotationSpeed = 500f;

		#endregion
		#region Variables: Gravity

		private float _velocity;
		private float _gravity = -9.81f;
		[SerializeField] private float gravityMultiplier = 3.0f;

		#endregion
		#region Variables: Jumping

		private int _numberOfJumps;
		[SerializeField] private float jumpPower;
		[SerializeField] private int maxNumberOfJumps = 2;

		#endregion

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
			_mainCamera = Camera.main;
		}

		public void Start()
		{
			EventBus<PlayerDied>.AddListener(OnPlayerDied);
			EventBus<GamePaused>.AddListener(OnGamePaused);
			EventBus<GameResumed>.AddListener(OnGameResumed);
			EventBus<GameStart>.AddListener(OnGameStart);
			EventBus<HealthUpdated>.Emit(this, new HealthUpdated(PlayerType, _currentHealth, _maxHealth));
		}

		private void Update()
		{
			if (_gameActive == false) return;
			ApplyRotation();
			ApplyGravity();
			ApplyMovement();
		}

		public void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Laser laser))
			{
				LaserHit();
			}
		}

		private void ApplyGravity()
		{
			if (IsGrounded() && _velocity < 0.0f)
			{
				_velocity = -1.0f;
			}
			else
			{
				_velocity += _gravity * gravityMultiplier * Time.deltaTime;
			}

			_direction.y = _velocity;
		}

		private void ApplyRotation()
		{
			if (_input.sqrMagnitude == 0) return;

			_direction = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * new Vector3(_input.x, 0.0f, _input.y);
			Quaternion targetRotation = Quaternion.LookRotation(_direction, Vector3.up);

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
		}

		private void ApplyMovement()
		{
			float targetSpeed = movement.isSprinting ? movement.speed * movement.multiplier : movement.speed;
			movement.currentSpeed = Mathf.MoveTowards(movement.currentSpeed, targetSpeed, movement.acceleration * Time.deltaTime);

			Vector3 moveSpeed = _direction;
			moveSpeed.x *= movement.currentSpeed;
			moveSpeed.z *= movement.currentSpeed;

			_characterController.Move(moveSpeed * Time.deltaTime);
		}

		private void OnGamePaused(object sender, GamePaused e)
		{
			_gameActive = false;
			_animSpeed = _animator.speed;
			_animator.speed = 0;
			_characterController.enabled = false;
		}

		private void OnGameResumed(object sender, GameResumed e)
		{
			_gameActive = true;
			_animator.speed = _animSpeed;
			_characterController.enabled = true;
		}

		public void Move(InputAction.CallbackContext context)
		{
			if (_gameActive == false) return;

			_input = context.ReadValue<Vector2>();
			_animator.SetFloat(ConstValues.Walk, _input.magnitude);
			_direction = new Vector3(_input.x, 0.0f, _input.y);
		}

		public void Jump(InputAction.CallbackContext context)
		{
			if (_gameActive == false) return;
			if (!context.started) return;
			if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
			if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

			_animator.SetTrigger(ConstValues.Jump);
			_numberOfJumps++;
			_velocity = jumpPower;
		}

		public void Sprint(InputAction.CallbackContext context)
		{
			if (_gameActive == false) return;
			movement.isSprinting = context.started || context.performed;
		}

		public void JumpDown(InputAction.CallbackContext context)
		{
			if (_gameActive == false) return;
			if (context.started && IsGrounded() == false)
			{
				JumpLand = true;
			}
		}

		private void LaserHit()
		{
			if (_currentHealth <= 3)
			{
				_gameActive = false;
				_currentHealth = 0;
				_animator.SetTrigger(ConstValues.Die);
				EventBus<PlayerDied>.Emit(this, new PlayerDied(PlayerType));
			}
			else
			{
				_currentHealth -= 3;
			}
			_highlightEffect.HitFX();
			var go = Instantiate(_hitParticle, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
			Destroy(go, 2);
			EventBus<HealthUpdated>.Emit(this, new HealthUpdated(PlayerType, _currentHealth, _maxHealth));
		}

		private IEnumerator WaitForLanding()
		{
			yield return new WaitUntil(() => !IsGrounded());
			yield return new WaitUntil(IsGrounded);

			_numberOfJumps = 0;
			yield return new WaitForSeconds(0.2f);
			JumpLand = false;
		}

		public bool IsGrounded() => _characterController.isGrounded;

		private void OnPlayerDied(object sender, PlayerDied e)
		{
			_gameActive = false;
			if (e.PlayerType != PlayerType)
			{
				_winConfetti.SetActive(true);
				_animator.SetTrigger(ConstValues.Win);
			}
		}

		private void OnGameStart(object sender, GameStart e)
		{
			_gameActive = true;
		}

		public void OnDestroy()
		{
			EventBus<PlayerDied>.RemoveListener(OnPlayerDied);
			EventBus<GamePaused>.RemoveListener(OnGamePaused);
			EventBus<GameResumed>.RemoveListener(OnGameResumed);
			EventBus<GameStart>.RemoveListener(OnGameStart);
		}
	}

	public enum PlayerType
	{
		Player1,
		Player2,
	}

	[Serializable]
	public struct Movement
	{
		public float speed;
		public float multiplier;
		public float acceleration;

		[HideInInspector] public bool isSprinting;
		[HideInInspector] public float currentSpeed;
	}
}