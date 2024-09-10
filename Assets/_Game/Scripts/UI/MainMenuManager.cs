using LaserTennis;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	public void Play()
	{
		SceneLoader.Instance.LoadAddressableScene(ConstValues.GameScene);
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
