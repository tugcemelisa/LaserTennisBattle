using System;
using UnityEngine;

namespace LaserTennis
{
	public class Foot : MonoBehaviour
	{
		public Action OnGrounded;

		public void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out Ground ground))
			{
				OnGrounded?.Invoke();
			}
		}
	}
}
