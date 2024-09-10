using System;

namespace LaserTennis
{
	public interface IJump
	{
		Action OnJump { get; set; }
	}
}