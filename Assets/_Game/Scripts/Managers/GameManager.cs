using UnityEngine;

namespace LaserTennis
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private bool _hideCursor;

		private void Start()
		{
			Cursor.visible = !_hideCursor;
		}
	}
}