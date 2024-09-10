using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfigurationManager : MonoBehaviour
{
	private List<PlayerConfiguration> _playerConfigs;
	[SerializeField] private int _maxPlayers;

	public void SetPlayerMaterial(int index, Material material)
	{
		_playerConfigs[index].PlayerMaterial = material;
	}

	public void SetPlayerReady(int index, bool b)
	{
		_playerConfigs[index].IsReady = b;
		if (_playerConfigs.Count == _maxPlayers && _playerConfigs.All(p => p.IsReady))
		{
			// Load Game Scene
		}
	}

	public void OnPlayerJoined(PlayerInput playerInput)
	{
		if (_playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex) == false)
		{
			_playerConfigs.Add(new PlayerConfiguration(playerInput));
		}
	}
}

public class PlayerConfiguration
{
	public PlayerConfiguration(PlayerInput playerInput)
	{
		PlayerIndex = playerInput.playerIndex;
		PlayerInput = playerInput;
	}

	public PlayerInput PlayerInput { get; set; }
	public int PlayerIndex { get; set; }
	public bool IsReady { get; set; }
	public Material PlayerMaterial { get; set; }
}
