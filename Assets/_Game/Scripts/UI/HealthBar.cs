using UnityEngine;

namespace LaserTennis
{
	public class HealthBar : MonoBehaviour
	{
		[SerializeField] private string _property;
		[SerializeField] private Material _material;

		public void Health(int health, int maxHealth)
		{
			_material.SetFloat(_property, maxHealth - health);
		}
	}
}