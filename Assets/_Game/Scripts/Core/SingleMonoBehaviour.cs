using UnityEngine;

namespace LaserTennis
{
	public class SingleMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance { get; set; }
		public static T Instance
		{
			get
			{
				//if (_instance == null)
				//{
				//    var go = new GameObject(typeof(T).Name);
				//    _instance = go.AddComponent<T>();
				//}

				return _instance;
			}
			private set => _instance = value;
		}

		protected virtual void Awake()
		{
			if (_instance != null)
			{
				Destroy(this.gameObject);
				return;
			}

			_instance = this.GetComponent<T>();
		}
	}
}