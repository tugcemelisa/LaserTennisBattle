using UnityEngine;

namespace LaserTennis
{
	public class DollyCam : MonoBehaviour
	{


		public void DollyEnd()
		{
			EventBus<GameStart>.Emit(this, new GameStart());
		}
	}
}